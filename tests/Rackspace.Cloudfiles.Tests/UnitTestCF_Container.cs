using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{
	[TestFixture]
	public class UnitTestCF_Container
	{
		[Test]
		public void CreateObject()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			Container container = new CF_Container(conn, client, "foo");
			Assert.AreSame(container.CreateObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		public void TestGetObject()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "get-object";
			Container container = new CF_Container(conn, client, "foo");
			Assert.AreSame(container.GetObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		public void TestFailGetObject()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-get-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			Assert.AreSame(container.GetObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetObjectFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetObjectFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
		}
		[Test]
	    [ExpectedException(typeof(ObjectNotFoundException))]
		public void TestGetObjectFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetObjectFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
		}
//
		[Test]
		public void TestGetObjects()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "get-container";
			Container container = new CF_Container(conn, client, "foo");
			Assert.AreSame(container.GetObjects().GetType(), typeof(List<StorageObject>));
		}
		[Test]
		public void TestFailGetObjects()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-get-container";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			Assert.AreSame(container.GetObjects().GetType(), typeof(List<StorageObject>));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetObjectsFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjects();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetObjectsFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjects();
		}
		[Test]
	    [ExpectedException(typeof(ContainerNotFoundException))]
		public void TestGetObjectsFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjects();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetObjectsFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjects();
		}
//
		[Test]
		public void TestGetObjectList()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "get-container";
			Container container = new CF_Container(conn, client, "foo");
			Assert.AreSame(container.GetObjectList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetObjectList()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-get-container";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			Assert.AreSame(container.GetObjectList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestGetObjectListFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjectList();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestGetObjectListFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjectList();
		}
		[Test]
	    [ExpectedException(typeof(ContainerNotFoundException))]
		public void TestGetObjectListFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjectList();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestGetObjectListFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObjectList();
		}
//
		[Test]
		public void TestDeleteObject()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "delete-object";
			Container container = new CF_Container(conn, client, "foo");
			container.DeleteObject("foo");
		}
		[Test]
		public void TestFailDeleteObject()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-delete-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			container.DeleteObject("foo");
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestDeleteObjectFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.DeleteObject("foo");
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestDeleteObjectFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.DeleteObject("foo");
		}
		[Test]
	    [ExpectedException(typeof(ObjectNotFoundException))]
		public void TestDeleteObjectFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.DeleteObject("foo");
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestDeleteObjectFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.DeleteObject("foo");
		}
//
		[Test]
		public void TestAddMetadata()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailAddMetadata()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestAddMetadataFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestAddMetadataFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(ObjectNotFoundException))]
		public void TestAddMetadataFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestAddMetadataFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.AddMetadata(new Dictionary<string, string>());
		}
//
		[Test]
		public void TestAddHeaders()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailAddHeaders()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestAddHeadersFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestAddHeadersFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(ObjectNotFoundException))]
		public void TestAddHeadersFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestAddHeadersFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.AddHeaders(new Dictionary<string, string>());
		}
//
		[Test]
		public void TestAddCDNHeaders()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailAddCDNHeaders()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestAddCDNHeadersFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestAddCDNHeadersFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(CDNNotEnabledException))]
		public void TestAddCDNHeadersFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestAddCDNHeadersFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
//
		[Test]
		public void TestMakePublic()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "put-object";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePublic(999, false);
		}
		[Test]
		public void TestFailMakePublic()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-put-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			container.MakePublic(999, false);
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestMakePublicFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePublic(999, false);
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestMakePublicFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePublic(999, false);
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestMakePublicFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePublic(999, false);
		}
//
		[Test]
		public void TestMakePrivate()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePrivate();
		}
		[Test]
		public void TestFailMakePrivate()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(conn, client, "foo");
			container.Retries = 1;
			container.MakePrivate();
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestMakePrivateFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePrivate();
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestMakePrivateFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePrivate();
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestMakePrivateFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.MakePrivate();
		}
		[Test]
        public void TestMembers()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "head-container";
			Container container = new CF_Container(conn, client, "foo");
			Assert.AreEqual(container.BytesUsed, 1);
			Assert.AreEqual(container.ObjectCount, 1);
			Assert.AreEqual(container.Metadata["foo"], "foo");
			conn.UserCreds.AuthToken = "head-cdn-container";
			Assert.AreEqual(container.TTL, 999);
		    Assert.AreEqual(container.CdnEnabled, true);
			Assert.AreEqual(container.CdnLogRetention, true);
			Assert.AreEqual(container.CdnSslUri.ToString(), "https://foo.com/");
			Assert.AreEqual(container.CdnStreamingUri.ToString(), "http://foo.com/");
			Assert.AreEqual(container.CdnUri.ToString(), "http://foo.com/");
			Assert.AreEqual(container.StorageUrl.ToString(), "https://foo.com/foo");
			Assert.AreEqual(container.CdnManagementUrl.ToString(), "https://foo.com/foo");
		}
	}
}