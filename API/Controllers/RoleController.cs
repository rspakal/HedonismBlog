using API.APIModels;
using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ServicesLibrary;
using ServicesLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "administrator")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;                  

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var _roleModels = await _roleService.GetAllAsync();
                return Ok(_roleModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpPost("role/create")]
        public async Task<IActionResult> Create(RoleModel roleModel)
        {
            try
            {
                await _roleService.CreateAsync(roleModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpPut("role/edit")]
        public async Task<IActionResult> Edit(RoleModel roleModel)
        {
            try
            {
                await _roleService.Update(roleModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");

            }
        }

        [HttpDelete("role/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _roleService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }
    }
}
