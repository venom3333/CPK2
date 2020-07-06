using System;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.Models;
using CPK.Api.Models.News;
using CPK.NewsModule.Dto;
using CPK.NewsModule.Entities;
using CPK.NewsModule.PrimaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPK.Api.Controllers
{
    [ApiController]
    [Route("api/v1/News")]
    [Authorize(Roles = "cpkadmin")]
    public sealed class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        private const uint MaxTake = 1000;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("filter")]
        public async Task<IActionResult> GetAll(NewsFilterModel filter)
        {
            var News = await _service.Get(
                new NewsFilter(
                    new PageFilter(
                        filter.Skip,
                        filter.Take,
                        MaxTake
                    ),
                    filter.Id,
                    filter.Title,
                    filter.Descending,
                    filter.OrderBy
                )
            );
            return Ok(new PageResultModel<NewsModel>()
            {
                ProductsFilter = News.PageFilter,
                TotalCount = News.TotalCount,
                Value = News.Value.Select(p => new NewsModel(p)).ToList()
            });
        }

        [HttpPost("add")]
        public async Task<Guid> Add(AddNewsModel model)
        {
            var id = Guid.NewGuid();
            await _service.Add(
                new News(
                    new Id(id),
                    new Title(model.Title),
                    new ShortDescription(model.ShortDescription),
                    new Text(model.Text),
                    new Image(model.ImageId)
                )
            );
            return id;
        }

        [HttpPut("update")]
        public async Task<int> Update(NewsModel model)
        {
            return await _service.Update(model.ToNews());
        }

        [HttpDelete("remove/{id}/{version}")]
        public async Task<int> Remove(string id, string version)
        {
            return await _service.Remove(new ConcurrencyToken<Id>(version, new Id(new Guid(id))));
        }
    }
}