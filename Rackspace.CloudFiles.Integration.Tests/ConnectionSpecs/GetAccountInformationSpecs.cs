using System;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain;


namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs.GetAccountInformationSpecs
{
    [TestFixture]
    public class When_retrieving_account_information_from_a_container_using_connection : TestBase
    {
        private AccountInformation account;

        protected override void SetUp()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                account = connection.GetAccountInformation();
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_return_the_size_and_quantity_of_items_in_the_account()
        {
            Assert.That(account, Is.Not.Null);
            Assert.That(account.BytesUsed, Is.GreaterThan(0));
        }
    }

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_json_format_and_container_exists : TestBase
    {
        private string jsonReturnValue;

        protected override void SetUp()
        {
            connection.CreateContainer(Constants.CONTAINER_NAME);

            try
            {
                jsonReturnValue = connection.GetAccountInformationJson();
            }
            catch (Exception e)
            {
                Assert.Fail("FAIL: " + e.Message);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }


        [Test]
        public void Should_get_serialized_json_format()
        {
            const string expectedSubString = "{\"name\":[ ]?\"" + Constants.CONTAINER_NAME + "\",[ ]?\"count\":[ ]?0,[ ]?\"bytes\":[ ]?0}";
            Assert.That(Regex.Match(jsonReturnValue, ".*" + expectedSubString + ".*").Success, Is.True);
        }
    }

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_xml_format_and_container_exists : TestBase
    {
        private XmlDocument xmlReturnValue;

        protected override void SetUp()
        {
            connection.CreateContainer(Constants.CONTAINER_NAME);

            try
            {
                xmlReturnValue = connection.GetAccountInformationXml();
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_get_serialized_xml_format()
        {
            const string expectedSubString = "<container><name>" + Constants.CONTAINER_NAME + "</name><count>0</count><bytes>0</bytes></container>";
            Assert.That(Regex.Match(xmlReturnValue.InnerXml, expectedSubString).Success || string.IsNullOrEmpty(xmlReturnValue.InnerXml), Is.True);
        }
    }
}