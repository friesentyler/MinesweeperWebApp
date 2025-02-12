namespace MinesweeperWebApp.Models
{
    public class ButtonViewModel
    {
        public IEnumerable<ButtonModel> Buttons { get; set; }
        public bool AllSameColor { get; set; }
    }
}
