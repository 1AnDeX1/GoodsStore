using AutoMapper;
using GoodsStore.Dto;
using GoodsStore.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Products, ProductsDto>();
            CreateMap<ProductsDto, Products>();
        }
    }
}
