# History

## v1.5.2.0 (2011-2-20)
#### New Features
 * Allow user to purge individual items or entire containers from CDN. PurgePublicContainer and PurgePublicStorageItem
#### Bugfixes
 * Fixed duplicate logging with log4net (Closes gh-9)
#### Removed Features
 * Starting removal of SpecMaker, Ryan Svihla's BDD framework.

## v1.5.2.0 (2011-2-20)
#### New Features
 * Changed README and History files to markdown (.md)
 * Changed build process from custom Ruby files to use [Albacore](http://github.com/DerickBailey/Albacore) gem
#### Bugfixes
 * Create UserCredentials constructor to allow user to change Auth url (US,UK,Mosso) (re-Closes gh-22)
#### Removed Features
 * removed all usages of useragent acl and referrer acl, this does include trimming the pararmeter list to the SetDetailsOnPublicContainer method on the IConnection interface

## v1.5.1.0 (2011-1-24)
#### New Features
 * downgraded master branch to .NET 3.5 runtime due to end-user feedback
   the solution and projects are still VS2010 version
 * created NET40 branch for .NET 4.0 runtime
#### Bugfixes
 * fix UK url (Closed gh-22)
#### Removed Features

## v1.5.0.0 (2010-12-23)
#### New Features
 * upgraded project to .NET 4.0 and VS2010 (Closed gh-20)
 * upgrade nunit to work with .NET 4.0
 * removed docu due to no support for .NET 4.0, will replace soon
 * fixed using statements due to namespace change in NUnit SyntaxHelpers
 * add 2011 to COPYING and other files
 * update readme to notify user that vs2010 and .net4.0 are necessary and update history file with correct information, last 3 version updates
#### Bugfixes
 * fix misspelling of cloudfiles in assemblyinfo generation - product name
#### Removed Features

## v1.4.2.7 (2010-9-2)
#### New Features
#### Bugfixes
 * Making sure FileStreams are read-only (Closed gh-13)
#### Removed Features

## v1.4.2.6 (2010-8-23)
#### New Features
#### Bugfixes
 * Use regex match for utf-8/utf8 in xml reponse content-type (Closed gh-12)
#### Removed Features

## v1.4.2.5 (2010-7-29)
#### New Features
#### Bugfixes
 * Fixed CloudFilesResponse to allow text/plain contentbody
 * Refactored tests to handle old CloudFiles system and new CloudFiles system
#### Removed Features

## v1.4.2.4 (2010-7-16)
#### New Features
#### Bugfixes
 * Reinstated the ability to make containers public
 * Code cleanup
#### Removed Features

## v1.4.2.3 (2010-5-12)
#### New Features
#### Bugfixes
 * added office 2007 mimetypes since some browsers were downloading them as zip files 
#### Removed Features

## v1.4.2.2 (2010-3-23)
#### New Features
 * Added support for servicenet.This will eliminate the bandwidth charges that would be associated with your requests otherwise. 
Please keep in mind that you will still be charged for requests and storage.
#### Bugfixes
#### Removed Features

## v1.4.2.0 (2009-11-13)
#### New Features
 * Added support for setting referrer acl and user agent acl on CDN enabled containers
#### Bugfixes
 * Removed some methods in the public api that were misleading and were not behaving well (namely setting ttl and logging on public containers)
#### Removed Features

## v1.4.1.3 (2009-11-4)
#### New Features
#### Bugfixes
 * Fixed bug in call in CF_Container that was incorrectly downoading an object rather than just checking on its existence.
#### Removed Features

## v1.4.1.2 (2009-11-3)
#### New Features
#### Bugfixes
 * Fixed bug with null ContentLength on CF_Object
 * Corrected MakePath to only worry about making directory objects. 
 NOTICE: MakePath now will NOT ignore '.' in the path.  This could be 
 a breaking change to your existing code, now directory objects will be created regardless.
 * Set content type on directory objects to "application/directory".
#### Removed Features

## v1.4.1.0 (2009-08-31)
#### New Features
 * Increased test coverage using SpecMaker

## v1.4.0.1 (2009-08-28)
#### New Features
#### Bugfixes
 *  fixed bug in retrieving a public container list.
#### Removed Features

## v1.4.0.0 (2009-08-12)
#### New Features
 * added support for enabling/disabling access logs on public containers
#### Bugfixes
 * small internal changes for testability
#### Removed Features
 
## v1.3.4.1 (2009-07-31)
#### New Features
#### Bugfixes
 * small changes to internal url handling to coincide with changes to REST api.
 * fixed issue where listing containers always returned empty
#### Removed Features

## v1.3.4 (2009-06-23)
#### New Features
 * converted build process from NAnt to Rake
 * made integration tests depedent on Credentials.config file
#### Bugfixes
#### Removed Features

## v1.3.3 (2009-06-15)
#### New Features
#### Bugfixes
 * remove SSL Certification validation from CloudFilesRequest due to it not being
   allowed in Medium Trust hosting environments
#### Removed Features

## v1.3.2 (2009-06-01)
#### New Features
#### Bugfixes
 * Connection.cs GetContainerInformation.  Fixed method not populating CDNUri property on Container object 
#### Removed Features

## v1.3.1 (2009-05-19)
#### New Features
 * Added TTL (time-to-live) parameter to making a container public
 * Log4Net wire-up for logging available, information in README
 * Better documentation, found in the README.rdoc file
 * Add repo to github.com/rackspace/csharp-cloudfiles (source and tests)
 * Progress information on file transfers
#### Bugfixes
#### Removed Features

## v1.3.0 (2009-03-16)
#### New Features
 * Container/Object lists in JSON or XML with item details
 * API support to return your account's total storage or at a Container level
 * Chunked Transfer-Encoding on PUT requests
 * "Pathname" support for pseudo-hierarchical folders/directoriess
 * Increased name length for Containers/Objects
 * Adding Ruby to the list of supported language APIs
#### Bugfixes
#### Removed Features