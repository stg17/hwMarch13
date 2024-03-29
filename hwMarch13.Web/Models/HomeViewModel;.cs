using hwMarch13.Data;

namespace hwMarch13.Web.Models
{
    public class HomeViewModel
    {
        public List<AdViewModel> ads { get; set; }
    }

    public class AdViewModel
    {
        public Ad Ad { get; set; }
        public string Name { get; set; }
        public bool IsMine { get; set; }
    }
}
