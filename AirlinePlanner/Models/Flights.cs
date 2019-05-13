using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
  public class Flight
  {
    private int _id;
    private string _name;
    private int _cityId;

    public Flight(string name, int cityId, int id = 0)
    {
      _id = id;
      _cityId = cityId;
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
    public void SetName(string Name)
    {
      _name = name;
    }
    public static  List <Flight> GetAll(int cityId)
    {
      List<Flight> allFlights = new List<Flight>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM Flight where city_id = @cityId;";
      MySqlParameter cityParam = new MySqlParameter();
      cityParam.ParameterName = "@cityId";
      cityIdParam.Value = cityId;
      cmd.Parameters.Add(cityIdParam);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        Flight flight = new Flight (rdr.GetString(1),rdr.GetInt32(0));
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
      cmd.CommandText = @"INSERT INTO flight(name, city_id) VALUES(@name, @cityId);";
      MySqlParameter infoParam = new MySqlParameter();
      nameParam.ParameterName = "@name";
      nameParam.Value = this._name;
      cmd.Parameters.Add(infoParam);
      MySqlParameter cityIdParam = new MySqlParameter();
      cityIdParam.ParameterName = "@cityId";
      cityIdParam.Value = this._cityId;
      cmd.Parameters.add(cityIdParam);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
