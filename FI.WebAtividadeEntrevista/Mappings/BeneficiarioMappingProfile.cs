using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAtividadeEntrevista.Models;
using AutoMapper;

namespace FI.WebAtividadeEntrevista.Mappings
{
    public class BeneficiarioMappingProfile : Profile
    {
        public BeneficiarioMappingProfile()
        {
            CreateMap<BeneficiarioModel, Beneficiario>()
                .ForMember(dest => dest.IdCliente, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Options.Items["IdCliente"]));

            CreateMap<Beneficiario, BeneficiarioModel>()
                .ForMember(dest => dest.IdCliente, opt => opt.Ignore());
        }
    }
}