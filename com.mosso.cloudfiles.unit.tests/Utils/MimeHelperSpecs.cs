using System;
using com.mosso.cloudfiles.Utils;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Utils
{
    [TestFixture]
    public class When_finding_out_built_in_mime_type_for_a_specific_file_extension
    {
        private const string ApplicationOctetStream = "application/octet-stream";

        [Test]
        public void Should_get_octet_stream_as_default_when_file_is_null()
        {
            Assert.That(MimeHelper.GetMimeType(null), Is.EqualTo(ApplicationOctetStream));
        }

        [Test]
        public void Should_get_octet_stream_as_default_when_file_is_emptystring()
        {
            Assert.That(MimeHelper.GetMimeType(""), Is.EqualTo(ApplicationOctetStream));
        }

        [Test]
        public void Should_get_octet_stream_when_mime_type_is_not_in_user_list_or_uilt_in_list()
        {
            Assert.That(MimeHelper.GetMimeType(string.Format("{0}{1}", "file", Guid.NewGuid())), Is.EqualTo(ApplicationOctetStream));
        }

        [Test]
        public void Should_get_the_mime_type()
        {
            Assert.That(MimeHelper.GetMimeType("file.aif"), Is.EqualTo("audio/aiff"));
            Assert.That(MimeHelper.GetMimeType("file.xlt"), Is.EqualTo("application/vnd.ms-excel"));
            Assert.That(MimeHelper.GetMimeType("file.vmf"), Is.EqualTo("application/vocaltec-media-file"));
            Assert.That(MimeHelper.GetMimeType("file.rtf"), Is.EqualTo("text/richtext"));
            Assert.That(MimeHelper.GetMimeType("file.midi"), Is.EqualTo("audio/midi"));
        }
    }

    [TestFixture]
    public class When_finding_out_user_mime_types_for_a_specific_file_extension
    {
        private const string ApplicationOctetStream = "application/octet-stream";

        [Test]
        public void Should_get_octet_stream_when_file_is_null()
        {
            Assert.That(MimeHelper.GetMimeType(null), Is.EqualTo(ApplicationOctetStream));
        }

        [Test]
        public void Should_get_octet_stream_when_file_is_emptystring()
        {
            Assert.That(MimeHelper.GetMimeType(""), Is.EqualTo(ApplicationOctetStream));
        }

        [Test]
        public void Should_get_octet_stream_when_mime_type_is_not_in_user_list_or_built_in_list()
        {
            Assert.That(MimeHelper.GetMimeType(string.Format("{0}.{1}", "file", Guid.NewGuid())), Is.EqualTo(ApplicationOctetStream));
        }

        [Test]
        public void Should_get_the_user_mime_type()
        {
            var fileExtension = string.Format(".{0}", Guid.NewGuid());
            const string mimeType = "application/csharp-cloufiles-mimetype-tests";
            MimeHelper.UserTypes.Add(fileExtension, mimeType);
            Assert.That(MimeHelper.GetMimeType(string.Format("{0}.{1}", "file", fileExtension)), Is.EqualTo(mimeType));
        }
    }
}