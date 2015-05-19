using System.IO;

namespace Mailgun.Attachments
{
    /// <summary>
    /// A generic email attachment from a stream. Can be extended to provide mroe specific attachment types or used as-is.
    /// </summary>
    public class MailgunAttachment
    {
        /// <summary>
        /// Gets or sets the name of the attachment.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the stream containing the attachment contents.
        /// </summary>
        public Stream FileContentStream { get; set; }


        /// <summary>
        /// Constructs a new MailgunAttachment with the specified name and content stream.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileContentStream"></param>
        public MailgunAttachment(string filename, Stream fileContentStream)
        {
            FileName = filename;
            FileContentStream = fileContentStream;
        }
    }
}
