using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Repositories;
using MatBlazor;
using Microsoft.Extensions.Logging;

namespace CPK.Spa.Client.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IApiRepository _repository;
        private readonly ILogger<FileService> _logger;

        public FileService(IApiRepository repository, ILogger<FileService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public string Error { get; private set; }

        public string FileUri(FileModel model) => _repository.GetFullUrl($"files/{model.Id}");

        public async Task<Guid> Upload(IMatFileUploadEntry entry)
        {
            await using var ms = new MemoryStream();
            await entry.WriteToStreamAsync(ms);
            var model = new FileModel
            {
                Content = ms,
                FileName = entry.Name,
                ContentType = entry.Type,
                Size = entry.Size
            };
            (Guid Guid, string Message) result = await _repository.UploadFile(model);
            if (!string.IsNullOrWhiteSpace(result.Message)) Error = result.Message.ToString();
            return result.Guid;
        }

        public async Task<FileModel> Get(Guid id)
        {
            (FileModel Model, string Message) result = await _repository.GetFile(id);
            if (string.IsNullOrWhiteSpace(result.Message)) Error = result.Message;
            return result.Model;
        }

        public async Task Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
