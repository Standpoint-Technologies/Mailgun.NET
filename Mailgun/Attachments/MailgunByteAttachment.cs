using System.IO;

namespace Mailgun.Attachments
{
    /// <summary>
    /// An email attachment with content from a byte array.
    /// </summary>
    public class MailgunByteAttachment : MailgunAttachment
    {
        private byte[] _content;

        /// <summary>
        /// Gets or sets the content bytes of the attachment.
        /// </summary>
        public byte[] Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                FileContentStream = new MemoryStream(_content);
            }
        }

        /// <summary>
        /// Constructs a new MailgunByteAttachment with the specified filename and content.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        public MailgunByteAttachment(string filename, byte[] content)
            : base(filename, new MemoryStream(content))
        {
            _content = content;
        }
    }
}
