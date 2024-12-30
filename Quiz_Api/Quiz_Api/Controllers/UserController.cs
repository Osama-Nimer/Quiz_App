using Data_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Data_Layer.UsersData;

namespace Quiz_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("GetUser")]
        public ActionResult<UsersData.UserDTO> GetUser(UsersData.UserDTO UserDTO)
        {
            Business_Layer.User user = Business_Layer.User.FindUserbyUserNameAndPassword(UserDTO.UserName, UserDTO.Password);
            if (user == null)
            {
                return NotFound("No User Found!");
            }
            return Ok(new UsersData.UserDTO(user.UserID, user.UserName, user.Email, user.Password));
        }
        [HttpGet("GetAllUsers")]
        public ActionResult<UsersData.UserDTO> GetAllUsers()
        {
            List<UsersData.UserDTO> users = Business_Layer.User.GetAllUsers();
            if (users.Count == 0)
            {
                return NotFound("No Users Found!");
            }
            return Ok(users);
        }


        [HttpPost("AddNewUser")]
        public ActionResult<UsersData.UserDTO> AddNewUser(UsersData.UserDTO UserDTO)
        {
            if (UserDTO == null || string.IsNullOrEmpty(UserDTO.UserName) || string.IsNullOrEmpty(UserDTO.Password))
            {
                return BadRequest("Invalid task data or user does not exist.");
            }

            Business_Layer.User newUser = new Business_Layer.User(new UsersData.UserDTO(UserDTO.UserID, UserDTO.UserName,UserDTO.Email, UserDTO.Password));
            newUser.Save();

            UserDTO.UserID = newUser.UserID;

            return CreatedAtAction(nameof(AddNewUser), new { id = UserDTO.UserID }, UserDTO);
        }
        [HttpGet]
        public bool IsUserExist(UserDTO UserDTO)
        {
            return Business_Layer.User.IsUserExist(UserDTO);
        }

    }
}
