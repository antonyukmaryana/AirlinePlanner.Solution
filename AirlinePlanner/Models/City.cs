using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
  public class City
  {
    private string _name;
    private int _id;

    public City(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }
    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public static List<City> GetAll()
    {
      List<City>allCities = new List<City>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM city;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;


      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        City city = new City(name, cityId);
        allCities.Add(city);
      }

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO city (arrival_city) VALUES(@arrivalCity);";
      MySqlParameter arrivalParam = new MySqlParameter();
      arrivalParam.ParameterName = "@arrivalCity";
      arrivalParam.Value = this._name;
      cmd.Parameters.Add(arrivalParam);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM flight WHERE id = @FlightId; DELETE FROM city_flight WHERE flight_id = @FlightId;";
      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = this.GetId();
      cmd.Parameters.Add(flightIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddFlight(Flight newFlight)
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"INSERT INTO city_flight (city_id, flight_id) VALUES (@CityId, @FlightId);";
       MySqlParameter city_id = new MySqlParameter();
       city_id.ParameterName = "@CityId";
       city_id.Value = _id;
       cmd.Parameters.Add(city_id);
       MySqlParameter flight_id = new MySqlParameter();
       flight_id.ParameterName = "@FlightId";
       flight_id.Value = newFlight.GetId();
       cmd.Parameters.Add(flight_id);
       cmd.ExecuteNonQuery();
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
    }
    public List<Flight> GetFlights()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT flight.* FROM city JOIN city_flight ON (city.id = city_flight.arrival_city_id) JOIN flight ON (city_flight.flight_id = flight.id) WHERE city.id = @CityId;";
      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = _id;
      cmd.Parameters.Add(cityIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Flight> flights = new List<Flight> {};
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightDeparture = rdr.GetString(1);
        string flightArrival = rdr.GetString(2);
        string status = rdr.GetString(3);
        int flightArrTime = rdr.GetInt32(4);
        Flight newFlight = new Flight(flightDeparture, flightArrival, status, flightArrTime, flightId);
        flights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return flights;
    }
  }
}
