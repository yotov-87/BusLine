using BusLine.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusLine.Data
{
    public class DataService : IDataService
    {
        private const string CONNECTION_STRING = @"Server=(localdb)\mssqllocaldb;Database=BusLineDB;Integrated Security=true;";

        public IndexViewModel GetStations()
        {
            var model = new IndexViewModel();

            using SqlConnection sqlConnection = new SqlConnection(CONNECTION_STRING);
            using SqlCommand command = new SqlCommand("GetStations", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
            SqlTransaction transaction;
            transaction = sqlConnection.BeginTransaction();
            command.Connection = sqlConnection;
            command.Transaction = transaction;
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string value = reader["StationValue"].ToString();
                    string name = reader["StationName"].ToString();
                    model.Stations.Add(int.Parse(value), name);
                }
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return model;
        }

        public List<int> GetAvailableSeats(int startStations, int endStations)
        {
            List<int> takenSeats = new List<int>();
            List<int> allSeatsInBus = GetAllSeats();
            using SqlConnection sqlConnection = new SqlConnection(CONNECTION_STRING);
            using SqlCommand command = new SqlCommand("GetSeats", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@StartStationValue", startStations);
            command.Parameters.AddWithValue("@EndStationValue", endStations);
            sqlConnection.Open();
            SqlTransaction transaction;
            transaction = sqlConnection.BeginTransaction();
            command.Connection = sqlConnection;
            command.Transaction = transaction;
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string value = reader["SeatsNumber"].ToString();
                    takenSeats.Add(int.Parse(value));
                }
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            List<int> freeSeats = allSeatsInBus.Where(x => !takenSeats.Any(y => x == y)).ToList();

            return freeSeats;
        }

        private List<int> GetAllSeats()
        {
            List<int> allSeatsInBus = new List<int>();
            using SqlConnection sqlConnection = new SqlConnection(CONNECTION_STRING);
            using SqlCommand command = new SqlCommand("GetAllSeatsInBus", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
            SqlTransaction transaction;
            transaction = sqlConnection.BeginTransaction();
            command.Connection = sqlConnection;
            command.Transaction = transaction;
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string value = reader["SeatsNumber"].ToString();
                    allSeatsInBus.Add(int.Parse(value));
                }
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return allSeatsInBus;
        }

        public void ReserveSeats(ReserveViewModel model)
        {
            using SqlConnection sqlConnection = new SqlConnection(CONNECTION_STRING);
            using SqlCommand command = new SqlCommand("AddSeatsStations", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();            
            foreach (var seat in model.selectedSeats)
            {

                command.Parameters.AddWithValue("@StartStationValue", model.startStations);
                command.Parameters.AddWithValue("@EndStationValue", model.endStations);
                command.Parameters.AddWithValue("@SeatsNumber", int.Parse(seat));
                command.Parameters.AddWithValue("@UserId", model.UserId);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
        }
    }
}
