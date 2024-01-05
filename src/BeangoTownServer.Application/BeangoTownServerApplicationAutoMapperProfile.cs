using AutoMapper;
using BeangoTownServer.NFT;
using BeangoTownServer.Rank;
using BeangoTownServer.Trace;

namespace BeangoTownServer
{
    public class BeangoTownServerApplicationAutoMapperProfile : Profile
    {
        public BeangoTownServerApplicationAutoMapperProfile()
        {
            CreateMap<RankDto, UserSeasonRankIndex>().ForMember(dest => dest.SumScore,
                opts => opts.MapFrom(src => src.Score)).ReverseMap();
            CreateMap<RankDto, UserWeekRankIndex>().ForMember(dest => dest.SumScore,
                opts => opts.MapFrom(src => src.Score)).ReverseMap();
            CreateMap<UserWeekRankIndex, WeekRankDto>().ForMember(destination => destination.Score,
                opt => opt.MapFrom(source => source.SumScore));
            CreateMap<RankSeasonConfigIndex, SeasonDto>().ReverseMap();
            CreateMap<RankWeekIndex, WeekDto>().ReverseMap();
            CreateMap<WeekRankDto, UserWeekRankIndex>().ForMember(dest => dest.SumScore,
                opts => opts.MapFrom(src => src.Score)).ReverseMap();
            CreateMap<GetUserActionDto, UserActionIndex>();
            CreateMap<BeanPassInfoDto, BeanPassResultDto>().ReverseMap();
        }

    }
}