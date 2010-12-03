//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace com.mosso.cloudfiles.domain.request
{
    #region Using
    using System;
    using com.mosso.cloudfiles.domain.request.Interfaces;
    #endregion

    /// <summary>
    /// A class to represent getting a set of containers in a web request
    /// </summary>
    public class GetContainers : IAddToWebRequest
    {
        private readonly string _storageUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainers"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public GetContainers(string storageUrl)
        {
            _storageUrl = storageUrl;

            if (string.IsNullOrEmpty(storageUrl))
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Creates the corresponding URI for this request.
        /// </summary>
        /// <returns>A new URI</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl);
        }

        /// <summary>
        /// Applies the appropiate method to the specified request for this implementation.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "GET";
        }
    }
}