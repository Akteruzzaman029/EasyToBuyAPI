using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class UploadedFileModel : BaseEntity
    {
        public string UserId { get; set; }  = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>(); // Stored as binary data
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
    }
}
