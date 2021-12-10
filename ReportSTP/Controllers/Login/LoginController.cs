using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportSTP.Models;
using ReportSTP.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportSTP.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginForUser loginForUser)
        {

            if(loginForUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginForUser.UserName);

                if (result != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            

            return View(loginForUser);
        }

        
    }
}
