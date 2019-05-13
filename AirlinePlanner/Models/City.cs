using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace AirlinePlanner
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
      cmd.CommandText = @"SELECT * FROM City;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;


      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);

        City city = new City(cityName, cityId);
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
      cmd.CommandText = @"INSERT INTO City (name) VALUES(@CityName);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@CityName";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
  }
}
