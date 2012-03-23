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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestCreateContainerFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.CreateContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestCreateContainerFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.CreateContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestCreateContainerFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.CreateContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ArgumentNullException))]
		public void TestCreateContainerArgNull()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			account.CreateContainer(null);
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetContainerFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetContainerFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.GetContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ContainerNotFoundException))]
		public void TestGetContainerFailNotFound()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-found";
			account.GetContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetContainerFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.GetContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ArgumentNullException))]
		public void TestGetContainerArgNull()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			account.GetContainer(null);
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetContainersFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetContainers();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetContainersFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.GetContainers();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetContainersFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.GetContainers();
		}
		[Test]
	    [ExpectedException(typeof(ArgumentNullException))]
		public void TestGetContainersArgNull()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			account.GetContainers(null);
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetContainerListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetContainerList();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetContainerListFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.GetContainerList();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetContainerListFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.GetContainerList();
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestPublicContainerListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetPublicContainerList();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetPublicListFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail--1";
			account.GetPublicContainerList();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetPublicListFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-401";
			account.GetPublicContainerList();
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestDeleteContainerListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            account.DeleteContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestDeleteContainerFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            account.DeleteContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestDeleteContainerFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            account.DeleteContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(ContainerNotFoundException))]
		public void TestDeleteContainerFailNotEmpty()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-found";
            account.DeleteContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ContainerNotEmptyException))]
		public void TestDeleteContainerFailNotFound()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-empty";
            account.DeleteContainer("foo");
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestUpdateMetadataListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestUpdateMetadataFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestUpdateMetadataFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            account.UpdateHeaders(new Dictionary<string, string>());
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
		[ExpectedException(typeof(CloudFilesException))]
		public void TestUpdateHeadersListFail()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestUpdateHeadersFailTimeout()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestUpdateHeadersFailUnauthorized()
		{
			var creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			var account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            account.UpdateHeaders(new Dictionary<string, string>());
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