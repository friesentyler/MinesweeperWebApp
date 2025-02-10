namespace MinesweeperWebApp.Models
{
    public class RegistrationErrorViewModel
    {
        public RegistrationErrorViewModel(string error) { Error = error; }
        public string Error { get; set; }
    }
}
