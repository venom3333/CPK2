using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.FilesModule.Entities;
using CPK.FilesModule.SecondaryPorts;
using Microsoft.EntityFrameworkCore;
using File = CPK.FilesModule.Entities.File;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class FilesRepository : IFilesRepository
    {
        private readonly CpkContext _context;
        private readonly IConfig _config;

        public FilesRepository(CpkContext context, IConfig config)
        {
            _context = context;
            _config = config;
        }
        public async Task<List<FileBase>> GetAll()
        {
            var files = await _context.Files.ToListAsync();
            return files.Select(f => f.ToInfo()).ToList();
        }

        public async Task<File> Get(Guid id)
        {
            var file = await _context.Files.FindAsync(id);
            return file.ToFile();
        }

        public async Task<Guid> Find(byte[] hash, long size)
        {
            var file = await _context.Files.FirstOrDefaultAsync(f => f.Hash == hash && f.Size == size);
            return file?.Id ?? default;
        }

        public async Task Add(File file)
        {
            var path = Path.Combine(_config.FilesDir, $"{file.Created:yyyy_MM_dd_hh_mm_ss_fff}_{file.Id}");
            using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, bufferSize: 4_000_000);
            await file.Data.CopyToAsync(stream);
            await stream.FlushAsync();
            _context.Files.Add(new FileDto(file, path));
        }
    }
}
