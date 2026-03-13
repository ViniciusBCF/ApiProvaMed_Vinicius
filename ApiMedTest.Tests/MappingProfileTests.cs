using ApiMedTest.Configuration;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace ApiMedTest.Tests
{
    [Trait("Aplicação", "Perfil de mapeamento")]
    public class MappingProfileTests
    {
        [Fact]
        public void DeveValidarMappingProfile()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfig());
            }, NullLoggerFactory.Instance);

            mapperConfig.AssertConfigurationIsValid();
        }
    }
}
