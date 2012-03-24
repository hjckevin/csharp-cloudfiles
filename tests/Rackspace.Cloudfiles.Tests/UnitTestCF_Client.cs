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
			var headers  = new Dictionary<string, string> {{"request-type", "cdn-account"}};
		    var res = client.GetCDNAccount("foo", "foo", headers, new Dictionary<string, string>(), false);
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
		public void TestGetCDNAccountFail()
		{
			Client client = new CF_Client(new FakeHttpRequestFactory());
			var headers  = new Dictionary<string, string> {{"request-type", "cdn-account-fail"}};
		    Assert.Throws<ClientException>(() => client.GetCDNAccount("foo", "foo", headers, new Dictionary<string, string>(), false));
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
		    _headers = headers;
			_method = method;
		}
		private readonly Dictionary<string, string> _headers;
		private readonly string _method;
        public bool AllowWriteStreamBuffering{ set; get; }
		public bool SendChunked { set; get; }
		public long ContentLength { set; get; }
		public Stream GetRequestStream()
	    {
	        return new MemoryStream();
		}
		public IHttpResponse GetResponse()
		{
		    return new FakeHttpResponse(_method, _headers);
		}
    }
    public class FakeHttpResponse : IHttpResponse
    {
        private readonly int _status = -1;
		public int Status { get { return _status; } }
        private const string _reason = "foo";
        public string Reason { get { return _reason; } }
	    private readonly Stream _stream;
		public Stream ResponseStream { get {return _stream;} }
	    private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
		public Dictionary<string, string> Headers { get {return _headers;} }
		public void Close() {}
	    public FakeHttpResponse(string method, Dictionary<string, string> headers)
	    {
	        Method = method;
	        switch(headers["request-type"])
			{
			    case "cdn-account":
		            _status = 200;
		    	    _stream = _to_stream("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
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

        public string Method { get; set; }

        private Stream _to_stream(string to_stream)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(to_stream));
		}
    }
}