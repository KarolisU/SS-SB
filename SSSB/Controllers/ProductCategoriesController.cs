using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSSB.Auth.Model;
using SSSB.Data.Dtos.ProductCategories;
using SSSB.Data.Entities;
using SSSB.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [Route("api/productCategories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductCategoriesRepository _productCategoriesRepository;
        private readonly IAuthorizationService _authorizationService;

        public ProductCategoriesController(IProductCategoriesRepository productCategoriesRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _productCategoriesRepository = productCategoriesRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetAllAsync()
        {
            //var advertisements = await _advertisementsRepository.GetAsync(productCategoryId);

            //return Ok(advertisements.Select(o => _mapper.Map<AdvertisementDto>(o)));

            var a = await _productCategoriesRepository.GetAllAsync();
            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, a, PolicyNames.SameUser);
            //if (!authorizationResult.Succeeded)
            //{
            //    // 404
            //    return NotFound();
            //    // 403
            //    //return Forbid();
            //}
            //return Ok(advertisements.Select(o => _mapper.Map<AdvertisementDto>(o)));
            //var comments = await _commentsRepository.GetAsync(productCategoryId, advertisementId);
            if (a.Count == 0) return NotFound($"Couldn't find a product category");
            return Ok( a.Select(o => _mapper.Map<ProductCategoryDto>(o)));
        }

        // /api/topics/1/posts/2
        [HttpGet("{productCategoryId}")]
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult<ProductCategoryDto>> GetAsync(int productCategoryId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Product category with id '{productCategoryId}' not found.");

            return Ok(_mapper.Map<ProductCategoryDto>(productCategory));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductCategoryDto>> PostAsync(CreateProductCategoryDto productCategoryDto)
        {
            //var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            //if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");
            var addUserId = User.FindFirst(CustomClaims.UserId).Value;
            //var users = User.FindFirst(CustomClaims.UserId).Value;
            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            //advertisement.ProductCategoryId = productCategoryId;
            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, productCategory, PolicyNames.SameUser);
            //if (!authorizationResult.Succeeded)
            //{
            //    // 404
            //    return NotFound();
            //    // 403
            //    //return Forbid();
            //}

            await _productCategoriesRepository.InsertAsync(productCategory, addUserId);

            return Created($"/api/productCategories/{productCategory.Id}", _mapper.Map<ProductCategoryDto>(productCategory));
        }

        [HttpPut("{productCategoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductCategoryDto>> PutAsync(int productCategoryId, UpdateProductCategoryDto productCategoryDto)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Product category with id '{productCategoryId}' not found.");

            //var oldAdvertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            //if (oldAdvertisement == null)
            //    return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, productCategory, PolicyNames.SameUser);
            if (!authorizationResult.Succeeded)
            {
                // 404
                //return NotFound();
                // 403
                return Forbid();
            }

            //oldPost.Body = postDto.Body;
            _mapper.Map(productCategoryDto, productCategory);

            await _productCategoriesRepository.UpdateAsync(productCategory);

            return Ok(_mapper.Map<ProductCategoryDto>(productCategory));
        }

        [HttpDelete("{productCategoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductCategoryDto>> DeleteAsync(int productCategoryId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null)
                return NotFound($"Couldn't find a product category");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, productCategory, PolicyNames.SameUser);
            if (!authorizationResult.Succeeded)
            {
                // 404
                //return NotFound();
                // 403
                return Forbid();
            }

            await _productCategoriesRepository.DeleteAsync(productCategory);

            // 204
            return NoContent();
        }
    }
}

