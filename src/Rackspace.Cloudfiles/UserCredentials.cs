using System;
namespace Rackspace.Cloudfiles
{
	/// <summary>
	/// Passed to Connection Object to provide User and Account Information.
	/// </summary>
    public class UserCredentials
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.UserCredentials"/> class.
		/// </summary>
		/// <param name='username'>
		/// Username.
		/// </param>
		/// <param name='api_key'>
		/// Api_key.
		/// </param>
	    public UserCredentials(string username, string api_key)
		{
		    ApiKey = null;
		    AuthToken = null;
		    StorageUrl = null;
		    CdnMangementUrl = null;
		    AuthUrl = new Uri("https://auth.api.rackspacecloud.com/v1.0");
	        UserName = username;
		    ApiKey = api_key;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Rackspace.Cloudfiles.UserCredentials"/> class.
		/// </summary>
		/// <param name='username'>
		/// Username.
		/// </param>
		/// <param name='api_key'>
		/// Api_key.
		/// </param>
		/// <param name='auth_url'>
		/// Overload Default Auth URL
		/// </param>
	    public UserCredentials(string username, string api_key, string auth_url)
		{
		    ApiKey = null;
		    AuthToken = null;
		    StorageUrl = null;
		    CdnMangementUrl = null;
		    AuthUrl = new Uri(auth_url);
	        UserName = username;
		    ApiKey = api_key;
		}
		/// <summary>
		/// Adds cached credentials.
		/// </summary>
		/// <param name='auth_token'>
		/// Auth_token.
		/// </param>
		/// <param name='storage_url'>
		/// Storage_url.
		/// </param>
		/// <param name='cdn_management_url'>
		/// Cdn_management_url.
		/// </param>
		public void AddCachedCredentials(string auth_token, string storage_url,
		                       string cdn_management_url)
		{
			AuthToken = auth_token;
			StorageUrl = new Uri(storage_url);
			CdnMangementUrl = new Uri(cdn_management_url);
		}
		/// <summary>
		/// Adds cached credentials.
		/// </summary>
		/// <param name='auth_token'>
		/// Auth_token.
		/// </param>
		/// <param name='storage_url'>
		/// Storage_url.
		/// </param>
		public void AddCachedCredentials(string auth_token, string storage_url)
		{
			AuthToken = auth_token;
			StorageUrl = new Uri(storage_url);
		}
		/// <summary>
		/// Switches the auth Url to the UK auth system.
		/// </summary>
		public void UkAuth()
		{
			AuthUrl = new Uri("https://lon.auth.api.rackspacecloud.com/v1.0");
		}
		public string UserName { get; set; }
		public string ApiKey { get; set; }
		public string AuthToken { get; set; }
		public Uri StorageUrl { get; set; }
		public Uri CdnMangementUrl { get; set; }
		public Uri AuthUrl { get; set; }
    }
}

