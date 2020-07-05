using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CPK.FilesModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class CategoryFileDto : FileDto
    {

        public CategoryFileDto()
        {
            //For framework
        }

        public CategoryFileDto(File file, string path) : base(file, path)
        {
            
        }
    }
}
