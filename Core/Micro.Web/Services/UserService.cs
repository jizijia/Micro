using Micro.Core;
using Micro.Core.Data;
using Micro.Web.Infrastructure.Domain;
using Micro.Web.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.Web.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IClientContext _clientContext;
        public UserService(IRepository<User> userRepository,
            IClientContext clientContext)
        {
            _userRepository = userRepository;
            _clientContext = clientContext;
        }

        public async Task<LoginOutputModel> LoginAsync(LoginInputModel loginInputModel)
        {
            Guard.ArgumentIsNotNull(loginInputModel, nameof(loginInputModel));
            Guard.ArgumentNotNullOrEmpty(loginInputModel.UserName, nameof(loginInputModel.UserName));
            Guard.ArgumentNotNullOrWhiteSpace(loginInputModel.Password, nameof(loginInputModel.Password));

            var user = await _userRepository.GetAsync(x => x.Name == loginInputModel.UserName);
            if (!user.IsActivie)
                throw new CustomException("User is not activie.");
            if (user.Password != loginInputModel.Password)
                throw new CustomException("Username or password is error.");
            string token = _clientContext.CreateToken(new
            {
                Id = user.Id,
                UserName = user.Name
            });
            return new LoginOutputModel()
            {
                ReturnUrl = string.Empty,
                Token = token
            };
        }


    }
}
