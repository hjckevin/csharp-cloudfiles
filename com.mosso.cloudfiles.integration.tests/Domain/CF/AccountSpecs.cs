using System;
using System.Text.RegularExpressions;
using System.Threading;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;


namespace com.mosso.cloudfiles.integration.tests.Domain.CF.AccountSpecs
{
    [TestFixture]
    public class AccountIntegrationTestBase
    {
        protected UserCredentials userCreds;
        protected IConnection connection;
        protected IAccount account;

        [SetUp]
        public void SetUp()
        {
            userCreds = new UserCredentials(Credentials.USERNAME, Credentials.API_KEY);
            connection = new Connection(userCreds);

            account = connection.Account;
        }
    }

    [TestFixture]
    public class When_creating_a_new_container : AccountIntegrationTestBase
    {
        [Test]
        public void should_give_you_container_instance_when_successful()
        {

            var originalContainerCount = account.ContainerCount;
            var originalBytesUsed = account.BytesUsed;
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 0));
                var container = account.GetContainer(Constants.CONTAINER_NAME);
                Assert.That(typeof(CF_Container), Is.EqualTo(container.GetType()));
            }
            finally
            {
                if(account.ContainerExists(Constants.CONTAINER_NAME))
                    account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed));
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerAlreadyExistsException))]
        public void should_throw_container_already_exists_exception_if_the_container_already_exists()
        {
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                account.CreateContainer(Constants.CONTAINER_NAME);
            }
            finally
            {
                account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
            }
        }
    }

    [TestFixture]
    public class When_deleting_a_container_and_the_container_is_not_empty : AccountIntegrationTestBase
    {
        [Test]
        [ExpectedException(typeof(ContainerNotEmptyException))]
        public void should_throw_container_not_empty_exception_if_the_container_not_empty()
        {
            IContainer container = null;
            try
            {

                container = account.CreateContainer(Constants.CONTAINER_NAME);
                container.AddObject(Constants.StorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);

                account.DeleteContainer(Constants.CONTAINER_NAME);
            }
            finally
            {

                if(container != null && account.ContainerExists(Constants.CONTAINER_NAME))
                {
                    container.DeleteObject(Constants.StorageItemName);
                    Assert.That(container.ObjectExists(Constants.StorageItemName), Is.False);
                    account.DeleteContainer(Constants.CONTAINER_NAME);    
                }
                
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
            }
        }
    }

    [TestFixture]
    public class When_adding_containers_and_objects_to_the_account : AccountIntegrationTestBase
    {
        [Test]
        public void should_keep_count_of_each_container_and_object()
        {
            IContainer container = null;
            var originalContainerCount = account.ContainerCount;
            try
            {

                container = account.CreateContainer(Constants.CONTAINER_NAME);
                container.AddObject(Constants.StorageItemName);
                container.AddObject(Constants.HeadStorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(container.ObjectCount, Is.EqualTo(2));
            }
            finally
            {
                if (container != null && account.ContainerExists(Constants.CONTAINER_NAME))
                {
                    container.DeleteObject(Constants.StorageItemName);
                    container.DeleteObject(Constants.HeadStorageItemName);
                    Assert.That(container.ObjectCount, Is.EqualTo(0));
                    Assert.That(container.ObjectExists(Constants.StorageItemName), Is.False);
                    Assert.That(container.ObjectExists(Constants.HeadStorageItemName), Is.False);
                    account.DeleteContainer(Constants.CONTAINER_NAME);
                }

                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount));
            }
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_an_account_and_containers_exist : AccountIntegrationTestBase
    {
        [Test]
        public void should_return_json_string_with_container_names_and_item_count_and_bytes_used()
        {
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                const string expectedJson = "{\"name\":[ ]?\"" + Constants.CONTAINER_NAME + "\",[ ]?\"count\":[ ]?0,[ ]?\"bytes\":[ ]?0}";
                Assert.That(Regex.Match(account.JSON, ".*"+expectedJson+".*").Success, Is.True);
            }
            finally
            {
                account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
            }
            
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_an_account_and_no_containers_exist : AccountIntegrationTestBase
    {
        [Test, Ignore("Only works if account has no pre-existing containers")]
        public void should_return_json_string_emptry_brackets()
        {
            const string expectedJson = "[]";
            Assert.That(account.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_an_account_and_containers_exist : AccountIntegrationTestBase
    {
        [Test]
        public void should_return_xml_document_with_container_names_and_item_count_and_bytes_used_or_emptystring_due_to_lack_of_propagation_up()
        {
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                const string expectedXml = "<container><name>" + Constants.CONTAINER_NAME + "</name><count>0</count><bytes>0</bytes></container>";
                var accountXml = account.XML;
                Assert.That(accountXml.InnerXml.Contains(expectedXml) || String.IsNullOrEmpty(accountXml.InnerXml), Is.True);
            }
            finally
            {
                account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
            }
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_an_account_and_no_containers_exist : AccountIntegrationTestBase
    {
        [Test, Ignore("Only works if account has no pre-existing containers")]
        public void should_return_xml_document_with_account_name()
        {
            const string pattern = "^<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\\.*\"></account>$";
            Assert.That(Regex.Match(account.XML.InnerXml, pattern).Success, Is.True);
        }
    }
}