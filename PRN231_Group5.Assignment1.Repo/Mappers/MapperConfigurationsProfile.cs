using AutoMapper;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Category;
using PRN231_Group5.Assignment1.Repo.VIewModels.Member;
using PRN231_Group5.Assignment1.Repo.VIewModels.Product;

namespace PRN231_Group5.Assignment1.Repo.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<CreateMemberViewModel, Member>();
            CreateMap<UpdateMemberViewModel, Member>();

            CreateMap<CreateProductViewModel, Product>();
            CreateMap<UpdateProductViewModel, Product>();

            CreateMap<CreateCategoryViewModel, Category>();
            CreateMap<UpdateCategoryViewModel, Category>();
        }
    }
}

