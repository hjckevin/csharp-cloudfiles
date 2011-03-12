using System;
using System.IO;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.request.Interfaces;
using com.mosso.cloudfiles.unit.tests.CustomMatchers;
using Moq;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Domain.request
{
    [TestFixture]
    public class When_adding_storage_object
    {
        private Mock<ICloudFilesRequest> mock;
        private PutStorageDirectory createContainer;

        [SetUp]
        public void SetUp() 
        {
            createContainer = new PutStorageDirectory("http://storageurl", "containername", "objname");
            mock = new Mock<ICloudFilesRequest>();
            createContainer.Apply(mock.Object);
        }
        [Test]
        public void should_append_container_and_object_name_to_storage_url()
        {
            Assert.AreEqual("http://storageurl/containername/objname", createContainer.CreateUri().ToString());
        }
        [Test]
        public void should_use_PUT_method()
        {
            mock.VerifySet(x => x.Method = "PUT");
        }
        [Test]
        public void should_have_content_type_of_application_directory()
        {
            mock.VerifySet(x => x.ContentType = "application/directory");
        }
        [Test]
        public void should_set_content_with_basic_empty_object()
        {
            mock.Verify(x => x.SetContent(It.IsAny<MemoryStream>(), It.IsAny<Connection.ProgressCallback>()));
        }
    }
    [TestFixture]
    public class When_creating_uri_and_storage_item_has_forward_slashes_at_the_beginning
    {
        private PutStorageDirectory item;
        private Uri url;

        [SetUp]
        public void SetUp()
        {
            item = new PutStorageDirectory("http://storeme", "itemcont", "/dir1/dir2");
            url = item.CreateUri();
        }
        [Test]
        public void should_remove_all_forward_slashes()
        {
            url.EndsWith("dir1/dir2");
        }
    }
}