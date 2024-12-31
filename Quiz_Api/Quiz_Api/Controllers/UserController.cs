using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            return Ok(new UsersData.UserDTO(user.UserID, user.UserName, user.Email, user.Password, user.Role));
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

            Business_Layer.User newUser = new Business_Layer.User(new UsersData.UserDTO(UserDTO.UserID, UserDTO.UserName,UserDTO.Email, UserDTO.Password, UserDTO.Role));
            newUser.Save();

            UserDTO.UserID = newUser.UserID;

            return CreatedAtAction(nameof(AddNewUser), new { id = UserDTO.UserID }, UserDTO);
        }
        [HttpGet]
        public bool IsUserExist(UserDTO UserDTO)
        {
            return Business_Layer.User.IsUserExist(UserDTO);
        }

        [HttpDelete("DeleteUser")]
        public ActionResult Delete(UserDTO user)
        {
            if (user.UserID < 1)
            {
                return BadRequest($"Not accepted ID {user.UserID}");
            }
            if (Business_Layer.User.DeleteUser(user))

                return Ok($"Task with ID {user.UserID} has been deleted.");
            else
                return NotFound($"Task with ID {user.UserID} not found. no rows deleted!");
        }

        [HttpPut("UpdateUser")]
        public ActionResult<UserDTO> Update(UserDTO userDTO)
        {
            Business_Layer.User user = Business_Layer.User.FindUserbyUserNameAndPassword(userDTO.UserName,userDTO.Password);
            if (user == null)
            {
                return NotFound($"User {userDTO.UserName} not found.");
            }

            if (!user.Save())
            {
                return BadRequest("Failed to update the user.");
            }

            return Ok(userDTO);
        }


    }
}
