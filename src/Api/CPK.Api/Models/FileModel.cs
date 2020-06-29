using System;
using CPK.FilesModule.Entities;

namespace CPK.Api.Models
{
    public sealed class FileModel
    {
        public Guid Id { get; set; }
        public byte[] Hash { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string ContentType { get; set; }

        public FileModel()
        {
            //For framework
        }

        public FileModel(FileBase @base)
        {
            Id = @base.Id;
            Hash = @base.Hash;
            Size = @base.Size;
            Name = @base.Name;
            Created = @base.Created;
            ContentType = @base.ContentType;
        }
    }
}
