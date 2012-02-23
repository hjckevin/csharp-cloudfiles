using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rackspace.Cloudfiles;
namespace Rackspace.Cloudfiles.Test
{
	[TestFixture]
	public class TestConnection
	{
		Connection conn;
		private string username = "conradweidenkel";
		private string api_key = "1e6304d0e9a0e4cad054069bd5c3a531";
		private string prefix = "._cloud_files_C_sharp_tests_";
		private readonly string container_meta_prefix = "x-container-meta-";
		private List<string> created_containers;
		private List<Dictionary<string, string>> container_object_pair;
		[SetUp]
		public void Setup()
		{
			created_containers = new List<string>();
			container_object_pair = new List<Dictionary<string, string>>();
			UserCredentials user_creds = new UserCredentials(this.username, this.api_key);
			this.conn = new Connection(user_creds);
		}
		[TearDown]
		public void TearDown()
		{
			foreach (Dictionary<string, string> dict in container_object_pair)
			{
				this.conn.DeleteStorageItem(dict["container"], dict["object"]);
			}
			foreach (string container in created_containers)
			{
				this.conn.DeleteContainer(container);
			}
		}
		[Test]
		public void TestConstructor()
		{
			//We test the constructor every time we setup the functional tests..
			//So this may seem like light testing but the object is already instantiated.
			List<Dictionary<string, string>> containers;
			string container = this.CreateName();
			Console.WriteLine(container);
			created_containers.Add(container);
			this.conn.CreateContainer(container);
			containers = this.conn.GetContainers();
			Assert.Greater(containers.Count, 1);		
		}
		[Test]
		public void TestCreateContainer()
		{
			string container_name = this.CreateName();
			created_containers.Add(container_name);
			this.conn.CreateContainer(container_name);
			Container container = this.conn.GetContainerInformation(container_name);
			Assert.AreEqual(container.Name, container_name);
		}
		[Test]
		public void TestDeleteContainer()
		{
		    List<Dictionary<string, string>> containers;
			Dictionary<GetContainersListParameters, string> list_params = new Dictionary<GetContainersListParameters, string>();
			list_params.Add(GetContainersListParameters.Prefix, this.prefix);
			string container_name = this.CreateName();
			this.conn.CreateContainer(container_name);
			containers = this.conn.GetContainers(list_params);
			Assert.AreEqual(containers.Count, 1);
			this.conn.DeleteContainer(container_name);
			containers = this.conn.GetContainers(list_params);
			Assert.AreEqual(containers.Count, 0);
		}
		[Test]
	    public void TestDeleteStorageItem()
		{
			List<Dictionary<string, string>> objects = new List<Dictionary<string, string>>();
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			this.created_containers.Add(container_name);
			this.conn.CreateContainer(container_name);
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
            Stream stream = new MemoryStream(byte_arr);
			this.conn.PutStorageItem(container_name, stream, object_name);
			this.conn.DeleteStorageItem(container_name, object_name);
			objects = this.conn.GetContainerItemList(container_name);
			Assert.IsTrue(objects.Count == 0);
		}
		[Test]
	    public void TestGetAccountInformation()
		{
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			Dictionary<string, string> cont_obj_pair = new Dictionary<string, string>();
			cont_obj_pair.Add("container", container_name);
			cont_obj_pair.Add("object", object_name);
			this.created_containers.Add(container_name);
			this.container_object_pair.Add(cont_obj_pair);
			this.conn.CreateContainer(container_name);
			this.conn.PutStorageItem(container_name, stream, object_name);
			AccountInformation account = this.conn.GetAccountInformation();
			Assert.IsTrue(account.BytesUsed > 0);
			Assert.IsTrue(account.ContainerCount > 0);
		}
		[Test]
	    public void TestGetContainerInformation()
		{
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			Dictionary<string, string> cont_obj_pair = new Dictionary<string, string>();
			cont_obj_pair.Add("container", container_name);
			cont_obj_pair.Add("object", object_name);
			this.created_containers.Add(container_name);
			this.container_object_pair.Add(cont_obj_pair);
			this.conn.CreateContainer(container_name);
			this.conn.PutStorageItem(container_name, stream, object_name);
			Container cont = this.conn.GetContainerInformation(container_name);
			Assert.IsTrue(cont.ByteCount == 1);
			Assert.IsTrue(cont.ObjectCount == 1);
		}
		[Test]
	    public void TestGetContainerItemList()
		{
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			Dictionary<string, string> cont_obj_pair = new Dictionary<string, string>();
			cont_obj_pair.Add("container", container_name);
			cont_obj_pair.Add("object", object_name);
			this.created_containers.Add(container_name);
			this.container_object_pair.Add(cont_obj_pair);
			this.conn.CreateContainer(container_name);
			this.conn.PutStorageItem(container_name, stream, object_name);
			List<Dictionary<string, string>> cont = this.conn.GetContainerItemList(container_name);
			Assert.IsTrue(cont.Count == 1);
			Assert.IsTrue(cont[0]["name"] == object_name);
		}
		[Test]
	    public void TestGetContainers()
		{
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			Dictionary<GetContainersListParameters, string> query = 
				new Dictionary<GetContainersListParameters, string>();
			query.Add(GetContainersListParameters.Prefix, this.prefix);
			Dictionary<string, string> cont_obj_pair = new Dictionary<string, string>();
			cont_obj_pair.Add("container", container_name);
			cont_obj_pair.Add("object", object_name);
			this.created_containers.Add(container_name);
			this.container_object_pair.Add(cont_obj_pair);
			this.conn.CreateContainer(container_name);
			this.conn.PutStorageItem(container_name, stream, object_name);
			List<Dictionary<string, string>> cont = this.conn.GetContainers(query);
			Assert.IsTrue(cont.Count == 1);
			Assert.IsTrue(cont[0]["name"] == container_name);
		}
		[Test]
	    public void TestGetPublicContainers()
		{
			string container_name = this.CreateName();
			this.created_containers.Add(container_name);
			this.conn.CreateContainer(container_name);
			List<Dictionary<string, string>> cont = this.conn.GetPublicContainers();
			Assert.IsTrue(cont.Count >= 1);
		}
		[Test]
	    public void TestGetStorageItem()
		{
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			this.created_containers.Add(container_name);
			Dictionary<string, string> cont_object_pair = new Dictionary<string, string>();
			cont_object_pair.Add("container", container_name);
			cont_object_pair.Add("object", object_name);
			this.container_object_pair.Add(cont_object_pair);
			this.conn.CreateContainer(container_name);
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			this.conn.PutStorageItem(container_name, stream, object_name);
			StorageItem obj = this.conn.GetStorageItem(container_name, object_name);
			Assert.IsTrue(obj.ContentLength == 1);
			obj.ObjectStream.Read(byte_arr, 0, 1);
			Assert.IsTrue(encoder.GetString(byte_arr) == "a");
			stream.Close();
			obj.ObjectStream.Close();
		}
		[Test]
	    public void TestMarkContainerAsPublic()
		{
		    string container_name = this.CreateName();
			this.created_containers.Add(container_name);
			this.conn.CreateContainer(container_name);
			this.conn.MarkContainerAsPublic(container_name);
			Container cont = this.conn.GetContainerInformation(container_name);
			Assert.IsTrue(cont.CdnEnabled == true);
		}
		[Test]
	    public void TestMarkContainerAsPrivate()
		{
			string container_name = this.CreateName();
			this.created_containers.Add(container_name);
			this.conn.CreateContainer(container_name);
			this.conn.MarkContainerAsPublic(container_name);
			Container cont = this.conn.GetContainerInformation(container_name);
			Assert.IsTrue(cont.CdnEnabled);
			this.conn.MarkContainerAsPrivate(container_name);
			cont = this.conn.GetContainerInformation(container_name);
			Assert.IsFalse(cont.CdnEnabled);
		}
		[Test]
	    public void TestPutStorageItem()
		{
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			this.created_containers.Add(container_name);
			Dictionary<string, string> cont_object_pair = new Dictionary<string, string>();
			cont_object_pair.Add("container", container_name);
			cont_object_pair.Add("object", object_name);
			this.container_object_pair.Add(cont_object_pair);
			this.conn.CreateContainer(container_name);
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			this.conn.PutStorageItem(container_name, stream, object_name);
			StorageItem obj = this.conn.GetStorageItem(container_name, object_name);
			Assert.IsTrue(obj.ContentLength == 1);
			obj.ObjectStream.Read(byte_arr, 0, 1);
			Assert.IsTrue(encoder.GetString(byte_arr) == "a");
			stream.Close();
			obj.ObjectStream.Close();
		}
		[Test]
	    public void TestSetAccountMetaInformation()
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("X-Account-Meta-foo", "foo"); 
			this.conn.SetAccountMetaInformation(headers);
			AccountInformation acct = this.conn.GetAccountInformation();
			Assert.IsTrue(acct.Metadata.ContainsKey("foo"));
			Assert.IsTrue(acct.Metadata["foo"] == "foo");
		}
		[Test]
	    public void TestSetContainerMetaInformation()
		{
			string container_name = this.CreateName();
			this.created_containers.Add(container_name);
			this.conn.CreateContainer(container_name);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("X-Container-Meta-foo", "foo"); 
			this.conn.SetContainerMetaInformation(container_name, headers);
			Container cont = this.conn.GetContainerInformation(container_name);
			Assert.IsTrue(cont.Metadata.ContainsKey("foo"));
			Assert.IsTrue(cont.Metadata["foo"] == "foo");
		}
		[Test]
	    public void TestSetObjectMetaInformation()
		{
			string container_name = this.CreateName();
			string object_name = this.CreateName();
			this.created_containers.Add(container_name);
			Dictionary<string, string> cont_obj_pair = new Dictionary<string, string>();
			cont_obj_pair.Add("container", container_name);
			cont_obj_pair.Add("object", object_name);
			this.container_object_pair.Add(cont_obj_pair);
			this.conn.CreateContainer(container_name);
			UTF8Encoding encoder = new UTF8Encoding();
			byte[] byte_arr = encoder.GetBytes("a");
			Stream stream = new MemoryStream(byte_arr);
			this.conn.PutStorageItem(container_name, stream, object_name);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("X-Object-Meta-foo", "foo"); 
			this.conn.SetStorageItemMetaInformation(container_name, object_name, headers);
			StorageItem obj = this.conn.GetStorageItem(container_name, object_name);
			Assert.IsTrue(obj.Metadata.ContainsKey("foo"));
			Assert.IsTrue(obj.Metadata["foo"] == "foo");
		}
		public string CreateName()
		{
			return this.prefix + Guid.NewGuid().ToString();
		}
	}
}

