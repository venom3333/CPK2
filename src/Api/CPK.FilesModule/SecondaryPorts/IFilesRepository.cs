using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.FilesModule.Entities;

namespace CPK.FilesModule.SecondaryPorts
{
    public interface IFilesRepository
    {
        Task<List<FileBase>> GetAll();
        Task<File> Get(Guid id);
        Task<Guid> Find(byte[] hash, long size);
        Task Add(File file);
        Task Remove(Guid id, Guid? parentEntityId = null);
    }
}
