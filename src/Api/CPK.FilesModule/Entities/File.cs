using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.FilesModule.Entities
{
    public sealed class File : FileBase
    {
        public Stream Data { get; }

        public static byte[] ComputeFileHash(Stream stream)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(stream);
        }

        public File(string name, Stream data, string contentType) : this(name, DateTime.UtcNow, data, contentType) { }

        public File(string name, DateTime created, Stream data, string contentType) : this(ComputeFileHash(data), name, created, data, contentType) { }

        public File(byte[] hash, string name, DateTime created, Stream data, string contentType) : this(new Guid(hash), hash, name, created, data, contentType) { }

        public File(Guid id, byte[] hash, string name, DateTime created, Stream data, string contentType) : this(id, hash, data.Length, name, created, data, contentType) { }

        public File(Guid id, byte[] hash, long size, string name, DateTime created, Stream data, string contentType) : base(id, hash, size, name, created, contentType)
        {
            Validator.Begin(data, nameof(data))
                .NotNull()
                .Map(data?.Length ?? 0, "data.Length")
                .IsEquals(size)
                .ThrowApiException(nameof(File), nameof(File));
            data.Seek(0, SeekOrigin.Begin);
            Data = data;
        }

        public bool IsImage()
        {
            byte[] chkBytejpg = { 255, 216, 255, 224 };
            byte[] chkBytebmp = { 66, 77 };
            byte[] chkBytegif = { 71, 73, 70, 56 };
            byte[] chkBytepng = { 137, 80, 78, 71 };
            byte[] bytFile = new byte[4];
            Data.Read(bytFile, 0, bytFile.Length);
            Data.Seek(0, SeekOrigin.Begin);
            return EqBytes(chkBytejpg) ||
                   EqBytes(chkBytebmp) ||
                   EqBytes(chkBytegif) ||
                   EqBytes(chkBytepng);
            bool EqBytes(byte[] bytes) => bytes.Zip(bytFile, (a, b) => a == b).All(x => x);
        }
    }
}
