using AutoMapper;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Category;
using PRN231_Group5.Assignment1.Repo.VIewModels.Member;
using PRN231_Group5.Assignment1.Repo.VIewModels.Order;
using PRN231_Group5.Assignment1.Repo.VIewModels.OrderDetail;
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

            CreateMap<CreateOrderViewModel, Order>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<UpdateOrderViewModel, Order>();

            CreateMap<CreateOrderDetailViewModel, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailViewModel>();
            CreateMap<UpdateOrderDetailViewModel, OrderDetail>();
        }
    }
}

