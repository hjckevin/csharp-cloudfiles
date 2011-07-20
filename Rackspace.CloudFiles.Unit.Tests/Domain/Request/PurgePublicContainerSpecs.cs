using System;
using System.Net;
using Rackspace.CloudFiles.Unit.Tests.CustomMatchers;
using Moq;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain.Request;
using Rackspace.CloudFiles.Domain.Request.Interfaces;

namespace Rackspace.CloudFiles.Unit.Tests.Domain.Request.DeleteContainerSpecs
{
    [TestFixture]
    public class PurgePublicContainerSpecs
    {
        [Test]
        public void when_purging_a_public_container_and_cdn_management_url_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer(null, "containername"));
        }
        [Test]
        public void when_purging_a_public_container_and_cdn_management_url_is_empty_string()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer("", "containername"));
        }
        [Test]
        public void when_purging_a_public_container_and_container_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer("http://cdnmanagementurl", null));
        }
        [Test]
        public void when_purging_a_public_container_and_container_name_is_empty_string()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer("http://cdnmanagementurl", ""));
        }
        [Test]
        public void when_purging_a_public_container_without_a_purge_email_address()
        {
            var deleteContainer = new DeleteContainer("http://cdnmanagementurl", "containername");
            var mockrequest = new Mock<ICloudFilesRequest>();
            var webheaders = new WebHeaderCollection();
            mockrequest.SetupGet(x => x.Headers).Returns(webheaders);

            Assert.That(deleteContainer.CreateUri().ToString(), Is.EqualTo("http://cdnmanagementurl/containername"));

            deleteContainer.Apply(mockrequest.Object);

            mockrequest.VerifySet(x => x.Method = "DELETE");
            webheaders.KeyValueFor(Constants.X_PURGE_EMAIL).HasValueOf(null);
        }
        [Test]
        public void when_purging_a_public_container_wit_a_single_purge_email_address()
        {
            var deleteContainer = new DeleteContainer("http://cdnmanagementurl", "containername", new[] {"me@me.com"});
            var mockrequest = new Mock<ICloudFilesRequest>();
            var webheaders = new WebHeaderCollection();
            mockrequest.SetupGet(x => x.Headers).Returns(webheaders);

            Assert.That(deleteContainer.CreateUri().ToString(), Is.EqualTo("http://cdnmanagementurl/containername"));

            deleteContainer.Apply(mockrequest.Object);

            mockrequest.VerifySet(x => x.Method = "DELETE");
            webheaders.KeyValueFor(Constants.X_PURGE_EMAIL).HasValueOf("me@me.com");
        }
        [Test]
        public void when_purging_a_public_container_wit_multiple_purge_email_addresses()
        {
            var deleteContainer = new DeleteContainer("http://cdnmanagementurl", "containername", new[] { "me@me.com", "you@you.com" });
            var mockrequest = new Mock<ICloudFilesRequest>();
            var webheaders = new WebHeaderCollection();
            mockrequest.SetupGet(x => x.Headers).Returns(webheaders);

            Assert.That(deleteContainer.CreateUri().ToString(), Is.EqualTo("http://cdnmanagementurl/containername"));

            deleteContainer.Apply(mockrequest.Object);

            mockrequest.VerifySet(x => x.Method = "DELETE");
            webheaders.KeyValueFor(Constants.X_PURGE_EMAIL).HasValueOf("me@me.com,you@you.com");
        }
    }
}