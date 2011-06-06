using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.request.Interfaces;
using com.mosso.cloudfiles.unit.tests.CustomMatchers;
using Moq;
using NUnit.Framework;


namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetContainersSpecs
{
    public class Base_GetContainers
    {
        protected Uri uri;
        protected Dictionary<GetListParameters, string> parameters;
        protected Mock<ICloudFilesRequest> _mockrequest;

        protected Mock<ICloudFilesRequest> GetMockrequest(Dictionary<GetListParameters, string> parameters,
                                                          out Uri uri)
        {
            var getContainerItemList = new GetContainers("http://storageurl", parameters);
            var _mockrequest = new Mock<ICloudFilesRequest>();
            getContainerItemList.Apply(_mockrequest.Object);
            uri = getContainerItemList.CreateUri();
            return _mockrequest;
        }
    }

    [TestFixture]
    public class when_getting_list_of_containers_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainers(null);
        }
    }

    [TestFixture]
    public class when_getting_list_of_containers_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainers("");
        }
    }

 
    [TestFixture]
    public class when_getting_list_of_containers
    {
        private GetContainers getContainers;

        [SetUp]
        public void setup()
        {
            getContainers = new GetContainers("http://storageurl");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainers.CreateUri().ToString(), Is.EqualTo("http://storageurl/"));
        }

        [Test]
        public void should_have_a_http_get_method()
        {
            Asserts.AssertMethod(getContainers, "GET");
            
        }
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_marker_query_parameter : Base_GetContainers
    {
        [SetUp]
        public void SetUp()
        {
            parameters = new Dictionary<GetListParameters, string> { { GetListParameters.Marker, "abc" } };
            _mockrequest = GetMockrequest(parameters, out uri);
        }

        [Test]
        public void should_have_url_with_storage_url_at_beginning()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void should_have_url_with_query_string_with_marker_at_the_end()
        {
            Console.WriteLine(uri);
            uri.EndsWith("?marker=abc");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }
}