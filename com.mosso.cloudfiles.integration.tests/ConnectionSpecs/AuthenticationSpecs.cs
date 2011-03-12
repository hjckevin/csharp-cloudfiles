using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.utils;
using NUnit.Framework;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs
{
    [TestFixture]
    public class When_passing_authentication_to_the_connection
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_argument_null_exception_with_null_authentication()
        {
            new Connection(null);
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
                new Connection(new UserCredentials(Credentials.USERNAME, Credentials.API_KEY, AuthUrl.UK)), "The remote server returned an error: (401) Unauthorized.");
        }

        [Test]
        public void Should_throw_not_authorized_exception_when_credentials_are_invalid()
        {
            Assert.Throws<WebException>(() => new Connection(new UserCredentials("invalid_username", "invalid_api_key")), "The remote server returned an error: (401) Unauthorized.");
        }
    }
}