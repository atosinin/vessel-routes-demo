using AutoMapper;
using DotNetApi.Helpers;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Localization;
using DotNetApi.Models;
using DotNetApi.Repositories;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace DotNetApi.Services
{
    public interface IWhateverService
    {
        List<WhateverDTO> GetAllWhateverDTOs();
        List<WhateverDTO> GetAllWhateverDTOsByUserAccountId(string userId);
        WhateverDTO GetWhateverDTOById(int whateverId);
        void CreateFromWhateverDTO(WhateverDTO whateverCreateDTO);
        void UpdateFromWhateverDTO(WhateverDTO whateverUpdateDTO);
        void DeleteWhateverById(int whateverId);
    }

    public class WhateverService : IWhateverService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _user;

        public WhateverService(
            IUnitOfWork uow,
            IMapper mapper,
            ClaimsPrincipal user)
        {
            _uow = uow;
            _mapper = mapper;
            _user = user;
        }

        public List<WhateverDTO> GetAllWhateverDTOs()
        {
            List<Whatever> allWhatevers = _uow.Whatevers.GetAll();
            return _mapper.Map<List<Whatever>, List<WhateverDTO>>(allWhatevers);
        }

        public List<WhateverDTO> GetAllWhateverDTOsByUserAccountId(string userId)
        {
            if (!_user.HasAccessToUserAccountById(userId))
                throw new WhateverBadRequestException("Unauthorized user");
            List<Whatever> allWhatevers = _uow.Whatevers.GetAllByUserAccountId(userId);
            return _mapper.Map<List<Whatever>, List<WhateverDTO>>(allWhatevers);
        }

        public WhateverDTO GetWhateverDTOById(int whateverId)
        {
            Whatever? whatever = _uow.Whatevers.GetById(whateverId);
            if (whatever is null)
                throw new WhateverBadRequestException("Invalid Id for table 'Whatever'");
            if (!_user.HasAccessToWhatever(whatever))
                throw new WhateverBadRequestException("Unauthorized user");
            return _mapper.Map<WhateverDTO>(whatever);
        }

        public void CreateFromWhateverDTO(WhateverDTO whateverCreateDTO)
        {
            Whatever whateverToCreate = new(_user.Identity!.Name!)
            {
                UserAccountId = _user.GetUserAccountId()
            };
            whateverToCreate.CreateFromWhateverDTO(whateverCreateDTO);
            _uow.Whatevers.Create(whateverToCreate);
            _uow.SaveChanges();
        }

        public void UpdateFromWhateverDTO(WhateverDTO whateverUpdateDTO)
        {
            Whatever? whatever = _uow.Whatevers.GetById(whateverUpdateDTO.WhateverId);
            if (whatever is null)
                throw new WhateverBadRequestException("Invalid Id for table 'Whatever'");
            if (!_user.HasAccessToWhatever(whatever))
                throw new WhateverBadRequestException("Unauthorized user");
            whatever.UpdateFromWhateverDTO(whateverUpdateDTO, _user.Identity!.Name!);
            _uow.SaveChanges();
        }

        public void DeleteWhateverById(int whateverId)
        {
            Whatever? whatever = _uow.Whatevers.GetById(whateverId);
            if (whatever is null)
                throw new WhateverBadRequestException("Invalid Id for table 'Whatever'");
            if (!_user.HasAccessToWhatever(whatever))
                throw new WhateverBadRequestException("Unauthorized user");
            _uow.Whatevers.Delete(whatever);
            _uow.SaveChanges();
        }
    }
}
