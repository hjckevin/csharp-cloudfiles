using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{
    [TestFixture]
	public class UnitTestCF_Connection
	{
		[Test]
		public void TestAuthenticate()
		{
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			Assert.AreEqual(conn.UserCreds.UserName, "foo");
			Assert.AreEqual(conn.UserCreds.AuthUrl, "http://foo.com");
			Assert.AreEqual(conn.UserCreds.ApiKey, "auth");
			conn.Authenticate();
			Assert.AreEqual(conn.UserCreds.CdnMangementUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(conn.UserCreds.StorageUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(conn.UserCreds.AuthToken, "foo");
		}
		[Test]
		public void TestFailAuthenticate()
		{
			var creds = new UserCredentials("foo", "fail-auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			conn.Retries = 1;
			Assert.AreEqual(conn.UserCreds.UserName, "foo");
			Assert.AreEqual(conn.UserCreds.AuthUrl, "http://foo.com/");
			Assert.AreEqual(conn.UserCreds.ApiKey, "fail-auth");
			conn.Authenticate();
			Assert.AreEqual(conn.UserCreds.CdnMangementUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(conn.UserCreds.StorageUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(conn.UserCreds.AuthToken, "foo");
		}
		[Test]
		[ExpectedException(typeof(AuthenticationFailedException))]
		public void TestAuthenticateFail()
		{
			var creds = new UserCredentials("foo", "fail", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			Assert.AreEqual(conn.UserCreds.UserName, "foo");
			Assert.AreEqual(conn.UserCreds.AuthUrl, "http://foo.com/");
			Assert.AreEqual(conn.UserCreds.ApiKey, "fail");
			conn.Authenticate();
		}
		[Test]
		public void TestConnectionCheckMembers()
		{
			var creds = new UserCredentials("foo", "auth", "http://foo.com");
			Client client = new FakeClient();
			Connection conn = new CF_Connection(creds, client);
			Assert.AreEqual(conn.UserCreds.UserName, "foo");
			Assert.AreEqual(conn.UserCreds.AuthUrl, "http://foo.com/");
			Assert.AreEqual(conn.UserCreds.ApiKey, "auth");
			conn.Authenticate();
			Assert.AreEqual(conn.UserCreds.CdnMangementUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(conn.UserCreds.StorageUrl.ToString(), "https://foo.com/");
			Assert.AreEqual(conn.UserCreds.AuthToken, "foo");
			Assert.True(conn.HasCDN);
			conn.UserCreds.CdnMangementUrl = null;
			Assert.IsFalse(conn.HasCDN);
			conn.UserAgent = "foo";
			Assert.AreEqual(conn.UserAgent, "foo");
			conn.Timeout = 1;
			Assert.AreEqual(conn.Timeout, 1);
			conn.Retries = 1;
			Assert.AreEqual(conn.Retries, 1);
		}
	}
}