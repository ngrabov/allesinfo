using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using allinfo.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace allinfo.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<Writer> _userManager;

        public ManageUsersController(UserManager<Writer> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = (await _userManager.GetUsersInRoleAsync("Administrator")).ToArray();
            var everyone = await _userManager.Users.ToArrayAsync();

            var model = new ManageUsers
            {
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }
    }
}