ABSOLUTE_PATH = File.expand_path(File.dirname(__FILE__))

COMPILE_TARGET = 'debug'
INTEGRATION_TESTS_CONFIG_FILE = File.join(ABSOLUTE_PATH, 'Rackspace.CloudFiles.Integration.Tests','bin',COMPILE_TARGET,'Credentials.config')
PRODUCT = "csharp-cloudfiles"
COPYRIGHT = "Copyright (c) 2008, 2009 2010, 2011, Rackspace Managed Hosting.  All Rights Reserved";
COMPANY = "Rackspace Managed Hosting"
DESCRIPTION = "C#.NET API for Rackspace Cloud Files Cloud Storage"
COMMON_ASSEMBLY_INFO = File.join(ABSOLUTE_PATH,'Rackspace.CloudFiles','Properties','AssemblyInfo.cs')
CLR_VERSION = 'v3.5'
SLN_FILE = File.join(ABSOLUTE_PATH,'Rackspace.CloudFiles.sln')
ZIP_FILE_PREFIX = ["csharp-cloudfiles-DOTNET",CLR_VERSION].join
CORE_DLL_DIR = File.join(ABSOLUTE_PATH,'Rackspace.CloudFiles','bin',COMPILE_TARGET)
INTEGRATION_TESTS_DLL = File.join(ABSOLUTE_PATH,'Rackspace.CloudFiles.Integration.Tests','bin',COMPILE_TARGET,'Rackspace.CloudFiles.Integration.Tests.dll')
UNIT_TESTS_DLL = File.join(ABSOLUTE_PATH,'Rackspace.CloudFiles.Unit.Tests','bin',COMPILE_TARGET,'Rackspace.CloudFiles.Unit.Tests.dll')
BUILDS_DIR = File.join(ABSOLUTE_PATH,'builds')

TEAM_CITY_BUILD_NUMBER = ENV['BUILD_NUMBER']
RELEASE_BUILD_NUMBER = "#{TEAM_CITY_BUILD_NUMBER || '0.0.0.0'}"

NUNIT_CMD_EXE = File.join(ABSOLUTE_PATH,'lib','nunit','nunit-console.exe')
FRAMEWORK_DIR = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', CLR_VERSION)
MSBUILD_EXE = File.join(FRAMEWORK_DIR, 'msbuild.exe')