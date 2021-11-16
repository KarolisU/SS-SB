using AutoMapper;
using SSSB.Data.Dtos.Advertisements;
using SSSB.Data.Dtos.Auth;
using SSSB.Data.Dtos.Comments;
using SSSB.Data.Dtos.ProductCategories;
using SSSB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data
{
    public class SSSBRestProfile : Profile
    {
        public SSSBRestProfile()
        {
            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<CreateProductCategoryDto, ProductCategory>();
            CreateMap<UpdateProductCategoryDto, ProductCategory>();

            CreateMap<Advertisement, AdvertisementDto>();
            CreateMap<CreateAdvertisementDto, Advertisement>();
            CreateMap<UpdateAdvertisementDto, Advertisement>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<SSSBUser, UserDto>();
        }
    }
}
