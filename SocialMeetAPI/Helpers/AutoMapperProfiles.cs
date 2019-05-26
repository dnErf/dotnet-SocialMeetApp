
using System.Linq;
using AutoMapper;
using SocialMeetAPI.Dtos;
using SocialMeetAPI.Models;

namespace SocialMeetAPI.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<User, UserForListDto>()
        .ForMember(dest => dest.PhotoUrl, opt => {
          opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
        })
        .ForMember(dest => dest.Age, opt => {
          opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
        });
      CreateMap<User, UserForDetailedDto>()
        .ForMember(dest => dest.PhotoUrl, opt => {
          opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
        })
        .ForMember(dest => dest.Age, opt => {
          opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()); // automapper version not supported
        });
      CreateMap<Photo, PhotosForDetailedDto>();
      CreateMap<UserForUpdateDto, User>();
      CreateMap<Photo, PhotoForReturnDto>();
      CreateMap<PhotoForCreationDto, Photo>();
    }
  }
}