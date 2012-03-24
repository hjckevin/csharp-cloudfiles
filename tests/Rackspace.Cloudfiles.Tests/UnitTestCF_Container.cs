using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{
	[TestFixture]
	public class UnitTestCF_Container
	{
	    private CF_Connection _conn;
	    private FakeClient _client;

        [SetUp]
        public void Setup()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            _client = new FakeClient();
            _conn = new CF_Connection(creds, _client);
            _conn.Authenticate();
        }

		[Test]
		public void CreateObject()
		{
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.AreSame(container.CreateObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		public void TestGetObject()
		{
			_conn.UserCreds.AuthToken = "get-object";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.AreSame(container.GetObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		public void TestFailGetObject()
		{
			_conn.UserCreds.AuthToken = "fail-get-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			Assert.AreSame(container.GetObject("foo").GetType(), typeof(CF_Object));
		}
		[Test]
		public void TestGetObjectFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.Throws<CloudFilesException>(() => container.GetObject("foo"));
		}
		[Test]
		public void TestGetObjectFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.GetObject("foo"));
		}
		[Test]
		public void TestGetObjectFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.GetObject("foo"));
		}
		[Test]
		public void TestGetObjectFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.GetObject("foo"));
		}
		[Test]
		public void TestGetObjects()
		{
			_conn.UserCreds.AuthToken = "get-container";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.AreSame(container.GetObjects().GetType(), typeof(List<StorageObject>));
		}
		[Test]
		public void TestFailGetObjects()
		{
			_conn.UserCreds.AuthToken = "fail-get-container";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			Assert.AreSame(container.GetObjects().GetType(), typeof(List<StorageObject>));
		}
		[Test]
		public void TestGetObjectsFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.Throws<CloudFilesException>(() => container.GetObjects());
		}
		[Test]
		public void TestGetObjectsFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.GetObjects());
		}
		[Test]
		public void TestGetObjectsFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ContainerNotFoundException>(() => container.GetObjects());
		}
		[Test]
		public void TestGetObjectsFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.GetObjects());
		}
		[Test]
		public void TestGetObjectList()
		{
			_conn.UserCreds.AuthToken = "get-container";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.AreSame(container.GetObjectList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestFailGetObjectList()
		{
			_conn.UserCreds.AuthToken = "fail-get-container";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			Assert.AreSame(container.GetObjectList().GetType(), typeof(List<Dictionary<string, string>>));
		}
		[Test]
		public void TestGetObjectListFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.Throws<CloudFilesException>(() => container.GetObjectList());
		}
		[Test]
		public void TestGetObjectListFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.GetObjectList());
		}
		[Test]
		public void TestGetObjectListFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ContainerNotFoundException>(() => container.GetObjectList());
		}
		[Test]
		public void TestGetObjectListFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.GetObjectList());
		}
		[Test]
		public void TestDeleteObject()
		{
			_conn.UserCreds.AuthToken = "delete-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.DeleteObject("foo");
		}
		[Test]
		public void TestFailDeleteObject()
		{
			_conn.UserCreds.AuthToken = "fail-delete-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			container.DeleteObject("foo");
		}
		[Test]
		public void TestDeleteObjectFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.Throws<CloudFilesException>(() => container.DeleteObject("foo"));
		}
		[Test]
		public void TestDeleteObjectFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.DeleteObject("foo"));
		}
		[Test]
		public void TestDeleteObjectFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.DeleteObject("foo"));
		}
		[Test]
		public void TestDeleteObjectFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.DeleteObject("foo"));
		}
		[Test]
		public void TestAddMetadata()
		{
			_conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailAddMetadata()
		{
			_conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			container.AddMetadata(new Dictionary<string, string>());
		}
		[Test]
		public void TestAddMetadataFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CloudFilesException>(() => container.AddMetadata(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddMetadataFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.AddMetadata(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddMetadataFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.AddMetadata(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddMetadataFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.AddMetadata(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddHeaders()
		{
			_conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailAddHeaders()
		{
			_conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			container.AddHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestAddHeadersFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CloudFilesException>(() => container.AddHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddHeadersFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.AddHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddHeadersFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.AddHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddHeadersFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.AddHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddCDNHeaders()
		{
			_conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestFailAddCDNHeaders()
		{
			_conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			container.AddCdnHeaders(new Dictionary<string, string>());
		}
		[Test]
		public void TestAddCDNHeadersFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CloudFilesException>(() => container.AddCdnHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddCDNHeadersFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.AddCdnHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddCDNHeadersFailNotFound()
		{
			_conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CDNNotEnabledException>(() => container.AddCdnHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestAddCDNHeadersFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.AddCdnHeaders(new Dictionary<string, string>()));
		}
		[Test]
		public void TestMakePublic()
		{
			_conn.UserCreds.AuthToken = "put-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.MakePublic(999, false);
		}
		[Test]
		public void TestFailMakePublic()
		{
			_conn.UserCreds.AuthToken = "fail-put-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			container.MakePublic(999, false);
		}
		[Test]
		public void TestMakePublicFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CloudFilesException>(() => container.MakePublic(999, false));
		}
		[Test]
		public void TestMakePublicFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.MakePublic(999, false));
		}
		[Test]
		public void TestMakePublicFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.MakePublic(999, false));
		}
		[Test]
		public void TestMakePrivate()
		{
			_conn.UserCreds.AuthToken = "post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.MakePrivate();
		}
		[Test]
		public void TestFailMakePrivate()
		{
			_conn.UserCreds.AuthToken = "fail-post-object";
			Container container = new CF_Container(_conn, _client, "foo");
			container.Retries = 1;
			container.MakePrivate();
		}
		[Test]
		public void TestMakePrivateFail()
		{
			_conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CloudFilesException>(container.MakePrivate);
		}
		[Test]
		public void TestMakePrivateFailTimeout()
		{
			_conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(container.MakePrivate);
		}
		[Test]
		public void TestMakePrivateFailUnauthorized()
		{
			_conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(container.MakePrivate);
		}
		[Test]
        public void TestMembers()
		{
			_conn.UserCreds.AuthToken = "head-container";
			Container container = new CF_Container(_conn, _client, "foo");
			Assert.AreEqual(container.BytesUsed, 1);
			Assert.AreEqual(container.ObjectCount, 1);
			Assert.AreEqual(container.Metadata["foo"], "foo");
			_conn.UserCreds.AuthToken = "head-cdn-container";
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