# Rackspace Cloud Files CSharp API

## Description

This is a .NET/C# interface into the [Rackspace CloudFiles](http://www.rackspacecloud.com/cloud_hosting_products/files) service. 
Cloud Files is a reliable, scalable and affordable web-based storage hosting for backing up and archiving all your static content.  
Cloud Files is the first and only cloud service that leverages a tier one CDN provider to create such an easy and complete storage-to-delivery 
solution for media content.

## NuGet

PM> Install-Package csharp-cloudfiles

## Contributing

1. Your code **WILL NOT** be accepted without tests.  period.
2. Please make sure your autocrlf setting is true

	git config core.autocrlf true
	
   We have to do this because one of the primary maintainers is using Mono (Linux) to develop this library.

3. [Fork](http://help.github.com/fork-a-repo/) the repository, clone, code, push, and then issue a [pull request](http://help.github.com/send-pull-requests/)

## Issues vs Inquiries/Questions

Please put issues on Github and ask questions/inquiries on the mailing list

## Mailing List

The [mailing list](http://groups.google.com/group/csharp-cloudfiles)

## Creating Issues

Please read the [wiki](http://wiki.github.com/rackspace/csharp-cloudfiles/) about what information is best to help people fix your issues, 
then create an issue on the [issues tab](http://github.com/rackspace/csharp-cloudfiles/issues).

## Getting dll and using in your project

Go to the [downloads page](http://github.com/rackspace/csharp-cloudfiles/downloads) and download the latest "stable" version or go and grab the latest build from our [continuous integration server (TeamCity)](http://teamcity.codebetter.com/viewType.html?tab=buildTypeStatusDiv&buildTypeId=bt320)  
Unzip the file, unzip the bin zip, and grab the following file:

	Rackspace.Cloudfiles.dll

Reference them from your application.  Look at the examples below once you've done this.  Example folder structure:

	/Your_project
		/lib
			/cloudfiles
				Rackspace.Cloudfiles.dll
		/src
			...

## Necessary prerequisites

Visual Studio 2010 and .NET 3.5/.NET 4.0 (depending on the zip file you chose)

## [Examples](https://github.com/rackspace/csharp-cloudfiles/wiki/Code-Examples)

## Committers

[Contributors](http://github.com/rackspace/csharp-cloudfiles/contributors)

## License

See COPYING for license information.
Copyright (c) 2008, 2009 2010, 2011, Rackspace US, Inc.
