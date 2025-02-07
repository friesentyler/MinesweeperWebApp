namespace MinesweeperWebApp.Models
{
    public class GroupViewModel
    {
        public bool IsSelected { get; set; }
        public string GroupName { get; set; }
    }

    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<GroupViewModel> Groups { get; set; }

        public RegisterViewModel() 
        { 
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
