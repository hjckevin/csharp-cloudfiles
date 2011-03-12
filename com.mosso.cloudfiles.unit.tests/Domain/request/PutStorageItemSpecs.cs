using System;
using System.IO;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.request.Interfaces;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.unit.tests.CustomMatchers;
using Moq;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.PutStorageItemSpecs
{
    [TestFixture]
    public class PutStorageItemSpec
    {
        [Test]
        public void should_remove_all_slashes_when_creating_uri_and_storage_item_has_forward_slashes_at_the_beginning()
        {
            var memstream = new MemoryStream();
            PutStorageItem item = new PutStorageItem("http://storeme", "itemcont", "/stuffhacks.txt", memstream);
            Uri url = item.CreateUri();
            url.EndsWith("stuffhacks.txt");
        }
        [Test]
        public void should_throw_file_not_found_exception_when_putting_a_storage_item_via_local_file_path_and_the_local_file_does_not_exist()
        {
            var mock = new Mock<ICloudFilesRequest>();
            Assert.Throws<FileNotFoundException>(()=> new PutStorageItem("a", "a", "a", "a").Apply(mock.Object));
        }
        [Test]
        public void should_throw_container_name_exception_when_putting_a_storage_item_via_local_file_path_and_the_container_name_exceeds_the_maximum_length()
        {
            Assert.Throws<ContainerNameException>(()=> new PutStorageItem("a", new string('a', Constants.MAX_CONTAINER_NAME_LENGTH + 1), "a", "a"));
        }
        [Test]
        public void should_throw_container_name_exception_when_putting_a_storage_item_via_stream_and_the_container_name_exceeds_the_maximum_length()
        {
            var s = new MemoryStream(new byte[0]);
            Assert.Throws<ContainerNameException>(() =>
                                                   new PutStorageItem("a",
                                                                      new string('a',
                                                                                 Constants.MAX_CONTAINER_NAME_LENGTH + 1),
                                                                      "a", s));
        }
        [Test]
        public void should_throw_container_name_exception_when_putting_a_storage_item_via_local_file_path_and_the_storage_item_name_exceeds_the_maximum_length()
        {
            Assert.Throws<StorageItemNameException>(
                   () => new PutStorageItem("a", "a", new string('a', Constants.MAX_OBJECT_NAME_LENGTH + 1), "a"));
        }
        [Test]
        public void should_throw_container_name_exception_when_putting_a_storage_item_via_stream_and_the_storage_item_name_exceeds_the_maximum_length()
        {
            var s = new MemoryStream(new byte[0]);
            Assert.Throws<StorageItemNameException>(
                () => new PutStorageItem("a", "a", new string('a', Constants.MAX_OBJECT_NAME_LENGTH + 1), s));
        }
    }
    
}