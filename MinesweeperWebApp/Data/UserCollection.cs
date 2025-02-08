using MinesweeperWebApp.Models;

namespace MinesweeperWebApp.Data
{
    public class UserCollection : IUserManager
    {
        private List<UserModel> _users;

        public UserCollection()
        {
            _users = new List<UserModel>();
            // Create some user accounts
            GenerateUserData();
        }

        private void GenerateUserData()
        {
            UserModel user1 = new UserModel();
            user1.Username = "kolter";
            user1.SetPassword("mine1");
            user1.Groups = "Admin, User";
            AddUser(user1);

            UserModel user2 = new UserModel();
            user2.Username = "tyler";
            user2.SetPassword("mine1");
            user2.Groups = "Admin, User";
            AddUser(user2);
        }


        public int AddUser(UserModel user)
        {
            // Set the user's ID to the next available number
            user.Id = _users.Count + 1;
            _users.Add(user);
            return user.Id;
        }


        public int CheckCredentials(string username, string password)
        {
            // Given a username and password, find a matching user. Return the user's ID.
            foreach (UserModel user in _users)
            {
                if (user.Username == username && user.VerifyPassword(password))
                {
                    return user.Id;
                }
            }

            // No matches found, invalid login.
            return 0;
        }

        public void DeleteUser(UserModel user)
        {
            _users.Remove(user);
        }

        public List<UserModel> GetAllUsers()
        {
            return _users;
        }

        public UserModel GetUserById(int id)
        {
            // Using a lambda expression to find the user with the matching ID
            return _users.Find(u => u.Id == id);
        }


        public void UpdateUser(UserModel user)
        {
            // Find the user with the matching ID and replace it
            _users[_users.FindIndex(u => u.Id == user.Id)] = user;
        }

        public UserModel GetUserByUsername(string userName)
        {
            UserModel user = null;
            return user;
        }

    }
}

