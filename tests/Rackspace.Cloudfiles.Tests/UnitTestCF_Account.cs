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
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "put-container";
			Assert.AreSame(account.CreateContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestFailCreateContainer()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-put-container";
			Assert.AreSame(account.CreateContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestCreateContainerFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.CreateContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestCreateContainerFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.CreateContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestCreateContainerFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.CreateContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ArgumentNullException))]
		public void TestCreateContainerArgNull()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.CreateContainer(null);
		}
		[Test]
		public void TestGetContainer()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "head-container";
			Assert.AreSame(account.GetContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		public void TestFailGetContainer()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-head-container";
			Assert.AreSame(account.GetContainer("foo").GetType(), typeof(CF_Container));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetContainerFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetContainerFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.GetContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ContainerNotFoundException))]
		public void TestGetContainerFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-found";
			account.GetContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetContainerFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.GetContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ArgumentNullException))]
		public void TestGetContainerArgNull()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.GetContainer(null);
		}
		[Test]
		public void TestGetContainers()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainers().GetType(), typeof(List<Container>));
		}
		[Test]
		public void TestFailGetContainers()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetContainers().GetType(), typeof(List<Container>));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetContainersFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetContainers();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetContainersFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.GetContainers();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetContainersFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.GetContainers();
		}
		[Test]
	    [ExpectedException(typeof(ArgumentNullException))]
		public void TestGetContainersArgNull()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.GetContainers(null);
		}
		[Test]
		public void TestGetContainerList()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetContainerList()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetContainerListFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetContainerList();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetContainerListFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
			account.GetContainerList();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetContainerListFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
			account.GetContainerList();
		}
		[Test]
		public void TestGetPublicContainerList()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "get-account";
			Assert.AreSame(account.GetContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetPublicContainerList()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-get-account";
			Assert.AreSame(account.GetPublicContainerList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestPublicContainerListFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
			account.GetPublicContainerList();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetPublicListFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail--1";
			account.GetPublicContainerList();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetPublicListFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-401";
			account.GetPublicContainerList();
		}
		[Test]
		public void TestDeleteContainer()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "delete-container";
            account.DeleteContainer("foo");
		}
		[Test]
		public void TestFailDeleteContainer()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-delete-container";
            account.DeleteContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestDeleteContainerListFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            account.DeleteContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestDeleteContainerFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            account.DeleteContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestDeleteContainerFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            account.DeleteContainer("foo");
		}
		[Test]
		[ExpectedException(typeof(ContainerNotFoundException))]
		public void TestDeleteContainerFailNotEmpty()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-found";
            account.DeleteContainer("foo");
		}
		[Test]
	    [ExpectedException(typeof(ContainerNotEmptyException))]
		public void TestDeleteContainerFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-not-empty";
            account.DeleteContainer("foo");
		}
		[Test]
		public void TestUpdateMetadata()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "post-object";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailUpdateMetadata()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-post-object";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestUpdateMetadataListFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            account.UpdateMetadata(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestUpdateMetadataFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestUpdateMetadataFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestUpdateHeaders()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "post-object";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailUpdateHeaders()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			account.Retries = 1;
			conn.UserCreds.AuthToken = "fail-post-object";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestUpdateHeadersListFail()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestUpdateHeadersFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-timeout";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestUpdateHeadersFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			CF_Account account = new CF_Account(conn, client);
			conn.UserCreds.AuthToken = "fail-unauthorized";
            account.UpdateHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestMembers()
		{
			UserCredentials creds = new UserCredentials("foo", "foo", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "head-container";
			CF_Account account = new CF_Account(conn, client);
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