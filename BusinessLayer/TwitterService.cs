using System;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;
using Model.Interfaces;
using TestDataGenerator;

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

        public List<MessageModel> GetMessages(int take, int skip)
        {
            return _dataService.GetMessages(take, skip);
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

    public class MockTwitterService : IBusinessService
    {
        public int AddMessage(MessageModel model)
        {
            var catalog = new Catalog();
            var instance = catalog.CreateInstance<int>();
            return instance;
        }

        public List<MessageModel> GetComments(int messageId)
        {
            var catalog = new Catalog();
            var instance = catalog.CreateInstance<List<MessageModel>>();
            return instance;
        }

        public List<MessageModel> GetMessages(int take, int skip)
        {
            var catalog = new Catalog();
            var result = new List<MessageModel>();
            for (var i = 0; i < take; i++)
            {
                var instance = catalog.CreateInstance<MessageModel>();
                result.Add(instance);
            }

            return result;
        }
    }
}
