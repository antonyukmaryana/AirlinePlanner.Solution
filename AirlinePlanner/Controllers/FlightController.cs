using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
  public class Flights : Controller
{
  [HttpGet("/flights")]
  public ActionResult Index()
  {
    List<Flight> allFlights = Flight.GetAll();
    return View (allFlights);
  }
  [HttpGet("/flights/new")]
  public ActionResult New()
  {
    return View();
  }
  [HttpPost("/flights/new")]
  public ActionResult Create (string departureCity, string arrivalCity, string status, int arrivalTime)
  {
    Flight flight = new Flight(departureCity, arrivalCity, status, arrivalTime, 0);
    flight.Save();
    List<Flight> allFlights = Flight.GetAll();
    return View("Index", allFlights);
  }
  }
}
