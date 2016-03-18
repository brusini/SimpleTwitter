using System;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;
using Model.Interfaces;

namespace BusinessLayer
{
    public class TwitterService : IBusinessService
    {
        private readonly IDataService _dataService;
        public TwitterService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public TwitterService()
        {
            _dataService = new TwitterDataService();
        }

        public List<MessageModel> GetMessages(string userName, int take, int skip)
        {
            return _dataService.GetMessages(userName, take, skip);
        }

        public List<MessageModel> GetComments(int messageId)
        {
            return _dataService.GetComments(messageId);
        }

        public int AddMessage(MessageModel model)
        {
            return _dataService.AddMessage(model);
        }
    }
}
