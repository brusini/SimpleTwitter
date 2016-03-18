using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IBusinessService
    {
        List<MessageModel> GetMessages(int take, int skip);
        List<MessageModel> GetComments(int messageId);
        int AddMessage(MessageModel model);
    }
}
