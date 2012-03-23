using System;
using System.Collections.Generic;
using System.Xml;
using OpenStack.Swift;
namespace Rackspace.Cloudfiles
{
	public class CF_Client : Client
	{
		public CF_Client(){ }
		public CF_Client(IHttpRequestFactory http_factory) : base(http_factory) { }
	}
	public class Client : SwiftClient
	{
		private readonly IHttpRequestFactory _http_factory;
	    public Client()
	    {
			_http_factory = new HttpRequestFactory();
		}
		public Client(IHttpRequestFactory http_factory) : base(http_factory)
		{
			_http_factory = http_factory;
		}
		public virtual AccountResponse GetCDNAccount(string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
		{

			headers["X-Auth-Token"] = token;
			query["format"] = "xml";
			var request = _http_factory.GetHttpRequest("GET", url, headers, query);
			var response = request.GetResponse();
			var reader = new XmlTextReader(response.ResponseStream);
			var containers = new List<Dictionary<string, string>>();
			var info = new Dictionary<string, string>();
		    while (reader.Read())
		    {
		        switch (reader.NodeType)
				{
				    case XmlNodeType.Element:
				        if (reader.Name != "xml" && 
					        reader.Name != "container" && 
					        reader.Name != "account")
					    {
				            var key = reader.Name;
						    reader.Read();
						    Console.WriteLine(key + ":" + reader.Value);
						    info.Add(key, reader.Value);
					    }
					    break;
				    case XmlNodeType.EndElement:
					    if (reader.Name == "container")
					    {
						    containers.Add(info);
						    info = new Dictionary<string, string>();
					    }
					     break;
				}
		    }
		    if (full_listing)
			{
			    do
				{
					int nmarker = containers.Count - 1;
					query["marker"] = containers[nmarker]["name"];
					var tmp = GetCDNAccount(url, token, headers, query, false);
				    if (tmp.Containers.Count > 0)
					{
						containers.AddRange(tmp.Containers);
					}
					else
					{
						break;
					}
				} while (true);
			}
			response.Close();
			return new AccountResponse(response.Headers, response.Reason, response.Status, containers);
	    }
    }
}