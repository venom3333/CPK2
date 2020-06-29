using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.FilesModule.Entities;

namespace CPK.FilesModule.PrimaryPorts
{
    public interface IFilesService
    {
        Task<List<FileBase>> GetAll();
        Task<Guid> Upload(File file);
        Task<File> Download(Guid id);
    }
}
