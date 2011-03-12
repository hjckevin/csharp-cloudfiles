using System;
using System.IO;
using System.Xml;

namespace com.mosso.cloudfiles.integration.tests
{
    internal static class Credentials
    {
        private static readonly CredentialsConfigParser _credentialsConfigParser = new CredentialsConfigParser();

        public static readonly string USERNAME = _credentialsConfigParser.GetUsername();
        public static readonly string API_KEY = _credentialsConfigParser.GetApiKey();
        public static readonly string AUTH_ENDPOINT = _credentialsConfigParser.GetAuthEndpoint();
    }

    internal class CredentialsConfigParser
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