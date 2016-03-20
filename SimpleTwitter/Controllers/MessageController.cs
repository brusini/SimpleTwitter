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
        public IHttpActionResult GetMessages(int take, int skip)
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

        [Route("messages/add"), HttpPut]
        public IHttpActionResult AddMessage(MesageSubmitModel model)
        {
            var id = _bllService.AddMessage(new MessageModel { TextMessage = model.Message.Trim(), UserName = model.UserName.Trim() });
            return Ok(id);
        }

        [Route("comments/add"), HttpPut]
        public IHttpActionResult Comment(MesageSubmitModel model)
        {
            var id = _bllService.AddMessage(new MessageModel { MessageId = model.MessageId.Value, TextMessage = model.Message.Trim(), UserName = model.UserName.Trim() });
            return Ok(id);
        }
    }
}
