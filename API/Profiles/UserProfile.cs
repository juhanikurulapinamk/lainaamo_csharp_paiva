
using API.Dtos;
using API.Models;
using AutoMapper;

namespace API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // tässä määritellään, mistä-> mihin mäppäys tehdään
            // eli pystymme nyt luomaan automapperin avulla automaattisesti
            // AppUser-tyyppisistä instansseista UserDto-tyyppisiä instansseja
            CreateMap<AppUser, UserDto>();
        }
    }
}