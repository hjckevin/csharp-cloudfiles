using System;
using System.Collections.Generic;
using Openstack.Swift;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// CF_Connection object used to connect to the storage system.
	/// </summary>
	public class CF_Connection : Connection
	{
		private Client _client;
		private int _retires = 0;
		private int _num_retries_attempted = 0;
		private int _timeout = 5000;
		private string _user_agent = "csharp-cloudfiles/3.0";
		private UserCredentials _user_creds;
		/// <summary>
		/// Gets the UserCredentials Object to dynamically update account connection information
		/// </summary>
		public override UserCredentials UserCreds
		{
			get { return this._user_creds; }
		}
		/// <summary>
		/// Gets a value indicating whether this instance has CDN.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has CDN; otherwise, <c>false</c>.
		/// </value>
		public override bool HasCDN
		{
			get { return this._user_creds.CdnMangementUrl == null ? false : true; }
		}
		/// <summary>
		/// Gets or sets retries.
		/// </summary>
		/// <value>
		/// Number of times to retry defaults to zero
		/// </value>
		public override int Retries
		{
			get { return this._retires; }
			set { this._retires = value; }
		}
		/// <summary>
		/// Gets or sets the timeout of the tcp connection.
		/// </summary>
		public override int Timeout
		{
			get { return this._timeout; }
			set { this._timeout = value; }
		}
		/// <summary>
		/// Gets or sets the user agent.
		/// </summary>
		/// <value>
		/// The user agent.
		/// </value>
		public override string UserAgent
		{
			get { return this._user_agent; }
			set { this._user_agent = value; }
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.CF_Connection"/> class.
		/// </summary>
		/// <param name='creds'>
		/// An instance of a UserCredentials object
		/// </param>
		public CF_Connection (UserCredentials creds)
		{
			this._client = new CF_Client();
			this._user_creds = creds;
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
			this._client = client;
			this._user_creds = creds;
		}
		/// <summary>
		/// Authenticates against the specified AuthUrl.
		/// </summary>
		public override void Authenticate()
		{
			this.Authenticate(false);
		}
		/// <summary>
		/// Authenticates against a specified AuthUrl you can also set if you would like to use Snet.
		/// </summary>
		/// <param name="snet">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public override void Authenticate(bool snet)
		{
			try
			{
		        AuthResponse res = this._client.GetAuth(this._user_creds.AuthUrl.ToString(), this._user_creds.UserName, this._user_creds.ApiKey, new Dictionary<string, string>(), new Dictionary<string, string>(), snet);
			    this._user_creds.StorageUrl =  new Uri(res.Headers["x-storage-url"]);
			    this._user_creds.AuthToken = res.Headers["x-auth-token"];
			    if(res.Headers.ContainsKey("x-cdn-management-url"))
			    {
			        this._user_creds.CdnMangementUrl = new Uri(res.Headers["x-cdn-management-url"]);
			    }
			}
			catch (ClientException)
			{
				if(this._num_retries_attempted <= this._retires)
				{
					++ this._num_retries_attempted;
					this.Authenticate();
				}
				else
				{
					this._num_retries_attempted = 0;
					throw new AuthenticationFailedException();
				}
			}
		}
	}
	public abstract class Connection
	{
		public abstract void Authenticate();
		public abstract void Authenticate(bool snet);
		public abstract bool HasCDN { get; }
		public abstract UserCredentials UserCreds { get; }
		public abstract int Retries { get; set; }
		public abstract int Timeout { get; set; }
		public abstract string UserAgent { get; set; }
	}
}