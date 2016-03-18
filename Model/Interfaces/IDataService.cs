using System;
using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IDataService
    {
        List<MessageModel> GetMessages(int take, int skip);
        List<MessageModel> GetComments(int messageId);
        int AddMessage(MessageModel model);
    }
}
