using AutoMapper;
using SVG.API.Application.Commands.Data;
using SVG.API.Models.Entity;
using SVG.API.Models.Response.Data;

namespace SVG.API.Infrastructure.AutoMapperExtensions
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<FileDataModel, FileReadModel>();
            CreateMap<SaveFileCommand, FileDataModel>();
        }
    }
}