namespace Theatre
{
    using AutoMapper;
    using System.Globalization;
    using System.Linq;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.DataProcessor.ImportDto;

    class TheatreProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public TheatreProfile()
        {
            CreateMap<Play, ExportPlayDto>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString("c", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Casts.Where(c => c.IsMainCharacter).OrderByDescending(a => a.FullName).ToArray()))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating == 0 ? "Premier" : src.Rating.ToString()));

            CreateMap<Cast, ExportActorDto>()
                .ForMember(dest => dest.FullName, mo => mo.MapFrom(src => src.FullName.ToString()))
                .ForMember(dest => dest.MainCharacter, mo => mo.MapFrom(src => $"Plays main character in '{src.Play.Title}'."));

            CreateMap<Theatre, ExportTheatreDto>()
                .ForMember(dest => dest.Name, mo => mo.MapFrom(src => src.Name))
                .ForMember(dest => dest.Halls, mo => mo.MapFrom(src => src.NumberOfHalls))
                .ForMember(dest => dest.TotalIncome, mo => mo.MapFrom(src => src.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Sum(t => t.Price)))
                .ForMember(dest => dest.Tickets, mo => mo.MapFrom(src => src.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).OrderByDescending(t => t.Price).ToList()));

            CreateMap<Ticket, ExportTicketDto>()
                .ForMember(dest => dest.Price, mo => mo.MapFrom(src => src.Price))
                .ForMember(dest => dest.RowNumber, mo => mo.MapFrom(src => src.RowNumber));
        }
    }
}
