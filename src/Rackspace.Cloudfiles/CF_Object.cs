using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using OpenStack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Object object used to represent a Cloud Files Object
	/// </summary>
	public class CF_Object : StorageObject
	{
        private readonly Client _client;
        private readonly Container _cont;
        private int _retires;
        private int _num_retries_attempted;
        private readonly Connection _conn;
        private bool _reload_properties = true;
        private readonly string _name;
        private readonly string _etag;
        private readonly string _content_type;
        private const long _content_length = -1;
        private Dictionary<string, string> _headers;
        private Dictionary<string, string> _metadata;

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
			_client = new CF_Client();
			_conn = conn;
			_cont = new CF_Container(conn, container_name);
			_name = object_name;
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
			_client = client;
			_conn = conn;
			_cont = new CF_Container(conn, container_name);
			_name = object_name;	
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
			_client = client;
			_conn = conn;
			_cont = container;
			_name = object_name;
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
		/// Gets or sets the size of the chunk.
		/// </summary>
		/// <value>
		/// The size of the chunk.
		/// </value>
		public int ChunkSize
		{
			get { return _client.ChunkSize; }
			set { _client.ChunkSize = value; }
		}
		/// <summary>
		/// Gets the Object name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name
		{
			get { return _name; }
		}
		/// <summary>
		/// Gets the etag.
		/// </summary>
		/// <value>
		/// The etag.
		/// </value>
		public string Etag
		{
			get { return _reload_properties ? _head_object().Headers["etag"] : _etag; }
		}
		/// <summary>
		/// Gets the Content type.
		/// </summary>
		/// <value>
		/// The type of the content.
		/// </value>
		public string ContentType
		{
			get { return _reload_properties ? _head_object().Headers["content-type"] : _content_type; }
		}
		/// <summary>
		/// Gets the content length.
		/// </summary>
		/// <value>
		/// The length of the content.
		/// </value>
		public long  ContentLength
		{
			get { return _reload_properties ? long.Parse(_head_object().Headers["content-length"]) : _content_length; }
		}
		/// <summary>
		/// Gets the headers.
		/// </summary>
		/// <value>
		/// The headers.
		/// </value>
		public Dictionary<string, string> Headers
		{
			get { return _reload_properties ? Common.ProcessMetadata(_head_object().Headers)["headers"] : _headers; }
		}
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>
		/// The metadata.
		/// </value>
		public Dictionary<string, string> Metadata
		{
			get { return _reload_properties ? Common.ProcessMetadata(_head_object().Headers)["metadata"] : _metadata; }
		}
		/// <summary>
		/// Gets the storage URL.
		/// </summary>
		/// <value>
		/// The storage URL.
		/// </value>
		public Uri StorageUrl
		{
			get { return new Uri(_conn.UserCreds.StorageUrl + _cont.Name + "/" + Name); }
		}
		/// <summary>
		/// Gets the cdn management URL.
		/// </summary>
		/// <value>
		/// The cdn management URL.
		/// </value>
		public Uri CdnManagementUrl
		{
			get { return _conn.UserCreds.CdnMangementUrl != null ? new Uri(_conn.UserCreds.CdnMangementUrl + _cont.Name + "/" + Name) : null; }
		}
		/// <summary>
		/// Gets the cdn URI.
		/// </summary>
		/// <value>
		/// The cdn URI.
		/// </value>
		public Uri CdnUri
		{
			get { return _cont.CdnUri != null ? new Uri(_cont.CdnUri + Name) : null; }
		}
		/// <summary>
		/// Gets the cdn ssl URI.
		/// </summary>
		/// <value>
		/// The cdn ssl URI.
		/// </value>
		public Uri CdnSslUri
		{
			get { return _cont.CdnSslUri != null ? new Uri(_cont.CdnSslUri + Name) : null; }
		}
		/// <summary>
		/// Gets the cdn streaming URI.
		/// </summary>
		/// <value>
		/// The cdn streaming URI.
		/// </value>
		public Uri CdnStreamingUri
		{
			get { return _cont.CdnStreamingUri != null ? new Uri(_cont.CdnStreamingUri + Name) : null; }
		}
		private ObjectResponse _head_object()
		{
		    var headers = new Dictionary<string, string>();
		    headers["user-agent"] = _conn.UserAgent;
		    _client.Timeout = _conn.Timeout;
		    try
		    {
			    var res = _client.HeadObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _cont.Name, Name, headers, new Dictionary<string, string>());
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
						    return _head_object();
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
						    return _head_object();
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
						    return _head_object();
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
		/// Saves to file.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public void SaveToFile(string path)
		{
			SaveToFile(path, false);
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
		public void SaveToFile(string path, bool verify_etag)
		{
			if (String.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException();
			}
            var headers = new Dictionary<string, string>();
			headers["user-agent"] = _conn.UserAgent;
			_client.Timeout = _conn.Timeout;
			Stream save_to = File.OpenWrite(path);
			var buffer = new byte[ChunkSize];
		    try
		    {
			    var res = _client.GetObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _cont.Name, Name, headers, new Dictionary<string, string>());
		        int read;
		        while ((read = res.ObjectData.Read(buffer, 0, buffer.Length)) > 0)
				{
					save_to.Write(buffer, 0, read);
				}
				save_to.Close();
				res.ObjectData.Close();
				if (verify_etag)
				{
					save_to = File.OpenRead(path);
					var md5 = MD5.Create();
					md5.ComputeHash(save_to);
					var sbuilder = new StringBuilder();
				    var hash = md5.Hash;
				    foreach(var b in hash)
				    {
					    sbuilder.Append(b.ToString("x2").ToLower());
				    }
					var converted_md5 = sbuilder.ToString();
					if (converted_md5 != res.Headers["etag"].ToLower())
					{
						File.Delete(path);
						throw new InvalidETagException();
					}
				}
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted <= _retires)
					    {
						    ++ _num_retries_attempted;
						    File.Delete(path);
						    SaveToFile(path, verify_etag);
						    break;
					    }
				        throw new TimeoutException();
				    case 401:
					    if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    _conn.Authenticate();
						    File.Delete(path);
						    SaveToFile(path, verify_etag);
						    break;
					    }
				        throw new UnauthorizedException();
				    case 404:
					    throw new ContainerNotFoundException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    File.Delete(path);
						    SaveToFile(path, verify_etag);
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
		/// Writes from file.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public void WriteFromFile(string path)
		{
			WriteFromFile(path, new Dictionary<string, string>());
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
		public void WriteFromFile(string path, Dictionary<string, string> headers)
		{
			Stream stream = System.IO.File.OpenRead(path);
			Write(stream, headers);
		}
		/// <summary>
		/// Write the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		public void Write(Stream data)
		{
			Write(data, new Dictionary<string, string>());
		}
		/// <summary>
		/// Writes an object stream.
		/// </summary>
		/// <param name="data">
		/// A <see cref="Stream"/>
		/// </param>
		/// <param name="headers">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public void Write(Stream data, Dictionary<string, string> headers)
		{   
			if (data == null)
			{
				throw new ArgumentNullException();
			}
		    headers["user-agent"] = _conn.UserAgent;
			_client.Timeout = _conn.Timeout;
		    try
		    {
			    _client.PutObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _cont.Name, Name, data, headers, new Dictionary<string, string>());
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    Write(data, headers);
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
						    Write(data, headers);
						    break;
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    case 422:
					    throw new InvalidETagException();
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    Write(data, headers);
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
		/// Reads from the object.
		/// </summary>
		/// <returns>
		/// A <see cref="Stream"/>
		/// </returns>
		public Stream Read()
		{
			var headers = new Dictionary<string, string>();
		    headers["user-agent"] = _conn.UserAgent;
			_client.Timeout = _conn.Timeout;
			try
			{
				return _client.GetObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _cont.Name, Name, headers, new Dictionary<string, string>()).ObjectData;
			}
			catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return Read();
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
						    return Read();
					    }
					    else
					    {
						    throw new UnauthorizedException();
					    }
				    default:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    return Read();
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
		/// Syncs Metadata
		/// </summary>
		/// <param name="metadata">
		/// A <see cref="Dictionary<System.String, System.String>"/>
		/// </param>
		public void SyncMetadata(Dictionary<string, string> metadata)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException();
			}
			var headers = new Dictionary<string, string>();
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
		    headers["user-agent"] = _conn.UserAgent;
			_client.Timeout = _conn.Timeout;
			try
			{
			    _client.PostObject(_conn.UserCreds.StorageUrl.ToString(), _conn.UserCreds.AuthToken, _cont.Name, _name, headers, new Dictionary<string, string>());
			}
		    catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    SyncMetadata(metadata);
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
						    SyncMetadata(metadata);
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
						    SyncMetadata(metadata);
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
		/// Purges object from cdn.
		/// </summary>
		public void PurgeFromCdn()
		{
			PurgeFromCdn("");
		}
		/// <summary>
		/// Purges object from cdn.
		/// </summary>
		/// <param name='emails'>
		/// Emails.
		/// </param>
		public void PurgeFromCdn(string[] emails)
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
			PurgeFromCdn(femail);
		}
		/// <summary>
		/// Purges Object from CDN
		/// </summary>
		/// <param name="email">
		/// A <see cref="System.String"/>
		/// </param>
		public void PurgeFromCdn(string email)
		{
			if (email == null)
			{
				throw new ArgumentNullException();
			}
			if (!_conn.HasCDN)
			{
				throw new CDNNotEnabledException();
			}
			var headers = new Dictionary<string, string>();
			if (email.Length > 0)
			{
			    headers["x-purge-email"] = email;
			}
		    headers["user-agent"] = _conn.UserAgent;
			_client.Timeout = _conn.Timeout;
			try
			{
				_client.DeleteObject(_conn.UserCreds.CdnMangementUrl.ToString(), _conn.UserCreds.AuthToken, _cont.Name, _name, headers, new Dictionary<string, string>());
			}
		    catch (ClientException e)
			{
				switch (e.Status)
				{
				    case -1:
				        if (_num_retries_attempted < _retires)
					    {
						    ++ _num_retries_attempted;
						    PurgeFromCdn(email);
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
						    PurgeFromCdn(email);
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
						    PurgeFromCdn(email);
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
	public interface StorageObject
	{
		int Retries { get; set; }
		int ChunkSize  { get; set; }
		string Name  { get; }
		string Etag { get; }
		string ContentType { get; }
		long  ContentLength { get; }
		Dictionary<string, string> Headers { get; }
		Dictionary<string, string> Metadata { get; }
		Uri StorageUrl { get; }
		Uri CdnManagementUrl { get; }
		Uri CdnUri { get; }
		Uri CdnSslUri { get; }
	    Uri CdnStreamingUri { get; }
		void SaveToFile(string path);
		void SaveToFile(string path, bool verify_etag);
		void WriteFromFile(string path);
		void WriteFromFile(string path, Dictionary<string, string> headers);
		void Write(Stream data);
		void Write(Stream data, Dictionary<string, string> headers);
		Stream Read();
		void SyncMetadata(Dictionary<string, string> metadata);
		void PurgeFromCdn();
		void PurgeFromCdn(string[] emails);
		void PurgeFromCdn(string email);
	}
}