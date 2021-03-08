using Layer.Architecture.Application.Models;
using Layer.Architecture.Domain.Entities;
using Layer.Architecture.Domain.Interfaces;
using Layer.Architecture.Service.Validators;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Layer.Architecture.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IBaseService<User> _baseUserService;

        public UserController(IBaseService<User> baseUserService)
        {
            _baseUserService = baseUserService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserModel user)
        {
            if (user == null)
                return NotFound();

            return Execute(() => _baseUserService.Add<CreateUserModel, UserModel, UserValidator>(user));
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateUserModel user)
        {
            if (user == null)
                return NotFound();

            return Execute(() => _baseUserService.Update<UpdateUserModel, UserModel, UserValidator>(user));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();

            Execute(() =>
            {
                _baseUserService.Delete(id);
                return true;
            });

            return new NoContentResult();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Execute(() => _baseUserService.Get<UserModel>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 0)
                return NotFound();

            return Execute(() => _baseUserService.GetById<UserModel>(id));
        }

        private IActionResult Execute(Func<object> func)
        {
            try
            {
                var result = func();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
