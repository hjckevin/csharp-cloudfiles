using System;

namespace Rackspace.Cloudfiles.Constants
{
	public static class Headers
	{
		//Auth Constants
		public const string AuthToken = "x-auth-token";
		public const string CdnManagementUrl = "x-cdn-management-url";
		public const string StorageUrl = "x-storage-url";
		//Account Constants
		public const string AccountMetaDataPrefix = "x-account-meta-";
		public const string AccountBytesUsed = "x-account-bytes-used";
		public const string AccountContainerCount = "x-account-container-count";
		public const string AccountObjectCount = "x-account-object-count";
		//Container Constants
		public const string ContainerMetaDataPrefix = "x-container-meta-";
		public const string ContainerBytesUsed = "x-container-bytes-used";
		public const string ContainerObjectCount = "x-container-object-count";
		public const string WebIndex = "x-container-meta-web-index";
		public const string WebError = "x-container-meta-web-error";
		public const string WebListings = "x-container-meta-web-listings";
		public const string WebListingsCSS = "x-container-meta-web-listings-css";
		public const string VersionsLocation = "x-versions-location";
		//CDN Container Constants
		public const string CdnUri = "x-cdn-uri";
		public const string CdnSslUri = "x-cdn-ssl-uri";
		public const string CdnStreamingUri = "x-cdn-streaming-uri";
		public const string CdnTTL = "x-ttl";
		public const string CdnLogRetention = "x-log-retention";
		public const string CdnEnabled = "x-cdn-enabled";
		//Object Constants
		public const string ObjectMetaDataPrefix = "x-object-meta-";
		public const string ObjectDeleteAfter = "x-delete-after";
		public const string ObjectDeleteAt = "x-delete-at";
		public const string Etag = "etag";
		public const string ContentType = "content-type";
		public const string ContentLength = "content-length";
		//Cdn Object Constants
		public const string CdnPurgeEmail = "x-purge-email";
		//Misc Headers
		public const string UserAgent = "user-agent";
	}
	public static class Misc
	{
		public const string DefaultUserAgent = "csharp-cloudfiles/3.0.3.1";
		public const int DefaultTimeout = 60 * 60 * 1000;
		public const string ProcessedHeadersMetadataKey = "metadata";
		public const string ProcessedHeadersHeaderKey = "headers";
	}
	public static class Query
	{
		public const string Limit = "limit";
		public const string Marker = "marker";
		public const string Prefix = "prefix";
		public const string Format = "format";
		public const string Delimiter = "delimiter";
		public const string EnabledOnly = "enabled_only";
	}
	public static class StatusCodes
	{
		public const int Timeout = -1;
		public const int Unauthorized = 401;
		public const int NotFound = 404;
		public const int Conflict = 409;
		public const int UnprocessableEntity = 422;
	}
	public static class Urls
	{
		public const string UsAuthUrl = "https://auth.api.rackspacecloud.com/v1.0";
		public const string UkAuthUrl = "https://lon.auth.api.rackspacecloud.com/v1.0";
	}
}

