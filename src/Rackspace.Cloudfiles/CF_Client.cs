using System;
using System.Collections.Generic;
using System.Xml;
using Openstack.Swift;
namespace Rackspace.Cloudfiles
{
	public class CF_Client : Client
	{
		public CF_Client() : base() { }
		public CF_Client(IHttpRequestFactory http_factory) : base(http_factory) { }
	}
	public class Client : SwiftClient
	{
		private IHttpRequestFactory _http_factory;
	    public Client() : base()
		{
			this._http_factory = new HttpRequestFactory();
		}
		public Client(IHttpRequestFactory http_factory) : base(http_factory)
		{
			this._http_factory = http_factory;
		}
		public virtual AccountResponse GetCDNAccount(string url, string token, Dictionary<string, string> headers, Dictionary<string, string> query, bool full_listing)
		{

			headers["X-Auth-Token"] = token;
			query["format"] = "xml";
			IHttpRequest request = this._http_factory.GetHttpRequest("GET", url, headers, query);
			IHttpResponse response = request.GetResponse();
			XmlTextReader reader = new XmlTextReader(response.ResponseStream);
			List<Dictionary<string, string>> containers = new List<Dictionary<string, string>>();
			Dictionary<string, string> info = new Dictionary<string, string>();
			string key;
		    while (reader.Read())
		    {   
				switch (reader.NodeType)
				{
				    case XmlNodeType.Element:
				        if (reader.Name != "xml" && 
					        reader.Name != "container" && 
					        reader.Name != "account")
					    {
				            key = reader.Name;
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
						    key = "";
					     }
					     break;
				     default:
					     break;
				}
			}
			if (full_listing)
			{
				AccountResponse tmp;
				do
				{
					int nmarker = containers.Count - 1;
					query["marker"] = containers[nmarker]["name"];
					tmp = this.GetCDNAccount(url, token, headers, query, false);
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