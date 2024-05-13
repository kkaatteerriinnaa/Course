using Course.Data.DAL;
using Course.Data.Entities;
using Course.Data.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataAccessor _dataAccessor;
        public AuthController(DataAccessor dataAcessor)
        {
            _dataAccessor = dataAcessor;
        }

        [HttpGet]
        public object Get([FromQuery(Name = "e-mail")] String email, String? password)
        {
            var user = _dataAccessor.UserDao.Authorize(email, password ?? "");
            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new { Status = "Authfailed" };
            }
            else
            {
                HttpContext.Session.SetString("auth-user-id", user.Id.ToString());
                return user;
            }
        }
        [HttpPost]
        public object Post()
        {
            return new { Status = "Post works" };
        }

        [HttpPost]
        public object Put()
        {
            return new { Status = "Put works" };
        }

        [HttpPatch]
        public object Patch(String email, String code)
        {
            if (_dataAccessor.UserDao.ConfirmEmail(email, code))
            {
                Response.StatusCode = StatusCodes.Status202Accepted;
                return new { Status = "OK" };
            }
            else
            {
                Response.StatusCode = StatusCodes.Status409Conflict;
                return new { Status = "Error" };
            }
        }

        [HttpGet("token")]
        public Token? GetToken(String email, String? password)
        {
            var user = _dataAccessor.UserDao.Authorize(email, password ?? "");
            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }
            return _dataAccessor.UserDao.CreateTokenForUser(user);
        }
    }
}
