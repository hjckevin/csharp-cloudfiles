# Rackspace Cloud Files CSharp API

## Description

This is a .NET/C# interface into the [Rackspace CloudFiles](http://www.rackspacecloud.com/cloud_hosting_products/files) service. 
Cloud Files is a reliable, scalable and affordable web-based storage hosting for backing up and archiving all your static content.  
Cloud Files is the first and only cloud service that leverages a tier one CDN provider to create such an easy and complete storage-to-delivery 
solution for media content.

## **Creating Issues**

Please read the [wiki](http://wiki.github.com/rackspace/csharp-cloudfiles/) about what information is best to help people fix your issues, 
then create an issue on the {issues tab](http://github.com/rackspace/csharp-cloudfiles/issues).

## Getting dll and using in your project

Go to the [downloads page](http://github.com/rackspace/csharp-cloudfiles/downloads) and download the latest version.  
Unzip the file, unzip the bin zip, and grab the following files:

	com.mosso.cloudfiles.dll
	log4net.dll
	log4Net.config

Reference them from your application.  Look at the examples below once you done this.  Example folder structure:

	/Your_project
		/lib
			/cloudfiles
				com.mosso.cloudfiles.dll
				log4net.dll
		/src
			...

## Necessary prerequisites

Visual Studio 2010 and .NET 3.5/.NET 4.0 (depending on the zip file you chose)

Running the tests requires Ruby and Rake to be installed.  We suggest you use the [Ruby Installer For Windows](http://rubyinstaller.org/). 
We currently use the 1.8.6 version.  After that is installed you will need to install the rake and albacore(v0.2.2) gems.

	gem install rake
	gem install albacore -v 0.2.2

## Getting source, compiling, and using in your project

Follow the instructions [here](http://help.github.com/win-git-installation/) to install msysgit (Git for Windows users) 
and the instructions [here](http://help.github.com/msysgit-key-setup/) on how to get your ssh keys setup for using Github.

	git clone git://github.com/rackspace/csharp-cloudfiles.git

This will create the csharp-cloudfiles directory locally on your computer.  Go into that folder and run:

	rake compile

This will compile the project and give you a resulting dll in ...csharp-cloudfiles/bin/debug/

Look above for an example folder structure.

## Logging

Logging is done with the log4net.dll and log4net.config file that are included in the source/downloads.
You just need to edit the log4net.config file to turn it on:

	change:
	<log4net debug="false">
	
	to:
	<log4net debug="true">
	
so that logging starts and you get the desired logging output and format.

Currently the log is going to log/com.mosso.cloudfiles.log, where you have the dll referenced.

 
	<file value="logs/com.mosso.cloudfiles.log" />   (in the log4net.config file)

Please reference the [log4net documentation](http://logging.apache.org/log4net/release/config-examples.html) on how to edit that config file.

## Testing

Once installed you need to create your integration tests credentials config file.

Run 

	rake create_credentials_config

from the project solution directory.  This will create a Credentials.config file in root of the repository (same place as the sln file):

  <?xml version="1.0" encoding="utf-8"?>
  <credentials>
    <username>PUT USERNAME HERE</username>
    <api_key>PUT API KEY HERE</api_key>
  </credentials>

Just replace the placeholder text.  This file *is not* under source control.  It is ignored by the .gitignore file.


# Examples coming soon on our [wiki](http://wiki.github.com/rackspace/csharp-cloudfiles/).  prior examples were out of date

## Committers

[Contributors](http://github.com/rackspace/csharp-cloudfiles/contributors)

## License

See COPYING for license information.
Copyright (c) 2008, 2009 2010, 2011, Rackspace US, Inc.