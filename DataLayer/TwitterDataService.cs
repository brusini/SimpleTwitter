using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;

namespace DataLayer
{
    public class TwitterDataService : IDataService
    {
        public List<MessageModel> GetMessages(int take, int skip)
        {
            using (var db = new DBModelDataContext())
            {
                var msgs = db.Messages
                        .OrderByDescending(r => r.DatePosted)
                        .Take(take)
                        .Skip(skip)
                        .Select(r => new MessageModel
                        {
                            UserName = r.UserName,
                            DatePosted = r.DatePosted,
                            Id = r.Id,
                            MessageId = r.MessageId,
                            TextMessage = r.TextMessage
                        }).ToList();

                return msgs;
            }
        }

        public List<MessageModel> GetComments(int messageId)
        {
            using (var db = new DBModelDataContext())
            {
                var replies = db.Messages.Where(r => r.MessageId.HasValue && r.MessageId.Value == messageId)
                    .Select(r => new MessageModel
                    {
                        UserName = r.UserName,
                        DatePosted = r.DatePosted,
                        Id = r.Id,
                        MessageId = r.MessageId,
                        TextMessage = r.TextMessage
                    }).ToList();

                return replies;
            }
        }

        public int AddMessage(MessageModel model)
        {
            using (var db = new DBModelDataContext())
            {
                var dbmsg = new Message
                {
                    MessageId = model.MessageId,
                    UserName = model.UserName,
                    TextMessage = model.TextMessage,
                    DatePosted = DateTime.Now
                };

                db.Messages.InsertOnSubmit(dbmsg);
                db.SubmitChanges();

                return dbmsg.Id;
            }
        }
    }
}
