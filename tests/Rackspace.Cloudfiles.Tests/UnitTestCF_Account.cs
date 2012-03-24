using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{    
	[TestFixture]
	public class UnitTestCF_Account
	{
		[Test]
		public void TestCreateContainer()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "put-container";
			Assert.AreSame(account.CreateContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestFailCreateContainer()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-put-container";
			Assert.AreSame(account.CreateContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestCreateContainerFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			Assert.Throws<CloudFilesException>(() => account.CreateContainer("foo"));
		}
		[Test]
		public void TestCreateContainerFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.CreateContainer("foo"));
		}
		[Test]
		public void TestCreateContainerFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.CreateContainer("foo"));
		}
		[Test]
		public void TestCreateContainerArgNull()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
            Assert.Throws<ArgumentNullException>(() => account.CreateContainer(null));
		}
		[Test]
		public void TestGetContainer()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "head-container";
			Assert.AreSame(account.GetContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestFailGetContainer()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-head-container";
			Assert.AreSame(account.GetContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestGetContainerFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			Assert.Throws<CloudFilesException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerFailNotFound()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-found";
            Assert.Throws<ContainerNotFoundException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.GetContainer("foo"));
		}
		[Test]
		public void TestGetContainerArgNull()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
            Assert.Throws<ArgumentNullException>(() => account.GetContainer(null));
		}
		[Test]
		public void TestGetContainers()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainers().GetType(), typeof(List<Container>));
		}
		[Test]
		public void TestFailGetContainers()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetContainers().GetType(), typeof(List<Container>));
		}
		[Test]
		public void TestGetContainersFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			Assert.Throws<CloudFilesException>(() => account.GetContainers());
		}
		[Test]
		public void TestGetContainersFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.GetContainers());
		}
		[Test]
		public void TestGetContainersFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.GetContainers());
		}
		[Test]
		public void TestGetContainersArgNull()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
            Assert.Throws<ArgumentNullException>(() => account.GetContainers(null));
		}
		[Test]
		public void TestGetContainerList()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetContainerList()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestGetContainerListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.GetContainerList());
		}
		[Test]
		public void TestGetContainerListFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.GetContainerList());
		}
		[Test]
		public void TestGetContainerListFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.GetContainerList());
		}
		[Test]
		public void TestGetPublicContainerList()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetPublicContainerList()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetPublicContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestPublicContainerListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.GetPublicContainerList());
		}
		[Test]
		public void TestGetPublicListFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail--1";
            Assert.Throws<TimeoutException>(() => account.GetPublicContainerList());
		}
		[Test]
		public void TestGetPublicListFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-401";
            Assert.Throws<UnauthorizedException>(() => account.GetPublicContainerList());
		}
		[Test]
		public void TestDeleteContainer()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "delete-container";
            account.DeleteContainer("foo");
		}
		[Test]
		public void TestFailDeleteContainer()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-delete-container";
            account.DeleteContainer("foo");
		}
		[Test]
		public void TestDeleteContainerListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailNotEmpty()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-found";
            Assert.Throws<ContainerNotFoundException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestDeleteContainerFailNotFound()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-empty";
            Assert.Throws<ContainerNotEmptyException>(() => account.DeleteContainer("foo"));
		}
		[Test]
		public void TestUpdateMetadata()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "post-object";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailUpdateMetadata()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-post-object";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestUpdateMetadataListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.UpdateMetadata(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateMetadataFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateMetadataFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateHeaders()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "post-object";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailUpdateHeaders()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
		    var account = new CF_Account(conn, client) {Retries = 1};
		    conn.UserCreds.AuthToken = "fail-post-object";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestUpdateHeadersListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            Assert.Throws<CloudFilesException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateHeadersFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            Assert.Throws<TimeoutException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestUpdateHeadersFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            Assert.Throws<UnauthorizedException>(() => account.UpdateHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestMembers()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "head-container";
			var account = new CF_Account(conn, client);
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