using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CPK.FilesModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class FileDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Path { get; set; }
        public byte[] Hash { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string ContentType { get; set; }

        public FileBase ToInfo() => new FileBase(Id, Hash, Size, Name, Created,ContentType);

        public File ToFile() => new File(Id, Hash, Size, Name, Created, System.IO.File.OpenRead(Path),ContentType);

        public FileDto()
        {
            //For framework
        }

        public FileDto(File file, string path)
        {
            Id = file.Id;
            Path = path;
            Hash = file.Hash;
            Size = file.Size;
            Name = file.Name;
            Created = file.Created;
            ContentType = file.ContentType;
        }
    }
}
