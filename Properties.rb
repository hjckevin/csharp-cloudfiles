ABSOLUTE_PATH = File.expand_path(File.dirname(__FILE__))

COMPILE_TARGET = 'release'
INTEGRATION_TESTS_CONFIG_FILE = File.join(ABSOLUTE_PATH, 'com.mosso.cloudfiles.integration.tests','bin',COMPILE_TARGET,'Credentials.config')
PRODUCT = "csharp-cloudfiles"
COPYRIGHT = "Copyright (c) 2008, 2009 2010, 2011, Rackspace Managed Hosting.  All Rights Reserved";
COMPANY = "Rackspace Managed Hosting"
DESCRIPTION = "C#.NET API for Rackspace Cloud Files Cloud Storage"
COMMON_ASSEMBLY_INFO = File.join(ABSOLUTE_PATH,'com.mosso.cloudfiles','Properties','AssemblyInfo.cs')
CLR_VERSION = 'v4.0.30319'
SLN_FILE = File.join(ABSOLUTE_PATH,'com.mosso.cloudfiles.sln')
ZIP_FILE_PREFIX = ["csharp-cloudfiles-DOTNET",CLR_VERSION].join
CORE_DLL_DIR = File.join(ABSOLUTE_PATH,'com.mosso.cloudfiles','bin',COMPILE_TARGET)
INTEGRATION_TESTS_DLL = File.join(ABSOLUTE_PATH,'com.mosso.cloudfiles.integration.tests','bin',COMPILE_TARGET,'com.mosso.cloudfiles.integration.tests.dll')
UNIT_TESTS_DLL = File.join(ABSOLUTE_PATH,'com.mosso.cloudfiles.unit.tests','bin',COMPILE_TARGET,'com.mosso.cloudfiles.unit.tests.dll')
RELEASE_BUILD_NUMBER = "1.5.2.0"
BUILDS_DIR = File.join(ABSOLUTE_PATH,'builds')

NUNIT_CMD_EXE = File.join(ABSOLUTE_PATH,'lib','nunit','nunit-console.exe')
FRAMEWORK_DIR = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', CLR_VERSION)
MSBUILD_EXE = File.join(FRAMEWORK_DIR, 'msbuild.exe')