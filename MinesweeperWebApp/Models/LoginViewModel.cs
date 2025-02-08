using System.ComponentModel.DataAnnotations;

namespace MinesweeperWebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(25)]
        public string Username { get; set; }
        [Required]
        [StringLength(25)]
        public string Password { get; set; }
    }
}
