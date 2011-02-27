using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;


namespace com.mosso.cloudfiles.unit.tests
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

            storageUrl = response.Headers[utils.Constants.X_STORAGE_URL];
            authToken = response.Headers[utils.Constants.X_AUTH_TOKEN];
            Assert.That(authToken.Length, Is.EqualTo(32));
            SetUp();
        }

        protected virtual void SetUp()
        {
        }
    }
}