using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
  public class Cities : Controller
  {
  [HttpGet("/cities")]
  public ActionResult Index()
  {
    List<City> allCities = City.GetAll();
    return View(allCities);
  }
  [HttpGet("/cities/new")]
  public ActionResult New()
  {
    return View();
  }
  [HttpPost("/cities/new")]
  public ActionResult Create(string arrivalCity)
  {
    City city = new City(arrivalCity, 0);
    city.Save();
    List <City> allCities = City.GetAll();
    return View ("Index", allCities);
  }
}
}
