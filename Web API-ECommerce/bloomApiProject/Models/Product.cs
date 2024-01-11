using System;
using System.ComponentModel.DataAnnotations;

namespace bloomApiProject.Models{
public class Products{
    public Guid id {get;set;}
    [StringLength(25,MinimumLength =5)]
    [Required (ErrorMessage ="Please enter product name")]
    public string? name { get; set; }
    [Required (ErrorMessage ="Please enter product price")]
    public string? price { get; set; }
    [Required (ErrorMessage ="Please enter product category")]
    public string? category { get; set; }
    [StringLength(500)]
    [Required (ErrorMessage ="Please enter product description")]
    public string? description { get; set; }
    [Required (ErrorMessage ="Please enter product image")]
    public string? image { get; set; }
    public string? quantity{get; set;}
    public string? productId{get;set;}
    public bool bestSeller{get;set;}
}
}
