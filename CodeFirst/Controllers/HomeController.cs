using CodeFirst.Data;
using CodeFirst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeFirst.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly StudentContext _db;

        public HomeController(StudentContext db)
        {
            _db = db;
        }
        //public IActionResult Login()
        //{
        //    var claims = new[] { new Claim(ClaimTypes.Name, res.Name),
        //                                new Claim(ClaimTypes.Email, res.Email) };

        //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    varauthProperties = new AuthenticationProperties
        //    {
        //        IsPersistent = true
        //    };
        //    HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //                        new ClaimsPrincipal(identity),
        //    authProperties);

        //    return View();
        //}
        public IActionResult Table()
        {
            var res = _db.StuDetails.ToList();
            return View(res);
        }
        [HttpGet]
        public IActionResult Show()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Show(Student obj)
        {
            Student abj = new Student();
            abj.Id= obj.Id;
            abj.Name= obj.Name;
            abj.Email=obj.Email;
            abj.Phone=obj.Phone;
            if (obj.Id == 0)
            {
                _db.StuDetails.Add(abj);
                _db.SaveChanges();
            }
            else
            {
                _db.Entry(abj).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Table");
        }
        public IActionResult delete(int id)
        {
            var deleteitem = _db.StuDetails.Where(m => m.Id == id).First();
            _db.StuDetails.Remove(deleteitem);
            _db.SaveChanges();
            return RedirectToAction("Table");
            
        }
        public IActionResult edit(int id)
        {
            Student abj = new Student();
            var edititem = _db.StuDetails.Where(m => m.Id == id).First();

            abj.Id=edititem.Id;
            abj.Name=edititem.Name;
            abj.Email=edititem.Email;
            abj.Phone=edititem.Phone;

            return View("Show",abj);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
