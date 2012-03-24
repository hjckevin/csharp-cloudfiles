using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{    
	[TestFixture]
	public class UnitTestCF_Account
	{
	    private CF_Connection _conn;
	    private FakeClient _client;

        [SetUp]
        public void Setup()
        {
            var creds = new UserCredentials("foo", "foo", "http://foo.com");
            _client = new FakeClient();
            _conn = new CF_Connection(creds, _client);
            _conn.Authenticate();
        }

		[Test]
		public void TestCreateContainer()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "put-container";
			Assert.AreSame(account.CreateContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestFailCreateContainer()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-put-container";
			Assert.AreSame(account.CreateContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestCreateContainerFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
			Assert.Throws<CloudFilesException>(() => account.CreateContainer("foo"));
		}
		[Test]
		public void TestCreateContainerFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.CreateContainer("foo"));
		}
		[Test]
		public void TestCreateContainerFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.CreateContainer("foo"));
		}
		[Test]
		public void TestCreateContainerArgNull()
		{
			var account = new CF_Account(_conn, _client);
            Assert.Throws<ArgumentNullException>(() => account.CreateContainer(null));
		}
		[Test]
		public void TestGetContainer()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "head-container";
			Assert.AreSame(account.GetContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestFailGetContainer()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-head-container";
			Assert.AreSame(account.GetContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestGetContainerFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
			var ex = Assert.Throws<CloudFilesException>(() => account.GetContainer("foo"));
            Assert.That(ex.Message, Is.EqualTo("Error: 500"));
		}
		[Test]
		public void TestGetContainerFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerFailNotFound()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-not-found";
            Assert.Throws<ContainerNotFoundException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerArgNull()
		{
			var account = new CF_Account(_conn, _client);
            Assert.Throws<ArgumentNullException>(() => account.GetContainer(null));
		}
		[Test]
		public void TestGetContainers()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainers().GetType(), typeof(List<Container>));
		}
		[Test]
		public void TestFailGetContainers()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetContainers().GetType(), typeof(List<Container>));
		}
		[Test]
		public void TestGetContainersFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
			Assert.Throws<CloudFilesException>(() => account.GetContainers());
		}
		[Test]
		public void TestGetContainersFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.GetContainers());
		}
		[Test]
		public void TestGetContainersFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.GetContainers());
		}
		[Test]
		public void TestGetContainersArgNull()
		{
			var account = new CF_Account(_conn, _client);
            Assert.Throws<ArgumentNullException>(() => account.GetContainers(null));
		}
		[Test]
		public void TestGetContainerList()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetContainerList()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestGetContainerListFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.GetContainerList());
		}
		[Test]
		public void TestGetContainerListFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.GetContainerList());
		}
		[Test]
		public void TestGetContainerListFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.GetContainerList());
		}
		[Test]
		public void TestGetPublicContainerList()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetPublicContainerList()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetPublicContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestPublicContainerListFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.GetPublicContainerList());
		}
		[Test]
		public void TestGetPublicListFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail--1";
            Assert.Throws<TimeoutException>(() => account.GetPublicContainerList());
		}
		[Test]
		public void TestGetPublicListFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-401";
            Assert.Throws<UnauthorizedException>(() => account.GetPublicContainerList());
		}
		[Test]
		public void TestDeleteContainer()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "delete-container";
            account.DeleteContainer("foo");
		}
		[Test]
		public void TestFailDeleteContainer()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-delete-container";
            account.DeleteContainer("foo");
		}
		[Test]
		public void TestDeleteContainerListFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailNotEmpty()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-not-found";
            Assert.Throws<ContainerNotFoundException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailNotFound()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-not-empty";
            Assert.Throws<ContainerNotEmptyException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestUpdateMetadata()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "post-object";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailUpdateMetadata()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-post-object";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestUpdateMetadataListFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.UpdateMetadata(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateMetadataFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateMetadataFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateHeaders()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "post-object";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailUpdateHeaders()
		{
		    var account = new CF_Account(_conn, _client) {Retries = 1};
		    _conn.UserCreds.AuthToken = "fail-post-object";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestUpdateHeadersListFail()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateHeadersFailTimeout()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateHeadersFailUnauthorized()
		{
			var account = new CF_Account(_conn, _client);
			_conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestMembers()
		{
			_conn.UserCreds.AuthToken = "head-container";
			var account = new CF_Account(_conn, _client);
			Assert.AreEqual(account.Metadata["foo"], "foo");
			Assert.AreEqual(account.Headers["blah"], "foo");
			Assert.AreEqual(account.BytesUsed, 1);
			Assert.AreEqual(account.ObjectCount, 1);
			Assert.AreEqual(account.ContainerCount, 1);
			Assert.AreEqual(account.StorageUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(account.CdnManagementUrl.ToString(), "https://foo.com/");
		}
	}
}