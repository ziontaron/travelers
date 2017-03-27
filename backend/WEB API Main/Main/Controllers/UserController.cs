using Reusable;
using System;
using System.Web.Http;

namespace ReusableWebAPI.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseController<User>
    {
        public UserController(IUserLogic logic) : base(logic) { }

        [HttpGet Route("getByRole/{role}")]
        public CommonResponse getByRole(string role)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                response.Success(_logic.GetListWhere(u => u.Value, u => u.Role == role));
            }
            catch (Exception ex)
            {
                return response.Error(ex.ToString());
            }
            return response;
        }
    }
}