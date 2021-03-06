﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;

namespace Usable
{
    class Program
    {
        static void Main(string[] args) { }
        /// transaction
        private static void ExecuteSqlTransaction(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
	            command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText =
                        "Insert into Region (RegionID, RegionDescription) VALUES (100, 'Description')";
                    command.ExecuteNonQuery();
                    command.CommandText =
                        "Insert into Region (RegionID, RegionDescription) VALUES (101, 'Description')";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                }

                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                        " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
            }
        }
       //////////////////////////////////////////////
    
    }
} 
    
    
