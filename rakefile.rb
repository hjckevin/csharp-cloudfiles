require 'rubygems'
require 'albacore'
require File.expand_path('../Properties', __FILE__)
require "fileutils"

desc "compiles, runs tests and creates zip file"
task :all => [:default]

desc "compiles, runs tests and creates zip file"
task :default => [:compile, :tests, :zip]

################
#  COMPILE THE CODE
################

desc "Update the version information for the build"
assemblyinfo :assemblyinfo do |asm|
  puts "The build number is #{RELEASE_BUILD_NUMBER}"
  asm.version = RELEASE_BUILD_NUMBER
  asm.company_name = COMPANY
  asm.product_name = PRODUCT
  asm.description = DESCRIPTION
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO
end

desc "Compiles the app"
msbuild :compile => :assemblyinfo do |msb|
  msb.command = MSBUILD_EXE
  msb.properties :configuration => COMPILE_TARGET
  msb.targets :Rebuild
  msb.verbosity = "minimal"
  msb.solution = SLN_FILE
end

################
#  RUN TESTS
################
desc "Run integration and unit tests"
task :tests => [:unit_tests, :integration_tests]

desc "Run unit tests"
nunit :unit_tests => :compile do |nunit|
  nunit.command = NUNIT_CMD_EXE
  nunit.assemblies UNIT_TESTS_DLL
  nunit.options '/xml=csharp-cloudfiles-unit-tests-results.xml'
end

desc "Run integration tests"
nunit :integration_tests => :compile  do |nunit|
  if ENV['CRED_FILE_LOC']
	puts "ENVIRONMENT VARIABLE: #{ENV['CRED_FILE_LOC']}"
    puts "copying file from #{ENV['CRED_FILE_LOC']} to #{INTEGRATION_TESTS_CONFIG_FILE}"
	copy(ENV['CRED_FILE_LOC'], INTEGRATION_TESTS_CONFIG_FILE)
  end
  
  if !File.exists?(INTEGRATION_TESTS_CONFIG_FILE)
	if File.exists?("#{ABSOLUTE_PATH}/Credentials.config")
	  puts "copying file from #{ABSOLUTE_PATH}/Credentials.config to #{INTEGRATION_TESTS_CONFIG_FILE}"
	  copy("#{ABSOLUTE_PATH}/Credentials.config", INTEGRATION_TESTS_CONFIG_FILE)
	  exit
	end
    puts "Credentials.config file does not exist.  Please run 'rake create_credentials_config'"
    exit
  end
  
  nunit.command = NUNIT_CMD_EXE
  nunit.assemblies INTEGRATION_TESTS_DLL
  nunit.options '/xml=csharp-cloudfiles-integration-tests-results.xml'
end

########################
#  CREATING ZIP FILES
########################

desc "Create a binary zip"
zip do |zip|
  puts "CREATING ZIP"
  Dir.mkdir BUILDS_DIR if !File.directory?(BUILDS_DIR)  
  file = "#{ZIP_FILE_PREFIX}-bin-#{RELEASE_BUILD_NUMBER}.zip"
  File.delete(file) if File.exists?(file)

  zip.output_path = BUILDS_DIR
  zip.directories_to_zip CORE_DLL_DIR
  zip.output_file = file

  puts "ZIP CREATION COMPLETE"
end

##################
#  CONFIG FILE
##################

desc "create credentials config template file"
task :create_credentials_config do
  credentialsConfigTemplateBuilder = IntegrationTestsCredentialsFilesBuilder.new
  credentialsConfigTemplateBuilder.write
end

class IntegrationTestsCredentialsFilesBuilder
	def write
		template = %q{<?xml version="1.0" encoding="utf-8"?>
    <credentials>
      <username>PUT USERNAME HERE</username>
      <api_key>PUT API KEY HERE</api_key>
	  <auth_endpoint>https://auth.api.rackspacecloud.com/v1.0</auth_endpoint>
	  <!-- <auth_endpoint>https://lon.auth.api.rackspacecloud.com/v1.0</auth_endpoint> -->
    </credentials>
		}.gsub(/^    /, '')
		  
	  erb = ERB.new(template, 0, "%<>")
	  
	  File.open("#{ABSOLUTE_PATH}/Credentials.config", 'w') do |file|
		  file.puts erb.result(binding) 
	  end
	end
end