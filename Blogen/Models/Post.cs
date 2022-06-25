using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace blogen.Models
{
    public class Post
    {
        [Key]
        [Display(Name = "#")]
        public int ID { get; set; }
        [Required]
        [StringLength(200)]
        [Column(TypeName ="nvarchar")]
        public string Title { get; set; }
        [ForeignKey("Category")]
        public int CatID { get; set; }
        public DateTime UpdateDate { get; set; }
        [Required]
        [AllowHtml]
        public string Body { get; set; }
        public String ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase PostImage { get; set; }

        //Navigation Properties
        public Category Category { get; set; }
    }
}