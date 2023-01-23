namespace TeisterMask
{
    using AutoMapper;
    using TeisterMask.Data.Models;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.DataProcessor.ImportDto;

    public class TeisterMaskProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TeisterMaskProfile()
        {
            CreateMap<Project, ExportProjectDto>()
                .ForMember(dest => dest.ProjectName, mo => mo.MapFrom(src => src.Name))
                .ForMember(dest => dest.HasEndDate, mo => mo.MapFrom(src => src.DueDate != null ? "Yes" : "No"));

            CreateMap<Task, ExportTaskDto>()
                .ForMember(dest => dest.Name, mo => mo.MapFrom(src => src.Name))
                .ForMember(dest => dest.Label, mo => mo.MapFrom(src => src.LabelType.ToString()));
                
        }
    }
}
