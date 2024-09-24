using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebAPI.Controllers;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostMannersController : BaseController<HostManner>
    {
        public HostMannersController(IBaseService<HostManner> baseService) : base(baseService)
        {
        }

        //public override async Task<IActionResult> GetPagedData([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        //{
        //    Expression<Func<HostManner, bool>> filter = hm => !string.IsNullOrEmpty(hm.Label) && hm.Label.Contains("Nhẹ nhàng, sâu lắng");

        //    Func<IEnumerable<HostManner>, IOrderedEnumerable<HostManner>> orderBy = hms => hms.OrderBy(hm => hm.Label);

        //    var result = await BaseService.GetPagedDataAsync(filter, orderBy, pageIndex, pageSize);
        //    return Ok(result);
        //}
    }
}
