using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenStack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Container object that provides Container level functionality and information
	/// </summary>
	public class CF_Container : Container
	{
        private readonly string _name;
        private int _retires;
        private int _num_retries_attempted;
        private bool _reload_properties = true;
	    private const bool _reload_cdn_properties = true;
	    private bool _cdn_log_retention;
        private bool _cdn_enabled;
        private Uri _cdn_uri;
        private Uri _cdn_ssl_uri;
        private Uri _cdn_streaming_uri;
        private Dictionary<string, string> _metadata;
        private Dictionary<string, string> _headers;
        private Dictionary<string, string> _cdn_headers;
        private long _object_count = -1;
        private long _bytes_used = -1;
        private long _ttl = -1;
        private readonly Client _client;
        private readonly Connection _conn;
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Container"/> class.
		/// </summary>
		/// <param name='conn'>
		/// Conn.
		/// </param>
		/// <param name='client'>
		/// Client.
		/// </param>
		/// <param name='container_name'>
		/// Container_name.
		/// </param>
		public CF_Container(Connection conn, Client client, string container_name)
		{
			Common.ValidateContainerName(container_name);
			_client = client;
			_conn = conn;
			_name = container_name;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Container"/> class.
		/// </summary>
		/// <param name='conn'>
		/// Conn.
		/// </param>
		/// <param name='container_name'>
		/// Container_name.
		/// </param>
		public CF_Container (Connection conn, string container_name)
		{
			Common.ValidateContainerName(container_name);
			_client = new CF_Client();
			_conn = conn;
			_name = container_name;
		}
		
		/// <summary>
		/// Gets the Container Name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name
		{
			get { return _name; }
		}
		/// <summary>
		/// Gets Raw headers.
		/// </summary>
		/// <value>
		/// The headers.
		/// </value>
		public Dictionary<string, string> Headers
		{
			get { return _reload_properties ? Common.ProcessMetadata(_head_container().Headers)["headers"] : _headers; }
		}
		/// <summary>
		/// Gets the cdn headers.
		/// </summary>
		/// <value>
		/// The cdn headers.
		/// </value>
		public Dictionary<string, string> CdnHeaders
		{
			get { return (_head_cdn_container() ? _cdn_headers : null); }
		}
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public Dictionary<string, string> Metadata
		{
			get { return _reload_properties ? Common.ProcessMetadata(_head_container().Headers)["metadata"] : _metadata; }
		}
		/// <summary>
		/// Gets the object count.
		/// </summary>
		/// <value>
		/// The object count.
		/// </value>
		public long ObjectCount
		{
			get { return _reload_properties ? long.Parse(_head_container().Headers["x-container-object-count"]) : _object_count; }
		}
		/// <summary>
		/// Gets the bytes used.
		/// </summary>
		/// <value>
		/// The bytes used.
		/// </value>
		public long BytesUsed
		{
			get { return _reload_properties ? long.Parse(_head_container().Headers["x-container-bytes-used"]) : _bytes_used; }
		}
		/// <summary>
		/// Gets the TT.
		/// </summary>
		/// <value>
		/// The TT.
		/// </value>
		public long TTL
		{
			get { return (_head_cdn_container() ? _ttl : -1); }
		}
		/// <summary>
		/// Gets or sets the retries.
		/// </summary>
		/// <value>
		/// The retries.
		/// </value>
		public int Retries
		{
			get { return _retires; }
			set { _retires = value; }
		}
		/// <summary>
		/// Gets the storage URL.
		/// </summary>
		/// <value>
		/// The storage URL.
		/// </value>
		public Uri StorageUrl
		{
			get { return new Uri(_conn.UserCreds.StorageUrl + Name); }
		}
		/// <summary>
		/// Gets the cdn management URL.
		/// </summary>
		/// <value>
		/// The cdn management URL.
		/// </value>
		public Uri CdnManagementUrl
		{
			get { return _conn.HasCDN ? new Uri(_conn.UserCreds.CdnMangementUrl + Name) : null; }
		}
		/// <summary>
		/// Gets the cdn URI.
		/// </summary>
		/// <value>
		/// The cdn URI.
		/// </value>
		public Uri CdnUri
		{
			get { return (_head_cdn_container() ? _cdn_uri : null); }
		}
		/// <summary>
		/// Gets the cdn ssl URI.
		/// </summary>
		/// <value>
		/// The cdn ssl URI.
		/// </value>
		public Uri CdnSslUri
		{
			get { return (_head_cdn_container() ? _cdn_ssl_uri : null); }
		}
		/// <summary>
		/// Gets the cdn streaming URI.
		/// </summary>
		/// <value>
		/// The cdn streaming URI.
		/// </value>
		public Uri CdnStreamingUri
		{
			get { return (_head_cdn_container() ? _cdn_streaming_uri : null); }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Rackspace.Cloudfiles.CF_Container"/> cdn enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if cdn enabled; otherwise, <c>false</c>.
		/// </value>
		public bool CdnEnabled
		{
			get { return (_head_cdn_container() && _cdn_enabled); }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Rackspace.Cloudfiles.CF_Container"/> cdn log retention.
		/// </summary>
		/// <value>
		/// <c>true</c> if cdn log retention; otherwise, <c>false</c>.
		/// </value>
		public bool CdnLogRetention
		{
			get { return (_head_cdn_container() && _cdn_log_retention); }
		}
		private ContainerResponse _head_container()
		{
			var headers = new Dictionary<string, string> {{"user-agent", _conn.UserAgent}};
		    _client.Timeout = _conn.Timeout;
		    try
		    {
			    var res = _client.HeadContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, Name, headers, new Dictionary<string, string>());
				_object_count = long.Parse(res.Headers["x-container-object-count"]);
				_bytes_used = long.Parse(res.Headers["x-container-bytes-used"]);
				var processed_headers = Common.ProcessMetadata(res.Headers);
				_headers = processed_headers["headers"];
				_metadata = processed_headers["metadata"];
				_reload_properties = false;
				return res;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return _head_container();
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    return _head_container();
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					     throw new ContainerNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return _head_container();
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		private bool _head_cdn_container()
		{
		    try
			{
				if (_conn.HasCDN)
				{
				    var headers = new Dictionary<string, string> {{"user-agent", _conn.UserAgent}};
				    _client.Timeout = _conn.Timeout;
			        var res = _client.HeadContainer(_conn.UserCreds.CdnMangementUrl.ToString(), _conn.UserCreds.AuthToken, Name, headers, new Dictionary<string, string>());
				    _cdn_uri = new Uri(res.Headers["x-cdn-uri"]);
				    _cdn_ssl_uri = new Uri(res.Headers["x-cdn-ssl-uri"]);
				    _cdn_streaming_uri = new Uri(res.Headers["x-cdn-streaming-uri"]);
					_ttl = long.Parse(res.Headers["x-ttl"]);
					_cdn_enabled = bool.Parse(res.Headers["x-cdn-enabled"]);
					_cdn_log_retention = bool.Parse(res.Headers["x-log-retention"]);
					_cdn_headers = res.Headers;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return _head_cdn_container();
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    return _head_cdn_container();
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    return false;
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return _head_cdn_container();
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Creates the object.
		/// </summary>
		/// <returns>
		/// The object.
		/// </returns>
		/// <param name='object_name'>
		/// Object_name.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
	    public StorageObject CreateObject(string object_name)
		{
			if (object_name == null)
			{
				throw new ArgumentNullException();
			}
			Common.ValidateObjectName(object_name);
			return new CF_Object(_conn, _name, object_name);
		}
		/// <summary>
		/// Creates the Object.
		/// </summary>
		/// <param name="object_name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="StorageObject"/>
		/// </returns>
		public StorageObject GetObject(string object_name)
		{
			if (object_name == null)
			{
				throw new ArgumentNullException();
			}
			Common.ValidateObjectName(object_name);
			try
			{
			    _client.HeadObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, Name, object_name, _headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetObject(object_name);
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    return GetObject(object_name);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetObject(object_name);
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
			return new CF_Object(_conn, _name, object_name);
		}
		/// <summary>
		/// Gets a list of StorageObject objects.
		/// </summary>
		/// <returns>
		/// The objects.
		/// </returns>
		public List<StorageObject> GetObjects()
		{
			return GetObjects(false);
		}
		/// <summary>
		/// Gets a list of StorageObject objects.
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<StorageObject>"/>
		/// </returns>
		public List<StorageObject> GetObjects(bool full_listing)
		{
			return GetObjects(full_listing, new Dictionary<ObjectQuery, string>());
		}
		/// <summary>
		/// Gets a list of StorageObject objects.
		/// </summary>
		/// <param name="query">
		/// A <see cref="Dictionary<ObjectQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<StorageObject>"/>
		/// </returns>
		public List<StorageObject> GetObjects(Dictionary<ObjectQuery, string> query)
		{
			return GetObjects(false, query);
		}
		/// <summary>
		/// Gets a list of StorageObject objects.
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="query">
		/// A <see cref="Dictionary<ObjectQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<StorageObject>"/>
		/// </returns>
		public List<StorageObject> GetObjects(bool full_listing, Dictionary<ObjectQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			_client.Timeout = _conn.Timeout;
            var headers = new Dictionary<string, string> {{ "user-agent", _conn.UserAgent}};
			var queryp = new Dictionary<string, string>();
			foreach (var q in query)
			{
				switch (q.Key)
				{
				    case ObjectQuery.Limit:
					    queryp.Add("limit", q.Value);
					    break;
				    case ObjectQuery.Marker:
					    queryp.Add("marker", q.Value);
			            break;
				    case ObjectQuery.Prefix:
					    queryp.Add("prefix", q.Value);
					    break;
				    case ObjectQuery.Delimiter:
					    queryp.Add("delimiter", q.Value);
					    break;
				}
			}
			try
			{
			    return _client.GetContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _name, headers, queryp, full_listing).Objects.Select(obj => new CF_Object(_conn, _name, obj["name"])).Cast<StorageObject>().ToList();
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetObjects(full_listing, query);
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    return GetObjects(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetObjects(full_listing, query);
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Gets a list of Dictionaries that contain storage object info.
		/// </summary>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetObjectList()
		{
			return GetObjectList(false);
		}
		/// <summary>
		/// Gets a list of Dictionaries that contain storage object info.
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetObjectList(bool full_listing)
		{
			return GetObjectList(full_listing, new Dictionary<ObjectQuery, string>());
		}
		/// <summary>
		/// Gets a list of Dictionaries that contain storage object info.
		/// </summary>
		/// <param name="query">
		/// A <see cref="Dictionary<ObjectQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetObjectList(Dictionary<ObjectQuery, string> query)
		{
			return GetObjectList(false, query);
		}
		/// <summary>
		/// Gets a list of Dictionaries that contain storage object info.
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="query">
		/// A <see cref="Dictionary<ObjectQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetObjectList(bool full_listing, Dictionary<ObjectQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			_client.Timeout = _conn.Timeout;
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
			var queryp = new Dictionary<string, string>();
			foreach (var q in query)
			{
				switch (q.Key)
				{
				    case ObjectQuery.Limit:
					    queryp.Add("limit", q.Value);
					    break;
				    case ObjectQuery.Marker:
					    queryp.Add("marker", q.Value);
			            break;
				    case ObjectQuery.Prefix:
					    queryp.Add("prefix", q.Value);
					    break;
				    case ObjectQuery.Delimiter:
					    queryp.Add("delimiter", q.Value);
					    break;
				}
			}
			try
			{
				return _client.GetContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _name, headers, queryp, full_listing).Objects;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetObjectList(full_listing, query);
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    return GetObjectList(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetObjectList(full_listing, query);
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Deletes an Object.
		/// </summary>
		/// <param name="object_name">
		/// A <see cref="System.String"/>
		/// </param>
		public void DeleteObject(string object_name)
		{
			Common.ValidateObjectName(object_name);
			var headers = new Dictionary<string, string> {{ "user-agent", _conn.UserAgent }};
			_client.Timeout = _conn.Timeout;
			try
			{
			    _client.DeleteObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, Name, object_name, headers, new Dictionary<string, string>());
				_reload_properties = true;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    DeleteObject(object_name);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    DeleteObject(object_name);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    DeleteObject(object_name);
                            break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Adds the metadata.
		/// </summary>
		/// <param name='metadata'>
		/// Metadata.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
		public void AddMetadata(Dictionary<string, string> metadata)
		{
			if (metadata.Equals(null))
			{
				throw new ArgumentNullException();
			}
			var headers = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> m in metadata)
			{
				if (m.Key.Contains("x-contaner-meta-"))
				{
					headers.Add(m.Key, m.Value);
				}
				else
				{
					headers.Add("x-container-meta-" + m.Key, m.Value);
				}
			}
			AddHeaders(headers);
		}
		/// <summary>
		/// Adds Headers.
		/// </summary>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public void AddHeaders(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
		    _client.Timeout = _conn.Timeout;
			headers["user-agent"] = _conn.UserAgent;
			try
			{
			    _client.PostContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, Name, headers, new Dictionary<string, string>());
				_reload_properties = true;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    AddHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    AddHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    AddHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Adds custom CDN headers
		/// </summary>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public void AddCdnHeaders(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
			if (!_conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
		    _client.Timeout = _conn.Timeout;
			headers["user-agent"] = _conn.UserAgent;
			try
			{
			    _client.PostContainer(_conn.UserCreds.CdnMangementUrl.ToString(), _conn.UserCreds.AuthToken.ToString(), Name, headers, new Dictionary<string, string>());
				_reload_properties = true;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    AddCdnHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    AddCdnHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new CDNNotEnabledException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    AddCdnHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
				_num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		public void MakePublic()
		{
			MakePublic(259200, false);
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		/// <param name='ttl'>
		/// Ttl.
		/// </param>
		public void MakePublic(long ttl)
		{
			MakePublic(ttl, false);
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		/// <param name='log_retention'>
		/// Log_retention.
		/// </param>
		public void MakePublic(bool log_retention)
		{
			MakePublic(259200, log_retention);
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		/// <param name="ttl">
		/// A <see cref="System.Int64"/>
		/// </param>
		/// <param name="log_retention">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void MakePublic(long ttl, bool log_retention)
		{
			if (ttl.Equals(null) || log_retention.Equals(null))
			{
				throw new ArgumentNullException();
			}
			if (!_conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
			if (ttl > 1577836800 || ttl < 900)
			{
				throw new TTLLengthException("TTL range must be 900 to 1577836800 seconds TTL: " + ttl.ToString(CultureInfo.InvariantCulture));
			}
		    _client.Timeout = _conn.Timeout;
			var headers = new Dictionary<string, string>
                {
                    {"user-agent", _conn.UserAgent},
                    {"x-ttl", ttl.ToString(CultureInfo.InvariantCulture)},
                    {"x-log-retention", log_retention.ToString(CultureInfo.InvariantCulture)}
                };
		    try
			{
			    _client.PutContainer(_conn.UserCreds.CdnMangementUrl.ToString(), _conn.UserCreds.AuthToken, Name, headers, new Dictionary<string, string>());
				_ttl = ttl;
				_cdn_log_retention = log_retention;
				_cdn_enabled = true;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    MakePublic(ttl, log_retention);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    MakePublic(ttl, log_retention);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    MakePublic(ttl, log_retention);
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
                _num_retries_attempted = 0;
				_ttl = ttl;
				_cdn_log_retention = log_retention;
			}
		}
		/// <summary>
		/// Makes the Container private.
		/// </summary>
		public void MakePrivate()
		{
			if (!_conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
		    _client.Timeout = _conn.Timeout;
			var headers = new Dictionary<string, string>
			                  {
			                      {"user-agent", _conn.UserAgent},
                                  {"x-cdn-enabled", "false"}
			                  };
		    try
			{
			    _client.PostContainer(_conn.UserCreds.CdnMangementUrl.ToString(), _conn.UserCreds.AuthToken.ToString(), Name, headers, new Dictionary<string, string>());
				_cdn_enabled = false;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    MakePrivate();
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    MakePrivate();
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new CDNNotEnabledException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    MakePrivate();
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString(CultureInfo.InvariantCulture));
					    }
			    }
		    }
			finally
			{
                _num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Sets the TTL of a Container
		/// </summary>
		/// <param name="ttl">
		/// A <see cref="System.Int64"/>
		/// </param>
		public void SetTTL(long ttl)
		{
			if (!_conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
			if (ttl > 1577836800 || ttl < 900)
			{
				throw new TTLLengthException("TTL range must be 900 to 1577836800 seconds TTL: " + ttl.ToString());
			}
			var headers = new Dictionary<string, string> {{"x-ttl",ttl.ToString(CultureInfo.InvariantCulture)}};
			AddCdnHeaders(headers);

		}
		/// <summary>
		/// Sets CDN log retention.
		/// </summary>
		/// <param name="log_retention">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void SetCdnLogRetention(bool log_retention)
		{
			if (log_retention.Equals(null))
			{
				throw new ArgumentNullException();
			}
			if (!_conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
            var headers = new Dictionary<string, string> {{"x-log-retention",log_retention.ToString(CultureInfo.InvariantCulture)}};
			AddCdnHeaders(headers);
		}
	}
	public interface Container
	{
		string Name { get; }
		Dictionary<string, string> Headers { get; }
		Dictionary<string, string> CdnHeaders { get; }
		Dictionary<string, string> Metadata { get; }
        int Retries { get; set; }
		long ObjectCount { get;}
		long BytesUsed { get; }
		long TTL { get; }
		Uri StorageUrl { get; }
		Uri CdnManagementUrl { get; }
		Uri CdnUri { get; }
		Uri CdnSslUri { get; }
		Uri CdnStreamingUri { get; }
		bool CdnEnabled { get; }
		bool CdnLogRetention { get; }
	    StorageObject CreateObject(string object_name);
		StorageObject GetObject(string object_name);
		List<StorageObject> GetObjects();
		List<StorageObject> GetObjects(bool full_listing);
		List<StorageObject> GetObjects(Dictionary<ObjectQuery, string> query);
		List<StorageObject> GetObjects(bool full_listing, Dictionary<ObjectQuery, string> query);
		List<Dictionary<string, string>> GetObjectList();
		List<Dictionary<string, string>> GetObjectList(bool full_listing);
		List<Dictionary<string, string>> GetObjectList(Dictionary<ObjectQuery, string> query);
		List<Dictionary<string, string>> GetObjectList(bool full_listing, Dictionary<ObjectQuery, string> query);
		void DeleteObject(string object_name);
		void AddMetadata(Dictionary<string, string> metadata);
		void AddHeaders(Dictionary<string, string> headers);
		void AddCdnHeaders(Dictionary<string, string> headers);
		void MakePublic();
		void MakePublic(long ttl);
		void MakePublic(bool log_retention);
		void MakePublic(long ttl, bool log_retention);
		void MakePrivate();		
	    void SetTTL(long ttl);
		void SetCdnLogRetention(bool log_retention);
	}
}