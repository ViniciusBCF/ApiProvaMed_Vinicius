using ApiMedTest.Domain.Entities;
using ApiMedTest.Service.DTOs;
using ApiMedTest.Service.ViewModels;
using AutoMapper;

namespace ApiMedTest.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Contato, ContatoViewModel>();

            CreateMap<ContatoDTO, ContatoViewModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StatusContato, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore());
        }
    }
}
