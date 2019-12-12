using Core.Data.Srvice;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SceneriesController : ControllerBase
    {
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IEnumerable<FeatureCollection>>> Get()
        // public async Task<IActionResult> Get()
        {
            var sceneries = await Task.FromResult(FileHandlerService.XlsHandler());
            var result = new FeatureCollection
            {
                Features = sceneries.Select(p => new FeaturePoint
                {
                    Geometry = new Geometry(p.Coordinates.Longitude, p.Coordinates.Latidue),
                    Properties = new Property
                    {
                        Name = p.Name
                    }
                })
            };
            return Ok(result);
        }
    }
}
