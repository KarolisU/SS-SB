using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSSB.Auth.Model;
using SSSB.Data.Dtos.Comments;
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
    [Route("api/productCategories/{productCategoryId}/advertisements/{advertisementId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IAdvertisementsRepository _advertisementsRepository;
        private readonly IMapper _mapper;
        private readonly IProductCategoriesRepository _productCategoriesRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IAuthorizationService _authorizationService;

        public CommentsController(IAdvertisementsRepository advertisementsRepository, IMapper mapper, IProductCategoriesRepository productCategoriesRepository, ICommentsRepository commentsRepository, IAuthorizationService authorizationService)
        {
            _advertisementsRepository = advertisementsRepository;
            _mapper = mapper;
            _productCategoriesRepository = productCategoriesRepository;
            _commentsRepository = commentsRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllAsync(int productCategoryId, int advertisementId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null) return NotFound($"Couldn't find a advertisement with id of {advertisementId}");
            
            
            
            var comments = await _commentsRepository.GetAsync(productCategoryId, advertisementId);
            if (comments.Count == 0) return NotFound($"Couldn't find a comments");

            return Ok(comments.Select(o => _mapper.Map<CommentDto>(o)));
        }

        // /api/topics/1/posts/2
        [HttpGet("{commentId}")]
        public async Task<ActionResult<CommentDto>> GetAsync(int productCategoryId, int advertisementId, int commentId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null) return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            var comment = await _commentsRepository.GetAsync(productCategoryId, advertisementId, commentId);
            if (comment == null) return NotFound($"Couldn't find a comment with id of {commentId}");

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> PostAsync(int productCategoryId, int advertisementId, CreateCommentDto commentDto)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null) return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            var comment = _mapper.Map<Comment>(commentDto);
            //comment.Advertisement.ProductCategoryId = productCategoriesId;
            comment.AdvertisementId = advertisementId;
            //comment.Advertisement.Id = advertisementId;
            var addUserId = User.FindFirst(CustomClaims.UserId).Value;
            await _commentsRepository.InsertAsync(comment, addUserId);

            return Created($"/api/productCategories/{productCategoryId}/advertisements/{advertisementId}/comments/{comment.Id}", _mapper.Map<CommentDto>(comment));
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult<CommentDto>> PostAsync(int productCategoryId, int advertisementId, int commentId, UpdateCommentDto commentDto)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null) return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            var oldComment = await _commentsRepository.GetAsync(productCategoryId, advertisementId, commentId);
            if (oldComment == null)
                return NotFound($"Couldn't find a comment with id of {commentId}");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, oldComment, PolicyNames.SameUser);
            if (!authorizationResult.Succeeded)
            {
                // 404
                //return NotFound();
                // 403
                return Forbid();
            }


            //oldPost.Body = postDto.Body;
            _mapper.Map(commentDto, oldComment);

            await _commentsRepository.UpdateAsync(oldComment);

            return Ok(_mapper.Map<CommentDto>(oldComment));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCategoriesId"></param>
        /// <param name="advertisementId"></param>
        /// <returns></returns>
        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteAsync(int productCategoryId, int advertisementId, int commentId)
        {
            var productCategory = await _productCategoriesRepository.GetAsync(productCategoryId);
            if (productCategory == null) return NotFound($"Couldn't find a product category with id of {productCategoryId}");

            var advertisement = await _advertisementsRepository.GetAsync(productCategoryId, advertisementId);
            if (advertisement == null) return NotFound($"Couldn't find a advertisement with id of {advertisementId}");

            var comment = await _commentsRepository.GetAsync(productCategoryId, advertisementId, commentId);
            if (comment == null)
                return NotFound($"Couldn't find a comment with id of {commentId}");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, comment, PolicyNames.SameUser);
            if (!authorizationResult.Succeeded)
            {
                // 404
                //return NotFound();
                // 403
                return Forbid();
            }

            await _commentsRepository.DeleteAsync(comment);

            // 204
            return NoContent();
        }
    }
}
