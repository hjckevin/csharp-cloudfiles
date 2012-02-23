using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Openstack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Object object used to represent a Cloud Files Object
	/// </summary>
	public class CF_Object : StorageObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Object"/> class.
		/// </summary>
		/// <param name='conn'>
		/// Conn.
		/// </param>
		/// <param name='container_name'>
		/// Container_name.
		/// </param>
		/// <param name='object_name'>
		/// Object_name.
		/// </param>
		public CF_Object(Connection conn, string container_name, string object_name)
		{
			Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			this._client = new CF_Client();
			this._conn = conn;
			this._cont = new CF_Container(conn, container_name);
			this._name = object_name;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Object"/> class.
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
		/// <param name='object_name'>
		/// Object_name.
		/// </param>
		public CF_Object(Connection conn, Client client, string container_name, string object_name)
		{
			Common.ValidateContainerName(container_name);
			Common.ValidateObjectName(object_name);
			this._client = client;
			this._conn = conn;
			this._cont = new CF_Container(conn, container_name);
			this._name = object_name;	
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Object"/> class.
		/// </summary>
		/// <param name='conn'>
		/// Conn.
		/// </param>
		/// <param name='container'>
		/// Container.
		/// </param>
		/// <param name='client'>
		/// Client.
		/// </param>
		/// <param name='object_name'>
		/// Object_name.
		/// </param>
		public CF_Object(Connection conn, Container container, Client client, string object_name)
		{
			Common.ValidateObjectName(object_name);
			this._client = client;
			this._conn = conn;
			this._cont = container;
			this._name = object_name;
		}
		private Client _client;
		private Container _cont;		
		private int _retires = 0;
		private int _num_retries_attempted = 0;
		private Connection _conn;
		private bool _reload_properties = true;
		private string _name = null;
		private string _etag = null;
		private string _content_type = null;
		private long _content_length = -1;
		private Dictionary<string, string> _headers = null;
		private Dictionary<string, string> _metadata = null;
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
		/// Gets or sets the size of the chunk.
		/// </summary>
		/// <value>
		/// The size of the chunk.
		/// </value>
		public override int ChunkSize
		{
			get { return this._client.ChunkSize; }
			set { this._client.ChunkSize = value; }
		}
		/// <summary>
		/// Gets the Object name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public override string Name
		{
			get { return this._name; }
		}
		/// <summary>
		/// Gets the etag.
		/// </summary>
		/// <value>
		/// The etag.
		/// </value>
		public override string Etag
		{
			get { return this._reload_properties ? this._head_object().Headers["etag"] : this._etag; }
		}
		/// <summary>
		/// Gets the Content type.
		/// </summary>
		/// <value>
		/// The type of the content.
		/// </value>
		public override string ContentType
		{
			get { return this._reload_properties ? this._head_object().Headers["content-type"] : this._content_type; }
		}
		/// <summary>
		/// Gets the content length.
		/// </summary>
		/// <value>
		/// The length of the content.
		/// </value>
		public override long  ContentLength
		{
			get { return this._reload_properties ? long.Parse(this._head_object().Headers["content-length"]) : this._content_length; }
		}
		/// <summary>
		/// Gets the headers.
		/// </summary>
		/// <value>
		/// The headers.
		/// </value>
		public override Dictionary<string, string> Headers
		{
			get { return this._reload_properties ? Common.ProcessMetadata(this._head_object().Headers)["headers"] : this._headers; }
		}
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public override Dictionary<string, string> Metadata
		{
			get { return this._reload_properties ? Common.ProcessMetadata(this._head_object().Headers)["metadata"] : this._metadata; }
		}
		/// <summary>
		/// Gets the storage URL.
		/// </summary>
		/// <value>
		/// The storage URL.
		/// </value>
		public override Uri StorageUrl
		{
			get { return new Uri(this._conn.UserCreds.StorageUrl.ToString() + this._cont.Name + "/" + this.Name); }
		}
		/// <summary>
		/// Gets the cdn management URL.
		/// </summary>
		/// <value>
		/// The cdn management URL.
		/// </value>
		public override Uri CdnManagementUrl
		{
			get { return this._conn.UserCreds.CdnMangementUrl != null ? new Uri(this._conn.UserCreds.CdnMangementUrl.ToString() + this._cont.Name + "/" + this.Name) : null; }
		}
		/// <summary>
		/// Gets the cdn URI.
		/// </summary>
		/// <value>
		/// The cdn URI.
		/// </value>
		public override Uri CdnUri
		{
			get { return this._cont.CdnUri != null ? new Uri(this._cont.CdnUri.ToString() + this.Name) : null; }
		}
		/// <summary>
		/// Gets the cdn ssl URI.
		/// </summary>
		/// <value>
		/// The cdn ssl URI.
		/// </value>
		public override Uri CdnSslUri
		{
			get { return this._cont.CdnSslUri != null ? new Uri(this._cont.CdnSslUri.ToString() + this.Name) : null; }
		}
		/// <summary>
		/// Gets the cdn streaming URI.
		/// </summary>
		/// <value>
		/// The cdn streaming URI.
		/// </value>
		public override Uri CdnStreamingUri
		{
			get { return this._cont.CdnStreamingUri != null ? new Uri(this._cont.CdnStreamingUri.ToString() + this.Name) : null; }
		}
		private ObjectResponse _head_object()
		{
		    Dictionary<string, string> headers = new Dictionary<string, string>();
		    headers["user-agent"] = this._conn.UserAgent;
		    this._client.Timeout = this._conn.Timeout;
		    try
		    {
			    ObjectResponse res = this._client.HeadObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._cont.Name, this.Name, headers, new Dictionary<string, string>());
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
						    return this._head_object();
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
						    return this._head_object();
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
						    return this._head_object();
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
		/// Saves to file.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public override void SaveToFile(string path)
		{
			this.SaveToFile(path, false);
		}
		/// <summary>
		/// Saves to file.
		/// </summary>
		/// <param name="path">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="verify_etag">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public override void SaveToFile(string path, bool verify_etag)
		{
			if (String.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException();
			}
            Dictionary<string, string> headers = new Dictionary<string, string>();
			headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			Stream save_to = File.OpenWrite(path);
			byte[] buffer = new byte[this.ChunkSize];
			int read = 0;
		    try
		    {
			    ObjectResponse res = this._client.GetObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._cont.Name, this.Name, headers, new Dictionary<string, string>());
				while ((read = res.ObjectData.Read(buffer, 0, buffer.Length)) > 0)
				{
					save_to.Write(buffer, 0, read);
				}
				save_to.Close();
				res.ObjectData.Close();
				if (verify_etag)
				{
					save_to = File.OpenRead(path);
					MD5 md5 = MD5.Create();
					md5.ComputeHash(save_to);
					StringBuilder sbuilder = new StringBuilder();
				    byte[] hash = md5.Hash;
				    foreach(Byte b in hash)
				    {
					    sbuilder.Append(b.ToString("x2").ToLower());
				    }
					string converted_md5 = sbuilder.ToString();
					if (converted_md5 != res.Headers["etag"].ToLower())
					{
						File.Delete(path);
						throw new InvalidETagException();
					}
				}
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted <= this._retires)
					    {
						    ++ this._num_retries_attempted;
						    File.Delete(path);
						    this.SaveToFile(path, verify_etag);
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
						    File.Delete(path);
						    this.SaveToFile(path, verify_etag);
						    break;
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
						    File.Delete(path);
						    this.SaveToFile(path, verify_etag);
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
		/// Writes from file.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public override void WriteFromFile(string path)
		{
			this.WriteFromFile(path, new Dictionary<string, string>());
		}
		/// <summary>
		/// Writes from file.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='headers'>
		/// Headers.
		/// </param>
		public override void WriteFromFile(string path, Dictionary<string, string> headers)
		{
			Stream stream = System.IO.File.OpenRead(path);
			this.Write(stream, headers);
		}
		/// <summary>
		/// Write the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		public override void Write(Stream data)
		{
			this.Write(data, new Dictionary<string, string>());
		}
		/// <summary>
		/// Writes an object stream.
		/// </summary>
		/// <param name="data">
		/// A <see cref="Stream"/>
		/// </param>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param> <summary>
		/// 
		/// </summary>
		/// <param name="data">
		/// A <see cref="Stream"/>
		/// </param>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public override void Write(Stream data, Dictionary<string, string> headers)
		{   
			if (data == null)
			{
				throw new ArgumentNullException();
			}
		    headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
		    try
		    {
			    this._client.PutObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._cont.Name, this.Name, data, headers, new Dictionary<string, string>());
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.Write(data, headers);
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
						    this.Write(data, headers);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 422:
					    throw new InvalidETagException();
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.Write(data, headers);
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
		/// Reads from the object.
		/// </summary>
		/// <returns>
		/// A <see cref="Stream"/>
		/// </returns>
		public override Stream Read()
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
		    headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
				return this._client.GetObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._cont.Name, this.Name, headers, new Dictionary<string, string>()).ObjectData;
			}
			catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.Read();
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
						    return this.Read();
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    return this.Read();
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
		/// Syncs Metadata
		/// </summary>
		/// <param name="metadata">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public override void SyncMetadata(Dictionary<string, string> metadata)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> m in metadata)
			{
				if (m.Key.Contains("x-object-meta-"))
				{
					headers.Add(m.Key, m.Value);
				}
				else
				{
					headers.Add("x-object-meta-" + m.Key, m.Value);
				}
			}
		    headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
			    this._client.PostObject(this._conn.UserCreds.StorageUrl.ToString(), this._conn.UserCreds.AuthToken, this._cont.Name, this._name, headers, new Dictionary<string, string>());
			}
		    catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.SyncMetadata(metadata);
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
						    this.SyncMetadata(metadata);
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
						    this.SyncMetadata(metadata);
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
		/// Purges object from cdn.
		/// </summary>
		public override void PurgeFromCdn()
		{
			this.PurgeFromCdn("");
		}
		/// <summary>
		/// Purges object from cdn.
		/// </summary>
		/// <param name='emails'>
		/// Emails.
		/// </param>
		public override void PurgeFromCdn(string[] emails)
		{
			int length = 0;
			string femail = "";
			foreach (string email in emails)
			{
				length ++;
				femail += email;
				if (emails.Length < length)
				{
					femail += ",";
				}
			}
			this.PurgeFromCdn(femail);
		}
		/// <summary>
		/// Purges Object from CDN
		/// </summary>
		/// <param name="email">
		/// A <see cref="System.String"/>
		/// </param>
		public override void PurgeFromCdn(string email)
		{
			if (email == null)
			{
				throw new ArgumentNullException();
			}
			if (!this._conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			if (email.Length > 0)
			{
			    headers["x-purge-email"] = email;
			}
		    headers["user-agent"] = this._conn.UserAgent;
			this._client.Timeout = this._conn.Timeout;
			try
			{
				this._client.DeleteObject(this._conn.UserCreds.CdnMangementUrl.ToString(), this._conn.UserCreds.AuthToken, this._cont.Name, this._name, headers, new Dictionary<string, string>());
			}
		    catch (Openstack.Swift.ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (this._num_retries_attempted < this._retires)
					    {
						    ++ this._num_retries_attempted;
						    this.PurgeFromCdn(email);
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
						    this.PurgeFromCdn(email);
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
						    this.PurgeFromCdn(email);
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
	public abstract class StorageObject
	{
		public abstract int Retries { get; set; }
		public abstract int ChunkSize  { get; set; }
		public abstract string Name  { get; }
		public abstract string Etag { get; }
		public abstract string ContentType { get; }
		public abstract long  ContentLength { get; }
		public abstract Dictionary<string, string> Headers { get; }
		public abstract Dictionary<string, string> Metadata { get; }
		public abstract Uri StorageUrl { get; }
		public abstract Uri CdnManagementUrl { get; }
		public abstract Uri CdnUri { get; }
		public abstract Uri CdnSslUri { get; }
	    public abstract Uri CdnStreamingUri { get; }
		public abstract void SaveToFile(string path);
		public abstract void SaveToFile(string path, bool verify_etag);
		public abstract void WriteFromFile(string path);
		public abstract void WriteFromFile(string path, Dictionary<string, string> headers);
		public abstract void Write(Stream data);
		public abstract void Write(Stream data, Dictionary<string, string> headers);
		public abstract Stream Read();
		public abstract void SyncMetadata(Dictionary<string, string> metadata);
		public abstract void PurgeFromCdn();
		public abstract void PurgeFromCdn(string[] emails);
		public abstract void PurgeFromCdn(string email);
	}
}