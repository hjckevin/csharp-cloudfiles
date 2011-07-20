using System;
using Moq;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain.Request;
using Rackspace.CloudFiles.Domain.Request.Interfaces;

namespace Rackspace.CloudFiles.Unit.Tests.Domain.Request.DeleteContainerSpecs
{
    [TestFixture]
    public class Delete_container_base
    {
        protected virtual void SetUp(){}
        [SetUp]
        public void SetUpBaseContext()
        {
            SetUp();
        }
        
    }
    public class When_deleting_container_and_some_data_is_missing:Delete_container_base
    {
        [Test]
        public void should_throw_exception_when_storage_url_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer(null, "containername"));
        }

        [Test]
        public void should_throw_exception_when_storage_url_is_empty()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer("", "containername"));
        }
        [Test]
        public void should_throw_exception_when_deleting_a_container_and_container_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer("http://storageurl", null));
        }
        [Test]
        public void should_throw_exception_when_deleting_a_container_and_container_name_is_empty()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteContainer("http://storageurl", ""));
        }
    }

    public class When_deleting_a_container: Delete_container_base
    {
        private DeleteContainer deleteContainer;
        private Mock<ICloudFilesRequest> mockrequest;

        protected override void SetUp()
        {
            deleteContainer = new DeleteContainer("http://storageurl", "containername");
            mockrequest = new Mock<ICloudFilesRequest>();
            deleteContainer.Apply(mockrequest.Object);
        }
        [Test]
        public void Should_have_url_made_of_storage_url_and_container_name()
        {
            Assert.AreEqual("http://storageurl/containername",deleteContainer.CreateUri().ToString());
        }
        [Test]
        public void Should_have_http_delete_method()
        {
            mockrequest.VerifySet(x => x.Method = "DELETE");
        }

    }

   
}