using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using BusinessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using SimpleTwitter.Controllers;
using SimpleTwitter.Models;

namespace SimpleTwitter.Tests.Controllers
{
    [TestClass]
    public class MessageControllerTest
    {
        [TestMethod]
        public void GetMessagesBadParamTakeTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var result = controller.GetMessages(-2, 0);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));

            var item = ((InvalidModelStateResult)result).ModelState;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsNotNull(item["take"]);
            Assert.IsTrue(item["take"].Errors.Count  > 0);
        }

        [TestMethod]
        public void GetMessagesBadParamSkipTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var result = controller.GetMessages(2, -1);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));

            var item = ((InvalidModelStateResult)result).ModelState;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsNotNull(item["skip"]);
            Assert.IsTrue(item["skip"].Errors.Count > 0);
        }

        [TestMethod]
        public void GetCommentsBadParamTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var result = controller.GetComments(-1);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));

            var item = ((InvalidModelStateResult)result).ModelState;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsNotNull(item["MessageId"]);
            Assert.IsTrue(item["MessageId"].Errors.Count > 0);
        }

        [TestMethod]
        public void GetMessagesMassageLengthTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var m = new MesageSubmitModel
            {
                //257 symbols, allowed 256
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has surv",
                UserName = "test"
            };

            controller.Configuration = new HttpConfiguration();
            controller.Validate(m);
            var result = controller.AddMessage(m);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));

            var item = ((InvalidModelStateResult)result).ModelState;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsNotNull(item["Message"]);
            Assert.IsTrue(item["Message"].Errors.Count > 0);
        }

        [TestMethod]
        public void GetMessagesUserNameLengthTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var m = new MesageSubmitModel
            {
                Message = "test",
                UserName = "testtesttesttesttesttesttesttesttesttesttesttesttest" //52 symbols, alowed 50
            };

            controller.Configuration = new HttpConfiguration();
            controller.Validate(m);
            var result = controller.AddMessage(m);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));

            var item = ((InvalidModelStateResult)result).ModelState;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsNotNull(item["UserName"]);
            Assert.IsTrue(item["UserName"].Errors.Count > 0);
        }

        [TestMethod]
        public void GetMessagesOkTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var result = controller.GetMessages(2, 0);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<MessageModel>>));

            var item = ((OkNegotiatedContentResult<List<MessageModel>>)result).Content;

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(2, item.Count());
        }

        [TestMethod]
        public void GetCommentsOkTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var result = controller.GetComments(1);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<MessageModel>>));

            var item = ((OkNegotiatedContentResult<List<MessageModel>>)result).Content;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsTrue(item.Count() > 0);
        }

        [TestMethod]
        public void AddMessageOkTest()
        {
            var mockService = new MockTwitterService();
            var controller = new MessageController(mockService);

            var m = new MesageSubmitModel
            {
                Message = "test",
                UserName = "test"
            };

            var result = controller.AddMessage(m);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<int>));

            var item = ((OkNegotiatedContentResult<int>)result).Content;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsTrue(item > 0);
        }

        
    }
}
