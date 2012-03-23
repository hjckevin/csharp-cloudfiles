using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.IO;
using OpenStack.Swift;
namespace Rackspace.Cloudfiles.Depricated
{
	[Obsolete]
	public class Connection
	{
		public const string US_AUTH_URL = "https://auth.api.rackspacecloud.com/v1.0";
        public const string UK_AUTH_URL = "https://lon.auth.api.rackspacecloud.com/v1.0";
		public const string DEFAULT_AUTH_URL = US_AUTH_URL;
		private readonly ProgressCallback callbacks;
		private readonly OperationCompleteCallback operation_complete_callback;
		private string username;
		private string api_key;
		private string auth_token;
		private string auth_url;
		private string storage_url;
		private string cdn_management_url;
		private string user_agent = "CloudFiles-C#/v3.0";
	    private readonly bool _snet;
	    private readonly Client _client;
		/*
		 * <summary>This Class is used to connect to Cloud Files</summary>
		 * <param name='user_creds'>Pass a valid UserCredentials <see cref="Rackspace.Cloudfiles.UserCredentials" /> 
		 * object to supply connection info</param>
		 */
		[Obsolete]
		public Connection(UserCredentials user_creds)
		{
			_client = new CF_Client();
			username = user_creds.UserName;
			api_key = user_creds.ApiKey;
			auth_token = user_creds.AuthToken;
			storage_url = user_creds.StorageUrl.ToString();
			cdn_management_url = user_creds.CdnMangementUrl.ToString();
			auth_url = user_creds.AuthUrl == null ? DEFAULT_AUTH_URL : user_creds.AuthUrl.ToString();
			if (auth_token == null || storage_url == null)
			{
				Authenticate();
			}
		}
		[Obsolete]
		public Connection(UserCredentials user_creds, bool snet) : 
	        this(user_creds)
		{
			_snet = snet;
		}
		[Obsolete]
		public string UserName
		{
			get { return username; }
			set { username = value; }
		}
		[Obsolete]
		public string ApiKey
		{
			get { return api_key; }
			set { api_key = value; }
		}
		[Obsolete]
		public string AuthToken
		{
			get { return auth_token; }
			set { auth_token = value; }
		}
		[Obsolete]
		public string StorageUrl
		{
			get { return storage_url; }
			set { storage_url = value; }
		}
		[Obsolete]
		public string CdnMangementUrl
		{
			get { return cdn_management_url; }
			set { cdn_management_url = value; }
		}
		[Obsolete]
		public string AuthUrl
		{
			get { return auth_url; }
			set { auth_url = value; }
		}
		[Obsolete]
		public int Timeout
		{
		    get { return _client.Timeout; }
			set { _client.Timeout = value; }
		}
		[Obsolete]
		public string UserAgent
		{
			get { return user_agent; }
			set { user_agent = value; }
		}
		[Obsolete]
		public Boolean HasCDN()
		{
			return !string.IsNullOrEmpty(CdnMangementUrl);
		}
		[Obsolete]
		public void AddProgessWatcher(ProgressCallback 
		                              callback)
		{
			if (callbacks == null)
			{
			    _client.Progress = callback;
			}
			else
			{
				_client.Progress += callback;
			}
		}
		[Obsolete]
		public void AddOperationCompleteCallback(
		                                         OperationCompleteCallback 
		                                         callback)
		{
			if (operation_complete_callback == null)
			{
			    _client.OperationComplete = callback;
			}
			else
			{
				_client.OperationComplete += callback;
			}
		}
		[Obsolete]
		public void Authenticate()
		{
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    try
			{
				AuthResponse res = _client.GetAuth(auth_url, username, api_key, headers, new Dictionary<string, string>(), _snet);
			    storage_url = res.Headers["x-storage-url"];
			    auth_token = res.Headers["x-auth-token"];
			    if (res.Headers.ContainsKey("x-cdn-management-url"))
				{
	                cdn_management_url = res.Headers["x-cdn-management-url"];
				}
			}
			catch (ClientException e)
			{
					switch (e.Status)
					{
				        case -1 :
					       throw new TimeoutException();
					    case 401:
						   throw new AuthenticationFailedException("Bad Username and API key");
				        default:
					        throw new AuthenticationFailedException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					}
	     	}
		}
		[Obsolete]
		public AccountInformation GetAccountInformation()
		{   
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    try
			{
			    return new AccountInformation(_client.HeadAccount(storage_url, auth_token, headers, new Dictionary<string, string>()).Headers);
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void CreateContainer(string container_name, Dictionary<string, string> headers)
		{
			Common.ValidateContainerName(container_name);
			headers.Add("user-agent", user_agent);
			try
			{
			    _client.PutContainer(storage_url, auth_token, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void CreateContainer(string container_name)
		{
			var headers = new Dictionary<string, string>();
			CreateContainer(container_name, headers);
		}
		[Obsolete]
		public void DeleteContainer(string container_name)
		{
			if (string.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    try
			{
			    _client.DeleteContainer(storage_url,auth_token, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    case 409:
					    throw new ContainerNotEmptyException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void DeleteStorageItem(string container_name, string object_name)
		{
			if (string.IsNullOrEmpty(container_name) || string.IsNullOrEmpty(object_name))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    try
			{
				_client.DeleteObject(storage_url,auth_token,container_name,object_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void PurgePublicObject(string container_name, string object_name, string[] email_addresses)
		{
			if (string.IsNullOrEmpty(container_name) || string.IsNullOrEmpty(object_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			var headers = new Dictionary<string, string>();
			var emails = "";
			var counter = 0;
			if (email_addresses.Length > 0)
			{
				foreach (string str in email_addresses)
				{
					counter++;
					emails = emails + str;
					if (counter < email_addresses.Length && !string.IsNullOrEmpty(str))
					{
						emails = emails + ",";
					}
				}
				if(emails.Length > 0)
				{
				    headers.Add("X-Purge-Email", emails);
				}
			}
			try
			{
			    _client.DeleteObject(cdn_management_url, auth_token, container_name, object_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void PurgePublicObject(string container_name, string object_name)
		{
			var email_addresses = new string[1];
			PurgePublicObject(container_name, object_name, email_addresses);
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetContainers(Dictionary<GetContainersListParameters, string> parameters)
		{
			var query = new Dictionary<string, string>();
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    if (parameters.ContainsKey(GetContainersListParameters.Limit))
			{
			    query.Add("limit", parameters[GetContainersListParameters.Limit]);
			}
			if (parameters.ContainsKey(GetContainersListParameters.Marker))
			{
			    query.Add("marker", parameters[GetContainersListParameters.Marker]);
			}
			if (parameters.ContainsKey(GetContainersListParameters.Prefix))
			{
				query.Add("prefix", parameters[GetContainersListParameters.Prefix]);
			}
			try
			{
			   return _client.GetAccount(StorageUrl, AuthToken, headers, query, false).Containers;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetContainers()
		{
			var parameters = new Dictionary<GetContainersListParameters, string>();
			return GetContainers(parameters);
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetPublicContainers(Dictionary<GetPublicContainersListParameters,string> parameters)
		{
			var query = new Dictionary<string, string>();
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    if (parameters.ContainsKey(GetPublicContainersListParameters.Limit))
			{
			    query.Add("limit", parameters[GetPublicContainersListParameters.Limit]);
			}
			if (parameters.ContainsKey(GetPublicContainersListParameters.Marker))
			{
			    query.Add("marker", parameters[GetPublicContainersListParameters.Marker]);
			}
			if (parameters.ContainsKey(GetPublicContainersListParameters.EnabledOnly))
			{
				query.Add("enabled_only", parameters[GetPublicContainersListParameters.EnabledOnly]);
			}	
			try
			{
			   return _client.GetCDNAccount(CdnMangementUrl, AuthToken, headers, query, false).Containers;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetPublicContainers()
		{
			var parameters = new Dictionary<GetPublicContainersListParameters,string>();
			return GetPublicContainers(parameters);
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetContainerItemList(string container_name, Dictionary<GetItemListParameters,string> parameters)
		{
			if (string.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			var query = new Dictionary<string, string>();
			var headers = new Dictionary<string, string> {{"user-agent", user_agent}};
		    if (parameters.ContainsKey(GetItemListParameters.Limit))
			{
			    query.Add("limit", parameters[GetItemListParameters.Limit]);
			}
			if (parameters.ContainsKey(GetItemListParameters.Prefix))
			{
			    query.Add("prefix", parameters[GetItemListParameters.Prefix]);
			}
			if (parameters.ContainsKey(GetItemListParameters.Marker))
			{
			    query.Add("marker", parameters[GetItemListParameters.Marker]);
			}
			if (parameters.ContainsKey(GetItemListParameters.Delimiter))
			{
			    query.Add("delimiter", parameters[GetItemListParameters.Delimiter]);
			}
			try
			{
			   return _client.GetContainer(StorageUrl,AuthToken,container_name, headers, query, false).Objects;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetContainerItemList(string container_name)
		{
			var parameters = new Dictionary<GetItemListParameters, string>();
			return GetContainerItemList(container_name, parameters);
		}
		[Obsolete]
		public Container GetContainerInformation(string container_name)
		{
		    if (string.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			var headers  = new Dictionary<string, string> {{"user-agent", user_agent}};
		    try
		    {
		        return HasCDN() ? new Container(container_name, _client.HeadContainer(storage_url, auth_token, container_name, headers, new Dictionary<string, string>()),_client.HeadContainer(cdn_management_url, auth_token, container_name, headers, new Dictionary<string, string>())) : new Container(container_name, _client.HeadContainer(storage_url, auth_token, container_name, headers, new Dictionary<string, string>()),new ContainerResponse(new Dictionary<string, string>(), null, -1, new List<Dictionary<string, string>>()));
		    }
		    catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path, Dictionary<string, string> metadata)
		{
		    var contents  = new FileStream(local_file_path, FileMode.Open, FileAccess.Read);
            var object_name = Path.GetFileName(local_file_path);
			PutStorageItem(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path)
		{
			var metadata = new Dictionary<string, string>();
			PutStorageItem(container_name, local_file_path, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path, string object_name)
		{
			var metadata = new Dictionary<string, string>();
			PutStorageItem(container_name, local_file_path, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path, string object_name, Dictionary<string, string> metadata)
		{
			var contents = new FileStream(local_file_path, FileMode.Open, FileAccess.Read);
			PutStorageItem(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, Stream object_stream, string object_name)
		{
			var metadata = new Dictionary<string, string>();
			PutStorageItem(container_name, object_stream, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, Stream object_stream, string object_name, Dictionary<string, string> metadata)
		{
		    if (string.IsNullOrEmpty(container_name) ||
			    string.IsNullOrEmpty(object_name))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			metadata.Add("user-agent", user_agent);
			try
			{
		        _client.PutObject(storage_url, auth_token, container_name, object_name, object_stream, metadata, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    case 422:
					    throw new InvalidETagException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, string local_file_path, Dictionary<string, string> metadata)
		{
		    Stream contents  = new FileStream(local_file_path, FileMode.Open,
			                                  FileAccess.Read);
            string object_name = Path.GetFileName(local_file_path);
			PutStorageItemAsync(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name,
		                           string local_file_path)
		{
			var metadata = new Dictionary<string, string>();
			PutStorageItemAsync(container_name, local_file_path, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, string local_file_path, string object_name)
		{
			var metadata = new Dictionary<string, string>();
			PutStorageItemAsync(container_name, local_file_path, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, string local_file_path, string object_name, Dictionary<string, string> metadata)
		{
			Stream contents = new FileStream(local_file_path, FileMode.Open, FileAccess.Read);
			PutStorageItemAsync(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, Stream object_stream, string object_name, Dictionary<string, string> metadata)
		{
		    var thread = new Thread( () => PutStorageItem(container_name, object_stream, object_name, metadata));
			thread.Start();
		}
		[Obsolete]
		public void GetStorageItem(string container_name, string object_name, string path, Dictionary<string, string> headers)
		{
			if (path.Equals(null))
			{
				throw new ArgumentNullException();
			}
			var res = GetStorageItem(container_name, object_name, headers);
		    int bytes_written = 0;
			Stream stream = new FileStream(path, FileMode.OpenOrCreate);
			var buff = new byte[1048576];
			try
			{
			    while (true)
			    {
			        int read = res.ObjectStream.Read(buff, 0, buff.Length);
			        if (read > 0)
					{	
				        bytes_written += buff.Length;
			            stream.Write(buff, 0, read);
				        if (callbacks != null)
				        {
					        callbacks(bytes_written);
				        }
					}
					else
					{
						break;
					}
			    }
			}
			finally
			{
			    stream.Close();
			    res.ObjectStream.Close();
			}
			if (operation_complete_callback != null)
			{
			    operation_complete_callback();
			}
		}
		[Obsolete]
		public void GetStorageItem(string container_name, string object_name, string path)
		{
			var headers = new Dictionary<string, string>();
			GetStorageItem(container_name, object_name, path, headers);
		}
		[Obsolete]
		public StorageItem GetStorageItem(string container_name, string object_name)
		{
			var headers = new Dictionary<string, string>();
			return GetStorageItem(container_name, object_name, headers);
		}
		[Obsolete]
		public StorageItem GetStorageItem(string container_name, string object_name, Dictionary<string, string> headers)
		{
			if (container_name == null|| object_name == null || headers == null)
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			headers.Add("user-agent", user_agent);
			try
			{
				ObjectResponse res = _client.GetObject(storage_url, auth_token, container_name, object_name, headers, new Dictionary<string, string>());
				return new StorageItem(object_name, res.Headers, res.ObjectData);
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}

		}
		[Obsolete]
		public void GetStorageItemAsync(string container_name, string object_name, string path,Dictionary<string, string> headers)
		{
			var thread = new Thread( () => GetStorageItem(container_name, object_name, path, headers));
			thread.Start();
		}
		[Obsolete]
		public void GetStorageItemAsync(string container_name, string object_name, string path)
		{
			var headers = new Dictionary<string, string>();
			GetStorageItemAsync(container_name, object_name, path, headers);
		}
		[Obsolete]
		public Dictionary<string, Uri> MarkContainerAsPublic(string container_name, long ttl, bool log_retention)
		{
			if (container_name.Equals(null) || ttl.Equals(null) || log_retention.Equals(null))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			if (ttl < 900 || ttl > 1577836800)
			{
				throw new TTLLengthException("TTL Too short or too long. Min Value 900 Max Value 1577836800" + " value used: " + ttl.ToString(CultureInfo.InvariantCulture));
			}
			if (!HasCDN())
			{
				throw new CDNNotEnabledException();
			}
			var headers = new Dictionary<string, string>
                {{"X-Log-Retention", log_retention.ToString(CultureInfo.InvariantCulture)},
                {"X-TTL", ttl.ToString(CultureInfo.InvariantCulture)}};
		    try
			{
				var res = _client.PutContainer(cdn_management_url, auth_token, container_name, headers, new Dictionary<string, string>());
			    var cdn_urls = new Dictionary<string, Uri>();
	    		if (res.Headers.ContainsKey("x-cdn-uri"))
    			{
     				cdn_urls.Add("x-cdn-uri", new Uri(res.Headers["x-cdn-uri"]));
	    		}
	    		if (res.Headers.ContainsKey("x-cdn-ssl-uri"))
    			{
		    		cdn_urls.Add("x-cdn-ssl-uri", new Uri(res.Headers["x-cdn-ssl-uri"]));
	    		}
	    		if (res.Headers.ContainsKey("x-cdn-streaming-uri"))
	    		{
	    			cdn_urls.Add("x-cdn-streaming-uri", new Uri(res.Headers["x-cdn-streaming-uri"]));
	    		}
	    		return cdn_urls;
	   		}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public Dictionary<string, Uri> MarkContainerAsPublic(string container_name, Int64 ttl)
		{
			return MarkContainerAsPublic(container_name, ttl, false);
		}
		[Obsolete]
		public Dictionary<string, Uri> MarkContainerAsPublic(string container_name)
		{
			return MarkContainerAsPublic(container_name, 900, false);
		}
		[Obsolete]
		public void MarkContainerAsPrivate(string container_name)
		{
			if (container_name == null)
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			if (!HasCDN())
			{
				throw new CDNNotEnabledException();
			}
			var headers = new Dictionary<string, string> {{"x-cdn-enabled", "false"}};
		    try
			{
				_client.PostContainer(cdn_management_url, auth_token, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
	    }
		[Obsolete]
		public void SetDetailsOnPublicContainer(string container_name, Dictionary<string, string> headers)
		{
			if (container_name == null)
			{
				throw new ArgumentNullException();
			}
			try
			{
				_client.PostContainer(cdn_management_url, auth_token, container_name,  headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void SetDetailsOnPublicContainer(string container_name, bool logging_enabled, long ttl)
		{
			var headers = new Dictionary<string, string>
                {{"x-logging-enabled", logging_enabled.ToString(CultureInfo.InvariantCulture)},
                {"x-ttl", ttl.ToString(CultureInfo.InvariantCulture)}};
		    SetDetailsOnPublicContainer(container_name, headers);
		}
		[Obsolete]
		public void SetStorageItemMetaInformation(string container_name, string object_name, Dictionary<string, string> headers)
		{
			if (container_name == null || object_name == null || headers == null)
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			try
			{
				_client.PostObject(storage_url, auth_token, container_name, object_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void SetContainerMetaInformation(string container_name, Dictionary<string, string> headers)
		{
			if (container_name == null || headers == null)
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			try
			{
				_client.PostContainer(storage_url, auth_token, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
		[Obsolete]
		public void SetAccountMetaInformation(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
			try
			{
				_client.PostAccount(storage_url, auth_token, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
					    throw new TimeoutException();
				    case 401:
					    throw new AuthenticationFailedException();
				    default:
					    throw new CloudFilesException("Error: " + e.Status);
				}
			}
		}
	}
    /*
     * Helper Classes and Functions
     */ 
	[Obsolete]
	public class AccountInformation
	{
		[Obsolete]
		public AccountInformation(Dictionary<string, string> headers)
		{
			Metadata = new Dictionary<string, string>();
			Headers = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> header in headers)
			{
		        if (header.Key.Contains("x-account-meta"))
				{
			        Metadata.Add(header.Key.Remove(0, 15), header.Value);
				}
				else if (header.Key == "x-account-container-count")
				{
			         ContainerCount = int.Parse(header.Value);
				}
				else if (header.Key == "x-account-bytes-used")
				{
			        BytesUsed = long.Parse(header.Value);
				}
				else
				{
			        Headers.Add(header.Key, header.Value);
				}
			}
		}
		[Obsolete]
		public int ContainerCount { get; set; }
		[Obsolete]
		public long BytesUsed { get; set; }
		[Obsolete]
		public Dictionary<string, string>Metadata;
		[Obsolete]
		public Dictionary<string, string>Headers;
	}
	[Obsolete]
    public class Container
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container_name">Name of the container</param>
        [Obsolete]
        public Container(string container_name)
        {
			                      
            Name = container_name;
            ObjectCount = -1;
            ByteCount = -1;
            TTL = -1;
			CdnUri = null;
			CdnSslUri = null;
			CdnStreamingUri = null;
			CdnEnabled = false;
			LogRetention = false;
			Metadata = new Dictionary<string, string>();
        }
		[Obsolete]
		public Container(string container_name, ContainerResponse resp, 
		                 ContainerResponse cdn_resp) : this(container_name)
		{
		    foreach (KeyValuePair<string, string> header in resp.Headers)
			{
				if (header.Key.Contains("x-container-meta"))
				{
				    var key = header.Key.Remove(0, 17);
				    Metadata.Add(key, header.Value);
				}
			}
			if (resp.Headers.ContainsKey("x-container-bytes-used"))
			{
			    ByteCount = Int64.Parse(resp.Headers["x-container-bytes-used"]);
			}
			if (resp.Headers.ContainsKey("x-container-object-count"))
			{
		        ObjectCount = Int64.Parse(resp.Headers["x-container-object-count"]);
			}
			if (cdn_resp.Headers.Count > 0)
			{
				TTL = Int64.Parse(cdn_resp.Headers["x-ttl"]);
				CdnUri = new Uri(cdn_resp.Headers["x-cdn-uri"]);
				CdnSslUri = new Uri(cdn_resp.Headers["x-cdn-ssl-uri"]);
				CdnStreamingUri = new Uri(cdn_resp.Headers["x-cdn-streaming-uri"]);
				LogRetention = Convert.ToBoolean(cdn_resp.Headers["x-log-retention"]);
				CdnEnabled = Convert.ToBoolean(cdn_resp.Headers["x-cdn-enabled"]);
			}
		}
		[Obsolete]
		public Dictionary<string, string> Metadata;
        /// <summary>
        /// Size of the container
        /// </summary>
        [Obsolete]
		public long ByteCount;
        /// <summary>
        /// Number of items in the container
        /// </summary>
        [Obsolete]
		public long ObjectCount;
        /// <summary>
        /// Name of the container
        /// </summary>
        [Obsolete]
		public string Name;
        /// <summary>
        /// The maximum time (in seconds) content should be kept alive on the CDN before it checks for freshness.
        /// </summary>
        [Obsolete]
		public long TTL;
        /// <summary>
        /// The URI one can use to access objects in this container via the CDN. No time based URL stuff will be included with this URI
        /// </summary>
        [Obsolete]
		public Uri CdnUri;
        /// <summary>
        /// The SSL URI one can use to access objects in this container via the CDN.
        /// </summary>
        [Obsolete]
		public Uri CdnSslUri;
         /// <summary>
        /// The Streaming URI one can use to access objects in this container via the CDN.
        /// </summary>
		[Obsolete]
		public Uri CdnStreamingUri;
		/// <summary>
		/// Boolean is CDN is enabled or not
		/// </summary>
		[Obsolete]
		public bool CdnEnabled;
		/// <summary>
		/// Boolean is CDN Log Retention Enabled
		/// </summary>
		[Obsolete]
		public bool LogRetention;
    }
	[Obsolete]
	public class StorageItem
	{
		[Obsolete]
		public string Name;
		[Obsolete]
		public Dictionary<string, string> Metadata;
		[Obsolete]
		public Dictionary<string, string> Headers;
		[Obsolete]
		public string ContentType;
		[Obsolete]
		public Stream ObjectStream;
		[Obsolete]
		public long ContentLength;
		[Obsolete]
		public DateTime LastModified;
		[Obsolete]
		public StorageItem(string object_name)
		{
			Name = object_name;
			ContentType = null;
			ObjectStream = null;
			ContentLength = -1;
			LastModified = new DateTime();
			Metadata = new Dictionary<string, string>();
			Headers = new Dictionary<string, string>();
		}
		[Obsolete]
		public StorageItem(string object_name, Dictionary<string, string> headers,
		                   Stream object_stream) : this(object_name)
		{
			ObjectStream = object_stream;
		    foreach (var header in headers)
			{
				if(header.Key.Contains("x-object-meta"))
				{
				    string key = header.Key.Remove(0, 14);
				    Metadata.Add(key, header.Value);
				}
				else if (header.Key.Equals("content-length"))
				{
					ContentLength = Int64.Parse(header.Value);
				}
				else if (header.Key.Equals("content-type"))
				{
					ContentType = header.Value.ToString(CultureInfo.InvariantCulture);
				}
				else if (header.Key.Equals("last-modified"))
				{
					LastModified = DateTime.Parse(header.Value);
				}
				else
				{
					Headers.Add(header.Key, header.Value);
				}
			}
		}
	}
	[Obsolete]
	public enum GetItemListParameters
    {
        Limit,
        Marker,
        Prefix,
        Path,
		Delimiter
    }
	[Obsolete]
	public enum GetContainersListParameters
    {
        Limit,
        Marker,
		Prefix
    }
	[Obsolete]
	public enum GetPublicContainersListParameters
	{
		Limit,
		Marker,
		EnabledOnly
	}
}