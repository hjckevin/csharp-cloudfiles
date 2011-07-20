using System;
using System.Net;
using System.Threading;
using Rackspace.CloudFiles.Domain;
using Rackspace.CloudFiles.Domain.Request;
using Rackspace.CloudFiles.Domain.Request.Interfaces;

namespace Rackspace.CloudFiles.Integration.Tests
{
    public class RequestFactoryWithAgentSupport : IRequestFactory
    {
        private readonly string _useragent;

        public RequestFactoryWithAgentSupport(string useragent)
        {
            _useragent = useragent;
        }

        public ICloudFilesRequest Create(Uri uri)
        {
            var webreq = (HttpWebRequest) WebRequest.Create(uri);
            webreq.UserAgent = _useragent;
            return new CloudFilesRequest(webreq);
        }

        public ICloudFilesRequest Create(Uri uri, ProxyCredentials creds)
        {
            throw new NotImplementedException();
        }
    }
}