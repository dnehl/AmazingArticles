using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Client.Models
{
    public class ArticleModel
    {
        [Required, MaxLength(32)]
        public string ArticleNumber { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Sales price must be bigger than 0")]
        public double SalesPrice { get; set; }
    }
}
