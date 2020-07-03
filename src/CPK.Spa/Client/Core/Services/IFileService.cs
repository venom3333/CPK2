using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CPK.Spa.Client.Core.Models;
using MatBlazor;

namespace CPK.Spa.Client.Core.Services
{
    public interface IFileService
    {
        string Error { get; }
        Task<Guid> Upload(IMatFileUploadEntry entry);
        Task<FileModel> Get(Guid id);
        Task Delete(string id);
        string FileUri(FileModel model);
    }
}