using System;
using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.request.Interfaces;
using Moq;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.DeleteStorageItemSpecs
{
    [TestFixture]
    public class When_parameters_missing_from_delete_storage_item
    {
        [Test]
        public void should_throw_when_deleting_a_storage_item_and_storage_url_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteStorageItem(null, "containername", "storageitemname"));
        }
        [Test]
        public void should_throw_when_deleting_a_storage_item_and_storage_url_is_emptry_string()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteStorageItem("", "containername", "storageitemname"));
        }
        [Test]
        public void should_throw_when_deleting_a_storage_item_and_storage_item_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteStorageItem("http://storageurl", "containername", null));
        }
        [Test]
        public void should_throw_when_deleting_a_storage_item_and_storage_item_name_is_emptry_string()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteStorageItem("http://storageurl", "containername", ""));
        }
        [Test]
        public void should_throw_when_deleting_a_storage_item_and_container_name_is_null()
        {
           Assert.Throws<ArgumentNullException>(() => new DeleteStorageItem("http://storageurl", null, "storageitemname"));
        }

        [Test]
        public void should_throw_when_deleting_a_storage_item_and_container_name_is_emptry_string()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteStorageItem("http://storageurl", "", "storageitemname"));
        }
    }
    [TestFixture]
    public class When_deleting_a_storage_item
    {
        private DeleteStorageItem deleteStorageItem;
        private Mock<ICloudFilesRequest> mockrequest;

        [SetUp]
        public void SetUp()
        {
            deleteStorageItem = new DeleteStorageItem("http://storageurl", "containername", "storageitemname");
            mockrequest = new Mock<ICloudFilesRequest>();
            mockrequest.SetupGet(x => x.Headers).Returns(new WebHeaderCollection());
            deleteStorageItem.Apply(mockrequest.Object);
        }
        [Test]
        public void should_start_with_storageurl_have_container_name_next_and_then_end_with_the_item_being_deleted()
        {
            Assert.AreEqual("http://storageurl/containername/storageitemname", deleteStorageItem.CreateUri().ToString());
        }
        [Test]
        public void should_use_http_delete_method()
        {
            mockrequest.VerifySet(x => x.Method = "DELETE");
        }

    }
}