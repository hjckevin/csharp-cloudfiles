using System;
using NUnit.Framework;
using Rackspace.CloudFiles.Exceptions;


namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs.DeleteContainerSpecs
{
    [TestFixture]
    public class When_deleting_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_delete_is_successful()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.DeleteContainer(Constants.CONTAINER_NAME);
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_exception_when_the_container_does_not_exist()
        {
            
            connection.DeleteContainer(Constants.CONTAINER_NAME);
        }

        [Test]
        public void Should_throw_exception_when_the_container_exists_but_is_not_empty()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);

            try
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
            catch (Exception ex)
            {
                Assert.That(ex.GetType(), Is.EqualTo(typeof (ContainerNotEmptyException)));
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_delete_everything_in_the_container_before_deleting_the_container_if_passed_boolean_flag()
        {
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.MakePath(Constants.CONTAINER_NAME, "photos");
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("photos/photo1.jpg"));
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("photos/photo2.jpg"));
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("movieobject.mov"));
            connection.MakePath(Constants.CONTAINER_NAME, "videos");
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("videos/movieobj4.mov"));

            connection.DeleteContainer(Constants.CONTAINER_NAME, true);

            bool containerExists = true;

            try
            {
                var container = connection.GetContainerInformation(Constants.CONTAINER_NAME);
                Assert.Fail("{0} container exists", container.Name);
            }
            catch(ContainerNotFoundException)
            {
                containerExists = false;
            }

            Assert.That(containerExists, Is.False);
        }
    }
}