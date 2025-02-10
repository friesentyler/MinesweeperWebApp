using MinesweeperWebApp.Models;

namespace MinesweeperWebApp.Data
{
    public class ValidationService
    {
        UserDAO userDAO = new UserDAO();
        public ValidationService() { }

        public string ValidateUser(UserModel user)
        {
            string errorString = "";

            UserModel userModel = userDAO.GetUserByUsername(user.Username);

            if (userModel != null)
            {
                errorString += "user already exists, ";
            }
            return errorString; 
        }
    }
}
