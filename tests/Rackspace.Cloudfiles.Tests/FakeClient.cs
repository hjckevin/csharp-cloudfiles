using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenStack.Swift;

namespace Rackspace.Cloudfiles.Tests
{
	public class FakeClient : Client 
	{
	    private int _retries;
		public override AuthResponse GetAuth(string url, string user, string key, Dictionary<string, string> headers, Dictionary<string, string> query, bool snet)
		{
		    Dictionary<string, string> rheaders;
		    switch (key)
			{
			    case "fail-auth":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-auth-token", "foo"},
					                       {"x-storage-url", "https://foo.com"},
					                       {"x-cdn-management-url", "https://foo.com"}
					                   };
				        return new AuthResponse(rheaders, "Foo", 201);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    default:
				    rheaders = new Dictionary<string, string>
				                   {
				                       {"x-auth-token", "foo"},
				                       {"x-storage-url", "https://foo.com"},
				                       {"x-cdn-management-url", "https://foo.com"}
				                   };
			        return new AuthResponse(rheaders, "Foo", 201);
			}
		}

	    public override AccountResponse GetAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
	    {
	        List<Dictionary<string, string>> containers;
	        Dictionary<string, string> rheaders;
	        switch (token)
			{
			    case "fail-get-account":
				    if (_retries == 1)
				    {
					    _retries = 0;
	                    containers = new List<Dictionary<string, string>>();
					    var container = new Dictionary<string, string> {{"name", "foo"}};
				        containers.Add(container);
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-account-container-count", "1"},
					                       {"x-account-object-count", "1"},
					                       {"x-account-bytes-used", "1"},
					                       {"x-account-meta-foo", "foo"}
					                   };
				        return new AccountResponse(rheaders, "Foo", 200, containers);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
	                    containers = new List<Dictionary<string, string>> {new Dictionary<string, string> {{"name", "foo"}}};
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-account-container-count", "1"},
					                       {"x-account-object-count", "1"},
					                       {"x-account-bytes-used", "1"},
					                       {"x-account-meta-foo", "foo"}
					                   };
			        return new AccountResponse(rheaders, "Foo", 200, containers);  
			}
	    }

	    public override AccountResponse HeadAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query)
	    {
	        Dictionary<string, string> rheaders;
	        switch (token)
			{
			    case "fail-head-account":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-account-container-count", "1"},
					                       {"x-account-object-count", "1"},
					                       {"x-account-bytes-used", "1"},
					                       {"x-account-meta-foo", "foo"},
					                       {"blah", "foo"}
					                   };
				        return new AccountResponse(rheaders, "Foo", 201, null);
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				    rheaders = new Dictionary<string, string>
				                   {
				                       {"x-account-container-count", "1"},
				                       {"x-account-object-count", "1"},
				                       {"x-account-bytes-used", "1"},
				                       {"x-account-meta-foo", "foo"},
				                       {"blah", "foo"}
				                   };
			        return new AccountResponse(rheaders, "Foo", 201, null);
			}
	    }

	    public override AccountResponse PostAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
			switch (token)
			{
			    case "fail-post-account":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new AccountResponse(new Dictionary<string, string>(), "Foo", 201, null);
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				    return new AccountResponse(new Dictionary<string, string>(), "Foo", 201, null);
			}
		}
		public override ContainerResponse GetContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
		{
		    List<Dictionary<string, string>> objects;
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-get-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    objects = new List<Dictionary<string, string>>();
				        var robject = new Dictionary<string, string> {{"name", "foo"}};
				        objects.Add(robject);
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-container-object-count", "1"},
					                       {"x-container-bytes-used", "1"},
					                       {"x-container-meta-foo", "foo"}
					                   };
				        return new ContainerResponse(rheaders, "Foo", 201, objects);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
					    objects = new List<Dictionary<string, string>>{new Dictionary<string, string> {{"name", "foo"}}};
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-container-object-count", "1"},
					                       {"x-container-bytes-used", "1"},
					                       {"x-container-meta-foo", "foo"}
					                   };
			        return new ContainerResponse(rheaders, "Foo", 201, objects);    
			}
		}

	    public override ContainerResponse HeadContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
	    {
	        Dictionary<string, string> rheaders;
	        switch (token)
			{
			    case "fail-head-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-versions-location", "foo"},
						                   {"x-container-meta-web-index", "foo"},
						                   {"x-container-meta-web-listings-css", "foo"},
						                   {"x-container-meta-web-error", "foo"},
						                   {"x-container-meta-web-listings", "true"},
					                       {"x-container-object-count", "1"},
					                       {"x-container-bytes-used", "1"},
					                       {"x-container-meta-foo", "foo"}
					                   };
				        return new ContainerResponse(rheaders, "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail-head-cdn-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-cdn-uri", "http://foo.com"},
					                       {"x-cdn-ssl-uri", "https://foo.com"},
					                       {"x-cdn-streaming-uri", "http://foo.com"},
					                       {"x-ttl", "foo"},
					                       {"x-cdn-enabled", "true"},
					                       {"x-log-retention", "true"}
					                   };
				        return new ContainerResponse(rheaders, "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "head-cdn-container":
				    rheaders = new Dictionary<string, string>
				                   {
				                       {"x-cdn-uri", "http://foo.com"},
				                       {"x-cdn-ssl-uri", "https://foo.com"},
				                       {"x-cdn-streaming-uri", "http://foo.com"},
				                       {"x-ttl", "999"},
				                       {"x-cdn-enabled", "true"},
				                       {"x-log-retention", "true"}
				                   };
			        return new ContainerResponse(rheaders, "Foo", 201, null);
			    default:
					var jheaders = new Dictionary<string, string>
					                   {
					                       {"x-versions-location", "foo"},
						                   {"x-container-meta-web-index", "foo"},
						                   {"x-container-meta-web-listings-css", "foo"},
						                   {"x-container-meta-web-error", "foo"},
						                   {"x-container-meta-web-listings", "true"},
					                       {"x-container-object-count", "1"},
					                       {"x-container-bytes-used", "1"},
					                       {"x-container-meta-foo", "foo"}
					                   };
			        return new ContainerResponse(jheaders, "Foo", 201, null);
			}
	    }

	    public override ContainerResponse PostContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-post-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ContainerResponse PutContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-put-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ContainerResponse DeleteContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-delete-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-empty":
				    throw new ClientException("FAIL!", 409);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ObjectResponse GetObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{				
			Stream stream = new MemoryStream();
			stream.Write(Encoding.UTF8.GetBytes("a"), 0, 1);
			stream.Seek(0, SeekOrigin.Begin);
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-get-object":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"content-length", "1"},
					                       {"content-type", "foo"},
					                       {"x-object-meta-foo", "foo"},
					                       {"etag", "0cc175b9c0f1b6a831c399e269772661"}
					                   };
				        return new ObjectResponse(rheaders, "Foo", 200, stream);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"content-length", "1"},
					                       {"content-type", "foo"},
					                       {"x-object-meta-foo", "foo"},
					                       {"etag", "0cc175b9c0f1b6a831c399e269772661"}
					                   };
			        return new ObjectResponse(rheaders, "Foo", 200, stream);  
			}
		}
		public override ObjectResponse HeadObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-head-object":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"content-length", "1"},
					                       {"content-type", "foo"},
					                       {"x-object-meta-foo", "foo"},
					                       {"etag", "foo"}
					                   };
				        return new ObjectResponse(rheaders, "Foo", 200, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"content-length", "1"},
					                       {"content-type", "foo"},
					                       {"x-object-meta-foo", "foo"},
					                       {"etag", "foo"}
					                   };
			        return new ObjectResponse(rheaders, "Foo", 200, null);  
			}
		}

	    public override ObjectResponse PostObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-post-object":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ObjectResponse PutObject (string url, string token, string container, string name, Stream contents, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-put-object":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-etag":
				    throw new ClientException("FAIL!", 422);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ObjectResponse DeleteObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-delete-object":
				    if (_retries == 1)
				    {
					    _retries = 0;
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    default:
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override AccountResponse GetCDNAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
		{
		    List<Dictionary<string, string>> objects;
		    Dictionary<string, string> robject;
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-get-container":
				    if (_retries == 1)
				    {
					    _retries = 0;
					    objects = new List<Dictionary<string, string>>();
				        robject = new Dictionary<string, string> {{"name", "foo"}};
				        objects.Add(robject);
					    rheaders = new Dictionary<string, string>
					                   {
					                       {"x-container-object-count", "1"},
					                       {"x-container-bytes-used", "1"},
					                       {"x-container-meta-foo", "foo"}
					                   };
				        return new AccountResponse(rheaders, "Foo", 201, objects);    
				    }
			        ++_retries;
			        throw new ClientException("FAIL!", 500);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-401":
				    throw new ClientException("FAIL!", 401);
			    case "fail--1":
				    throw new ClientException("FAIL!", -1);
			    default:
					objects = new List<Dictionary<string, string>>();
				    robject = new Dictionary<string, string> {{"name", "foo"}};
			        objects.Add(robject);
					rheaders = new Dictionary<string, string>
					               {
					                   {"x-container-object-count", "1"},
					                   {"x-container-bytes-used", "1"},
					                   {"x-container-meta-foo", "foo"}
					               };
			        return new AccountResponse(rheaders, "Foo", 201, objects);    
			}
		}
	}
}