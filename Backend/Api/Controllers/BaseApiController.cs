using Api.Helpers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected async Task<IActionResult> CreatePageResult<T>(ISpecification<T>spec ,IGenericRepository<T>repo, int pageSize, int pageIndex)where T : class
        {
            var items = await repo.ListAsync(spec);
            var count =await repo.CountAsync(spec);
            return Ok( new Pagination<T>(pageSize, pageIndex, count, items));
        }
        protected async Task<IActionResult> CreatePageResult<T,TResult>(ISpecification<T,TResult> spec, IGenericRepository<T> repo, int pageSize, int pageIndex) where T : class
        {
            var items = await repo.ListAsync(spec);
            var count = await repo.CountAsync(spec);
            return Ok(new Pagination<TResult>(pageSize, pageIndex, count, items));
        }


    }
}
