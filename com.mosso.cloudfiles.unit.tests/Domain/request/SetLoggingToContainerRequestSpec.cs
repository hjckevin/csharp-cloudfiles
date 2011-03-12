using System;
using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.request.Interfaces;
using com.mosso.cloudfiles.unit.tests.CustomMatchers;
using Moq;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Domain.request
{
    [TestFixture]
    public class Logging_container_base
    {
        protected Mock<ICloudFilesRequest> requestmock;
        protected WebHeaderCollection webheaders;

        protected virtual void SetupApply(bool isenabled)
        {
            var loggingtopublicontainer = new SetLoggingToContainerRequest("fakecontainer", "http://fake", isenabled);
            requestmock = new Mock<ICloudFilesRequest>();
            webheaders = new WebHeaderCollection();
            requestmock.SetupGet(x => x.Headers).Returns(webheaders);
            loggingtopublicontainer.Apply(requestmock.Object);
        }
    }
    
    public class When_logging_is_not_set:Logging_container_base
    {   
        [SetUp]
        public void SetUp()
        {
            SetupApply(false);
        }

        [Test]
        public void should_set_method_to_put()
        {
            requestmock.VerifySet(x => x.Method = "POST");
        }

        [Test]
        public void should_set_X_Log_Retention_to_False()
        {
            webheaders.KeyValueFor("X-Log-Retention").HasValueOf("False");
        }
    }
    public class When_logging_is_set:Logging_container_base
    {    
        [SetUp]
        public void SetUp()
        {
            SetupApply(true);
        }
        
        [Test]
        public void should_set_method_to_put()
        {
            requestmock.VerifySet(x => x.Method = "POST");
        }
        [Test]
        public void should_set_X_Log_Retention_to_True()
        {
            webheaders.KeyValueFor("X-Log-Retention").HasValueOf("True");
        }
    }
    public class When_creating_the_uri:Logging_container_base
    {
        private Uri uri;
        private string url;
        private string container;

        [SetUp]
        public void SetUp()
        {
            container = "mycontainer";
            url = "http://myurl.com";

            var loggingtopublicontainer = new SetLoggingToContainerRequest(container, url, true);
            uri = loggingtopublicontainer.CreateUri();
        }
        [Test]
        public void should_use_a_management_url_as_the_base_url()
        {
            uri.StartsWith(url);
        }
            
        [Test]
        public void should_put_public_container_at_end_of_url()
        {
            uri.EndsWith(container);
        }
    }
}