using System;
using System.Collections.Generic;
using Openstack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Account Object that makes account requests and provides Account information and status.
	/// </summary>
	public class CF_Account : Account
	{
		private Client _client;
		private Connection _conn;
		private int _retires = 0;
		private int _num_retries_attempted = 0;
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
			this._conn = conn;
			this._client = new CF_Client();
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
			this._conn = conn;
			this._client = client;
		}
		/// <summary>
		/// Gets a Connection object.
		/// </summary>
		/// <value>
		/// A Connection Object
		/// </value>
		public override Connection Conn
		{
			get { return this._conn; }
		}
		/// <summary>
		/// Gets the storage URL.
		/// </summary>
		/// <value>
		/// The storage URL.
		/// </value>
		public override Uri StorageUrl
		{
			get { return this._conn.UserCreds.StorageUrl; }
		}
		/// <summary>
		/// Gets the cdn management URL.
		/// </summary>
		/// <value>
		/// The cdn management URL.
		/// </value>
		public override Uri CdnManagementUrl
		{
			get { return this._conn.HasCDN ? this._conn.UserCreds.CdnMangementUrl : null; }
		}
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public override Dictionary<string, string> Metadata
		{
			get { return this._reload_properties ? Common.ProcessMetadata(this._head_account())["metadata"] : this._metadata; }
		}
		/// <summary>
		/// Gets the headers.
		/// </summary>
		/// <value>
		/// The headers.
		/// </value>
		public override Dictionary<string, string> Headers
		{
			get { return this._reload_properties ? Common.ProcessMetadata(this._head_account())["headers"] : this._headers; }
		}
		/// <summary>
		/// Gets the bytes used.
		/// </summary>
		/// <value>
		/// The bytes used.
		/// </value>
		public override long BytesUsed
		{
			get { return this._reload_properties ? long.Parse(this._head_account()["x-account-bytes-used"]) : long.Parse(this._headers["x-account-bytes-used"]); }
		}
		/// <summary>
		/// Gets the container count.
		/// </summary>
		/// <value>
		/// The container count.
		/// </value>
		public override long ContainerCount
		{			
		    get { return this._reload_properties ? long.Parse(this._head_account()["x-account-container-count"]) : long.Parse(this._headers["x-account-container-count"]); }
		}
		/// <summary>
		/// Gets the object count.
		/// </summary>
		/// <value>
		/// The object count.
		/// </value>
		public override long ObjectCount
		{			
		    get { return this._reload_properties ? long.Parse(this._head_account()["x-account-object-count"]) : long.Parse(this._headers["x-account-object-count"]); }
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
		private Dictionary<string, string> _head_account()
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
			    AccountResponse res = this._client.HeadAccount(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, headers, new Dictionary<string, string>());
				Dictionary<string, Dictionary<string, string>> processed_headers = Common.ProcessMetadata(res.Headers);
				this._metadata = processed_headers["metadata"];
				this._headers = processed_headers["headers"];
				this._reload_properties = false;
				return res.Headers;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this._head_account();
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
						    return this._head_account();
					    }
					    else
					    {
						    throw new AuthenticationFailedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this._head_account();
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
		/// Creates the container.
		/// </summary>
		/// <returns>
		/// The container.
		/// </returns>
		/// <param name='container_name'>
		/// Container_name.
		/// </param>
		public override Container CreateContainer(string container_name)
		{
			return this.CreateContainer(container_name, new Dictionary<string, string>());
		}
		/// <summary>
		/// Creates a container.
		/// </summary>
		/// <param name="container_name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		/// <returns>
		/// A <see cref="Container"/>
		/// </returns>
		public override Container CreateContainer(string container_name, Dictionary<string, string> headers)
		{
			if (String.IsNullOrEmpty(container_name) || headers == null)
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
			    this._client.PutContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, container_name, headers, new Dictionary<string, string>());
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.CreateContainer(container_name, headers);
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
						    return this.CreateContainer(container_name, headers);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.CreateContainer(container_name, headers);
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
			return new CF_Container(this._conn, container_name);
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
		public override Container GetContainer(string container_name)
		{
			if (String.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
			    this._client.HeadContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, container_name, headers, new Dictionary<string, string>());
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.CreateContainer(container_name, headers);
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
						    return this.CreateContainer(container_name, headers);
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
						    return this.CreateContainer(container_name, headers);
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
			return new CF_Container(this._conn, container_name);
		}
		/// <summary>
		/// Gets a list of container objects
		/// </summary>
		/// <returns>
		/// A <see cref="List<Container>"/>
		/// </returns>
		public override List<Container> GetContainers()
		{
			return this.GetContainers(false);
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
		public override List<Container> GetContainers(bool full_listing)
		{
			Dictionary<ContainerQuery, string> query = new Dictionary<ContainerQuery, string>();
			return this.GetContainers(full_listing, query);
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
		public override List<Container> GetContainers(Dictionary<ContainerQuery, string> query)
		{
			return this.GetContainers(false, query);
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
		public override List<Container> GetContainers(bool full_listing, Dictionary<ContainerQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
            this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> queryp = new Dictionary<string, string>();
			foreach (KeyValuePair<ContainerQuery, string> q in query)
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
				List<Container> containers = new List<Container>();
				foreach (Dictionary<string, string> cont in this._client.GetAccount(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, headers, queryp, full_listing).Containers)
				{
					containers.Add(new CF_Container(this._conn, cont["name"]));
				}
				return containers;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetContainers(full_listing, query);
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
						    return this.GetContainers(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetContainers(full_listing, query);
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
		/// Gets a list of containers and associated information
		/// </summary>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public override List<Dictionary<string, string>> GetContainerList()
		{
			return this.GetContainerList(false);
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
		public override List<Dictionary<string, string>> GetContainerList(bool full_listing)
		{
			Dictionary<ContainerQuery, string> query = new Dictionary<ContainerQuery, string>();
			return this.GetContainerList(full_listing, query);
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
		public override List<Dictionary<string, string>> GetContainerList(Dictionary<ContainerQuery, string> query)
		{
			return this.GetContainerList(false, query);
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
		public override List<Dictionary<string, string>> GetContainerList(bool full_listing, Dictionary<ContainerQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
            this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> queryp = new Dictionary<string, string>();
			foreach (KeyValuePair<ContainerQuery, string> q in query)
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
				return this._client.GetAccount(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, headers, queryp, full_listing).Containers;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetContainerList(full_listing, query);
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
						    return this.GetContainerList(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetContainerList(full_listing, query);
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
		/// Gets a list of public containers and associated information
		/// </summary>
		/// <returns>
		/// A <see cref="List<Dictionary<System.String, System.String>>"/>
		/// </returns>
		public override List<Dictionary<string, string>> GetPublicContainerList()
		{
			return this.GetPublicContainerList(false);
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
		public override List<Dictionary<string, string>> GetPublicContainerList(bool full_listing)
		{
			Dictionary<PublicContainerQuery, string> query = new Dictionary<PublicContainerQuery, string>();
			return this.GetPublicContainerList(full_listing, query);
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
		public override List<Dictionary<string, string>> GetPublicContainerList(Dictionary<PublicContainerQuery, string> query)
		{
			return this.GetPublicContainerList(false, query);
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
		public override List<Dictionary<string, string>> GetPublicContainerList(bool full_listing, Dictionary<PublicContainerQuery, string> query)
		{
			if (query == null)
			{
				throw new ArgumentNullException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
            this._client.Timeout = this._conn.Timeout;
			Dictionary<string, string> queryp = new Dictionary<string, string>();
			foreach (KeyValuePair<PublicContainerQuery, string> q in query)
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
				return this._client.GetCDNAccount(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, headers, queryp, full_listing).Containers;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetPublicContainerList(full_listing, query);
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
						    return this.GetPublicContainerList(full_listing, query);
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.GetPublicContainerList(full_listing, query);
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
		/// Deletes a Container
		/// </summary>
		/// <param name="container_name">
		/// A <see cref="System.String"/>
		/// </param>
		public override void DeleteContainer(string container_name)
		{
			if (String.IsNullOrEmpty(container_name))
			{
				throw new ArgumentNullException();
			}
			Common.ValidateContainerName(container_name);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
			    this._client.DeleteContainer(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, container_name, headers, new Dictionary<string, string>());
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.DeleteContainer(container_name);
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
						    this.DeleteContainer(container_name);
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
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.DeleteContainer(container_name);
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
		/// Updates Account Metadata.
		/// </summary>
		/// <param name='metadata'>
		/// Metadata.
		/// </param>
		/// <exception cref='ArgumentNullException'>
		/// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
		/// </exception>
		public override void UpdateMetadata(Dictionary<string, string> metadata)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException();
			}			
			Dictionary<string, string> headers = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> m in metadata)
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
            this.UpdateHeaders(headers);
		}
		/// <summary>
		/// Updates Account Headers
		/// </summary>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public override void UpdateHeaders(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException();
			}
		    this._client.Timeout = this._conn.Timeout;
			headers["user-agent"] = this._conn.UserAgent;
			try
			{
			    this._client.PostAccount(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, headers, new Dictionary<string, string>());
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
						    this.UpdateHeaders(headers);
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
						    this.UpdateHeaders(headers);
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
						    this.UpdateHeaders(headers);
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
	}
	public abstract class Account
	{
		public abstract Connection Conn { get; }
		public abstract Uri StorageUrl { get; }
		public abstract Uri CdnManagementUrl { get; }
		public abstract Dictionary<string, string> Metadata { get; }
		public abstract Dictionary<string, string> Headers { get; }
		public abstract int Retries { get; set; }
		public abstract long BytesUsed { get; }
		public abstract long ContainerCount { get; }
		public abstract long ObjectCount { get; }
		public abstract Container CreateContainer(string container_name);
		public abstract Container CreateContainer(string container_name, Dictionary<string, string> headers);
		public abstract Container GetContainer(string container_name);
		public abstract List<Container> GetContainers();
		public abstract List<Container> GetContainers(bool full_listing);
		public abstract List<Container> GetContainers(Dictionary<ContainerQuery, string> query);
		public abstract List<Container> GetContainers(bool full_listing, Dictionary<ContainerQuery, string> query);
		public abstract List<Dictionary<string, string>> GetContainerList();
		public abstract List<Dictionary<string, string>> GetContainerList(bool full_listing);
		public abstract List<Dictionary<string, string>> GetContainerList(Dictionary<ContainerQuery, string> query);
		public abstract List<Dictionary<string, string>> GetContainerList(bool full_listing, Dictionary<ContainerQuery, string> query);
		public abstract List<Dictionary<string, string>> GetPublicContainerList();
		public abstract List<Dictionary<string, string>> GetPublicContainerList(bool full_listing);
		public abstract List<Dictionary<string, string>> GetPublicContainerList(Dictionary<PublicContainerQuery, string> query);
		public abstract List<Dictionary<string, string>> GetPublicContainerList(bool full_listing, Dictionary<PublicContainerQuery, string> query);
		public abstract void DeleteContainer(string container_name);
		public abstract void UpdateMetadata(Dictionary<string, string> metadata);
		public abstract void UpdateHeaders(Dictionary<string, string> headers);
	}
}