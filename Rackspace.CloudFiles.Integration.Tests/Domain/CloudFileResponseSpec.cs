using System.IO;
using System.Net;
using Rackspace.CloudFiles.Domain.Response;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain.Request;
using Rackspace.CloudFiles.Domain.Response.Interfaces;
using Rackspace.CloudFiles.Integration.Tests;

namespace Rackspace.CloudFiles.integration.tests.Domain
{
    [TestFixture]
    public class CloudFileResponseSpec : TestBase
    {
        [Test]
        public void should_be_able_to_return_stream_more_than_once()
        {
            ICloudFilesResponse getStorageItemResponse = null;
            Stream stream = null;
            Stream streamcopy = null;
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);

                var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName);

                getStorageItemResponse = new GenerateRequestByType().Submit(getStorageItem, authToken);
                stream = getStorageItemResponse.GetResponseStream();
                streamcopy = getStorageItemResponse.GetResponseStream();
                Assert.AreEqual(stream.Length, streamcopy.Length);
                Assert.Greater(getStorageItemResponse.ContentLength, 0);
                Assert.IsTrue(stream.CanRead);
                Assert.IsTrue(streamcopy.CanRead);
            }
            finally
            {
                if (getStorageItemResponse != null) getStorageItemResponse.Close();
                if (stream != null) stream.Close();
                if (streamcopy != null) streamcopy.Close();
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }
    }
}