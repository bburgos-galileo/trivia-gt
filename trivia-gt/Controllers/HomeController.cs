﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using trivia_gt.Models;

namespace trivia_gt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Nombres = HttpContext.Session.GetString("Nombres");
            ViewBag.Imagen = @"http://drive.google.com/uc?export=view&id=" + HttpContext.Session.GetString("Imagen");
            ViewBag.ImagenLibro = @"http://drive.google.com/uc?export=view&id=1JC2aNjxTR3he5mywYJY_lnCwG0duZUpp";
            ViewBag.ImagenAnimo = @"http://drive.google.com/uc?export=view&id=1nocxQPDztHwLvbbacQcPUhmC2OmbTegL";
            ViewBag.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Visible = false;
            ViewBag.Nivel = 0;
            ViewBag.Percentage = 0;

            if (HttpContext.Session.GetString("mensaje") == null)
            {
                ViewBag.Mensaje = null;
            } else
            {
                ViewBag.Mensaje = HttpContext.Session.GetString("mensaje");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}