namespace MinesweeperWebApp.Models
{
    public class GroupViewModel
    {
        public bool IsSelected { get; set; }
        public string GroupName { get; set; }
    }

    public class RegisterViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
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
