using AutoMapper;

namespace HexMultiplicationFlashCards
{
    static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<FlashCard, DAL.FlashCard>()
                  .ForMember(fc => fc.Question, opt => opt.Ignore())
                  .ForMember(fc => fc.Id, opt => opt.Ignore());  //TODO: ensure we never create FCs and expect a certain ID
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
