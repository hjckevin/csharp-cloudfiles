using System;
using NUnit.Framework;
using Rackspace.CloudFiles.Exceptions;

namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs
{
    [TestFixture]
    public class when_purging_a_public_container : TestBase
    {
        [Test, Ignore("Can't really test this do to 45 minute delay")]
        public void Should_return_nothing_when_purge_is_successful()
        {
            //can't really test this do to 45 minute delay
        }

        [Test, Ignore("wait for response from Rackspace on why I'm not receiving a 404 as expected")]
        public void Should_throw_exception_when_the_container_does_not_exist()
        {
            Assert.Throws<ContainerNotFoundException>(() => connection.PurgePublicContainer(Guid.NewGuid().ToString()));
        }

        [Test, Ignore("Can't really test this do to 45 minute delay")]
        public void Should_not_throw_exception_when_the_container_exists_but_is_not_empty()
        {
        }
    }
}