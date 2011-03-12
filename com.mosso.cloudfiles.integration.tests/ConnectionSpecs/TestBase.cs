using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;


namespace com.mosso.cloudfiles.integration.tests
{
    public class TestBase
    {
        protected string storageUrl;
        protected string authToken;
        protected IConnection connection;

        [SetUp]
        public void SetUpBase()
        {
            var credentials = new UserCredentials(new Uri(Credentials.AUTH_ENDPOINT), Credentials.USERNAME,Credentials.API_KEY);
            var request = new GetAuthentication(credentials);
            var cfrequest = new CloudFilesRequest((HttpWebRequest) WebRequest.Create(request.CreateUri()));
            request.Apply(cfrequest);
            var response =
                new ResponseFactory().Create(cfrequest);
            
            storageUrl = response.Headers[Constants.XStorageUrl];
            authToken = response.Headers[Constants.XAuthToken];
            connection = new Connection(credentials);

            if (!connection.HasCDN()) Assert.Ignore("Provider does not support CDN Management");

            SetUp();
        }


        protected virtual void SetUp()
        {
        }
    }
}