using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
  public class Flights : Controller
{
  [HttpGet("/flights")]
  public ActionResult Index (int cityId)
  {
    List<Flight> allFlights = Flight.GetAll(cityId);
    return View (allFlights);
  }
  [HttpGet("/flights/new")]
  public ActionResult New()
  {
    return View();
  }
  [HttpPost("/flights/new")]
  public ActionResult Create (string name, int cityId)
  {
    Flight flight = new Flight(name, cityId, 0);
    flight.Save();
    List<Flight> allFlights = Flight.GetAll(cityId);
    return View("Index", allFlights);
  }
}
}
