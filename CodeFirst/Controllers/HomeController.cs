using CodeFirst.Data;
using CodeFirst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(loginT objUser)
        {
            var res = _db.LoginDetail.Where(a => a.Email == objUser.Email).FirstOrDefault();

            if (res == null)
            {

                TempData["Invalid"] = "Email is not found";
            }

            else
            {
                if (res.Email == objUser.Email && objUser.Password == objUser.Password)
                {

                    var claims = new[] { /*new Claim(ClaimTypes.Name, res.Name),*/
                                        new Claim(ClaimTypes.Email, res.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);


                    //HttpContext.Session.SetString("Name", objUser.Email);

                    return RedirectToAction("Index", "Home");

                }

                else
                {

                    ViewBag.Inv = "Wrong Email Id or password";

                    return View("Index");
                }


            }


            return View("Index");
        }
        [HttpGet]
        public IActionResult Registration()
        {
          return View();
        }
        [HttpPost]
        public IActionResult Registration(loginT abc)
        {
            loginT cbj = new loginT();
            cbj.Email = abc.Email;
            cbj.Password=abc.Password;
            cbj.ConfirmPassword=abc.ConfirmPassword;

            _db.LoginDetail.Add(cbj);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }

       
            public IActionResult LogOut()
            {
                HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return View("Login");
            }

        
        [Authorize] 
        public IActionResult Table()
        {
            var res = _db.StuDetails.ToList();
            return View(res);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Show()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Show(Student obj)
        {
            //Student abj = new Student();
            //abj.Id=obj.Id;
            //abj.Name= obj.Name;
            //abj.Email=obj.Email;
            //abj.Phone=obj.Phone;
            if (obj.Id == 0)
            {
                _db.StuDetails.Add(obj);
                _db.SaveChanges();
            }
            else
            {
                _db.Entry(obj).State = EntityState.Modified;
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
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
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
