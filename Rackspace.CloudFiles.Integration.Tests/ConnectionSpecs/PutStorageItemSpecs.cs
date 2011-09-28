using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;

using System.Threading;
using Rackspace.CloudFiles.Domain;
using Rackspace.CloudFiles.Exceptions;

namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs.PutStorageItemSpecs
{
    [TestFixture]
    public class When_uploading_a_file : TestBase
    {
        [Test]
        public void Should_return_nothing_when_the_file_is_uploaded_successfully()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            connection.DeleteContainer(Constants.CONTAINER_NAME);
        }

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            connection.PutStorageItem(Constants.CONTAINER_NAME+new Guid(), Constants.StorageItemName);
        }

        [Test]
        public void Should_upload_file_with_the_file_name_minus_the_file_path_in_uri_format()
        {            
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);

                var executingPath = Assembly.GetExecutingAssembly().CodeBase.Replace(@"Rackspace.CloudFiles.Integration.Tests.DLL", "") + Constants.StorageItemName;
                
                connection.PutStorageItem(Constants.CONTAINER_NAME, executingPath);

                var containerList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                Assert.That(containerList.Contains(Constants.StorageItemName), Is.True);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }

        [Test]
        public void Should_upload_file_with_the_file_name_minus_the_file_path_in_windows_path_format()
        {
            

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Path.GetFullPath(Constants.StorageItemName));

                var containerList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                Assert.That(containerList.Contains(Constants.StorageItemName), Is.True);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }
    }

    [TestFixture]
    public class When_putting_an_object_into_a_container : TestBase
    {

        [Test]
        public void Should_upload_the_content_type()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }

        [Test]
        public void Should_upload_the_content_type_when_using_dotnet_fileinfo_type()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            StorageItem storageItem = null;
            try
            {
                var file = new FileInfo(Constants.StorageItemNamePdf);
                var metadata = new Dictionary<string, string> 
                {{"Source", "1"}, {"Note", "2"}};

                connection.PutStorageItem(Constants.CONTAINER_NAME, file.Open(FileMode.Open), file.Name, metadata);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNamePdf);
                Assert.That(storageItem.ContentType, Is.EqualTo("application/pdf"));
            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }

    }

    [TestFixture]
    public class When_putting_an_object_into_a_container_with_meta_information : TestBase
    {

        [Test]
        public void Should_upload_the_meta_information()
        {

            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                var metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg, metadata);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                Assert.That(storageItem.Metadata[Constants.XMetaKeyHeader + Constants.MetadataKey], Is.EqualTo(Constants.MetadataValue));
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }

        [Test]
        public void Should_upload_the_meta_information_with_file_stream()
        {

            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                var file = new FileInfo(Constants.StorageItemNameJpg);
                var metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
                connection.PutStorageItem(Constants.CONTAINER_NAME, file.Open(FileMode.Open), Constants.StorageItemNameJpg, metadata);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                Assert.That(storageItem.Metadata[Constants.XMetaKeyHeader + Constants.MetadataKey], Is.EqualTo(Constants.MetadataValue));
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }
    }

   
    [TestFixture]
    public class When_signing_up_for_progress_updates_uploading_a_file : TestBase
    {
        private bool operationCompleted;
        private bool gotProgress;

        [Test]
        public void Should_successfully_receive_transfer_amounts()
        {
            operationCompleted = false;
            gotProgress = false;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                var progressCallback = new Connection.ProgressCallback(callback);
                var operationCompleteCallback = new Connection.OperationCompleteCallback(operationComplete);

                ((Connection) connection).OperationComplete += operationCompleteCallback;
                ((Connection) connection).AddProgressWatcher(progressCallback);
                connection.PutStorageItemAsync(Constants.CONTAINER_NAME, Constants.StorageItemName);

                //Sleep to make sure we receive the message
                Thread.Sleep(5000);
            }
            catch (Exception)
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }

        [Test]
        public void Should_successfully_receive_transfer_amounts_when_passing_a_stream()
        {
            operationCompleted = false;
            gotProgress = false;

            try
            {
                var file = new FileInfo(Constants.StorageItemNamePdf);
                connection.CreateContainer(Constants.CONTAINER_NAME);
                var progressCallback = new Connection.ProgressCallback(callback);
                var operationCompleteCallback = new Connection.OperationCompleteCallback(operationCompletePDF);

                ((Connection)connection).OperationComplete += operationCompleteCallback;
                ((Connection)connection).AddProgressWatcher(progressCallback);
                connection.PutStorageItemAsync(Constants.CONTAINER_NAME, file.Open(FileMode.Open), file.Name);

                //Sleep to make sure we receive the message
                Thread.Sleep(3000);

             
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            }
        }

        [TearDown]
        public void Teardown()
        {
            Assert.IsTrue(gotProgress);
            Assert.IsTrue(operationCompleted);
        }

        private void operationCompletePDF()
        {
            connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            operationCompleted = true;
        }

        private void operationComplete()
        {
            connection.DeleteContainer(Constants.CONTAINER_NAME, true);
            operationCompleted = true;
        }

        private void callback(int xfer)
        {
            if (xfer > 0)
            {
                gotProgress = true;
            }
        }

    }

// DO NOT DELETE
// USED FOR EDGE CASE TESTING WITH BIG FILES
// DO NOT WANT RUN ON CI
//    [TestFixture]
//    public class When_putting_a_object_greater_than_2_GB_into_cloud_files : TestBase
//    {
//        [Test]
//        public void Should_upload_the_file_successfully()
//        {
//            
//            connection.CreateContainer(Constants.CONTAINER_NAME);
//
//            try
//            {
//                connection.PutStorageItem(Constants.CONTAINER_NAME, @"C:\TestStorageItem.iso");
//
//                var items = connection.GetContainerItemList(Constants.CONTAINER_NAME);
//                Assert.That(items.Contains("TestStorageItem.iso"), Is.True);
//            }
//            finally
//            {
//                if(connection.GetContainerItemList(Constants.CONTAINER_NAME).Contains("TestStorageItem.iso"))
//                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "TestStorageItem.iso");
//                connection.DeleteContainer(Constants.CONTAINER_NAME);
//            }
//        }
//    }
}