using System;
using System.IO;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{
    [TestFixture]
    public class UnitTestCF_Object
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
        [Category("OnlyWorkWithMono")]
        public void TestSaveToFile()
        {
            Container container = new CF_Container(_conn, _client, "foo");
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
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
            _conn.UserCreds.AuthToken = "fail-get-object";
            Container container = new CF_Container(_conn, _client, "foo");
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
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
            _conn.UserCreds.AuthToken = "fail";
            Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<CloudFilesException>(() => container.GetObject("foo"));
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
            string file_name = Path.GetTempFileName();
            Assert.Throws<CloudFilesException>(() => obj.SaveToFile(file_name, true));
            File.Delete(file_name);
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
        public void TestWrite()
        {
            _conn.UserCreds.AuthToken = "put-object";
            Container container = new CF_Container(_conn, _client, "foo");
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
            obj.Write(new MemoryStream());
        }

        [Test]
        public void FailTestWrite()
        {
            _conn.UserCreds.AuthToken = "fail-put-object";
            Container container = new CF_Container(_conn, _client, "foo");
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
            obj.Retries = 1;
            obj.Write(new MemoryStream());
        }

        [Test]
        [ExpectedException(typeof (CloudFilesException))]
        public void TestWriteFail()
        {
            _conn.UserCreds.AuthToken = "fail";
            Container container = new CF_Container(_conn, _client, "foo");
            container.GetObject("foo");
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
            Assert.Throws<CloudFilesException>(() => obj.Write(new MemoryStream()));
        }

        [Test]
        public void TestWriteFailTimeout()
        {
            _conn.UserCreds.AuthToken = "fail-timeout";
            Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<TimeoutException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWriteFailNotFound()
        {
            _conn.UserCreds.AuthToken = "fail-not-found";
            Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<ObjectNotFoundException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWriteFailUnauthorized()
        {
            _conn.UserCreds.AuthToken = "fail-unauthorized";
            Container container = new CF_Container(_conn, _client, "foo");
            Assert.Throws<UnauthorizedException>(() => container.GetObject("foo"));
        }

        [Test]
        public void TestWriteFailEtag()
        {
            _conn.UserCreds.AuthToken = "fail-etag";
            Container container = new CF_Container(_conn, _client, "foo");
            container.GetObject("foo");
            StorageObject obj = new CF_Object(_conn, container, _client, "foo");
            Assert.Throws<InvalidETagException>(() => obj.Write(new MemoryStream()));
        }
    }
}