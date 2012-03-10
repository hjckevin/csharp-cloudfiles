using System.IO;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using OpenStack.Swift;

namespace Rackspace.Cloudfiles.Tests
{
	[TestFixture]
	public class TestCF_Client
	{
		[Test]
		public void TestGetCDNAccount()
		{
			Client client = new CF_Client(new FakeHttpRequestFactory());
			Dictionary<string, string> headers  = new Dictionary<string, string>();
			headers.Add("request-type", "cdn-account");
			AccountResponse res = client.GetCDNAccount("foo", "foo", headers, new Dictionary<string, string>(), false);
			Assert.AreEqual(res.Containers[0]["name"], "foo");
			Assert.AreEqual(res.Containers[0]["ttl"], "1");
			Assert.AreEqual(res.Containers[0]["cdn_url"], "http://foo.bar");
			Assert.AreEqual(res.Containers[0]["cdn_ssl_url"], "https://foo.bar");
			Assert.AreEqual(res.Containers[0]["cdn_streaming_url"], "http://foo.bar");
			Assert.AreEqual(res.Containers[0]["cdn_enabled"], "True");
			Assert.AreEqual(res.Containers[0]["log_retention"], "True");
			Assert.AreEqual(res.Status, 200);
		}
		[Test]
		[ExpectedException(typeof(ClientException))]
		public void TestGetCDNAccountFail()
		{
			Client client = new CF_Client(new FakeHttpRequestFactory());
			Dictionary<string, string> headers  = new Dictionary<string, string>();
			headers.Add("request-type", "cdn-account-fail");
			client.GetCDNAccount("foo", "foo", headers, new Dictionary<string, string>(), false);
		}
	}
    public class FakeHttpRequestFactory : IHttpRequestFactory
    {
        public IHttpRequest GetHttpRequest(string method, string url, Dictionary<string, string> headers, Dictionary<string, string> query)
        {
          return new FakeHttpRequest(method, headers);
        }
	}
    public class FakeHttpRequest : IHttpRequest
    {
        public FakeHttpRequest(string method, Dictionary<string, string> headers)
	    {
		    this._headers = headers;
			this._method = method;
		}
		private Dictionary<string, string> _headers;
		private string _method;
        public bool AllowWriteStreamBuffering{ set; get; }
		public bool SendChunked { set; get; }
		public long ContentLength { set; get; }
		public Stream GetRequestStream()
	    {
	        return new MemoryStream();
		}
		public IHttpResponse GetResponse()
		{
		    return new FakeHttpResponse(this._method, this._headers);
		}
    }
    public class FakeHttpResponse : IHttpResponse
    {
		private int _status = -1;
		public int Status { get { return this._status; } }
		private string _reason = "foo";
		public string Reason { get { return this._reason; } }
	    private Stream _stream;
		public Stream ResponseStream { get {return this._stream;} }
	    private Dictionary<string, string> _headers = new Dictionary<string, string>();
		public Dictionary<string, string> Headers { get {return this._headers;} }
		public void Close() {}
	    public FakeHttpResponse(string method, Dictionary<string, string> headers)
		{
			switch(headers["request-type"])
			{
			    case "cdn-account":
		            this._status = 200;
		    	    this._stream = this._to_stream("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
		    		                               "<account>\n" +
                                                   "<container>\n" + 
		    				                       "<name>foo</name>\n" +
		    				                       "<cdn_enabled>True</cdn_enabled>\n" +
			    			                       "<ttl>1</ttl>\n" +
			    			                       "<cdn_url>http://foo.bar</cdn_url>\n" +
			    			                       "<cdn_ssl_url>https://foo.bar</cdn_ssl_url>\n" +
			    	                               "<cdn_streaming_url>http://foo.bar</cdn_streaming_url>\n" +
			    	                               "<log_retention>True</log_retention>\n" +
				    		                       "</container>\n" +
				                                   "</account>");
				    break;
			    case "cdn-account-fail":
				    throw new ClientException("I am a teapot", 418);
			}

		}
		private Stream _to_stream(string to_stream)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(to_stream));
		}
    }
}