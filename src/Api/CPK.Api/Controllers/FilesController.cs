using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.Models;
using CPK.FilesModule.PrimaryPorts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = CPK.FilesModule.Entities.File;

namespace CPK.Api.Controllers
{
    [ApiController]
    [Route("api/v1/files")]
    public sealed class FilesController : ControllerBase
    {
        private readonly IFilesService _service;

        public FilesController(IFilesService service)
        {
            _service = service;
        }

        [Authorize(Roles = "cpkadmin")]
        [HttpPost("upload")]
        public async Task<Guid> Upload([FromForm(Name = "file")]IFormFile file)
       // public async Task<Guid> Upload(FileModel file)
        {
            var form = HttpContext.Request.Form;
            var sss = form.Files.Count;
            var contentType = form["ContentType"];
            var size = form["Size"];
            var fileName = form["FileName"];
            await using var stream = file.OpenReadStream();
            return await _service.Upload(new File(file.FileName, stream, contentType));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var file = await _service.Download(id);
            return File(file.Data, file.ContentType, file.Name, true);
        }

        [HttpGet]
        public async Task<List<FileModel>> GetAll()
        {
            var files = await _service.GetAll();
            return files.Select(f => new FileModel(f)).ToList();
        }
    }
}
