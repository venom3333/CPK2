using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.FilesModule.Entities;
using CPK.FilesModule.PrimaryPorts;
using CPK.FilesModule.SecondaryPorts;
using CPK.SharedModule.Entities;

namespace CPK.FilesModule.PrimaryAdapters
{
    public class FilesService : IFilesService
    {
        private readonly ICategoryFilesRepository _repository;
        private readonly IFilesUow _uow;

        public FilesService(ICategoryFilesRepository repository, IFilesUow uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public Task<List<FileBase>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<Guid> Upload(File file)
        {
            var old = await _repository.Find(file.Hash, file.Size);
            if (old != default)
                return old;
            // if (!file.IsImage())
            //     throw new ApiException(ApiExceptionCode.FileIsNotImage);
            await _repository.Add(file);
            await _uow.SaveAsync();
            return file.Id;
        }

        public Task<File> Download(Guid id)
        {
            return _repository.Get(id);
        }
    }
}
