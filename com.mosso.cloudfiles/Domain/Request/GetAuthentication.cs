//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace com.mosso.cloudfiles.domain.request
{
    #region Using
    using System;
    using com.mosso.cloudfiles.domain.request.Interfaces;
    using com.mosso.cloudfiles.utils;
    #endregion

    /// <summary>
    /// A class to represent getting authentication in a web request
    /// </summary>
    public class GetAuthentication : IAddToWebRequest
    {
        private readonly UserCredentials _userCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthentication"/> class.
        /// </summary>
        /// <param name="userCreds">the UserCredentials instace to use when attempting authentication</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        public GetAuthentication(UserCredentials userCreds)
        {
            if (userCreds == null)
            {
                throw new ArgumentNullException();
            }
            _userCredentials = userCreds;
        }

        /// <summary>
        /// Creates the corresponding URI using user credentials for authentication.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            if (_userCredentials == null)
            {
                throw new ArgumentNullException();
            }

            var uri = string.IsNullOrEmpty(_userCredentials.AccountName)
                ? _userCredentials.AuthUrl
                : new Uri(_userCredentials.AuthUrl + "/"
                    + _userCredentials.Cloudversion.Encode() + "/"
                    + _userCredentials.AccountName.Encode() + "/auth");

            return uri;
        }


        /// <summary>
        /// Applies the corresponding method and headers for this authentication request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
            request.Headers.Add(Constants.X_AUTH_USER, _userCredentials.Username.Encode());
            request.Headers.Add(Constants.X_AUTH_KEY, _userCredentials.Api_access_key.Encode());
        }
    }
}