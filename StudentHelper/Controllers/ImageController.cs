using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentHelper.Data;
using StudentHelper.Models;

namespace StudentHelper.Controllers
{
    public class ImageController : ApiController
    {

        public HttpResponseMessage GetImage(int id)
        {
            StudentHelperContext db = new StudentHelperContext();
            Image image = db.Images.Find(id);
            if(image != null)
            {
                MemoryStream ms = new MemoryStream(image.ImageData);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(ms)
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                return response;
            }
            var errorMessage = string.Format("Image with id = {0} doesn't exists", id);
            return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent(errorMessage) };
        }

        public static void DeleteImage(int id, StudentHelperContext dbContext)
        {
            Image image = dbContext.Images.Find(id);
            if (image != null)
            {
                dbContext.Images.Remove(image);
                dbContext.SaveChanges();
            }         
        }

        public static string SaveImage(byte[] data, HttpRequestMessage req, StudentHelperContext dbContext)
        {
            var image = new Image { ImageData = data };
            dbContext.Images.Add(image);
            dbContext.SaveChanges();
            return string.Format("http://{0}:{1}/api/image/{2}", req.RequestUri.Host, req.RequestUri.Port, image.Id);
        }
    }
}