using System;
using System.Collections.Generic;
using System.Web;
namespace Rackspace.Cloudfiles
{
	public class Common
	{
		public static void ValidateContainerName(string container_name)
		{
			if (string.IsNullOrEmpty(container_name))
				throw new ArgumentNullException("container_name", "ERROR: container_name cannot be Null!");
			if (HttpUtility.UrlEncode(container_name).Length > 256)
				throw new ContainerNameException("ERROR: encoded URL Length greater than 256 char's");
			if (container_name.Contains("/"))
				throw new ContainerNameException("ERROR: container_name contains a /");
		}
		public static void ValidateObjectName(string object_name)
		{
			if (string.IsNullOrEmpty(object_name))
				throw new ArgumentNullException();
			if (HttpUtility.UrlEncode(object_name).Length > 1024)
				throw new ObjectNameException("ERROR: Url Encoded Object Name exceeds 1024 char's");
		}
		public static Dictionary<string, Dictionary<string, string>> ProcessMetadata(Dictionary<string, string> headers)
		{
			var pheaders = new Dictionary<string, string>();
			var metadata = new Dictionary<string, string>();
			foreach (var header in headers)
			{
		        if (header.Key.ToLower().Contains("x-account-meta-"))
				{
					metadata.Add(header.Key.Remove(0, 15), header.Value);
				}
				else if(header.Key.ToLower().Contains("x-container-meta-"))
				{
					metadata.Add(header.Key.Remove(0, 17), header.Value);
				}
				else if(header.Key.ToLower().Contains("x-object-meta-"))
				{
					metadata.Add(header.Key.Remove(0, 14), header.Value);
				}
				else
				{
					pheaders.Add(header.Key.ToLower(), header.Value);
				}
		    }
			var processed_headers = new Dictionary<string, Dictionary<string, string>>();
			processed_headers["headers"] = pheaders;
			processed_headers["metadata"] = metadata;
			return processed_headers;
		}
	}
	public enum ObjectQuery
	{
		Prefix,
		Delimiter,
		Marker,
		Limit
	}
	public enum ContainerQuery
	{
		Prefix,
		Limit,
		Marker
	}
    public enum PublicContainerQuery
    {
		Limit,
		Marker,
	    EnbaledOnly
    }
}