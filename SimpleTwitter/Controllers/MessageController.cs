using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using BusinessLayer;
using Model;

namespace SimpleTwitter.Controllers
{
    [RoutePrefix("api")]
    public class MessageController : ApiController
    {
        [Route("messages"), HttpGet]
        [ResponseType(typeof(List<MessageModel>))]
        public IHttpActionResult Get(string userName, int take, int skip)
        {
            try
            {
                var serv = new TwitterService();
                var msgs = serv.GetMessages(userName, take, skip);

                return Ok(msgs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("comments"), HttpGet]
        [ResponseType(typeof(List<MessageModel>))]
        public IHttpActionResult GetComments(int messageId)
        {
            var serv = new TwitterService();
            var msgs = serv.GetComments(messageId);

            return Ok(msgs);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
