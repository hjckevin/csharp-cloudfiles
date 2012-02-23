using System;

namespace Rackspace.Cloudfiles
{
	public class AuthenticationFailedException : Exception
	{
		public AuthenticationFailedException() {}
		public AuthenticationFailedException(string message) : base(message) {}
	}
	public class CloudFilesException : Exception
	{
		public CloudFilesException() {}
		public CloudFilesException(string message) : base(message) {}
	}
	public class ContainerNameException : Exception
	{
		public ContainerNameException() {}
		public ContainerNameException(string message) : base(message) {}
	}
	public class ObjectNameException : Exception
	{
		public ObjectNameException() {}
		public ObjectNameException(string message) : base(message) {}
	}
	public class ContainerNotFoundException : Exception
	{
		public ContainerNotFoundException() {}
		public ContainerNotFoundException(string message) : base(message) {}
	}
	public class ObjectNotFoundException : Exception
	{
		public ObjectNotFoundException() {}
		public ObjectNotFoundException(string message) : base(message) {}
	}
	public class CDNNotEnabledException : Exception
	{
		public CDNNotEnabledException() {}
		public CDNNotEnabledException(string message) : base(message) {}
	}	
	public class TTLLengthException : Exception
	{
		public TTLLengthException() {}
		public TTLLengthException(string message) : base(message) {}
	}
	public class InvalidETagException : Exception
	{
		public InvalidETagException() {}
		public InvalidETagException(string message) : base(message) {}
	}
	public class ContainerNotEmptyException : Exception
	{
		public ContainerNotEmptyException() {}
		public ContainerNotEmptyException(string message) : base(message) {}
	}
	public class UnauthorizedException : Exception
	{
		public UnauthorizedException() {}
		public UnauthorizedException(string message) : base(message) {}
	}
}

