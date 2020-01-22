using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using StudentHelper.Data;
using StudentHelper.Models;
using StudentHelper.Models.DTOs;
using StudentHelper.Models.Pagination;

namespace StudentHelper.Controllers
{
    public class StaffsController : ApiController
    {
        private StudentHelperContext db = new StudentHelperContext();

        // GET: api/Staffs
        public IHttpActionResult GetStaff([FromUri] StaffFilter staffFilter)
        {
            IQueryable<Staff> queryable = db.Staffs;
            if (!string.IsNullOrEmpty(staffFilter.SearchTerm))
            {
                string lowerCased = staffFilter.SearchTerm.ToLower();
                queryable = queryable.Where(s => 
                    s.FirstName.ToLower().Contains(lowerCased) || 
                    s.LastName.ToLower().Contains(lowerCased)
                );
            }
            var staffPage = Pagination.CreatePage<Staff>(
                queryable, staffFilter.Page, staffFilter.PageSize, staffFilter.OrderBy, staffFilter.Ascending, Request
            );
            return Ok(staffPage);
        }

        // GET: api/Staffs/5
        [ResponseType(typeof(Staff))]
        public IHttpActionResult GetStaffDetails(int id)
        {
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Staff member with ID = {0} doesn't exists", id))
                };
                throw new HttpResponseException(resp);
            }

            return Ok(staff);
        }

        [Route("api/staff/all")]
        public IHttpActionResult GetAllStaffMembers()
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Staff, StaffListItem>()
                .ForMember(dest => dest.Name, c => c.MapFrom(source => source.FirstName + " " + source.LastName))
            );
            var allStaff = db.Staffs
                .OrderBy(x => x.FirstName + " " + x.LastName)
                .ProjectTo<StaffListItem>(configuration).ToList();
            return Ok(allStaff);
        }

        // PUT: api/Staffs/5
        [ResponseType(typeof(Staff))]
        public IHttpActionResult PutStaff(int id, [FromBody] Staff staffRequest)
        {
            if (id != staffRequest.Id || !StaffExists(id))
            {
                return BadRequest();
            }

            Staff staff = db.Staffs.Find(id);
            staff.FirstName = staffRequest.FirstName;
            staff.LastName = staffRequest.LastName;
            staff.Title = staffRequest.Title;
            staff.DetailsUrl = staffRequest.DetailsUrl;

            db.SaveChanges();

            return Ok(staff);
        }

        // PATCH: api/Staff/ChangeImage/5
        [Route("api/staff/changeImage/{id}")]
        public async Task<IHttpActionResult> PatchCourse(int id)
        {
            if (!StaffExists(id))
            {
                return BadRequest();
            }
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            Staff staff = db.Staffs.Find(id);
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            var imageBytes = await filesReadToProvider.Contents[0].ReadAsByteArrayAsync();

            int? oldImageId = Image.ExtractImageId(staff.ImageUrl);

            if (oldImageId != null)
            {
                ImageController.DeleteImage(oldImageId.Value, db);
            }
            staff.ImageUrl = ImageController.SaveImage(imageBytes, Request, db);

            db.SaveChanges();

            return Ok(staff);
        }

        // POST: api/Staffs
        public async Task<IHttpActionResult> PostStaff()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var jsonStaff = await filesReadToProvider.Contents[0].ReadAsStringAsync();
            Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);
            var imageBytes = await filesReadToProvider.Contents[1].ReadAsByteArrayAsync();

            staff.ImageUrl = ImageController.SaveImage(imageBytes, Request, db);
            db.Staffs.Add(staff);
            db.SaveChanges();

            return Created(Request.RequestUri.ToString() + "/" + staff.Id, staff);
        }

        // DELETE: api/Staffs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteStaff(int id)
        {
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            db.Staffs.Remove(staff);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StaffExists(int id)
        {
            return db.Staffs.Count(e => e.Id == id) > 0;
        }
    }
}