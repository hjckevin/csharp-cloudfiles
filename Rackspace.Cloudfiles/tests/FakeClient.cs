using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Openstack.Swift;
using Rackspace.Cloudfiles;
namespace Rackspace.Cloudfiles
{
	public class FakeClient : Rackspace.Cloudfiles.Client 
	{
		public FakeClient () { }
		private int _retries = 0;
		public override AuthResponse GetAuth(string url, string user, string key, Dictionary<string, string> headers, Dictionary<string, string> query, bool snet)
		{
		    Dictionary<string, string> rheaders;
		    switch (key)
			{
			    case "fail-auth":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-auth-token", "foo");
				        rheaders.Add("x-storage-url", "https://foo.com");
				        rheaders.Add("x-cdn-management-url", "https://foo.com");
				        return new AuthResponse(rheaders, "Foo", 201);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "auth":
			    default:
				    rheaders = new Dictionary<string, string>();
				    rheaders.Add("x-auth-token", "foo");
				    rheaders.Add("x-storage-url", "https://foo.com");
				    rheaders.Add("x-cdn-management-url", "https://foo.com");
				    return new AuthResponse(rheaders, "Foo", 201);
			}
		}

	    public override AccountResponse GetAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
	    {
	        List<Dictionary<string, string>> containers;
	        Dictionary<string, string> container;
	        Dictionary<string, string> rheaders;
	        switch (token)
			{
			    case "fail-get-account":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
	                    containers = new List<Dictionary<string, string>>();
					    container = new Dictionary<string, string>();
					    container.Add("name", "foo");
					    containers.Add(container);
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-account-container-count", "1");
				        rheaders.Add("x-account-object-count", "1");
				        rheaders.Add("x-account-bytes-used", "1");
				        rheaders.Add("x-account-meta-foo", "foo");
				        return new AccountResponse(rheaders, "Foo", 200, containers);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "get-account":
			    default:
	                    containers = new List<Dictionary<string, string>>();
					    container = new Dictionary<string, string>();
					    container.Add("name", "foo");
					    containers.Add(container);
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-account-container-count", "1");
				        rheaders.Add("x-account-object-count", "1");
				        rheaders.Add("x-account-bytes-used", "1");
				        rheaders.Add("x-account-meta-foo", "foo");
				        return new AccountResponse(rheaders, "Foo", 200, containers);  
			}
	    }

	    public override AccountResponse HeadAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query)
	    {
	        Dictionary<string, string> rheaders;
	        switch (token)
			{
			    case "fail-head-account":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-account-container-count", "1");
				        rheaders.Add("x-account-object-count", "1");
				        rheaders.Add("x-account-bytes-used", "1");
				        rheaders.Add("x-account-meta-foo", "foo");
					    rheaders.Add("blah", "foo");
				        return new AccountResponse(rheaders, "Foo", 201, null);
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "head-account":
			    default:
				    rheaders = new Dictionary<string, string>();
				    rheaders.Add("x-account-container-count", "1");
				    rheaders.Add("x-account-object-count", "1");
				    rheaders.Add("x-account-bytes-used", "1");
				    rheaders.Add("x-account-meta-foo", "foo");
					rheaders.Add("blah", "foo");
				    return new AccountResponse(rheaders, "Foo", 201, null);
			}
	    }

	    public override AccountResponse PostAccount (string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
			switch (token)
			{
			    case "fail-post-account":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new AccountResponse(new Dictionary<string, string>(), "Foo", 201, null);
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "head-account":
			    default:
				    return new AccountResponse(new Dictionary<string, string>(), "Foo", 201, null);
			}
		}
		public override ContainerResponse GetContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
		{
		    List<Dictionary<string, string>> objects;
		    Dictionary<string, string> robject;
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-get-container":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    objects = new List<Dictionary<string, string>>();
				        robject = new Dictionary<string, string>();
					    robject.Add("name", "foo");
					    objects.Add(robject);
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-container-object-count", "1");
				        rheaders.Add("x-container-bytes-used", "1");
				        rheaders.Add("x-container-meta-foo", "foo");
				        return new ContainerResponse(rheaders, "Foo", 201, objects);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "get-container":
			    default:
					    objects = new List<Dictionary<string, string>>();
				        robject = new Dictionary<string, string>();
					    robject.Add("name", "foo");
					    objects.Add(robject);
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-container-object-count", "1");
				        rheaders.Add("x-container-bytes-used", "1");
				        rheaders.Add("x-container-meta-foo", "foo");
				        return new ContainerResponse(rheaders, "Foo", 201, objects);    
			}
		}

	    public override ContainerResponse HeadContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
	    {
	        Dictionary<string, string> rheaders;
	        switch (token)
			{
			    case "fail-head-container":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-container-object-count", "1");
				        rheaders.Add("x-container-bytes-used", "1");
				        rheaders.Add("x-container-meta-foo", "foo");
				        return new ContainerResponse(rheaders, "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail-head-cdn-container":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-cdn-uri", "http://foo.com");
				        rheaders.Add("x-cdn-ssl-uri", "https://foo.com");
				        rheaders.Add("x-cdn-streaming-uri", "http://foo.com");
				        rheaders.Add("x-ttl", "foo");
				        rheaders.Add("x-cdn-enabled", "true");
				        rheaders.Add("x-log-retention", "true");
				        return new ContainerResponse(rheaders, "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "head-cdn-container":
				    rheaders = new Dictionary<string, string>();
				    rheaders.Add("x-cdn-uri", "http://foo.com");
				    rheaders.Add("x-cdn-ssl-uri", "https://foo.com");
				    rheaders.Add("x-cdn-streaming-uri", "http://foo.com");
				    rheaders.Add("x-ttl", "999");
				    rheaders.Add("x-cdn-enabled", "true");
				    rheaders.Add("x-log-retention", "true");
				    return new ContainerResponse(rheaders, "Foo", 201, null);
			    case "head-container":
			    default:
					Dictionary<string, string> jheaders = new Dictionary<string, string>();
				    jheaders.Add("x-container-object-count", "1");
				    jheaders.Add("x-container-bytes-used", "1");
				    jheaders.Add("x-container-meta-foo", "foo");
				    return new ContainerResponse(jheaders, "Foo", 201, null);
			}
	    }

	    public override ContainerResponse PostContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-post-container":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "post-container":
			    default:
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ContainerResponse PutContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-put-container":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "put-container":
			    default:
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ContainerResponse DeleteContainer (string url, string token, string container, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-delete-container":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
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
			    case "delete-container":
			    default:
				        return new ContainerResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ObjectResponse GetObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{				
			Stream stream = new MemoryStream();
			stream.Write(UnicodeEncoding.UTF8.GetBytes("a"), 0, 1);
			stream.Seek(0, SeekOrigin.Begin);
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-get-object":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("content-length", "1");
				        rheaders.Add("content-type", "foo");
				        rheaders.Add("x-object-meta-foo", "foo");
				        rheaders.Add("etag", "0cc175b9c0f1b6a831c399e269772661");
				        return new ObjectResponse(rheaders, "Foo", 200, stream);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "get-object":
			    default:
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("content-length", "1");
				        rheaders.Add("content-type", "foo");
				        rheaders.Add("x-object-meta-foo", "foo");
				        rheaders.Add("etag", "0cc175b9c0f1b6a831c399e269772661");
				        return new ObjectResponse(rheaders, "Foo", 200, stream);  
			}
		}
		public override ObjectResponse HeadObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
		    Dictionary<string, string> rheaders;
		    switch (token)
			{
			    case "fail-head-object":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("content-length", "1");
				        rheaders.Add("content-type", "foo");
				        rheaders.Add("x-object-meta-foo", "foo");
				        rheaders.Add("etag", "foo");
				        return new ObjectResponse(rheaders, "Foo", 200, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "head-object":
			    default:
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("content-length", "1");
				        rheaders.Add("content-type", "foo");
				        rheaders.Add("x-object-meta-foo", "foo");
				        rheaders.Add("etag", "foo");
				        return new ObjectResponse(rheaders, "Foo", 200, null);  
			}
		}

	    public override ObjectResponse PostObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-post-object":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "post-object":
			    default:
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ObjectResponse PutObject (string url, string token, string container, string name, System.IO.Stream contents, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-put-object":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
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
			    case "put-object":
			    default:
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
			}
		}
		public override ObjectResponse DeleteObject (string url, string token, string container, string name, Dictionary<string, string> headers, Dictionary<string, string> query)
		{
            switch (token)
			{
			    case "fail-delete-object":
				    if (this._retries == 1)
				    {
					    this._retries = 0;
				        return new ObjectResponse(new Dictionary<string, string>(), "Foo", 201, null);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-not-found":
				    throw new ClientException("FAIL!", 404);
			    case "fail-unauthorized":
				    throw new ClientException("FAIL!", 401);
			    case "fail-timeout":
				    throw new ClientException("FAIL!", -1);
			    case "delete-container":
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
				    if (this._retries == 1)
				    {
					    this._retries = 0;
					    objects = new List<Dictionary<string, string>>();
				        robject = new Dictionary<string, string>();
					    robject.Add("name", "foo");
					    objects.Add(robject);
					    rheaders = new Dictionary<string, string>();
				        rheaders.Add("x-container-object-count", "1");
				        rheaders.Add("x-container-bytes-used", "1");
				        rheaders.Add("x-container-meta-foo", "foo");
				        return new AccountResponse(rheaders, "Foo", 201, objects);    
				    }
				    else
				    {
					    ++this._retries;
					    throw new ClientException("FAIL!", 500);
				    }
			    case "fail":
				    throw new ClientException("FAIL!", 500);
			    case "fail-401":
				    throw new ClientException("FAIL!", 401);
			    case "fail--1":
				    throw new ClientException("FAIL!", -1);
			    case "get-container":
			    default:
					objects = new List<Dictionary<string, string>>();
				    robject = new Dictionary<string, string>();
					robject.Add("name", "foo");
					objects.Add(robject);
					rheaders = new Dictionary<string, string>();
				    rheaders.Add("x-container-object-count", "1");
				    rheaders.Add("x-container-bytes-used", "1");
				    rheaders.Add("x-container-meta-foo", "foo");
				    return new AccountResponse(rheaders, "Foo", 201, objects);    
			}
		}
	}
}