using System;
using System.IO;
using NUnit.Framework;
namespace Rackspace.Cloudfiles
{
	[TestFixture]
	public class UnitTestCF_Object
	{
		[Test]
		public void TestSaveToFile()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			Container container = new CF_Container(conn, client, "foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			string file_name = Path.GetTempFileName();
			obj.SaveToFile(file_name, true);
			StreamReader reader = File.OpenText(file_name);
			string contents =  reader.ReadToEnd();
			Assert.AreEqual(contents, "a");
			File.Delete(file_name);
		}
		[Test]
		public void TestFailSaveToObject()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-get-object";
			Container container = new CF_Container(conn, client, "foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Retries = 1;
			string file_name = Path.GetTempFileName();
			obj.SaveToFile(file_name, true);
			StreamReader reader = File.OpenText(file_name);
			string contents =  reader.ReadToEnd();
			Assert.AreEqual(contents, "a");
			File.Delete(file_name);
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestSaveToFileFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			string file_name = Path.GetTempFileName();
			obj.SaveToFile(file_name, true);
			File.Delete(file_name);
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
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			string file_name = Path.GetTempFileName();
			obj.SaveToFile(file_name, true);
			File.Delete(file_name);
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
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			string file_name = Path.GetTempFileName();
			obj.SaveToFile(file_name, true);
			File.Delete(file_name);
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
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			string file_name = Path.GetTempFileName();
			obj.SaveToFile(file_name, true);
			File.Delete(file_name);
		}
//
		[Test]
		public void TestWrite()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "put-object";
			Container container = new CF_Container(conn, client, "foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Write(new MemoryStream());
		}
		[Test]
		public void FailTestWrite()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-put-object";
			Container container = new CF_Container(conn, client, "foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Retries = 1;
			obj.Write(new MemoryStream());
		}
		[Test]
		[ExpectedException(typeof(CloudFilesException))]
		public void TestWriteFail()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
            obj.Write(new MemoryStream());
		}
		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void TestWriteFailTimeout()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-timeout";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Write(new MemoryStream());
		}
		[Test]
	    [ExpectedException(typeof(ObjectNotFoundException))]
		public void TestWriteFailNotFound()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-not-found";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Write(new MemoryStream());
		}
		[Test]
	    [ExpectedException(typeof(UnauthorizedException))]
		public void TestWriteFailUnauthorized()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-unauthorized";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Write(new MemoryStream());
		}
		[Test]
	    [ExpectedException(typeof(InvalidETagException))]
		public void TestWriteFailEtag()
		{
			UserCredentials creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Authenticate();
			conn.UserCreds.AuthToken = "fail-etag";
			Container container = new CF_Container(conn, client, "foo");
			container.GetObject("foo");
			StorageObject obj = new CF_Object(conn, container, client, "foo");
			obj.Write(new MemoryStream());
		}
	}
}