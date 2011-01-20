using System;
using NUnit.Framework;


namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.SetPublicContainerDetailsSpecs
{
    [TestFixture]
    public class When_marking_a_container_as_private_and_the_container_does_not_exist : TestBase
    {
        [Test]
        public void Should_occur_without_error()
        {
            if (!connection.HasCDN())
                Assert.Ignore("Provider does not support CDN Management");

            var containerList = connection.GetPublicContainers();
            Assert.That(containerList.Contains(Constants.CONTAINER_NAME), Is.False);
            connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
        }
    }

    [TestFixture]
    public class When_marking_a_container_as_private_and_it_is_private_already : TestBase
    {
        [Test]
        public void should_occur_without_error()
        {
            if (!connection.HasCDN())
                Assert.Ignore("Provider does not support CDN Management");
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_marking_a_container_as_private_and_it_is_public_already : TestBase
    {
        [Test]
        public void should_remove_it_from_the_public_containers_list()
        {
            if (!connection.HasCDN())
                Assert.Ignore("Provider does not support CDN Management");

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                Uri cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));

                var publicContainers = connection.GetPublicContainers();
                Assert.That(publicContainers.Contains(Constants.CONTAINER_NAME), Is.True, "Container was not added to public containers list");

                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);

                publicContainers = connection.GetPublicContainers();
                Assert.That(publicContainers.Contains(Constants.CONTAINER_NAME), Is.False, "Container was not removed from public containers list");
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}