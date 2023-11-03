﻿using FinanaceManagementLifesaver.DTO;
using FinanceManagementLifesaver.Interfaces;
using FinanceManagementLifesaver.Models;
using FinanceManagementLifesaver.ServiceResponse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagementLifesaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("{email}/{password}")]
        public IActionResult Get(UserLoginDTO userLoginDTO)
        {
            var user = _userService.GetUserByEmailAndPassword(userLoginDTO);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> Post(UserSaveDTO userSaveDTO)
        {
            ServiceResponse<User> response = await _userService.CreateUser(userSaveDTO);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Created(Request.HttpContext.ToString() , response);
        }
    }
}