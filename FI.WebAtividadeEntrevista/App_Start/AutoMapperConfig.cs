using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using FI.WebAtividadeEntrevista.Mappings;

namespace FI.WebAtividadeEntrevista
{
    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; set; }

        public static void RegisterMappings()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BeneficiarioMappingProfile>();
            });

            var mapper = config.CreateMapper();

            Mapper = mapper;
        }
    }
}