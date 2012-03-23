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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			Container container = new CF_Container(conn, client, "foo");
			Assert.AreSame(container.CreateObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		public void TestGetObject()
		{
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
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