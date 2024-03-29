using hwMarch13.Data;
using hwMarch13.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hwMarch13.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAds; Integrated Security=true;";

        public IActionResult Index()
        {
            var AdRepo = new AdRepository(_connectionString);
            var UserRepo = new UserRepository(_connectionString);
            var myId = 0;
            if(User.Identity.IsAuthenticated)
            {
                myId = UserRepo.GetByEmail(User.Identity.Name).Id;
            }
            var vm = new HomeViewModel()
            {
                ads = AdRepo.GetAllAds().Select(ad => new AdViewModel()
                {
                    Ad = ad,
                    Name = UserRepo.GetUserById(ad.UserId).Name,
                    IsMine = ad.UserId == myId
                }).ToList()
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var AdRepo = new AdRepository(_connectionString);
            var UserRepo = new UserRepository(_connectionString);
            ad.UserId = UserRepo.GetByEmail(User.Identity.Name).Id;
            AdRepo.AddAd(ad);
            return Redirect("/");
        }

        public IActionResult MyAccount()
        {
            var AdRepo = new AdRepository(_connectionString);
            var UserRepo = new UserRepository(_connectionString);
            var myId = 0;
            if (User.Identity.IsAuthenticated)
            {
                myId = UserRepo.GetByEmail(User.Identity.Name).Id;
            }
            var vm = new HomeViewModel()
            {
                ads = AdRepo.GetAllAds().Select(ad => new AdViewModel()
                {
                    Ad = ad,
                    Name = UserRepo.GetUserById(ad.UserId).Name,
                    IsMine = ad.UserId == myId
                }).ToList()
            };

            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            var repo = new AdRepository(_connectionString);
            repo.Delete(id);
            return Redirect("/");
        }
    }
}
