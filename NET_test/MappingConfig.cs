using AutoMapper;
using NET_test.Models;
using NET_test.Models.Dto;

namespace NET_test
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
   
    }
}
