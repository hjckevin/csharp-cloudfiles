using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;


namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs.MakePathSpecs
{
    [TestFixture]
    public class when_making_a_directory_structure : TestBase
    {
        private string mimetype = "application/directory";
        [Test]
        public void should_create_zero_byte_objects_with_content_type_application_directory()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MakePath(Constants.CONTAINER_NAME, "/dir1/dir2/dir3");

                var dir1 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1");
                Assert.That(dir1.ContentType, Is.EqualTo(mimetype));
                Assert.That(dir1.FileLength, Is.EqualTo(0));

                var dir2 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1/dir2");
                Assert.That(dir2.ContentType, Is.EqualTo(mimetype));
                Assert.That(dir2.FileLength, Is.EqualTo(0));

                var dir3 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/dir3");
                Assert.That(dir3.ContentType, Is.EqualTo(mimetype));
                Assert.That(dir3.FileLength, Is.EqualTo(0));
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }
        [Test]
        public void should_add_file_when_make_path_has_more_than_one_period()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MakePath(Constants.CONTAINER_NAME, "/dir1/dir2/subdir.sub");

                var dir1 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1");
                Assert.That(dir1.ContentType, Is.EqualTo(mimetype));
                Assert.That(dir1.FileLength, Is.EqualTo(0));

                var dir2 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1/dir2");
                Assert.That(dir2.ContentType, Is.EqualTo(mimetype));
                Assert.That(dir2.FileLength, Is.EqualTo(0));

                var dir3 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/subdir.sub");
                Assert.That(dir3.ContentType, Is.EqualTo(mimetype));
                Assert.That(dir3.FileLength, Is.EqualTo(0));

               
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }
        [Test]
        public void should_be_able_to_query_a_container_and_get_directory_like_results_with_the_path_query()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MakePath(Constants.CONTAINER_NAME, "/dir1/dir2/dir3");

                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/"+Constants.StorageItemName);
                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/"+Constants.StorageItemName);
                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/"+Constants.StorageItemNameGif);
                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/dir3/" + Constants.StorageItemName);
                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/dir3/" + Constants.StorageItemNameGif);
                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/dir3/" + Constants.StorageItemNameJpg);

                var items = connection.GetContainerItemList(Constants.CONTAINER_NAME, new Dictionary<GetListParameters, string> {{GetListParameters.Path, "dir1"}}, true);
                Assert.That(items.Count, Is.EqualTo(2));
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }
    }
}