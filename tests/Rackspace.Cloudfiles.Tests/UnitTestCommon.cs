using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rackspace.Cloudfiles.Tests
{
	[TestFixture]
	public class TestCommon
	{
		[Test]
		public void TestValidateContainerName()
		{
			Common.ValidateContainerName("foo");
		}
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEmptyValidateContainerName()
		{
			Common.ValidateContainerName("");
		}
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullValidateContainerName()
		{
			Common.ValidateContainerName(null);
		}
		[Test]
		[ExpectedException(typeof(ContainerNameException))]
		public void TestContainerNameLengthValidateContainerName()
		{
			var a = "aaaaaaaaaa";
			while(a.Length <= 256)
			{
				a += a;
			}
			Common.ValidateContainerName(a);
		}
		[Test]
		[ExpectedException(typeof(ContainerNameException))]
		public void TestContainerWithSlashValidateContainerName()
		{
			Common.ValidateContainerName("/");
		}
		[Test]
		public void TestValidateObjectName()
		{
			Common.ValidateObjectName("foo");
		}
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEmptyValidateObjectName()
		{
			Common.ValidateObjectName("");
		}
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullValidateObjectName()
		{
			Common.ValidateObjectName(null);
		}
		[Test]
		[ExpectedException(typeof(ObjectNameException))]
		public void TestObjectNameLengthValidateContainerName()
		{
			var a = "aaaaaaaaaa";
			while(a.Length <= 1024)
			{
				a += a;
			}
			Common.ValidateObjectName(a);
		}
		[Test]
		public void TestProcessMetadata()
		{
			var metadata = new Dictionary<string, string>
			                   {
			                       {"X-Account-Meta-account", "account"},
			                       {"X-Container-Meta-container", "container"},
			                       {"X-Object-Meta-object", "object"},
			                       {"x-foo-bar", "foo"}
			                   };
		    var ret =  Common.ProcessMetadata(metadata);
			Assert.IsTrue(ret["metadata"]["account"] == "account");
			Assert.IsTrue(ret["metadata"]["container"] == "container");
			Assert.IsTrue(ret["metadata"]["object"] == "object");
			Assert.IsTrue(ret["headers"]["x-foo-bar"] == "foo");
		}
	}
}