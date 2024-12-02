using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        public RoleService(IMapper mapper, IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
        public async Task CreateAsync(RoleModel roleModel)
        {
            var _role = _mapper.Map<Role>(roleModel);
            await _roleRepository.Create(_role);
        }

        public async Task DeleteAsync(int roleId)
        {
            await _roleRepository.Delete(roleId);
        }

        public async Task<List<RoleModel>> GetAllAsync()
        {
            var _roles = await _roleRepository.GetAll();
            var _roleModels = _mapper.Map<List<RoleModel>>(_roles);
            return _roleModels;
        }

        public async Task<RoleModel> GetAsync(int roleId)
        {
            var _role = await _roleRepository.Get(roleId);
            var _roleModel = _mapper.Map<RoleModel>(_role);
            return _roleModel;
        }

        public async Task Update(RoleModel roleModel)
        {
            var _role = _mapper.Map<Role>(roleModel);
            await _roleRepository.Update(_role);
        }

    }
}
