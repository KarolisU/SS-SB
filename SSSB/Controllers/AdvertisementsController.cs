using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSSB.Auth.Model;
using SSSB.Data.Dtos.Advertisements;
using SSSB.Data.Entities;
using SSSB.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin, RegisteredUser")]
    [Route("api/productCategories/{productCategoryId}/advertisements")]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementsRepository _advertisementsRepository;
        private readonly IMapper _mapper;
        private readonly IProductCategoriesRepository _productCategoriesRepository;
        private readonly IAuthorizationService _authorizationService;

        public AdvertisementsController(IAdvertisementsRepository advertisementsRepository, IMapper mapper, IProductCategoriesRepository productCategoriesRepository, IAuthorizationService authorizationService)
        {
            _advertisementsRepository = advertisementsRepository;
            _mapper = mapper;
            _productCategoriesRepository = productCategoriesRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertisementDto>>> GetAllAsync(int productCategoryId)
        {
            var productcategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productcategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");
            //var b = productcategory;
            var advertisements = await _advertisementsRepository.GetAsync(productCategoryId);
            //var a = advertisements;
            if (advertisements.Count == 0) return NotFound($"Couldn't find a advertisements");

            return Ok(advertisements.Select(o => _mapper.Map<AdvertisementDto>(o)));
        }

        // /api/topics/1/posts/2
        [HttpGet("{advertisementId}")]
        public async Task<ActionResult<AdvertisementDto>> GetAsync(int productCategoryId, int advertisementId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null) return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            return Ok(_mapper.Map<AdvertisementDto>(advertisement));
        }

        [HttpPost]
        public async Task<ActionResult<AdvertisementDto>> PostAsync(int productCategoryId, CreateAdvertisementDto advertisementDto)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = _mapper.Map<Advertisement>(advertisementDto);
            advertisement.ProductCategoryId = productCategoryId;
            //advertisement..ProductCategory.Id = productCategoryId;
            var addUserId = User.FindFirst(CustomClaims.UserId).Value;
            await _advertisementsRepository.InsertAsync(advertisement, addUserId);

            return Created($"/api/productCategories/{productCategoryId}/advertisements/{advertisement.Id}", _mapper.Map<AdvertisementDto>(advertisement));
        }

        [HttpPut("{advertisementId}")]
        public async Task<ActionResult<AdvertisementDto>> PostAsync(int productCategoryId, int advertisementId, UpdateAdvertisementDto advertisementDto)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var oldAdvertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (oldAdvertisement == null)
                return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, oldAdvertisement, PolicyNames.SameUser);
            if (!authorizationResult.Succeeded)
            {
                // 404
                //return NotFound();
                // 403
                return Forbid();
            }
            
            //oldPost.Body = postDto.Body;
            _mapper.Map(advertisementDto, oldAdvertisement);

            await _advertisementsRepository.UpdateAsync(oldAdvertisement);

            return Ok(_mapper.Map<AdvertisementDto>(oldAdvertisement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCategoriesId"></param>
        /// <param name="advertisementId"></param>
        /// <returns></returns>
        [HttpDelete("{advertisementId}")]
        public async Task<ActionResult> DeleteAsync(int productCategoryId, int advertisementId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null)
                return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, advertisement, PolicyNames.SameUser);
            if (!authorizationResult.Succeeded)
            {
                // 404
                //return NotFound();
                // 403
                return Forbid();
            }

            await _advertisementsRepository.DeleteAsync(advertisement);

            // 204
            return NoContent();
        }
    }
}
