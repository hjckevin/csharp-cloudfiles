using System;
using System.Collections;
using System.Net;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain;
using Rackspace.CloudFiles.Utils;

namespace Rackspace.CloudFiles.Integration.Tests.ConnectionSpecs
{
    [TestFixture]
    public class When_passing_authentication_to_the_connection
    {
        [Test]
        public void Should_throw_argument_null_exception_with_null_authentication()
        {
            Assert.Throws<ArgumentNullException>(() => new Connection(null));
        }

        [Test]
        public void Should_instantiate_engine_without_throwing_exception_when_authentication_passes()
        {
            new Connection(new UserCredentials(new Uri(Credentials.AUTH_ENDPOINT), Credentials.USERNAME, Credentials.API_KEY));
        }

        [Test]
        public void Should_authenticate_when_specific_auth_url_type_provided()
        {
            new Connection(new UserCredentials(Credentials.USERNAME, Credentials.API_KEY, AuthUrl.US));
        }

        [Test]
        public void Should_authenticate_when_uk_auth_url_type_provided()
        {
            Assert.Throws<WebException>(() => 
                new Connection(new UserCredentials(Credentials.USERNAME, Credentials.API_KEY, AuthUrl.UK)), 
                "The remote server returned an error: (401) Unauthorized.");
        }

        [Test]
        public void Should_throw_not_authorized_exception_when_credentials_are_invalid()
        {
            Assert.Throws<WebException>(() => new Connection(new UserCredentials("invalid_username", "invalid_api_key")), 
                "The remote server returned an error: (401) Unauthorized.");
        }
    }
}