using System;
using System.Net;
using NUnit.Framework;
using Rackspace.CloudFiles.Domain;
using Rackspace.CloudFiles.Domain.Request;


namespace Rackspace.CloudFiles.Integration.Tests
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


        protected virtual void SetUp(){}
    }
}