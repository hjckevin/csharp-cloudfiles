using System;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain;
using Rackspace.CloudFiles.Domain.Request;


namespace Rackspace.CloudFiles.Unit.Tests
{
    public class TestBase
    {
        protected string storageUrl;
        protected string authToken;

        [SetUp]
        public void SetUpBase()
        {
            var uri = new Uri(Constants.AUTH_URL);

            var request =
                new GetAuthentication(
                    new UserCredentials(
                        uri,
                        Constants.CREDENTIALS_USER_NAME,
                        Constants.CREDENTIALS_PASSWORD,
                        Constants.CREDENTIALS_CLOUD_VERSION,
                        Constants.CREDENTIALS_ACCOUNT_NAME));

            var response = new GenerateRequestByType().Submit(request, authToken);
                ;

            storageUrl = response.Headers[CloudFiles.Utils.Constants.X_STORAGE_URL];
            authToken = response.Headers[CloudFiles.Utils.Constants.X_AUTH_TOKEN];
            Assert.That(authToken.Length, Is.EqualTo(32));
            SetUp();
        }

        protected virtual void SetUp()
        {
        }
    }
}