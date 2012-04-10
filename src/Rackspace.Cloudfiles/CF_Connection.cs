using System;
using System.Collections.Generic;
using OpenStack.Swift;
using Rackspace.Cloudfiles.Constants;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Connection object used to connect to the storage system.
	/// </summary>
	public class CF_Connection : Connection
	{
		private readonly Client _client;
		private int _retires;
		private int _num_retries_attempted;
		private int _timeout = Constants.Misc.DefaultTimeout;
		private string _user_agent = Constants.Misc.DefaultUserAgent;
		private readonly UserCredentials _user_creds;
		/// <summary>
		/// Gets the UserCredentials Object to dynamically update account connection information
		/// </summary>
		public UserCredentials UserCreds
		{
			get { return _user_creds; }
		}
		/// <summary>
		/// Gets a value indicating whether this instance has CDN.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has CDN; otherwise, <c>false</c>.
		/// </value>
		public bool HasCDN
		{
			get { return _user_creds.CdnMangementUrl != null; }
		}
		/// <summary>
		/// Gets or sets retries.
		/// </summary>
		/// <value>
		/// Number of times to retry defaults to zero
		/// </value>
		public int Retries
		{
			get { return _retires; }
			set { _retires = value; }
		}
		/// <summary>
		/// Gets or sets the timeout of the tcp connection.
		/// </summary>
		public int Timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}
		/// <summary>
		/// Gets or sets the user agent.
		/// </summary>
		/// <value>
		/// The user agent.
		/// </value>
		public string UserAgent
		{
			get { return _user_agent; }
			set { _user_agent = value; }
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Connection"/> class.
		/// </summary>
		/// <param name='creds'>
		/// An instance of a UserCredentials object
		/// </param>
		public CF_Connection (UserCredentials creds)
		{
			_client = new CF_Client();
			_user_creds = creds;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Connection"/> class.
		/// </summary>
		/// <param name='creds'>
		/// An instance of a UserCredentials object
		/// </param>
		/// <param name='client'>
		/// a HTTP client object 
		/// </param>
		public CF_Connection(UserCredentials creds, Client client)
		{
			_client = client;
			_user_creds = creds;
		}
		/// <summary>
		/// Authenticates against the specified AuthUrl.
		/// </summary>
		public void Authenticate()
		{
			Authenticate(false);
		}
		/// <summary>
		/// Authenticates against a specified AuthUrl you can also set if you would like to use Snet.
		/// </summary>
		/// <param name="snet">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void Authenticate(bool snet)
		{
			try
			{
		        AuthResponse res = _client.GetAuth(_user_creds.AuthUrl.ToString(), _user_creds.UserName, _user_creds.ApiKey, new Dictionary<string, string>(), new Dictionary<string, string>(), snet);
			    _user_creds.StorageUrl =  new Uri(res.Headers[Constants.Headers.StorageUrl]);
			    _user_creds.AuthToken = res.Headers[Constants.Headers.AuthToken];
			    if(res.Headers.ContainsKey(Constants.Headers.CdnManagementUrl))
			    {
			        _user_creds.CdnMangementUrl = new Uri(res.Headers[Constants.Headers.CdnManagementUrl]);
			    }
			}
			catch (ClientException)
			{
				if(_num_retries_attempted <= _retires)
				{
					++ _num_retries_attempted;
					Authenticate();
				}
				else
				{
					_num_retries_attempted = 0;
					throw new AuthenticationFailedException();
				}
			}
		}
	}
	public interface Connection
	{
		void Authenticate();
		void Authenticate(bool snet);
		bool HasCDN { get; }
		UserCredentials UserCreds { get; }
		int Retries { get; set; }
		int Timeout { get; set; }
		string UserAgent { get; set; }
	}
}