using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JwtExample.Auth;
using StudentHelper.Data;
using StudentHelper.Models;

namespace StudentHelper.Controllers
{
    [JwtAuthentication]
    public class CommentsController : ApiController
    {
        private StudentHelperContext db = new StudentHelperContext();

        // GET: api/comments/5
        [Route("api/comments/{postId}")]
        public IHttpActionResult GetCommentsForPost(int postId)
        {

            Post post = db.Posts.Find(postId);

            if (post == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Post with ID = {0} doesn't exist", postId)
                    )
                };
                throw new HttpResponseException(resp);
            }

            return Ok(post.Comments);
        }


        [Route("api/Posts/{postId}/Comments")]
        public IHttpActionResult PostComment(int postId, Comment comment)
        {

            Post post = db.Posts.Find(postId);

            if (post == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Post with ID = {0} doesn't exist", postId)
                    )
                };
                throw new HttpResponseException(resp);
            }

            int userId = JwtAuthManager.GetUserIdFromRequest(Request);
            comment.UserDetails = db.Users.Find(userId).UserDetails;
            post.Comments.Add(comment);

            InitPopularityIfAbsent(userId, post.Course);

            db.SaveChanges();

            return Ok(comment);
        }

        private void InitPopularityIfAbsent(int userId, Course course)
        {
            if (course.PopularityStats.Count(p => p.UserDetailsId == userId) == 0)
            {
                Popularity newPopularityStat = new Popularity { UserDetailsId = userId, CourseId = course.Id, Votes = 0 };
                course.PopularityStats.Add(newPopularityStat);
            }
        }

        [Route("api/Comments/{id}/Like")]
        public IHttpActionResult PostCommentLike(int id)
        {

            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Comment with ID = {0} doesn't exist", id)
                    )
                };
                throw new HttpResponseException(resp);
            }
            comment.Likes++;

            Popularity.CommentLiked(comment.Post.CourseId, comment.UserDetails.UserDetailsId, db);

            db.SaveChanges();

            return Ok(comment);
        }

        [Route("api/Comments/{id}/Dislike")]
        public IHttpActionResult PostCommentDislike(int id)
        {

            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        string.Format("Comment with ID = {0} doesn't exist", id)
                    )
                };
                throw new HttpResponseException(resp);
            }
            comment.Dislikes++;

            Popularity.CommentDisliked(comment.Post.CourseId, comment.UserDetails.UserDetailsId, db);

            db.SaveChanges();

            return Ok(comment);
        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
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