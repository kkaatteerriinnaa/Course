using Course.Data.DAL;
using Course.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Course.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly DataAccessor _dataAccessor;
        public LocationController(DataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        [HttpGet("{id}")]
        public List<Location> DoGet(String id)
        {
            return _dataAccessor.ContentDao.GetLocations(id);
        }
        [HttpPost]
        public string DoPost([FromForm] LocationPostModel model)
        {
                String? fileName = null;
                if (model.Photo != null)
                {
                    String ext = Path.GetExtension(model.Photo.FileName);
                    String path = Directory.GetCurrentDirectory() + "/wwwroot/img/content/";

                    String pathName;
                    do
                    {
                        fileName = Guid.NewGuid() + ext;
                        pathName = path + fileName;
                    } while (System.IO.File.Exists(pathName));
                    using var stream = System.IO.File.OpenWrite(pathName);
                    model.Photo.CopyTo(stream);
                }
                try
                {
                    _dataAccessor.ContentDao.AddLocation(
                        name: model.Name,
                        description: model.Description,
                        CategoryId: model.CategoryId,
                        Stars: model.Stars,
                        PhotoUrl: fileName
                        );
                    Response.StatusCode = StatusCodes.Status201Created;
                    return "Ok";
                }
                catch (Exception ex)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return "Errors";
                }
          }
        [HttpPatch]
        public Location? DoPatch(String slug)
        {
            return _dataAccessor.ContentDao.GetLocationBySlug(slug);
        }
    }

    public class LocationPostModel
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Guid CategoryId { get; set; }
        public int Stars { get; set; }
        public IFormFile Photo { get; set; }
    }

}
