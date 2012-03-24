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
		public void TestEmptyValidateContainerName()
		{
			Assert.Throws<ArgumentNullException>(() => Common.ValidateContainerName(""));
		}
		[Test]
		public void TestNullValidateContainerName()
		{
			Assert.Throws<ArgumentNullException>(() => Common.ValidateContainerName(null));
		}
		[Test]
		public void TestContainerNameLengthValidateContainerName()
		{
			var a = "aaaaaaaaaa";
			while(a.Length <= 256)
			{
				a += a;
			}
            Assert.Throws<ContainerNameException>(() => Common.ValidateContainerName(a));
		}
		[Test]
		public void TestContainerWithSlashValidateContainerName()
		{
			Assert.Throws<ContainerNameException>(() => Common.ValidateContainerName("/"));
		}
		[Test]
		public void TestValidateObjectName()
		{
			Common.ValidateObjectName("foo");
		}
		[Test]
		public void TestEmptyValidateObjectName()
		{
			Assert.Throws<ArgumentNullException>(() => Common.ValidateObjectName(""));
		}
		[Test]
		public void TestNullValidateObjectName()
		{
            Assert.Throws<ArgumentNullException>(() => Common.ValidateObjectName(null));
		}
		[Test]
		public void TestObjectNameLengthValidateContainerName()
		{
			var a = "aaaaaaaaaa";
			while(a.Length <= 1024)
			{
				a += a;
			}
            Assert.Throws<ObjectNameException>(() => Common.ValidateObjectName(a));
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