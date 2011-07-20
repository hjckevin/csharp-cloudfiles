//----------------------------------------------
// See COPYING file for licensing information
//----------------------------------------------

using Rackspace.CloudFiles.Domain.Request.Interfaces;
using Rackspace.CloudFiles.Exceptions;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Domain.Request
{

    #region Using
    using System;
    using Request.Interfaces;
    using Exceptions;
    using Utils;
    #endregion

    /// <summary>
    /// A class to represent creating a container in a web request
    /// </summary>
    public class CreateContainer : IAddToWebRequest
    {
        private readonly string _storageUrl;
        private readonly string _containerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContainer"/> class.
        /// </summary>
        /// <param name="storageUrl">The customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">The name of the container where the storage item is located</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        /// <exception cref="StorageItemNameException">Thrown when the object name is invalid</exception>
        public CreateContainer(string storageUrl, string containerName)
        {
            if (string.IsNullOrEmpty(storageUrl) || string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException();
            }

            if (!ContainerNameValidator.Validate(containerName))
            {
                throw new ContainerNameException();
            }

            _storageUrl = storageUrl;
            _containerName = containerName;
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
        /// Applies the appropiate method to the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Apply(ICloudFilesRequest request)
        {
            request.Method = "PUT";
        }
    }
}