using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Forums.Web.Models
{
    public class PostData
    {
        [Required(ErrorMessage = "The content is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The title is required")]
        public string Title { get; set; }
    }
}
