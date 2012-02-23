using System;
using System.Collections.Generic;
using Openstack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Container object that provides Container level functionality and information
	/// </summary>
	public class CF_Container : Container
	{
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
			this._client = client;
			this._conn = conn;
			this._name = container_name;
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
			this._client = new CF_Client();
			this._conn = conn;
			this._name = container_name;
		}
		private string _name = null;
		private int _retires = 0;
		private int _num_retries_attempted = 0;
		private bool _reload_properties = true;
		private bool _reload_cdn_properties = true;
		private bool _cdn_log_retention = false;
		private bool _cdn_enabled = false;
		private Uri _cdn_uri = null;
		private Uri _cdn_ssl_uri = null;
		private Uri _cdn_streaming_uri = null;
		private Dictionary<string, string> _metadata = null;
		private Dictionary<string, string> _headers = null;
		private Dictionary<string, string> _cdn_headers = null;
		private long _object_count = -1;
		private long _bytes_used = -1;
		private long _ttl = -1;
		private Client _client;
		private Connection _conn;
		/// <summary>
		/// Gets the Container Name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public override string Name
		{
			get { return this._name; }
		}
		/// <summary>
		/// Gets Raw headers.
		/// </summary>
		/// <value>
		/// The headers.
		/// </value>
		public override Dictionary<string, string> Headers
		{
			get { return this._reload_properties ? Common.ProcessMetadata(this._head_container().Headers)["headers"] : this._headers; }
		}
		/// <summary>
		/// Gets the cdn headers.
		/// </summary>
		/// <value>
		/// The cdn headers.
		/// </value>
		public override Dictionary<string, string> CdnHeaders
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._cdn_headers : null) : this._cdn_headers; }
		}
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public override Dictionary<string, string> Metadata
		{
			get { return this._reload_properties ? Common.ProcessMetadata(this._head_container().Headers)["metadata"] : this._metadata; }
		}
		/// <summary>
		/// Gets the object count.
		/// </summary>
		/// <value>
		/// The object count.
		/// </value>
		public override long ObjectCount
		{
			get { return this._reload_properties ? long.Parse(this._head_container().Headers["x-container-object-count"]) : this._object_count; }
		}
		/// <summary>
		/// Gets the bytes used.
		/// </summary>
		/// <value>
		/// The bytes used.
		/// </value>
		public override long BytesUsed
		{
			get { return this._reload_properties ? long.Parse(this._head_container().Headers["x-container-bytes-used"]) : this._bytes_used; }
		}
		/// <summary>
		/// Gets the TT.
		/// </summary>
		/// <value>
		/// The TT.
		/// </value>
		public override long TTL
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._ttl : -1) : this._ttl; }
		}
		/// <summary>
		/// Gets or sets the retries.
		/// </summary>
		/// <value>
		/// The retries.
		/// </value>
		public override int Retries
		{
			get { return this._retires; }
			set { this._retires = value; }
		}
		/// <summary>
		/// Gets the storage URL.
		/// </summary>
		/// <value>
		/// The storage URL.
		/// </value>
		public override Uri StorageUrl
		{
			get { return new Uri(this._conn.UserCreds.StorageUrl.ToString() + this.Name); }
		}
		/// <summary>
		/// Gets the cdn management URL.
		/// </summary>
		/// <value>
		/// The cdn management URL.
		/// </value>
		public override Uri CdnManagementUrl
		{
			get { return this._conn.HasCDN ? new Uri(this._conn.UserCreds.CdnMangementUrl.ToString() + this.Name) : null; }
		}
		/// <summary>
		/// Gets the cdn URI.
		/// </summary>
		/// <value>
		/// The cdn URI.
		/// </value>
		public override Uri CdnUri
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._cdn_uri : null) : this._cdn_uri; }
		}
		/// <summary>
		/// Gets the cdn ssl URI.
		/// </summary>
		/// <value>
		/// The cdn ssl URI.
		/// </value>
		public override Uri CdnSslUri
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._cdn_ssl_uri : null) : this._cdn_ssl_uri; }
		}
		/// <summary>
		/// Gets the cdn streaming URI.
		/// </summary>
		/// <value>
		/// The cdn streaming URI.
		/// </value>
		public override Uri CdnStreamingUri
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._cdn_streaming_uri : null) : this._cdn_streaming_uri; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Rackspace.Cloudfiles.CF_Container"/> cdn enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if cdn enabled; otherwise, <c>false</c>.
		/// </value>
		public override bool CdnEnabled
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._cdn_enabled : false) : this._cdn_enabled; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Rackspace.Cloudfiles.CF_Container"/> cdn log retention.
		/// </summary>
		/// <value>
		/// <c>true</c> if cdn log retention; otherwise, <c>false</c>.
		/// </value>
		public override bool CdnLogRetention
		{
			get { return this._reload_cdn_properties ? (this._head_cdn_container() ? this._cdn_log_retention : false) : this._cdn_log_retention; }
		}
		private ContainerResponse _head_container()
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
		    try
		    {
			    ContainerResponse res = this._client.HeadContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this.Name, headers, new Dictionary<string, string>());
				this._object_count = long.Parse(res.Headers["x-container-object-count"]);
				this._bytes_used = long.Parse(res.Headers["x-container-bytes-used"]);
				Dictionary<string, Dictionary<string, string>> processed_headers = Common.ProcessMetadata(res.Headers);
				this._headers = processed_headers["headers"];
				this._metadata = processed_headers["metadata"];
				this._reload_properties = false;
				return res;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this._head_container();
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    return this._head_container();
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					     throw new ContainerNotFoundException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this._head_container();
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
			}
		}
		private bool _head_cdn_container()
		{
		    try
			{
				if (this._conn.HasCDN)
				{
				    Dictionary<string, string> headers = new Dictionary<string, string>();
	        		headers["user-agent"] = this._conn.UserAgent;
			        this._client.Timeout = this._conn.Timeout;
			        ContainerResponse res = this._client.HeadContainer(this._conn.UserCreds.CdnMangementUrl.ToString(), this._conn.UserCreds.AuthToken, this.Name, headers, new Dictionary<string, string>());
				    this._cdn_uri = new Uri(res.Headers["x-cdn-uri"]);
				    this._cdn_ssl_uri = new Uri(res.Headers["x-cdn-ssl-uri"]);
				    this._cdn_streaming_uri = new Uri(res.Headers["x-cdn-streaming-uri"]);
					this._ttl = long.Parse(res.Headers["x-ttl"]);
					this._cdn_enabled = bool.Parse(res.Headers["x-cdn-enabled"]);
					this._cdn_log_retention = bool.Parse(res.Headers["x-log-retention"]);
					this._cdn_headers = res.Headers;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this._head_cdn_container();
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    return this._head_cdn_container();
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    return false;
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this._head_cdn_container();
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
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
	    public override StorageObject CreateObject(string object_name)
		{
			if (object_name == null)
			{
				throw new ArgumentNullException();
			}
			Common.ValidateObjectName(object_name);
			return new CF_Object(this._conn, this._name, object_name);
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
		public override StorageObject GetObject(string object_name)
		{
			if (object_name == null)
			{
				throw new ArgumentNullException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			Common.ValidateObjectName(object_name);
			try
			{
			    this._client.HeadObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this.Name, object_name, _headers, new Dictionary<string, string>());
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetObject(object_name);
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    return this.GetObject(object_name);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetObject(object_name);
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
			}
			return new CF_Object(this._conn, this._name, object_name);
		}
		/// <summary>
		/// Gets a list of StorageObject objects.
		/// </summary>
		/// <returns>
		/// The objects.
		/// </returns>
		public override List<StorageObject> GetObjects()
		{
			return this.GetObjects(false);
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
		public override List<StorageObject> GetObjects(bool full_listing)
		{
			return this.GetObjects(full_listing, new Dictionary<ObjectQuery, string>());
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
		public override List<StorageObject> GetObjects(Dictionary<ObjectQuery, string> query)
		{
			return this.GetObjects(false, query);
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
		public override List<StorageObject> GetObjects(bool full_listing, Dictionary<ObjectQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			Dictionary<string, string> queryp = new Dictionary<string, string>();
			foreach (KeyValuePair<ObjectQuery, string> q in query)
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
				List<StorageObject> objects = new List<StorageObject>();
				foreach (Dictionary<string, string> obj in this._client.GetContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._name, headers, queryp, full_listing).Objects)
				{
					objects.Add(new CF_Object(this._conn, this._name, obj["name"]));
				}
				return objects;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetObjects(full_listing, query);
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    return this.GetObjects(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetObjects(full_listing, query);
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Gets a list of Dictionaries that contain storage object info.
		/// </summary>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public override List<Dictionary<string, string>> GetObjectList()
		{
			return this.GetObjectList(false);
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
		public override List<Dictionary<string, string>> GetObjectList(bool full_listing)
		{
			return this.GetObjectList(full_listing, new Dictionary<ObjectQuery, string>());
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
		public override List<Dictionary<string, string>> GetObjectList(Dictionary<ObjectQuery, string> query)
		{
			return this.GetObjectList(false, query);
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
		public override List<Dictionary<string, string>> GetObjectList(bool full_listing, Dictionary<ObjectQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			Dictionary<string, string> queryp = new Dictionary<string, string>();
			foreach (KeyValuePair<ObjectQuery, string> q in query)
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
				return this._client.GetContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._name, headers, queryp, full_listing).Objects;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetObjectList(full_listing, query);
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    return this.GetObjectList(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetObjectList(full_listing, query);
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Deletes an Object.
		/// </summary>
		/// <param name="object_name">
		/// A <see cref="System.String"/>
		/// </param>
		public override void DeleteObject(string object_name)
		{
			Common.ValidateObjectName(object_name);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
			    this._client.DeleteObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this.Name, object_name, headers, new Dictionary<string, string>());
				this._reload_properties = true;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.DeleteObject(object_name);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    this.DeleteObject(object_name);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.DeleteObject(object_name);
                            break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
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
		public override void AddMetadata(Dictionary<string, string> metadata)
		{
			if (metadata.Equals(null))
			{
				throw new ArgumentNullException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
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
			this.AddHeaders(headers);
		}
		/// <summary>
		/// Adds Headers.
		/// </summary>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public override void AddHeaders(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
		    this._client.Timeout = this._conn.Timeout;
			headers["user-agent"] = this._conn.UserAgent;
			try
			{
			    this._client.PostContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken.ToString(), this.Name, headers, new Dictionary<string, string>());
				this._reload_properties = true;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.AddHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    this.AddHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new ObjectNotFoundException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.AddHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Adds custom CDN headers
		/// </summary>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public override void AddCdnHeaders(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
			if (!this._conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
		    this._client.Timeout = this._conn.Timeout;
			headers["user-agent"] = this._conn.UserAgent;
			try
			{
			    this._client.PostContainer(this._conn.UserCreds.CdnMangementUrl.ToString(), this._conn.UserCreds.AuthToken.ToString(), this.Name, headers, new Dictionary<string, string>());
				this._reload_properties = true;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.AddCdnHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    this.AddCdnHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new CDNNotEnabledException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.AddCdnHeaders(headers);
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
				this._num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		public override void MakePublic()
		{
			this.MakePublic(259200, false);
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		/// <param name='ttl'>
		/// Ttl.
		/// </param>
		public override void MakePublic(long ttl)
		{
			this.MakePublic(ttl, false);
		}
		/// <summary>
		/// Makes the container public.
		/// </summary>
		/// <param name='log_retention'>
		/// Log_retention.
		/// </param>
		public override void MakePublic(bool log_retention)
		{
			this.MakePublic(259200, log_retention);
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
		public override void MakePublic(long ttl, bool log_retention)
		{
			if (ttl.Equals(null) || log_retention.Equals(null))
			{
				throw new ArgumentNullException();
			}
			if (!this._conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
			if (ttl > 1577836800 || ttl < 900)
			{
				throw new TTLLengthException("TTL range must be 900 to 1577836800 seconds TTL: " + ttl.ToString());
			}
		    this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			headers["x-ttl"] = ttl.ToString();
			headers["x-log-retention"] = log_retention.ToString();
			try
			{
			    this._client.PutContainer(this._conn.UserCreds.CdnMangementUrl.ToString(), this._conn.UserCreds.AuthToken, this.Name, headers, new Dictionary<string, string>());
				this._ttl = ttl;
				this._cdn_log_retention = log_retention;
				this._cdn_enabled = true;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.MakePublic(ttl, log_retention);
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    this.MakePublic(ttl, log_retention);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.MakePublic(ttl, log_retention);
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
                this._num_retries_attempted = 0;
				this._ttl = ttl;
				this._cdn_log_retention = log_retention;
			}
		}
		/// <summary>
		/// Makes the Container private.
		/// </summary>
		public override void MakePrivate()
		{
			if (!this._conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
		    this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			headers["x-cdn-enabled"] = "false";
			try
			{
			    this._client.PostContainer(this._conn.UserCreds.CdnMangementUrl.ToString(), this._conn.UserCreds.AuthToken.ToString(), this.Name, headers, new Dictionary<string, string>());
				this._cdn_enabled = false;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.MakePrivate();
						    break;
					    }
					    else
					    {
						    throw new TimeoutException();
					    }
				    case 401:
					    if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this._conn.Authenticate();
						    this.MakePrivate();
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					    throw new CDNNotEnabledException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.MakePrivate();
						    break;
					    }
					    else
					    {
						    throw new CloudFilesException("Error: " + e.Status.ToString());
					    }
			    }
		    }
			finally
			{
                this._num_retries_attempted = 0;
			}
		}
		/// <summary>
		/// Sets the TTL of a Container
		/// </summary>
		/// <param name="ttl">
		/// A <see cref="System.Int64"/>
		/// </param>
		public override void SetTTL(long ttl)
		{
			if (!this._conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
			if (ttl > 1577836800 || ttl < 900)
			{
				throw new TTLLengthException("TTL range must be 900 to 1577836800 seconds TTL: " + ttl.ToString());
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["x-ttl"] = ttl.ToString();
			this.AddCdnHeaders(headers);

		}
		/// <summary>
		/// Sets CDN log retention.
		/// </summary>
		/// <param name="log_retention">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public override void SetCdnLogRetention(bool log_retention)
		{
			if (log_retention.Equals(null))
			{
				throw new ArgumentNullException();
			}
			if (!this._conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
            Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["x-log-retention"] = log_retention.ToString();
			this.AddCdnHeaders(headers);
		}
	}
	public abstract class Container
	{
		public abstract string Name { get; }
		public abstract Dictionary<string, string> Headers { get; }
		public abstract Dictionary<string, string> CdnHeaders { get; }
		public abstract Dictionary<string, string> Metadata { get; }
        public abstract int Retries { get; set; }
		public abstract long ObjectCount { get;}
		public abstract long BytesUsed { get; }
		public abstract long TTL { get; }
		public abstract Uri StorageUrl { get; }
		public abstract Uri CdnManagementUrl { get; }
		public abstract Uri CdnUri { get; }
		public abstract Uri CdnSslUri { get; }
		public abstract Uri CdnStreamingUri { get; }
		public abstract bool CdnEnabled { get; }
		public abstract bool CdnLogRetention { get; }
	    public abstract StorageObject CreateObject(string object_name);
		public abstract StorageObject GetObject(string object_name);
		public abstract List<StorageObject> GetObjects();
		public abstract List<StorageObject> GetObjects(bool full_listing);
		public abstract List<StorageObject> GetObjects(Dictionary<ObjectQuery, string> query);
		public abstract List<StorageObject> GetObjects(bool full_listing, Dictionary<ObjectQuery, string> query);
		public abstract List<Dictionary<string, string>> GetObjectList();
		public abstract List<Dictionary<string, string>> GetObjectList(bool full_listing);
		public abstract List<Dictionary<string, string>> GetObjectList(Dictionary<ObjectQuery, string> query);
		public abstract List<Dictionary<string, string>> GetObjectList(bool full_listing, Dictionary<ObjectQuery, string> query);
		public abstract void DeleteObject(string object_name);
		public abstract void AddMetadata(Dictionary<string, string> metadata);
		public abstract void AddHeaders(Dictionary<string, string> headers);
		public abstract void AddCdnHeaders(Dictionary<string, string> headers);
		public abstract void MakePublic();
		public abstract void MakePublic(long ttl);
		public abstract void MakePublic(bool log_retention);
		public abstract void MakePublic(long ttl, bool log_retention);
		public abstract void MakePrivate();		
	    public abstract void SetTTL(long ttl);
		public abstract void SetCdnLogRetention(bool log_retention);
	}
}