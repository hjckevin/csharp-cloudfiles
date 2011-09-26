using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs.GetContainerItemListSpecs
{
    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> containerItems;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(containerItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_no_items_when_the_container_has_no_items()
        {
            List<string> containerItems;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(containerItems.Count, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_using_the_marker_list_parameter : TestBase
    {
        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> containerItems;
            List<string> fullList;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                for (var i = 0; i < 10; i++ )
                {
                    connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, i + Constants.StorageItemName);
                }
                fullList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME, new Dictionary<GetListParameters, string>
                                                                                               {
                                                                                                   {GetListParameters.Marker, "5"}
                                                                                               });
            }
            finally
            {
                for (var i = 0; i < 10; i++)
                {
                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, i + Constants.StorageItemName);
                }
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(fullList.Count, Is.EqualTo(10));
            Assert.That(containerItems.Count, Is.EqualTo(5));
        }
    }

    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_using_the_delimiter_list_parameter : TestBase
    {
        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> containerItems;
            List<string> fullList;
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("photos/photo1"));
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("photos/photo2"));
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("movieobject"));
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("videos/movieobj4"));
                fullList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME, new Dictionary<GetListParameters, string>
                                                                                               {
                                                                                                   {GetListParameters.Delimiter, "/"}
                                                                                               });
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("photos/photo1"));
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("photos/photo2"));
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("movieobject"));
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("videos/movieobj4"));
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(fullList.Count, Is.EqualTo(4));
            Assert.That(containerItems.Count, Is.EqualTo(3));
            Assert.That(containerItems[0], Is.EqualTo("movieobject"));
            Assert.That(containerItems[1], Is.EqualTo("photos/"));
            Assert.That(containerItems[2], Is.EqualTo("videos/"));
        }
    }

    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_using_the_path_list_parameter : TestBase
    {
        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> containerItems;
            List<string> fullList;
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("photos/photo1"));
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("photos/photo2"));
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("movieobject"));
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("videos/movieobj4"));
                fullList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME, new Dictionary<GetListParameters, string>
                                                                                               {
                                                                                                   {GetListParameters.Path, ""}
                                                                                               });
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("photos/photo1"));
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("photos/photo2"));
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("movieobject"));
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("videos/movieobj4"));
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(fullList.Count, Is.EqualTo(4));
            Assert.That(containerItems.Count, Is.EqualTo(1));
            Assert.That(containerItems[0], Is.EqualTo("movieobject"));
        }
    }

    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_and_some_of_them_start_with_a_pound_sign : TestBase
    {
        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> list;
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                for (var i = 0; i < 10; i++)
                {
                    connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, string.Format("#{0}{1}",i,Constants.StorageItemName));
                }
                list = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                
            }
            finally
            {
                for (var i = 0; i < 10; i++)
                {
                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, string.Format("#{0}{1}", i, Constants.StorageItemName));
                }
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(list.Count, Is.EqualTo(10));
        }
    }
}