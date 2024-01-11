using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bloomApiProject.Models{
    public class Users{
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? token { get; set; }
        public string? role { get; set; }
         public List<CartItem>? CartItems { get; set; }
        }

}