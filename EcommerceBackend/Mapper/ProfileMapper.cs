﻿using AutoMapper;
using EcommerceBackend.Dto;
using EcommerceBackend.Models;

namespace EcommerceBackend.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper() 
        {
            CreateMap<User,UserLoginDto>().ReverseMap();
            CreateMap<User,UserRegisterationDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CatAddDto>().ReverseMap();
            CreateMap<Products, ProductWithCategoryDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src._Category.CategoryName))
            .ReverseMap();
            CreateMap<Products, AddProductDto>().ReverseMap();
            CreateMap<Products, UpdateProductDto>().ReverseMap();
            CreateMap<Products, ProductviewDto>().ReverseMap();   
            CreateMap<CartItems, CartViewDto>().ReverseMap();
            CreateMap<WishList,WishListViewDto>().ReverseMap();
            CreateMap<UserAddress, AddNewAddressDto>().ReverseMap();
            CreateMap<UserAddress, GetAddressDto>().ReverseMap();
            CreateMap<User, UserViewDto>().ReverseMap();






        }
    }
}
