using System;
using System.IO;

namespace CPK.Spa.Client.Core.Models
{
    public sealed class FileModel
    {
        public Guid Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public Stream Content { get; set; }
    }
}