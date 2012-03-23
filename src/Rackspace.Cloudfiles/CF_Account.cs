using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenStack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Account Object that makes account requests and provides Account information and status.
	/// </summary>
	public class CF_Account : Account
	{
		private readonly Client _client;
		private readonly Connection _conn;
		private int _retires;
		private int _num_retries_attempted;
		private bool _reload_properties = true;
		private Dictionary<string, string> _metadata = new Dictionary<string, string>();
		private Dictionary<string, string> _headers = new Dictionary<string, string>();
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Account"/> class.
		/// </summary>
		/// <param name='conn'>
		/// Pass a valid Connection object
		/// </param>
		public CF_Account (Connection conn)
		{
			_conn = conn;
			_client = new CF_Client();
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Account"/> class.
		/// </summary>
		/// <param name='conn'>
		/// Pass a valid Connection object
		/// </param>
		/// <param name='client'>
		/// Pass a valid HTTP Client object
		/// </param>
		public CF_Account(Connection conn, Client client)
		{
			_conn = conn;
			_client = client;
		}
		/// <summary>
		/// Gets a Connection object.
		/// </summary>
		/// <value>
		/// A Connection Object
		/// </value>
		public Connection Conn
		{
			get { return _conn; }
		}
		/// <summary>
		/// Gets the storage URL.
		/// </summary>
		/// <value>
		/// The storage URL.
		/// </value>
		public Uri StorageUrl
		{
			get { return _conn.UserCreds.StorageUrl; }
		}
		/// <summary>
		/// Gets the cdn management URL.
		/// </summary>
		/// <value>
		/// The cdn management URL.
		/// </value>
		public Uri CdnManagementUrl
		{
			get { return _conn.HasCDN ? _conn.UserCreds.CdnMangementUrl : null; }
		}
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public Dictionary<string, string> Metadata
		{
			get { return _reload_properties ? Common.ProcessMetadata(_head_account())["metadata"] : _metadata; }
		}
		/// <summary>
		/// Gets the headers.
		/// </summary>
		/// <value>
		/// The headers.
		/// </value>
		public Dictionary<string, string> Headers
		{
			get { return _reload_properties ? Common.ProcessMetadata(_head_account())["headers"] : _headers; }
		}
		/// <summary>
		/// Gets the bytes used.
		/// </summary>
		/// <value>
		/// The bytes used.
		/// </value>
		public long BytesUsed
		{
			get { return _reload_properties ? long.Parse(_head_account()["x-account-bytes-used"]) : long.Parse(_headers["x-account-bytes-used"]); }
		}
		/// <summary>
		/// Gets the container count.
		/// </summary>
		/// <value>
		/// The container count.
		/// </value>
		public long ContainerCount
		{			
		    get { return _reload_properties ? long.Parse(_head_account()["x-account-container-count"]) : long.Parse(_headers["x-account-container-count"]); }
		}
		/// <summary>
		/// Gets the object count.
		/// </summary>
		/// <value>
		/// The object count.
		/// </value>
		public long ObjectCount
		{			
		    get { return _reload_properties ? long.Parse(_head_account()["x-account-object-count"]) : long.Parse(_headers["x-account-object-count"]); }
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
		private Dictionary<string, string> _head_account()
		{
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
			_client.Timeout = _conn.Timeout;
			try
			{
			    var res = _client.HeadAccount(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, headers, new Dictionary<string, string>());
				var processed_headers = Common.ProcessMetadata(res.Headers);
				_metadata = processed_headers["metadata"];
				_headers = processed_headers["headers"];
				_reload_properties = false;
				return res.Headers;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return _head_account();
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
						    return _head_account();
					    }
					    else
					    {
						    throw new AuthenticationFailedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return _head_account();
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
		/// Creates the container.
		/// </summary>
		/// <returns>
		/// The container.
		/// </returns>
		/// <param name='container_name'>
		/// Container_name.
		/// </param>
		public Container CreateContainer(string container_name)
		{
			return CreateContainer(container_name, new Dictionary<string, string>());
		}
		/// <summary>
		/// Creates a container.
		/// </summary>
		/// <param name="container_name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="headers">
		/// A <see cref="Dictionary{System.String, System.String}"/>
		/// </param>
		/// <returns>
		/// A <see cref="Container"/>
		/// </returns>
		public Container CreateContainer(string container_name, Dictionary<string, string> headers)
		{
			if (String.IsNullOrEmpty(container_name) || headers == null)
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			headers["user-agent"] = _conn.UserAgent;
			_client.Timeout = _conn.Timeout;
			try
			{
			    _client.PutContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return CreateContainer(container_name, headers);
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
						    return CreateContainer(container_name, headers);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return CreateContainer(container_name, headers);
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
			return new CF_Container(_conn, container_name);
		}
		/// <summary>
		/// Gets a Container.
		/// </summary>
		/// <param name="container_name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="Container"/>
		/// </returns>
		public Container GetContainer(string container_name)
		{
			if (String.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
			_client.Timeout = _conn.Timeout;
			try
			{
			    _client.HeadContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return CreateContainer(container_name, headers);
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
						    return CreateContainer(container_name, headers);
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
						    return CreateContainer(container_name, headers);
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
			return new CF_Container(_conn, container_name);
		}
		/// <summary>
		/// Gets a list of container objects
		/// </summary>
		/// <returns>
		/// A <see cref="List<Container>"/>
		/// </returns>
		public List<Container> GetContainers()
		{
			return GetContainers(false);
		}
		/// <summary>
		/// Gets a list of container objects
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Container>"/>
		/// </returns>
		public List<Container> GetContainers(bool full_listing)
		{
			return GetContainers(full_listing, new Dictionary<ContainerQuery, string>());
		}
		/// <summary>
		/// Gets a list of container objects
		/// </summary>
		/// <param name="query">
		/// A <see cref="Dictionary<ContainerQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Container>"/>
		/// </returns>
		public List<Container> GetContainers(Dictionary<ContainerQuery, string> query)
		{
			return GetContainers(false, query);
		}
		/// <summary>
		/// Gets a list of container objects
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="query">
		/// A <see cref="Dictionary<ContainerQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Container>"/>
		/// </returns>
		public List<Container> GetContainers(bool full_listing, Dictionary<ContainerQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
            _client.Timeout = _conn.Timeout;
			var queryp = new Dictionary<string, string>();
			foreach (var q in query)
			{
				switch (q.Key)
				{
				    case ContainerQuery.Limit:
					    queryp.Add("limit", q.Value);
					    break;
				    case ContainerQuery.Marker:
					    queryp.Add("marker", q.Value);
			            break;
				    case ContainerQuery.Prefix:
					    queryp.Add("prefix", q.Value);
					    break;
				}
			}
			try
			{
			    return _client.GetAccount(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, headers, queryp, full_listing).Containers.Select(cont => new CF_Container(_conn, cont["name"])).Cast<Container>().ToList();
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetContainers(full_listing, query);
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
						    return GetContainers(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetContainers(full_listing, query);
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
		/// Gets a list of containers and associated information
		/// </summary>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetContainerList()
		{
			return GetContainerList(false);
		}
		/// <summary>
		/// Gets a list of containers and associated information 
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetContainerList(bool full_listing)
		{
			return GetContainerList(full_listing, new Dictionary<ContainerQuery, string>());
		}
		/// <summary>
		/// Gets a list of containers and associated information
		/// </summary>
		/// <param name="query">
		/// A <see cref="Dictionary<ContainerQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetContainerList(Dictionary<ContainerQuery, string> query)
		{
			return GetContainerList(false, query);
		}
		/// <summary>
		/// Gets a list of containers and associated information
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="query">
		/// A <see cref="Dictionary<ContainerQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetContainerList(bool full_listing, Dictionary<ContainerQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
            _client.Timeout = _conn.Timeout;
			var queryp = new Dictionary<string, string>();
			foreach (var q in query)
			{
				switch (q.Key)
				{
				    case ContainerQuery.Limit:
					    queryp.Add("limit", q.Value);
					    break;
				    case ContainerQuery.Marker:
					    queryp.Add("marker", q.Value);
			            break;
				    case ContainerQuery.Prefix:
					    queryp.Add("prefix", q.Value);
					    break;
				}
			}
			try
			{
				return _client.GetAccount(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, headers, queryp, full_listing).Containers;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetContainerList(full_listing, query);
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
						    return GetContainerList(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetContainerList(full_listing, query);
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
		/// Gets a list of public containers and associated information
		/// </summary>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetPublicContainerList()
		{
			return GetPublicContainerList(false);
		}
		/// <summary>
		/// Gets a list of public containers and associated information
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetPublicContainerList(bool full_listing)
		{
			return GetPublicContainerList(full_listing, new Dictionary<PublicContainerQuery, string>());
		}
		/// <summary>
		/// Gets a list of public containers and associated information
		/// </summary>
		/// <param name="query">
		/// A <see cref="Dictionary<PublicContainerQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetPublicContainerList(Dictionary<PublicContainerQuery, string> query)
		{
			return GetPublicContainerList(false, query);
		}
		/// <summary>
		/// Gets a list of public containers and associated infomation
		/// </summary>
		/// <param name="full_listing">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="query">
		/// A <see cref="Dictionary<PublicContainerQuery, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public List<Dictionary<string, string>> GetPublicContainerList(bool full_listing, Dictionary<PublicContainerQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
            _client.Timeout = _conn.Timeout;
			var queryp = new Dictionary<string, string>();
			foreach (var q in query)
			{
				switch (q.Key)
				{
				    case PublicContainerQuery.Limit:
					    queryp.Add("limit", q.Value);
					    break;
				    case PublicContainerQuery.Marker:
					    queryp.Add("marker", q.Value);
			            break;
				    case PublicContainerQuery.EnbaledOnly:
					    queryp.Add("enabled_only", q.Value);
					    break;
				}
			}
			try
			{
				return _client.GetCDNAccount(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, headers, queryp, full_listing).Containers;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetPublicContainerList(full_listing, query);
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
						    return GetPublicContainerList(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return GetPublicContainerList(full_listing, query);
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
		/// Deletes a Container
		/// </summary>
		/// <param name="container_name">
		/// A <see cref="System.String"/>
		/// </param>
		public void DeleteContainer(string container_name)
		{
			if (String.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
            var headers = new Dictionary<string, string> { { "user-agent", _conn.UserAgent } };
			_client.Timeout = _conn.Timeout;
			try
			{
			    _client.DeleteContainer(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, container_name, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    DeleteContainer(container_name);
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
						    DeleteContainer(container_name);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 404:
					     throw new ContainerNotFoundException();
				    case 409:
					     throw new ContainerNotEmptyException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    DeleteContainer(container_name);
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
		/// Updates Account Metadata.
		/// </summary>
		/// <param name='metadata'>
		/// Metadata.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
		public void UpdateMetadata(Dictionary<string, string> metadata)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException();
			}			
			var headers = new Dictionary<string, string>();
			foreach (var m in metadata)
			{
				if (m.Key.Contains("x-account-meta-"))
				{
					headers.Add(m.Key, m.Value);
				}
				else
				{
					headers.Add("x-account-meta-" + m.Key, m.Value);
				}
			}
            UpdateHeaders(headers);
		}
		/// <summary>
		/// Updates Account Headers
		/// </summary>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public void UpdateHeaders(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
		    _client.Timeout = _conn.Timeout;
			headers["user-agent"] = _conn.UserAgent;
			try
			{
			    _client.PostAccount(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, headers, new Dictionary<string, string>());
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
						    UpdateHeaders(headers);
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
						    UpdateHeaders(headers);
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
						    UpdateHeaders(headers);
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
	}
	public interface Account
	{
		Connection Conn { get; }
		Uri StorageUrl { get; }
		Uri CdnManagementUrl { get; }
		Dictionary<string, string> Metadata { get; }
		Dictionary<string, string> Headers { get; }
		int Retries { get; set; }
		long BytesUsed { get; }
		long ContainerCount { get; }
		long ObjectCount { get; }
		Container CreateContainer(string container_name);
		Container CreateContainer(string container_name, Dictionary<string, string> headers);
		Container GetContainer(string container_name);
		List<Container> GetContainers();
		List<Container> GetContainers(bool full_listing);
		List<Container> GetContainers(Dictionary<ContainerQuery, string> query);
		List<Container> GetContainers(bool full_listing, Dictionary<ContainerQuery, string> query);
		List<Dictionary<string, string>> GetContainerList();
		List<Dictionary<string, string>> GetContainerList(bool full_listing);
		List<Dictionary<string, string>> GetContainerList(Dictionary<ContainerQuery, string> query);
		List<Dictionary<string, string>> GetContainerList(bool full_listing, Dictionary<ContainerQuery, string> query);
		List<Dictionary<string, string>> GetPublicContainerList();
		List<Dictionary<string, string>> GetPublicContainerList(bool full_listing);
		List<Dictionary<string, string>> GetPublicContainerList(Dictionary<PublicContainerQuery, string> query);
		List<Dictionary<string, string>> GetPublicContainerList(bool full_listing, Dictionary<PublicContainerQuery, string> query);
		void DeleteContainer(string container_name);
		void UpdateMetadata(Dictionary<string, string> metadata);
		void UpdateHeaders(Dictionary<string, string> headers);
	}
}