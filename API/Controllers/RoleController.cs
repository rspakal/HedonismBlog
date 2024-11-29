using API.APIModels;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> Roles()
        {
            try
            {
                var _roles = await _roleRepository.GetAll();
                var _roleAPIModels = _mapper.Map<List<RoleAPIModel>>(_roles);
                return Ok(_roleAPIModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpPost("role/create")]
        public async Task<IActionResult> Create(RoleAPIModel roleAPIModel)
        {
            var _role = _mapper.Map<Role>(roleAPIModel);
            try
            {
                await _roleRepository.Create(_role);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpGet("role/view")]
        public async Task<IActionResult> View(int id)
        {
            var _role = await _roleRepository.GetById(id);
            if (_role == null)
            {
                return StatusCode(500, $"Server internal error: No role found with given id := {id}");
            }
            var _roleAPIModel = _mapper.Map<RoleAPIModel>(_role);
            return Ok(_roleAPIModel);
        }

        [HttpPut("role/update")]
        public async Task<IActionResult> Update(RoleAPIModel roleAPIModel)
        {
            var _role = _mapper.Map<Role>(roleAPIModel);
            try
            {
                await _roleRepository.Update(_role);
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
                await _roleRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }
    }
}
