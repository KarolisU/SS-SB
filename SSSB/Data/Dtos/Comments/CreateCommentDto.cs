using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Dtos.Comments
{
    public record CreateCommentDto(
        [Required(ErrorMessage = "Comment text is required")] string CommentText,
        [Required(ErrorMessage = "Rating is required")][RegularExpression("^([0-9]|10)$", ErrorMessage = "Please enter valid rating")] int Rating);
}
