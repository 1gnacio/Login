using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleLogin.Controllers
{
    [Authorize(Policy = "MayorDeEdad")]
    public class PostsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trending()
        {
            return View();
        }
    }
}