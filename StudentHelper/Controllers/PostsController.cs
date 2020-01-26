using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JwtExample.Auth;
using StudentHelper.Data;
using StudentHelper.Models;
using StudentHelper.Models.Pagination;

namespace StudentHelper.Controllers
{
    [JwtAuthentication]
    public class PostsController : ApiController
    {
        private StudentHelperContext db = new StudentHelperContext();

        [Route("api/Courses/{courseId}/Posts")]
        public IHttpActionResult GetPostFromCourse(int courseId, int page = 1, int pageSize = 10)
        {
            if (db.Courses.Count(c => c.Id == courseId) == 0)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Cannot retrieve posts. Course with ID = {0} doesn't exist", courseId)
                    )
                };
                throw new HttpResponseException(resp);
            }
            var queryable = db.Posts.Where(p => p.CourseId == courseId);
            var postsPage = Pagination.CreatePage<Post>(
                queryable, page, pageSize, "CreatedAt", false, Request
            );
            return Ok(postsPage);
        }

        [ResponseType(typeof(Post))]
        [Route("api/Courses/{courseId}/Posts/Add")]
        public IHttpActionResult PostNewPost(int courseId, Post post)
        {
            Course course = db.Courses.Find(courseId);

            if (course == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Cannot add post. Course with ID = {0} doesn't exist", courseId)
                    )
                };
                throw new HttpResponseException(resp);
            }

            post.CreatedAt = DateTime.Now;
            int userId = JwtAuthManager.GetUserIdFromRequest(Request);
            post.UserDetailsId = userId;

            course.Posts.Add(post);

            InitPopularityIfAbsent(userId, course);

            db.SaveChanges();

            return Ok(post);
        }

        private void InitPopularityIfAbsent(int userId, Course course)
        {
            if(course.PopularityStats.Count(p => p.UserDetailsId == userId) == 0)
            {
                Popularity newPopularityStat = new Popularity { UserDetailsId = userId, CourseId = course.Id, Votes = 0 };
                course.PopularityStats.Add(newPopularityStat);
            }
        }

        [Route("api/Posts/{postId}/Like")]
        public IHttpActionResult PostPostLike(int postId)
        {
            // TODO: update popularity accordingly
            Post post = db.Posts.Find(postId);
            if(post == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Cannot like post. Post with ID = {0} doesn't exist", postId)
                    )
                };
                throw new HttpResponseException(resp);
            }
            
            post.Likes++;
            Popularity.PostLiked(post.CourseId, post.UserDetailsId, db);

            db.SaveChanges();

            return Ok();
        }

        [Route("api/Posts/{postId}/Dislike")]
        public IHttpActionResult PostPostDislike(int postId)
        {
            Post post = db.Posts.Find(postId);
            if (post == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Cannot dislike post. Post with ID = {0} doesn't exist", postId)
                    )
                };
                throw new HttpResponseException(resp);
            }
            
            post.Dislikes++;
            Popularity.PostDisliked(post.CourseId, post.UserDetailsId, db);
            
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
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

    }
}