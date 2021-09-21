using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Dto;
using webapi.Entities;
using webapi.Repository;

namespace Controllers.Controllers
{
    [Route("/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet()]
        public ActionResult<List<GetUserResponseDto>> GetUsers()
        {
            return StatusCode(StatusCodes.Status200OK, userRepository.GetUsers().Select(user => new GetUserResponseDto()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                CreateDate = user.CreateDate,
                UpdateDate = user.UpdateDate
            }));
        }

        [HttpGet("{id}")]
        public ActionResult<GetUserResponseDto> GetUserById(Guid id)
        {
            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found.");
            }
            return StatusCode(StatusCodes.Status200OK, new GetUserResponseDto()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                CreateDate = user.CreateDate,
                UpdateDate = user.UpdateDate
            });
        }

        [HttpPost()]
        public ActionResult<AddUserResponseDto> AddUser([FromBody] AddUserRequestDto data)
        {
            var isHaveUsername = userRepository.GetUserByUsername(data.Username);
            if (isHaveUsername != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Username is repeat.");
            }
            var model = new User()
            {
                Id = Guid.NewGuid(),
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Username = data.Username,
                Password = data.Password,
                CreateDate = new DateTime()
            };
            var user = userRepository.AddUser(model);
            return StatusCode(StatusCodes.Status201Created, new AddUserResponseDto()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                CreateDate = user.CreateDate
            });
        }

        [HttpPut()]
        public ActionResult<UpdateUserResponseDto> UpdateUser([FromBody] UpdateUserRequestDto data)
        {
            var isHaveUsername = userRepository.GetUserByUsername(data.Username);
            if (isHaveUsername != null && isHaveUsername.Id != data.Id)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Username is repeat.");
            }
            var model = new User()
            {
                Id = data.Id,
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Username = data.Username,
                Password = data.Password,
                UpdateDate = new DateTime()
            };
            var user = userRepository.UpdateUser(model);
            return StatusCode(StatusCodes.Status200OK, new UpdateUserResponseDto()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                CreateDate = user.CreateDate,
                UpdateDate = user.UpdateDate
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<DeleteUserResponseDto> DeleteUserById(Guid id)
        {
            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found.");
            }
            userRepository.DeleteUser(user);
            return new DeleteUserResponseDto()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                CreateDate = user.CreateDate,
                UpdateDate = user.UpdateDate,
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}
