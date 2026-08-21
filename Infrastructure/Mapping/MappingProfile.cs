using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Перетворення з Infrastructure.Models.CartItem на Domain.Models.CartItem
            CreateMap<Infrastructure.Models.CartItem, Domain.Models.CartItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.ProductSize, opt => opt.MapFrom(src => src.ProductSize));

            CreateMap<Infrastructure.Models.Cart, Domain.Models.Cart>()
             .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));  // Маппінг CartItems
            CreateMap<Domain.Models.Cart, Infrastructure.Models.Cart>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));  // Маппінг CartItems

            // Маппінг для Order
            CreateMap<Infrastructure.Models.Order, Domain.Models.Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<Domain.Models.Order, Infrastructure.Models.Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            // Маппінг для OrderItem
            CreateMap<Infrastructure.Models.OrderItem, Domain.Models.OrderItem>();
            CreateMap<Domain.Models.OrderItem, Infrastructure.Models.OrderItem>();

            CreateMap<Infrastructure.Models.Product, Domain.Models.Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom(src => src.ProductCategories))
            .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes))
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));

            CreateMap<Domain.Models.Product, Infrastructure.Models.Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom(src => src.ProductCategories))
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));

            // Маппінг між Domain та Infrastructure для ProductCategory
            CreateMap<Infrastructure.Models.ProductCategory, Domain.Models.ProductCategory>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<Domain.Models.ProductCategory, Infrastructure.Models.ProductCategory>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<Domain.Models.Category, Infrastructure.Models.Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom(src => src.ProductCategories));

            // Мапінг з Infrastructure.Models.Category до Domain.Models.Category
            CreateMap<Infrastructure.Models.Category, Domain.Models.Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom(src => src.ProductCategories));

            CreateMap<Domain.Models.Size, Infrastructure.Models.Size>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
              .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
              .ForMember(dest => dest.SizeTypeId, opt => opt.MapFrom(src => src.SizeTypeId))
              .ForMember(dest => dest.SizeType, opt => opt.MapFrom(src => src.SizeType)) // Якщо потрібно, мапимо SizeType
              .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));

            // Мапінг з Infrastructure.Models.Size до Domain.Models.Size
            CreateMap<Infrastructure.Models.Size, Domain.Models.Size>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.SizeTypeId, opt => opt.MapFrom(src => src.SizeTypeId))
                .ForMember(dest => dest.SizeType, opt => opt.MapFrom(src => src.SizeType)) // Якщо потрібно, мапимо SizeType
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));

            // Мапінг з Domain.Models.SizeType до Infrastructure.Models.SizeType
            CreateMap<Domain.Models.SizeType, Infrastructure.Models.SizeType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes));

            // Мапінг з Infrastructure.Models.SizeType до Domain.Models.SizeType
            CreateMap<Infrastructure.Models.SizeType, Domain.Models.SizeType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes));

            // Мапінг з Domain.Models.User до Infrastructure.Models.User
            CreateMap<Domain.Models.User, Infrastructure.Models.User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBitrh, opt => opt.MapFrom(src => src.DateOfBitrh));

            // Мапінг з Infrastructure.Models.User до Domain.Models.User
            CreateMap<Infrastructure.Models.User, Domain.Models.User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBitrh, opt => opt.MapFrom(src => src.DateOfBitrh));

            // Мапінг з Infrastructure.Models.ProductSize на Domain.Models.ProductSize
            CreateMap<Infrastructure.Models.ProductSize, Domain.Models.ProductSize>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.SizeId))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product)) // Навігаційна властивість
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size)); // Навігаційна властивість

            // Мапінг з Domain.Models.ProductSize на Infrastructure.Models.ProductSize
            CreateMap<Domain.Models.ProductSize, Infrastructure.Models.ProductSize>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.SizeId))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product)) // Навігаційна властивість
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size)); // Навігаційна властивість

            CreateMap<Infrastructure.Models.Review, Domain.Models.Review>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Domain.Models.Review, Infrastructure.Models.Review>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Мапінг для Cart
            CreateMap<Infrastructure.Models.Cart, Domain.Models.Cart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<Domain.Models.Cart, Infrastructure.Models.Cart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            // Мапінг для CartItem
            CreateMap<Infrastructure.Models.CartItem, Domain.Models.CartItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductSizeId, opt => opt.MapFrom(src => src.ProductSizeId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<Domain.Models.CartItem, Infrastructure.Models.CartItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductSizeId, opt => opt.MapFrom(src => src.ProductSizeId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));


        }
    }
}
