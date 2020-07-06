using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.FilesModule.Entities;
using CPK.FilesModule.SecondaryPorts;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
            return file?.ToFile();
        }

        public async Task Remove(Guid id, Guid? parentEntityId = null)
        {
            try
            {
                var isConstraintExists =
                    await _context.ProductCategories.AnyAsync(pc =>
                        pc.Id != parentEntityId && pc.ImageId == id);
                if (!isConstraintExists)
                {
                    isConstraintExists =
                        await _context.News.AnyAsync(parentEntity =>
                            parentEntity.Id != parentEntityId && parentEntity.ImageId == id);
                }

                if (!isConstraintExists)
                {
                    var image = await _context.Files.FindAsync(id);
                    if (image != null)
                    {
                        _context.Files.Remove(image);
                        if (System.IO.File.Exists(image.Path))
                        {
                            System.IO.File.Delete(image.Path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, ex.Message);
            }
        }

        public async Task<Guid> Find(byte[] hash, long size)
        {
            var file = await _context.Files.FirstOrDefaultAsync(f => f.Hash == hash && f.Size == size);
            return file?.Id ?? default;
        }

        public async Task Add(File file)
        {
            var path = Path.Combine(_config.FilesDir, $"{file.Created:yyyy_MM_dd_hh_mm_ss_fff}_{file.Id}");
            await using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None,
                bufferSize: 4_000_000);
            await file.Data.CopyToAsync(stream);
            await stream.FlushAsync();
            await _context.Files.AddAsync(new FileDto(file, path));
        }
    }
}