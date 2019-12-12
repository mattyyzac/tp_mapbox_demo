using Core.Data.GoogleServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Api
{
    [Route("api/g")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly Service.GoogleServices.Place _placeService;

        public GoogleController(Service.GoogleServices.Place placeService)
        {
            this._placeService = placeService;
        }

        [HttpGet("nearby/{lng}/{lat}/{name}/{lang?}")]
        public async Task<ActionResult<GooglePlaceData>> GetNearby(double lng, double lat, string name, string lang)
        {
            var ret = await this._placeService.Nearbysearch(lng, lat, name, lang);
            // return new JsonResult(ret); // same results by using Ok(ret);
            return Ok(ret);
        }

        [HttpGet("find/{name}")]
        public async Task<ActionResult<GooglePlaceData2>> FindFromText(string name)
        {
            var ret = await this._placeService.NearbySearch(name);
            return Ok(ret);
        }

        [HttpGet("detail/{placeid}/{lang?}")]
        public async Task<ActionResult<GooglePlaceDetailData>> GetDetail(string placeid, string lang)
        {
            var ret = await this._placeService.Detail(placeid, lang);
            return Ok(ret);
        }

        [HttpGet("r/{lng}/{lat}/{name}/{width}/{lang?}")]
        public async Task<IActionResult> GetMixedUp(double lng, double lat, string name, int width, string lang)
        {
            var nearby = await this._placeService.Nearbysearch(lng, lat, name, lang);
            if (nearby == null || nearby.status != "OK" || !nearby.results.Any())
            {
                return Ok(new
                {
                    name = string.Empty,
                    photo = string.Empty,
                    formattedAddress = string.Empty,
                    phoneNumber = string.Empty,
                    website = string.Empty
                });
            }

            if (!nearby.results.Any())
            {
                return Ok(new
                {
                    name = string.Empty,
                    photo = string.Empty,
                    formattedAddress = string.Empty,
                    phoneNumber = string.Empty,
                    website = string.Empty
                });
            }
            else
            {
                var result = nearby.results.FirstOrDefault();
                var detail = await this._placeService.Detail(result.place_id, lang);
                var detailResult = detail?.result;

                var photos = result.photos;
                var photo = string.Empty;
                if (photos != null && photos.Any())
                {
                    photo = await this._placeService.Photo(photos.FirstOrDefault().photo_reference, width);
                }

                return Ok(new
                {
                    detailResult.name,
                    photo,
                    formattedAddress = detailResult?.formatted_address,
                    phoneNumber = detailResult?.international_phone_number,
                    detailResult?.website
                });
            }
        }

        [HttpGet("r/{name}")]
        public async Task<IActionResult> GetMixedUp(string name)
        {
            var detail = await this._placeService.NearbySearch(name);
            if (detail != null && detail.status == "OK")
            {
                if (detail.candidates.Any())
                {
                    var candidate = detail.candidates.FirstOrDefault();
                    var photos = candidate.photos;
                    var photoBase64 = "";
                    if (photos.Any())
                    {
                        var photo = candidate.photos.FirstOrDefault();
                        photoBase64 = await this._placeService.Photo(photo.photo_reference, photo.width);
                    }
                    return Ok(new
                    {
                        candidate.name,
                        photo = photoBase64,
                        formattedAddress = candidate.formatted_address,
                        website = candidate.photos.Any() ? candidate.photos.FirstOrDefault().html_attributions.FirstOrDefault() : string.Empty
                    });
                }
                return Ok(new
                {
                    name = string.Empty,
                    photo = string.Empty,
                    formattedAddress = string.Empty,
                    website = string.Empty
                });
            }
            else
            {
                return Ok(new
                {
                    name = string.Empty,
                    photo = string.Empty,
                    formattedAddress = string.Empty,
                    website = string.Empty
                });
            }
        }
    }
}
