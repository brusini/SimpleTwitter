using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using SimpleTwitter.Models;

namespace SimpleTwitter.Controllers
{
    [RoutePrefix("api")]
    public class MessageController : ApiController
    {
        private readonly IBusinessService _bllService;

        public MessageController(IBusinessService service)
        {
            _bllService = service;
        }

        public MessageController()
        {
            _bllService = new TwitterService();
        }

        [Route("messages"), HttpGet]
        [ResponseType(typeof(List<MessageModel>))]
        public IHttpActionResult Get(int take, int skip)
        {
            try
            {
                var serv = new TwitterService();
                var msgs = serv.GetMessages(take, skip);

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
            var msgs = _bllService.GetComments(messageId);

            return Ok(msgs);
        }

        [Route("messages/add"), HttpPost]
        public IHttpActionResult Post(MesageSubmitModel model)
        {
            var id = _bllService.AddMessage(new MessageModel { TextMessage = model.Message.Trim(), UserName = model.UserName.Trim() });
            return Ok(id);
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
