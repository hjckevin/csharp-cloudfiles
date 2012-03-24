using System;
using System.IO;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{
    [TestFixture]
    public class UnitTestCF_Object
    {
        [Test]
        [Category("OnlyWorkWithMono")]
        public void TestSaveToFile()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            Container container = new CF_Container(conn, client, "foo");
            StorageObject obj = new CF_Object(conn, container, client, "foo");
            var file_name = Path.GetTempFileName();
            obj.SaveToFile(file_name, true);
            var reader = File.OpenText(file_name);
            var contents = reader.ReadToEnd();
            Assert.AreEqual(contents, "a");
            File.Delete(file_name);
        }

        [Test]
        [Category("OnlyWorkWithMono")]
        public void TestFailSaveToObject()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
            string contents = reader.ReadToEnd();
            Assert.AreEqual(contents, "a");
            File.Delete(file_name);
        }

        [Test]
        [Category("OnlyWorkWithMono")]
        public void TestSaveToFileFail()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<CloudFilesException>(() => container.GetObject("foo"));
            StorageObject obj = new CF_Object(conn, container, client, "foo");
            string file_name = Path.GetTempFileName();
            Assert.Throws<CloudFilesException>(() => obj.SaveToFile(file_name, true));
            File.Delete(file_name);
        }

        [Test]
        public void TestGetObjectFailTimeout()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-timeout";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<TimeoutException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestGetObjectFailNotFound()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-not-found";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestGetObjectFailUnauthorized()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-unauthorized";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWrite()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
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
        [ExpectedException(typeof (CloudFilesException))]
        public void TestWriteFail()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail";
            Container container = new CF_Container(conn, client, "foo");
            container.GetObject("foo");
            StorageObject obj = new CF_Object(conn, container, client, "foo");
            Assert.Throws<CloudFilesException>(() => obj.Write(new MemoryStream()));
        }

        [Test]
        public void TestWriteFailTimeout()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-timeout";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<TimeoutException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWriteFailNotFound()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-not-found";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWriteFailUnauthorized()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-unauthorized";
            Container container = new CF_Container(conn, client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWriteFailEtag()
        {
            var creds = new UserCredentials("foo", "auth", "http://foo.com");
            Client client = new FakeClient();
            Connection conn = new CF_Connection(creds, client);
            conn.Authenticate();
            conn.UserCreds.AuthToken = "fail-etag";
            Container container = new CF_Container(conn, client, "foo");
            container.GetObject("foo");
            StorageObject obj = new CF_Object(conn, container, client, "foo");
            Assert.Throws<InvalidETagException>(() => obj.Write(new MemoryStream()));
        }
    }
}