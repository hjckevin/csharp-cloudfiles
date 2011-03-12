using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.request.Interfaces;
using com.mosso.cloudfiles.unit.tests.CustomMatchers;
using Moq;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetContainerItemListSpecs
{
    public class Base_GetContainerItemList
    {
        protected Uri uri;
        protected Dictionary<GetItemListParameters, string> parameters;
        protected Mock<ICloudFilesRequest> _mockrequest;

        protected Mock<ICloudFilesRequest> GetMockrequest(Dictionary<GetItemListParameters, string> parameters,
                                                          out Uri uri)
        {
            var getContainerItemList = new GetContainerItemList("http://storageurl", "containername", parameters);
            var _mockrequest = new Mock<ICloudFilesRequest>();
            getContainerItemList.Apply(_mockrequest.Object);
            uri = getContainerItemList.CreateUri();
            return _mockrequest;
        }
    }

    [TestFixture]
    public class When_arguments_missing
    {
        [Test]
        public void should_throw_when_getting_a_list_of_items_in_a_container_and_storage_url_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetContainerItemList(null, "containername"));
        }

        [Test]
        public void should_throw_when_getting_a_list_of_items_in_a_container_and_storage_url_is_empty_string()
        {
            Assert.Throws<ArgumentNullException>(() => new GetContainerItemList(null, "containername"));
        }

        [Test]
        public void should_throw_when_getting_a_list_of_items_in_a_container_and_container_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetContainerItemList("http://storageurl", null));
        }

        [Test]
        public void when_getting_a_list_of_items_in_a_container_and_container_name_is_empty_string()
        {
            Assert.Throws<ArgumentNullException>(() => new GetContainerItemList("http://storageurl", ""));
        }
    }

    [TestFixture]
    public class When_getting_a_list_of_items_in_a_container_with_query_parameters
    {
        private Mock<ICloudFilesRequest> _mockrequest;
        private GetContainerItemList getContainerItemList;
        private Uri uri;

        [SetUp]
        public void SetUp()
        {
            getContainerItemList = new GetContainerItemList("http://storageurl", "containername");
            uri = getContainerItemList.CreateUri();
            _mockrequest = new Mock<ICloudFilesRequest>();
            getContainerItemList.Apply(_mockrequest.Object);
        }

        [Test]
        public void url_should_have_storage_url_at_beginning()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void url_should_have_container_name_at_the_end()
        {
            uri.EndsWith("containername");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }

    public class When_getting_a_list_of_items_in_a_container_with_limit_query_parameter : Base_GetContainerItemList
    {
        [SetUp]
        public void Setup()
        {
            parameters = new Dictionary<GetItemListParameters, string> {{GetItemListParameters.Limit, "2"}};
            _mockrequest = GetMockrequest(parameters, out uri);
        }

        [Test]
        public void url_should_have_storage_url_at_beginning()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void url_should_have_container_name_followed_by_query_string_and_limit_at_the_end()
        {
            uri.EndsWith("containername?limit=2");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }

    [TestFixture]
    public class when_getting_a_list_of_items_in_a_container_with_marker_query_parameter : Base_GetContainerItemList
    {
        [SetUp]
        public void SetUp()
        {
            parameters = new Dictionary<GetItemListParameters, string> {{GetItemListParameters.Marker, "abc"}};
            _mockrequest = GetMockrequest(parameters, out uri);
        }

        [Test]
        public void should_have_url_with_storage_url_at_beginning()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void should_have_url_with_container_name_followed_by_query_string_with_marker_at_the_end()
        {
            uri.EndsWith("containername?marker=abc");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }

    [TestFixture]
    public class When_getting_a_list_of_items_in_a_container_with_prefix_query_parameter : Base_GetContainerItemList
    {
        [SetUp]
        public void SetUp()
        {
            parameters = new Dictionary<GetItemListParameters, string> {{GetItemListParameters.Prefix, "a"}};
            _mockrequest = GetMockrequest(parameters, out uri);
        }

        [Test]
        public void should_have_url_with_storage_url_at_beginning()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void should_have_url_with_container_name_followed_by_query_string_with_prefix_at_the_end()
        {
            uri.EndsWith("containername?prefix=a");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }

    [TestFixture]
    public class When_getting_a_list_of_items_in_a_container_with_path_query_parameter : Base_GetContainerItemList
    {
        [SetUp]
        public void SetUp()
        {
            parameters = new Dictionary<GetItemListParameters, string>
                             {{GetItemListParameters.Path, "dir1/subdir2/"}};

            _mockrequest = GetMockrequest(parameters, out uri);
        }

        [Test]
        public void should_have_url_with_storage_url_at_beginnining()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void should_have_url_with_container_name_followed_by_query_string_with_path_at_the_end()
        {
            uri.EndsWith("containername?path=dir1/subdir2/");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }

    [TestFixture]
    public class When_getting_a_list_of_items_in_a_container_with_more_than_one_query_parameter :
        Base_GetContainerItemList
    {
        [SetUp]
        public void SetUp()
        {
            parameters = new Dictionary<GetItemListParameters, string>
                             {
                                 {GetItemListParameters.Limit, "2"},
                                 {GetItemListParameters.Marker, "abc"},
                                 {GetItemListParameters.Prefix, "a"},
                                 {GetItemListParameters.Path, "dir1/subdir2/"}
                             };
            _mockrequest = GetMockrequest(parameters, out uri);
        }

        [Test]
        public void should_have_url_with_storage_url_at_beginning()
        {
            uri.StartsWith("http://storageurl");
        }

        [Test]
        public void should_have_url_with_container_name_followed_by_query_strings()
        {
            uri.EndsWith("containername?limit=2&marker=abc&prefix=a&path=dir1/subdir2/");
        }

        [Test]
        public void should_use_HTTP_GET_method()
        {
            _mockrequest.VerifySet(x => x.Method = "GET");
        }
    }
}