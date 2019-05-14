using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
  public class Flight
  {
    private int _id;
    private string _departureCity;
    private string _arrivalCity;
    private string _status;
    private int _arrivalTime;

    public Flight(string departureCity, string arrivalCity, string status, int arrivalTime, int id = 0)
    {
      _id = id;
      _departureCity = departureCity;
      _arrivalCity = arrivalCity;
      _status = status;
      _arrivalTime = arrivalTime;
    }
    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetDepartureCity()
    {
      return _departureCity;
    }
    public void SetDepartureCity(string newDepartureCity)
    {
      _departureCity = newDepartureCity;

    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public void SetArrivalCity(string newArrivalCity)
    {
      _arrivalCity = newArrivalCity;
    }
    public string GetStatus()
    {
      return _status;
    }
    public void SetStatus(string newStatus)
    {
      _status = newStatus;
    }
    public int GetArrivalTime()
    {
      return _arrivalTime;
    }
    public void SetArrivalTime(int newArrivalTime)
    {
      _arrivalTime = newArrivalTime;
    }

    public static  List <Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flight;";
      // MySqlParameter cityParam = new MySqlParameter();
      // cityParam.ParameterName = "@cityId";
      // cityParam.Value = cityId;
      // cmd.Parameters.Add(cityParam);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        Flight flight = new Flight (rdr.GetString(1), rdr.GetString(2),rdr.GetString(3),rdr.GetInt32(4), rdr.GetInt32(0));
        allFlights.Add(flight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flight(departure_city, arrival_city, status, arrival_time) VALUES(@departureCity, @arrivalCity, @status, @arrivalTime);";
      MySqlParameter depParam = new MySqlParameter();
      depParam.ParameterName = "@departureCity";
      depParam.Value = this._departureCity;
      cmd.Parameters.Add(depParam);
      MySqlParameter arrParam = new MySqlParameter();
      arrParam.ParameterName = "@arrivalCity";
      arrParam.Value = this._arrivalCity;
      cmd.Parameters.Add(arrParam);
      MySqlParameter statusParam = new MySqlParameter();
      statusParam.ParameterName = "@status";
      statusParam.Value = this._status;
      cmd.Parameters.Add(statusParam);
      MySqlParameter timeParam = new MySqlParameter();
      timeParam.ParameterName = "@arrivalTime";
      timeParam.Value = this._arrivalTime;
      cmd.Parameters.Add(timeParam);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static Flight Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flight WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int flightId = 0;
      string flightDeparture = "";
      string flightArrival = "";
      string flightStatus = "";
      int flightArrTime = 0;
      // We remove the line setting a itemCategoryId value here.
      while(rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        flightDeparture = rdr.GetString(1);
        flightArrival = rdr.GetString(2);
        flightStatus = rdr.GetString(3);
        flightArrTime = rdr.GetInt32(4);
        // We no longer read the itemCategoryId here, either.
      }
      // Constructor below no longer includes a itemCategoryId parameter:
      Flight newFlight = new Flight(flightDeparture, flightArrival, flightStatus, flightArrTime, flightId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newFlight;
    }

    public void Edit(string newStatus)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE flight SET status = @newStatus WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter status = new MySqlParameter();
      status.ParameterName = "@newStatus";
      status.Value = newStatus;
      cmd.Parameters.Add(status);
      cmd.ExecuteNonQuery();
      _status = newStatus;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<City> GetCities()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT city_id FROM city_flight WHERE flight_id = @flightId;";
      MySqlParameter itemIdParameter = new MySqlParameter();
      itemIdParameter.ParameterName = "@flightId";
      itemIdParameter.Value = _id;
      cmd.Parameters.Add(itemIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> cityIds = new List<int> {};
      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        cityIds.Add(cityId);
      }
      rdr.Dispose();
      List<City> cities = new List<City> {};
      foreach (int cityId in cityIds)
      {
        var cityQuery = conn.CreateCommand() as MySqlCommand;
        cityQuery.CommandText = @"SELECT * FROM city WHERE id = @CityId;";
        MySqlParameter cityIdParameter = new MySqlParameter();
        cityIdParameter.ParameterName = "@CityId";
        cityIdParameter.Value = cityId;
        cityQuery.Parameters.Add(cityIdParameter);
        var cityQueryRdr = cityQuery.ExecuteReader() as MySqlDataReader;
        while(cityQueryRdr.Read())
        {
          int thisCityId = cityQueryRdr.GetInt32(0);
          string cityName = cityQueryRdr.GetString(1);
          City foundCity = new City(cityName, thisCityId);
          cities.Add(foundCity);
        }
        cityQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return cities;
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("DELETE FROM city WHERE id = @CityId; DELETE FROM city_flight WHERE city_id = @CityId;", conn);
      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
