using System.ComponentModel.DataAnnotations;

namespace MinesweeperWebApp.Models
{
    public class GroupViewModel
    {
        public bool IsSelected { get; set; }
        public string GroupName { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(25)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(25)]
        public string Lastname { get; set; }
        [Required]
        [StringLength(7)]
        public string Sex { get; set; }
        [Required]
        [Range(0, 120, ErrorMessage = "{0} must be between {1} and {2}")]
        public int Age { get; set; }
        [Required]
        [StringLength(25)]
        public string State { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(25)]
        public string Username { get; set; }
        [Required]
        [StringLength(25)]
        public string Password { get; set; }
        [Required]
        public List<GroupViewModel> Groups { get; set; }

        public RegisterViewModel() 
        { 
            Firstname = string.Empty;
            Lastname = string.Empty;
            Sex = string.Empty;
            Age = 0;
            State = string.Empty;
            Email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Groups = new List<GroupViewModel>
            {
                new GroupViewModel { GroupName = "Admin", IsSelected = false },
                new GroupViewModel { GroupName = "Users", IsSelected = false },
                new GroupViewModel { GroupName = "Students", IsSelected = false }
            };
        }
    }
}
