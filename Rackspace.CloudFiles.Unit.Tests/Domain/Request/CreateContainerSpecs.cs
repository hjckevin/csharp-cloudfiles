using System;
using Moq;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain.Request;
using Rackspace.CloudFiles.Domain.Request.Interfaces;

namespace Rackspace.CloudFiles.Unit.Tests.Domain.Request.CreateContainerSpecs
{
    [TestFixture]
    public class Base_container_context
    {
        protected virtual void SetupContext()
        {
        }

        [SetUp]
        public void SetUp()
        {
            SetupContext();
        }
    }

    public class When_creating_a_container_with_missing_data : Base_container_context
    {
        [Test]
        public void should_throw_exception_when_storage_url_is_null()
        {
            Assert.Throws<ArgumentNullException>(()=> new CreateContainer(null, "containername"));
        }
        [Test]
        public void should_throw_exception_when_storage_url_is_empty()
        {
            Assert.Throws<ArgumentNullException>(()=> new CreateContainer("", "containername"));
        }
        [Test]
        public void should_throw_exception_when_container_is_null()
        {
            Assert.Throws<ArgumentNullException>(()=> new CreateContainer("http://storageuri", null));
        }
        [Test]
        public void should_throw_exception_when_container_is_empty()
        {
            Assert.Throws<ArgumentNullException>(()=> new CreateContainer("http://storageuri", ""));
        }
    }
    public class When_creating_a_container: Base_container_context
    {
        private CreateContainer createContainer;
        private Mock<ICloudFilesRequest> mock;

        protected override void SetupContext()
        {
            createContainer = new CreateContainer("http://storageurl", "containername");   
            mock = new Mock<ICloudFilesRequest>();
            createContainer.Apply(mock.Object);
        }
        [Test]
        public void should_append_container_name_to_storage_url()
        {
             Assert.AreEqual("http://storageurl/containername", createContainer.CreateUri().ToString());
        }
        [Test]
        public void should_use_put_method()
        {
            mock.VerifySet(x => x.Method = "PUT");
        }
    }

}