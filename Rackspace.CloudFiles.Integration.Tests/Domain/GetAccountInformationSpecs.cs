using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain.Request;


namespace Rackspace.CloudFiles.Integration.Tests.domain.GetAccountSpecs
{
    [TestFixture]
  
    public class When_querying_for_account : TestBase
    {
        [Test]
        public void should_return_number_of_containers_and_bytes_used()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                var getAccountInformation = new GetAccountInformation(storageUrl);
                var response =new GenerateRequestByType().Submit(getAccountInformation, authToken);
                Assert.That(response.Headers[Constants.XAccountBytesUsed], Is.Not.Null);
                Assert.That(response.Headers[Constants.XAccountContainerCount], Is.Not.Null);
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }
    }

    [TestFixture]
    public class When_querying_for_account_and_the_account_does_not_exist : TestBase
    {
        [Test]
        [Ignore("FIX ME, getting 500 internal server error")]
        public void should_return_401_unauthorized()
        {
            var exceptionThrown = false;

            try
            {
                var lastGroup = storageUrl.Substring(storageUrl.LastIndexOf('-')+1);
                var badStorageUrl = storageUrl.Replace(lastGroup, new string('f', lastGroup.Length));
                var getAccountInformation = new GetAccountInformation(badStorageUrl);
                new GenerateRequestByType().Submit(getAccountInformation, authToken);
            }
            catch (WebException we)
            {
                exceptionThrown = true;
                var response = (HttpWebResponse)we.Response;
                if (response.StatusCode != HttpStatusCode.Unauthorized) Assert.Fail("Should be a 401 error");
            }

            Assert.That(exceptionThrown, Is.True);
        }
    }

    [TestFixture]
    public class When_querying_for_account_and_account_has_no_containers : TestBase
    {
        [Test]
        public void should_return_204_no_content_when_the_account_has_no_containers()
        {
            var getAccountInformation = new GetAccountInformation(storageUrl);
            var response = new GenerateRequestByType().Submit(getAccountInformation, authToken);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }

    [TestFixture]
    public class When_querying_for_account_in_json_format_and_container_exists : TestBase
    {
        [Test]
        public void should_return_account_information_in_json_format_including_name_count_and_bytes()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemName);

                    var getAccountInformationJson = new GetAccountInformationSerialized(storageUrl,  Format.JSON);
                    var getAccountInformationJsonResponse = new GenerateRequestByType().Submit(getAccountInformationJson, authToken);

                    if(getAccountInformationJsonResponse.ContentBody.Count == 0)
                        Assert.Fail("No content body returned in response");

                    const string expectedSubString = "{\"name\":[ ]?\"" + Constants.CONTAINER_NAME + "\",[ ]?\"count\":[ ]?\\d+,[ ]?\"bytes\":[ ]?\\d+}";
                    var contentBody = getAccountInformationJsonResponse.ContentBody;
                    getAccountInformationJsonResponse.Dispose();
                    foreach (var s in contentBody)
                    {
                        if (Regex.Match(s, expectedSubString).Success) return;  
                    }

                    Assert.Fail("Expected value: " + expectedSubString + " not found");
               
                }
                finally
                {

                    testHelper.DeleteItemFromContainer();
                }
            }    
        }
    }


    [TestFixture]
    public class When_querying_for_account_in_json_format_and_no_containers_exist : TestBase
    {
        [Test, Ignore("Only works if account has no pre-existing containers")]
        public void should_return_empty_brackets_and_ok_status_200()
        {
            var getAccountInformationJson = new GetAccountInformationSerialized(storageUrl,  Format.JSON);
            var getAccountInformationJsonResponse = new GenerateRequestByType().Submit(getAccountInformationJson, authToken);
            Assert.That(getAccountInformationJsonResponse.Status, Is.EqualTo(HttpStatusCode.OK));
            var contentBody = String.Join("",getAccountInformationJsonResponse.ContentBody.ToArray());
            getAccountInformationJsonResponse.Dispose();

            Assert.That(contentBody, Is.EqualTo("[]"));        
        }
    }

    [TestFixture]
    public class When_querying_for_account_in_xml_format_and_container_exists : TestBase
    {
        [Test]
        public void should_return_account_information_in_xml_format_including_name_count_and_size()
        {
            
            using (var testHelper = new TestHelper(authToken, storageUrl))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemName);

                    var accountInformationXml = new GetAccountInformationSerialized(storageUrl,  Format.XML);
                    var getAccountInformationXmlResponse = new GenerateRequestByType().Submit(accountInformationXml,authToken);

                    if (getAccountInformationXmlResponse.ContentBody.Count == 0)
                        Assert.Ignore("No content body returned in response");

                    var contentBody = "";
                    foreach (var s in getAccountInformationXmlResponse.ContentBody)
                    {
                        contentBody += s;
                    }

                    getAccountInformationXmlResponse.Dispose();
                    var xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(contentBody);
                    }
                    catch(XmlException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    const string expectedSubString = "<container><name>"+ Constants.CONTAINER_NAME +"</name><count>\\d*</count><bytes>\\d+</bytes></container>";
                    Assert.That(Regex.Match(contentBody, expectedSubString).Success, Is.True);
                }
                finally
                {
                    testHelper.DeleteItemFromContainer();
                }
            }
        } 
    }

    [TestFixture]
    public class When_querying_for_account_in_xml_format_and_no_container_exists : TestBase
    {
        [Test, Ignore("Only works if account has no pre-existing containers")]
        public void should_return_account_name_and_ok_status_200()
        {
            var accountInformationXml = new GetAccountInformationSerialized(storageUrl, Format.XML);
            var getAccountInformationXmlResponse =new GenerateRequestByType().Submit(accountInformationXml, authToken);
            Assert.That(getAccountInformationXmlResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            var contentBody = "";
            foreach (var s in getAccountInformationXmlResponse.ContentBody)
            {
                contentBody += s;
            }

            getAccountInformationXmlResponse.Dispose();
            const string expectedSubString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"></account>";
            Assert.That(contentBody, Is.EqualTo(expectedSubString));
        }
    }
}