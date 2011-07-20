using System.Text.RegularExpressions;
using NUnit.Framework;
using Rackspace.CloudFiles.Exceptions;


namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs.GetPublicContainerInformationSpecs
{
    [TestFixture]
    public class When_retrieve_public_container_information_and_there_is_a_public_container : TestBase
    {
        [Test]
        public void Should_retrieve_public_container_info_when_the_container_exists_and_is_public()
        {
            if (!connection.HasCDN())
                Assert.Ignore("Provider does not support CDN Management");

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);

                var container = connection.GetPublicContainerInformation(Constants.CONTAINER_NAME);

                Assert.That(Regex.Match(container.CdnUri, @"^http:\/\/.*\..*\.cf0\.rackcdn\.com$").Success, Is.True,
                    string.Format("{0} didn't match the regex", container.CdnUri));
                Assert.That(container.ByteCount, Is.EqualTo(0));
                Assert.That(Regex.Match(container.CdnSslUri, @"^https://.*\.ssl\.cf0\.rackcdn\.com$").Success, Is.True, 
                    string.Format("{0} didn't match the regex", container.CdnSslUri));
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

    }

    [TestFixture]
    public class When_retrieve_public_container_information_and_there_is_not_a_public_container : TestBase
    {

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_container_not_found_exception()
        {
            //TODO: Follow-up with CDN developer to verify why this is returning
            //TODO: the private container information with the same name

            Assert.Ignore("READ TODO");
            connection.GetPublicContainerInformation(Constants.CONTAINER_NAME);
            Assert.Fail("Should not get container information for an non-existant container");
        }
    }

    [TestFixture]
    public class When_retrieve_public_container_information_and_the_container_is_not_public : TestBase
    {

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_container_not_found_exception()
        {
            //TODO: Follow-up with CDN developer to verify why this is returning
            //TODO: the private container information with the same name

            Assert.Ignore("READ TODO");
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);

                connection.GetPublicContainerInformation(Constants.CONTAINER_NAME);

                Assert.Fail("Should not get container information for an existing non-public container");
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}