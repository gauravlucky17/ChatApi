using chatApi.Dapper;
using chatApi.Model.Common.AllModel;
using chatApi.Model.Common;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dapper.SqlMapper;

namespace chatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatApiController : ControllerBase
    {
        public readonly IDapper _dapper;

        public ChatApiController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpPost(nameof(SaveMessage))]
        public async Task<Message<int>> SaveMessage([FromBody] ChatMessage model)
        {
            Message<int> message = new Message<int>();
            var dbparams = new DynamicParameters();
            try
            {
                dbparams.Add("Mode", 1);
                dbparams.Add("SenderUser", model.SenderUser);
                dbparams.Add("ReceiverUser", model.ReceiverUser);
                dbparams.Add("Message", model.Message);
                message.Data = await Task.FromResult(_dapper.Post<int>("Usp_ChatMessages", dbparams));
            }
            catch (System.Exception ex)
            {
                throw;
            }
            if (message.Data == 1)
            {
                message.IsSuccess = true;
                message.ReturnMessage = "Save";
            }
            else
            {
                message.IsSuccess = true;
                message.ReturnMessage = "Save Successfully.";
            }
            return message;
        }
        [HttpGet(nameof(GetUserChat))]
        public async Task<Message<List<ChatMessage>>> GetUserChat(string SenderUser, string ReceiverUser)
        {
            int Mode = 2;
            Message<List<ChatMessage>> message = new Message<List<ChatMessage>>();
            var dbparams = new DynamicParameters();
            try
            {

                dbparams.Add("Mode", Mode);
                dbparams.Add("SenderUser", SenderUser);
                dbparams.Add("ReceiverUser", ReceiverUser);
                message.Data = await Task.FromResult(_dapper.GetAll<ChatMessage>("Usp_ChatMessages", dbparams));
            }
            catch (System.Exception ex)
            {
                throw;
            }
            if (message.Data.Count > 0)
            {
                message.IsSuccess = true;
                message.ReturnMessage = "Success";
            }
            else
            {
                message.IsSuccess = true;
                message.ReturnMessage = "No data.";
            }
            return message;
        }

        [HttpPost(nameof(userregistration))]
        public async Task<Message<int>> userregistration([FromBody] UserChatregistraion model)
        {
            Message<int> message = new Message<int>();
            var dbparams = new DynamicParameters();
            try
            {
                dbparams.Add("Mode", 3);
                dbparams.Add("user_name", model.user_name);
                dbparams.Add("email", model.email);
                dbparams.Add("phone", model.phone);
                //  dbparams.Add("date", model.ReceiverUser);
                dbparams.Add("password", model.password);

                message.Data = await Task.FromResult(_dapper.Post<int>("Usp_ChatMessages", dbparams));
            }
            catch (System.Exception ex)
            {
                throw;
            }
            if (message.Data == 1)
            {
                message.IsSuccess = true;
                message.ReturnMessage = "User already exists";
            }
            else
            {
                message.IsSuccess = true;
                message.ReturnMessage = "Registration Successfully.";
            }
            return message;
        }

        [HttpGet(nameof(GetChatLogin))]
        public async Task<Message<UserChatregistraion>> GetChatLogin(string email, string password)
        {
            //int Mode = 4;
            var message = new Message<UserChatregistraion>();
            var dbparams = new DynamicParameters();
            try
            {
                dbparams.Add("Mode", 4);
                dbparams.Add("email", email);
                dbparams.Add("password", password);
                message.Data = await Task.FromResult(_dapper.Get<UserChatregistraion>("Usp_ChatMessages", dbparams));
            }
            catch (System.Exception ex)
            {
                throw;
            }
            if (message.Data != null)
            {
                message.IsSuccess = true;
                message.ReturnMessage = "Login Successful";
            }
            else
            {
                message.IsSuccess = true;
                message.ReturnMessage = "Login Unsuccessful";
            }
            return message;



        }
        [HttpGet(nameof(GetChatUserData))]
        public async Task<Message<List<UserChatregistraion>>> GetChatUserData(int id)
        {
            Message<List<UserChatregistraion>> message = new Message<List<UserChatregistraion>>();
            var dbparams = new DynamicParameters();

            try
            {
                dbparams.Add("Mode", 5);
                dbparams.Add("id", id);
                message.Data = await Task.FromResult(_dapper.GetAll<UserChatregistraion>("Usp_ChatMessages", dbparams));
            }
            catch (System.Exception ex)
            {
                throw;
            }
            if (message.Data != null)
            {
                message.IsSuccess = true;
                message.ReturnMessage = "data found succesfull";

            }
            else
            {
                message.IsSuccess = true;
                message.ReturnMessage = "data not found succesfull";
            }
            return message;

        }
    }
}
