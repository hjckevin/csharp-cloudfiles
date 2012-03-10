using System;
using System.Collections.Generic;
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
		private OpenStack.Swift.ProgressCallback callbacks = null;
		private OpenStack.Swift.OperationCompleteCallback operation_complete_callback = null;
		private string username = null;
		private string api_key = null;
		private string auth_token = null;
		private string auth_url = null;
		private string storage_url = null;
		private string cdn_management_url = null;
		private string user_agent = "CloudFiles-C#/v3.0";
		private bool snet = false;
		private Client _client;
		/*
		 * <summary>This Class is used to connect to Cloud Files</summary>
		 * <param name='user_creds'>Pass a valid UserCredentials <see cref="Rackspace.Cloudfiles.UserCredentials" /> 
		 * object to supply connection info</param>
		 */
		[Obsolete]
		public Connection(UserCredentials user_creds)
		{
			this._client = new CF_Client();
			this.username = user_creds.UserName;
			this.api_key = user_creds.ApiKey;
			this.auth_token = user_creds.AuthToken;
			this.storage_url = user_creds.StorageUrl.ToString();
			this.cdn_management_url = user_creds.CdnMangementUrl.ToString();
			if (user_creds.AuthUrl == null)
			{
				this.auth_url = DEFAULT_AUTH_URL;
			}
			else
			{
				this.auth_url = user_creds.AuthUrl.ToString();
			}
			if (auth_token == null || this.storage_url == null)
			{
				this.Authenticate();
			}
		}
		[Obsolete]
		public Connection(UserCredentials user_creds, bool snet) : 
	        this(user_creds)
		{
			this.snet = snet;
		}
		[Obsolete]
		public string UserName
		{
			get { return this.username; }
			set { this.username = value; }
		}
		[Obsolete]
		public string ApiKey
		{
			get { return this.api_key; }
			set { this.api_key = value; }
		}
		[Obsolete]
		public string AuthToken
		{
			get { return this.auth_token; }
			set { this.auth_token = value; }
		}
		[Obsolete]
		public string StorageUrl
		{
			get { return this.storage_url; }
			set { this.storage_url = value; }
		}
		[Obsolete]
		public string CdnMangementUrl
		{
			get { return this.cdn_management_url; }
			set { this.cdn_management_url = value; }
		}
		[Obsolete]
		public string AuthUrl
		{
			get { return this.auth_url; }
			set { this.auth_url = value; }
		}
		[Obsolete]
		public int Timeout
		{
		    get { return this._client.Timeout; }
			set { this._client.Timeout = value; }
		}
		[Obsolete]
		public string UserAgent
		{
			get { return this.user_agent; }
			set { this.user_agent = value; }
		}
		[Obsolete]
		public Boolean HasCDN()
		{
			return !string.IsNullOrEmpty(CdnMangementUrl);
		}
		[Obsolete]
		public void AddProgessWatcher(OpenStack.Swift.ProgressCallback 
		                              callback)
		{
			if (this.callbacks == null)
			{
			    this._client.Progress = callback;
			}
			else
			{
				this._client.Progress += callback;
			}
		}
		[Obsolete]
		public void AddOperationCompleteCallback(OpenStack.Swift.
		                                         OperationCompleteCallback 
		                                         callback)
		{
			if (this.operation_complete_callback == null)
			{
			    this._client.OperationComplete = callback;
			}
			else
			{
				this._client.OperationComplete += callback;
			}
		}
		[Obsolete]
		public void Authenticate()
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
			try
			{
				AuthResponse res = this._client.GetAuth(this.auth_url, this.username, this.api_key, headers, new Dictionary<string, string>(), snet);
			    this.storage_url = res.Headers["x-storage-url"];
			    this.auth_token = res.Headers["x-auth-token"];
			    if (res.Headers.ContainsKey("x-cdn-management-url"))
				{
	                this.cdn_management_url = res.Headers["x-cdn-management-url"];
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
					        throw new AuthenticationFailedException("Error: " + e.Status.ToString());
					}
	     	}
		}
		[Obsolete]
		public AccountInformation GetAccountInformation()
		{   
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
			try
			{
			    return new AccountInformation(this._client.HeadAccount(this.storage_url, this.auth_token, headers, new Dictionary<string, string>()).Headers);
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
			headers.Add("user-agent", this.user_agent);
			try
			{
			    this._client.PutContainer(this.storage_url, this.auth_token, container_name, headers, new Dictionary<string, string>());
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
			Dictionary<string, string> headers = new Dictionary<string, string>();
			this.CreateContainer(container_name, headers);
		}
		[Obsolete]
		public void DeleteContainer(string container_name)
		{
			if (string.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
			try
			{
			    this._client.DeleteContainer(this.storage_url,this.auth_token, container_name, headers, new Dictionary<string, string>());
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
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
			try
			{
				this._client.DeleteObject(this.storage_url,this.auth_token,container_name,object_name, headers, new Dictionary<string, string>());
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
			Dictionary<string, string> headers = new Dictionary<string, string>();
			string emails = "";
			int counter = 0;
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
			    this._client.DeleteObject(this.cdn_management_url, this.auth_token, container_name, object_name, headers, new Dictionary<string, string>());
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
			string[] email_addresses = new string[1];
			this.PurgePublicObject(container_name, object_name, email_addresses);
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetContainers(Dictionary<GetContainersListParameters, string> parameters)
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
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
			   return this._client.GetAccount(this.StorageUrl, this.AuthToken, headers, query, false).Containers;
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
			Dictionary<GetContainersListParameters, string> parameters = new Dictionary<GetContainersListParameters, string>();
			return this.GetContainers(parameters);
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetPublicContainers(Dictionary<GetPublicContainersListParameters,string> parameters)
		{
			Dictionary<string, string> query = new Dictionary<string, string>();
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
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
			   return this._client.GetCDNAccount(this.CdnMangementUrl, this.AuthToken, headers, query, false).Containers;
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
			Dictionary<GetPublicContainersListParameters,string> parameters = new Dictionary<GetPublicContainersListParameters,string>();
			return this.GetPublicContainers(parameters);
		}
		[Obsolete]
		public List<Dictionary<string, string>> GetContainerItemList(string container_name, Dictionary<GetItemListParameters,string> parameters)
		{
			if (string.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			Dictionary<string, string> query = new Dictionary<string, string>();
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
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
			   return this._client.GetContainer(this.StorageUrl,this.AuthToken,container_name, headers, query, false).Objects;
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
			Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>();
			return this.GetContainerItemList(container_name, parameters);
		}
		[Obsolete]
		public Container GetContainerInformation(string container_name)
		{
		    if (string.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			Dictionary<string, string> headers  = new Dictionary<string, string>();
			headers.Add("user-agent", this.user_agent);
			try
			{
				if(this.HasCDN())
				{
					return new Container(container_name, this._client.HeadContainer(this.storage_url, this.auth_token, container_name, headers, new Dictionary<string, string>()),
						                 this._client.HeadContainer(this.cdn_management_url, this.auth_token, container_name, headers, new Dictionary<string, string>()));
				}
				else
				{
					return new Container(container_name, this._client.HeadContainer(this.storage_url, this.auth_token, container_name, headers, new Dictionary<string, string>()),
						                 new ContainerResponse(new Dictionary<string, string>(), null, -1, new List<Dictionary<string, string>>()));
				}
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
		    Stream contents  = new FileStream(local_file_path, FileMode.Open, FileAccess.Read);
            string object_name = Path.GetFileName(local_file_path);
			this.PutStorageItem(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path)
		{
			Dictionary<string, string> metadata = new Dictionary<string, string>();
			this.PutStorageItem(container_name, local_file_path, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path, string object_name)
		{
			Dictionary<string, string> metadata = new Dictionary<string, string>();
			this.PutStorageItem(container_name, local_file_path, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, string local_file_path, string object_name, Dictionary<string, string> metadata)
		{
			Stream contents = new FileStream(local_file_path, FileMode.Open, FileAccess.Read);
			this.PutStorageItem(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItem(string container_name, Stream object_stream, string object_name)
		{
			Dictionary<string, string> metadata = new Dictionary<string, string>();
			this.PutStorageItem(container_name, object_stream, object_name, metadata);
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
			metadata.Add("user-agent", this.user_agent);
			try
			{
		        this._client.PutObject(this.storage_url, this.auth_token, container_name, object_name, object_stream, metadata, new Dictionary<string, string>());
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
			this.PutStorageItemAsync(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name,
		                           string local_file_path)
		{
			Dictionary<string, string> metadata = new Dictionary<string, string>();
			this.PutStorageItemAsync(container_name, local_file_path, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, string local_file_path, string object_name)
		{
			Dictionary<string, string> metadata = new Dictionary<string, string>();
			this.PutStorageItemAsync(container_name, local_file_path, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, string local_file_path, string object_name, Dictionary<string, string> metadata)
		{
			Stream contents = new FileStream(local_file_path, FileMode.Open, FileAccess.Read);
			this.PutStorageItemAsync(container_name, contents, object_name, metadata);
		}
		[Obsolete]
		public void PutStorageItemAsync(string container_name, Stream object_stream, string object_name, Dictionary<string, string> metadata)
		{
		    Thread thread = new Thread( () =>
			    {
			        this.PutStorageItem(container_name, object_stream, object_name, metadata);
			    }
			);
			thread.Start();
		}
		[Obsolete]
		public void GetStorageItem(string container_name, string object_name, string path, Dictionary<string, string> headers)
		{
			if (path.Equals(null))
			{
				throw new ArgumentNullException();
			}
			StorageItem res = this.GetStorageItem(container_name, object_name, headers);
			int read;
			int bytes_written = 0;
			Stream stream = new FileStream(path, FileMode.OpenOrCreate);
			byte[] buff = new byte[1048576];
			try
			{
			    while (true)
			    {
					read = res.ObjectStream.Read(buff, 0, buff.Length);
					if (read > 0)
					{	
				        bytes_written += buff.Length;
			            stream.Write(buff, 0, read);
				        if (this.callbacks != null)
				        {
					        this.callbacks(bytes_written);
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
			if (this.operation_complete_callback != null)
			{
			    this.operation_complete_callback();
			}
		}
		[Obsolete]
		public void GetStorageItem(string container_name, string object_name, string path)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			this.GetStorageItem(container_name, object_name, path, headers);
		}
		[Obsolete]
		public StorageItem GetStorageItem(string container_name, string object_name)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			return this.GetStorageItem(container_name, object_name, headers);
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
			headers.Add("user-agent", this.user_agent);
			try
			{
				ObjectResponse res = this._client.GetObject(this.storage_url, this.auth_token, container_name, object_name, headers, new Dictionary<string, string>());
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
			Thread thread = new Thread( () =>
			    {
			        this.GetStorageItem(container_name, object_name, path, headers);
			    }
			);
			thread.Start();
		}
		[Obsolete]
		public void GetStorageItemAsync(string container_name, string object_name, string path)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			this.GetStorageItemAsync(container_name, object_name, path, headers);
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
				throw new TTLLengthException("TTL Too short or too long. Min Value 900 Max Value 1577836800" + " value used: " + ttl.ToString());
			}
			if (!this.HasCDN())
			{
				throw new CDNNotEnabledException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("X-Log-Retention", log_retention.ToString());
			headers.Add("X-TTL", ttl.ToString());
			try
			{
				ContainerResponse res = this._client.PutContainer(this.cdn_management_url, this.auth_token, container_name, headers, new Dictionary<string, string>());
			    Dictionary<string, Uri> cdn_urls = new Dictionary<string, Uri>();
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
			return this.MarkContainerAsPublic(container_name, ttl, false);
		}
		[Obsolete]
		public Dictionary<string, Uri> MarkContainerAsPublic(string container_name)
		{
			return this.MarkContainerAsPublic(container_name, 900, false);
		}
		[Obsolete]
		public void MarkContainerAsPrivate(string container_name)
		{
			if (container_name == null)
			{
				throw new ArgumentNullException();
			}
            Common.ValidateContainerName(container_name);
			if (!this.HasCDN())
			{
				throw new CDNNotEnabledException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("x-cdn-enabled", "false");
			try
			{
				this._client.PostContainer(this.cdn_management_url, this.auth_token, container_name, headers, new Dictionary<string, string>());
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
				this._client.PostContainer(this.cdn_management_url, this.auth_token, container_name,  headers, new Dictionary<string, string>());
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
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("x-logging-enabled", logging_enabled.ToString());
			headers.Add("x-ttl", ttl.ToString());
			this.SetDetailsOnPublicContainer(container_name, headers);
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
				this._client.PostObject(this.storage_url, this.auth_token, container_name, object_name, headers, new Dictionary<string, string>());
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
				this._client.PostContainer(this.storage_url, this.auth_token, container_name, headers, new Dictionary<string, string>());
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
				this._client.PostAccount(this.storage_url, this.auth_token, headers, new Dictionary<string, string>());
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
			this.Metadata = new Dictionary<string, string>();
			this.Headers = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> header in headers)
			{
		        if (header.Key.Contains("x-account-meta"))
				{
			        this.Metadata.Add(header.Key.Remove(0, 15), header.Value);
				}
				else if (header.Key == "x-account-container-count")
				{
			         this.ContainerCount = int.Parse(header.Value);
				}
				else if (header.Key == "x-account-bytes-used")
				{
			        this.BytesUsed = long.Parse(header.Value);
				}
				else
				{
			        this.Headers.Add(header.Key, header.Value);
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
        /// <param name="containerName">Name of the container</param>
        [Obsolete]
        public Container(string container_name)
        {
			                      
            this.Name = container_name;
            this.ObjectCount = -1;
            this.ByteCount = -1;
            this.TTL = -1;
			this.CdnUri = null;
			this.CdnSslUri = null;
			this.CdnStreamingUri = null;
			this.CdnEnabled = false;
			this.LogRetention = false;
			this.Metadata = new Dictionary<string, string>();
        }
		[Obsolete]
		public Container(string container_name, ContainerResponse resp, 
		                 ContainerResponse cdn_resp) : this(container_name)
		{
			string key = "";
			foreach (KeyValuePair<string, string> header in resp.Headers)
			{
				if (header.Key.Contains("x-container-meta"))
				{
					key = header.Key.Remove(0, 17);
				    this.Metadata.Add(key, header.Value);
				}
			}
			if (resp.Headers.ContainsKey("x-container-bytes-used"))
			{
			    this.ByteCount = Int64.Parse(resp.Headers["x-container-bytes-used"]);
			}
			if (resp.Headers.ContainsKey("x-container-object-count"))
			{
		        this.ObjectCount = Int64.Parse(resp.Headers["x-container-object-count"]);
			}
			if (cdn_resp.Headers.Count > 0)
			{
				this.TTL = Int64.Parse(cdn_resp.Headers["x-ttl"]);
				this.CdnUri = new Uri(cdn_resp.Headers["x-cdn-uri"]);
				this.CdnSslUri = new Uri(cdn_resp.Headers["x-cdn-ssl-uri"]);
				this.CdnStreamingUri = new Uri(cdn_resp.Headers["x-cdn-streaming-uri"]);
				this.LogRetention = Convert.ToBoolean(cdn_resp.Headers["x-log-retention"]);
				this.CdnEnabled = Convert.ToBoolean(cdn_resp.Headers["x-cdn-enabled"]);
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
			this.Name = object_name;
			this.ContentType = null;
			this.ObjectStream = null;
			this.ContentLength = -1;
			this.LastModified = new DateTime();
			Metadata = new Dictionary<string, string>();
			Headers = new Dictionary<string, string>();
		}
		[Obsolete]
		public StorageItem(string object_name, Dictionary<string, string> headers,
		                   Stream object_stream) : this(object_name)
		{
			this.ObjectStream = object_stream;
			string key = "";
			foreach (KeyValuePair<string, string> header in headers)
			{
				if(header.Key.Contains("x-object-meta"))
				{
					key = header.Key.Remove(0, 14);
					this.Metadata.Add(key, header.Value);
				}
				else if (header.Key.Equals("content-length"))
				{
					this.ContentLength = Int64.Parse(header.Value);
				}
				else if (header.Key.Equals("content-type"))
				{
					this.ContentType = header.Value.ToString();
				}
				else if (header.Key.Equals("last-modified"))
				{
					this.LastModified = DateTime.Parse(header.Value);
				}
				else
				{
					this.Headers.Add(header.Key, header.Value);
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