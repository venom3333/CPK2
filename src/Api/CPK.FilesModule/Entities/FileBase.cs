using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.FilesModule.Entities
{
    public class FileBase
    {
        public Guid Id { get; }
        public byte[] Hash { get; }
        public long Size { get; }
        public string Name { get; }
        public DateTime Created { get; }
        public string ContentType { get; }

        public FileBase(Guid id, byte[] hash, long size, string name, DateTime created, string contentType)
        {
            Validator.Begin(id, nameof(id))
                .NotDefault()
                .Map(hash, nameof(hash))
                .NotNull()
                .NotEmpty()
                .Map(size, nameof(size))
                .IsGreater(0)
                .Map(name, nameof(name))
                .NotNull()
                .NotWhiteSpace()
                .Map(created, nameof(created))
                .NotDefault()
                .IsLess(DateTime.UtcNow.AddDays(1))
                .Map(contentType, nameof(contentType))
                .NotNull()
                .NotWhiteSpace()
                .ThrowApiException(nameof(File), nameof(File));
            Id = id;
            Hash = hash;
            Size = size;
            Name = name;
            Created = created;
            ContentType = contentType;
        }
    }
}
