//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

namespace com.mosso.cloudfiles.domain.request
{
    #region Using
    using System;
    using com.mosso.cloudfiles.domain.request.Interfaces;
    using com.mosso.cloudfiles.exceptions;
    using com.mosso.cloudfiles.utils;
    #endregion

    /// <summary>
    /// A class to represent deleting a container in a web request
    /// </summary>
    public class DeleteContainer : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteContainer"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public DeleteContainer(string storageUrl,  string containerName)
        {
            _storageUrl = storageUrl;
            _containerName = containerName;

            if (string.IsNullOrEmpty(storageUrl) || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <returns>A new URI for this container</returns>
        public Uri CreateUri()
        {
            return new Uri(_storageUrl + "/" + _containerName.Encode());
        }

        /// <summary>
        /// Applies the apropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "DELETE";
        }
    }
}