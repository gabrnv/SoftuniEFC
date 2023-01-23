namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class FootballersProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public FootballersProfile()
        {
            CreateMap<Coach, ExportCoachDto>()
                .ForMember(dest => dest.FootballersCount, mo => mo.MapFrom(src => src.Footballers.Count))
                .ForMember(dest => dest.CoachName, mo => mo.MapFrom(coach => coach.Name))
                .ForMember(dest => dest.Footballers, mo => mo.MapFrom(src => src.Footballers.OrderBy(f => f.Name)));

            CreateMap<Footballer, ExportCoachFootballerDto>()
                .ForMember(dest => dest.Name, mo => mo.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, mo => mo.MapFrom(src => src.PositionType.ToString()));
        }
    }
}
