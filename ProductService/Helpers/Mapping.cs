using AutoMapper;
using ProductService.Contracts.Authentication;
using ProductService.Domain.Services.Models.Authentication;

namespace ProductService.Helpers
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(conf =>
            {
                conf.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;

                conf.CreateMap<TokenRequest, TokenModel>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
