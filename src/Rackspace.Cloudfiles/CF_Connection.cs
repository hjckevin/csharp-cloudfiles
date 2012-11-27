using System;
using System.Collections.Generic;

using System.Json;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

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


        ///<summary>
        /// Authentication for keystone + swift
        /// </summary>
        /// 
        public void AuthenticateForKeystone()
        {
            String[] strArr = _user_creds.UserName.Split(':');

            var auth = new Auth()
            {
                auth = new Tenant()
                {
                    tenantName = strArr[0],
                    passwordCredentials = new UserCred()
                    {
                        username = strArr[1],
                        password = _user_creds.ApiKey
                    }
                }
            };

            var jsonSer= new JavaScriptSerializer();
            var jsonStr = jsonSer.Serialize(auth);
            byte[] authData = System.Text.Encoding.ASCII.GetBytes(jsonStr);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_user_creds.AuthUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = authData.Length;
                request.GetRequestStream().Write(authData, 0, authData.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int statusCode = (int)response.StatusCode;
                if (statusCode >= 200 && statusCode < 300)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(),
                        System.Text.Encoding.GetEncoding("UTF-8"));
                    String responseStr = reader.ReadToEnd();

                    var serializer = new JavaScriptSerializer();
                    var jsonDict = (Dictionary<string, object>)serializer.DeserializeObject(responseStr);
                    var jsonAccess = (Dictionary<string, object>)jsonDict["access"];

                    var jsonSerciceCatalog = (object[])jsonAccess["serviceCatalog"];
                    foreach (var objServiceCatalog in jsonSerciceCatalog)
                    {
                        var jsonTemp = (Dictionary<string, object>)objServiceCatalog;
                        if (jsonTemp["type"].Equals("object-store"))
                        {
                            var jsonEndPoints = (object[])jsonTemp["endpoints"];
                            foreach (var objEndPoints in jsonEndPoints)
                            {
                                var jsonTemp2 = (Dictionary<string, object>)objEndPoints;
                                var objStorageURL = new object();
                                jsonTemp2.TryGetValue("publicURL", out objStorageURL);

                                this._user_creds.StorageUrl = new Uri(objStorageURL.ToString());
                            }
                        }
                    }

                    var jsonToken = (Dictionary<string, object>)jsonAccess["token"];
                    var objTokenId = new object();
                    jsonToken.TryGetValue("id", out objTokenId);

                    this._user_creds.AuthToken = objTokenId.ToString();

                }                
            }
            catch (ClientException)
            {
                if (_num_retries_attempted <= _retires)
                {
                    ++_num_retries_attempted;
                    AuthenticateForKeystone();
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
        void AuthenticateForKeystone();
		bool HasCDN { get; }
		UserCredentials UserCreds { get; }
		int Retries { get; set; }
		int Timeout { get; set; }
		string UserAgent { get; set; }
	}

    public class UserCred
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Tenant
    {
        public string tenantName { get; set; }
        public UserCred passwordCredentials { get; set; }
    }

    public class Auth
    {
        public Tenant auth { get; set; }
    }
}