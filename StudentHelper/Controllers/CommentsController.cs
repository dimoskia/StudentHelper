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

            int userId = JwtAuthManager.getUserIdFromRequest(Request);
            comment.UserDetails = db.Users.Find(userId).UserDetails;
            post.Comments.Add(comment);
            db.SaveChanges();

            return Ok(comment);
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