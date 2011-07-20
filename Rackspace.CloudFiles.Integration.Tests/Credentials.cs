using System;
using System.IO;
using System.Xml;

namespace Rackspace.CloudFiles.Integration.Tests
{
    internal static class Credentials
    {
        public static readonly string USERNAME = new CredentialsFactory().Get().GetUsername();
        public static readonly string API_KEY = new CredentialsFactory().Get().GetApiKey();
        public static readonly string AUTH_ENDPOINT = new CredentialsFactory().Get().GetAuthEndpoint();
    }

    internal interface ICredentialsRetriver
    {
        string GetUsername();
        string GetApiKey();
        string GetAuthEndpoint();
    }

    internal class CredentialsFactory
    {

        public ICredentialsRetriver Get()
        {
            var api_key = Environment.GetEnvironmentVariable("CSHARP_CLOUDFILES_API_KEY");
            var username = Environment.GetEnvironmentVariable("CSHARP_CLOUDFILES_USERNAME");
            var endpoint = Environment.GetEnvironmentVariable("CSHARP_CLOUDFILES_AUTH_ENDPOINT");

            if (!string.IsNullOrEmpty(api_key)
                && !string.IsNullOrEmpty(username)
                && !string.IsNullOrEmpty(endpoint))
            {
                return new CredentialsEnvironmentVariableGetter();
            }

            return new CredentialsConfigParser();
        }
    }

    internal class CredentialsEnvironmentVariableGetter : ICredentialsRetriver
    {
        public string GetUsername()
        {
            return Environment.GetEnvironmentVariable("CSHARP_CLOUDFILES_USERNAME");
        }

        public string GetApiKey()
        {
            return Environment.GetEnvironmentVariable("CSHARP_CLOUDFILES_API_KEY");
        }

        public string GetAuthEndpoint()
        {
            return Environment.GetEnvironmentVariable("CSHARP_CLOUDFILES_AUTH_ENDPOINT");
        }
    }

    internal class CredentialsConfigParser : ICredentialsRetriver
    {
        private readonly XmlDocument _xmlDocument;

        public CredentialsConfigParser()
        {
            var CONFIG_FILE_PATH = Directory.GetCurrentDirectory() + "\\Credentials.config";
            if (!File.Exists(CONFIG_FILE_PATH)) 
                throw new FileNotFoundException("Missing Credentials.config file");
            
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(CONFIG_FILE_PATH);
        }
        
        public string GetUsername()
        {
            var usernameNode = _xmlDocument.SelectSingleNode("/credentials/username");
            if(usernameNode == null)
                throw new XmlException("Username node missing from Credential.config file");

            return usernameNode.InnerText;
        }

        public string GetApiKey()
        {
            var apiKeyNode = _xmlDocument.SelectSingleNode("/credentials/api_key");
            if (apiKeyNode == null)
                throw new XmlException("API key node missing from Credential.config file");

            return apiKeyNode.InnerText;
        }

        public string GetAuthEndpoint()
        {
            var authEndpointNode = _xmlDocument.SelectSingleNode("/credentials/auth_endpoint");
            if (authEndpointNode == null)
                throw new XmlException("Auth endpoint node missing from Credential.config file");

            return authEndpointNode.InnerText;
        }
    }
}