using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace BestBUyCRUD
{
    class Program
    {
        static string response;
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Would you like to add a new Department?");
                Console.WriteLine("\n     Type Y for yes \n     or type EXIT to exit the program");
                response = Console.ReadLine().ToUpper();

                if (response == "Y")
                {
                    Console.WriteLine("What is the name of the new department?");

                    var departmentName = Console.ReadLine();
                    CreateDepartment(departmentName);
                }

            } while (response != "EXIT");
            do
            {
                Console.WriteLine("Would you like to update a Department?");
                Console.WriteLine("\n     Type Y for yes \n     or type EXIT to exit the program");
                response = Console.ReadLine().ToUpper();

                if (response == "Y")
                {

                    foreach (var item in GetAllDepartments())
                    {
                        Console.WriteLine(item);
                    }
                }
                Console.WriteLine("What department would you like to change?");
                var newDepart = Console.ReadLine().ToUpper();

                Console.WriteLine("What would you like to update the name to?");
                var departmentName = Console.ReadLine();
                UpdateDepartment(newDepart, departmentName);


                Console.WriteLine("Would you like to update another Department?");
                Console.WriteLine("\n     Type Y for yes \n     or type EXIT to exit the program");
                response = Console.ReadLine().ToUpper();

            } while (response != "EXIT");

            var depart = GetAllDepartments();

            foreach (var item in depart)
            {
                Console.WriteLine(item);
            }
        }
        public static IEnumerable<string> GetAllDepartments()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = File.ReadAllText("ConnectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Name FROM Departments;";

            using (conn)
            {
                conn.Open();
                List<string> allDepartments = new List<string>();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    var currentDepartment = reader.GetString("Name");
                    allDepartments.Add(currentDepartment);
                }
                return allDepartments;
            }
        }
        static void CreateDepartment(string departmentName)
        {
            var connStr = System.IO.File.ReadAllText("ConnectionString.txt");
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "INSERT INTO departments (Name) VALUES (@departmentName);";
                cmd.Parameters.AddWithValue("departmentName", departmentName);
                cmd.ExecuteNonQuery();
            }
        }
        static void UpdateDepartment(string DepartmentName, string changeTo) 
        {
            var connstr = System.IO.File.ReadAllText("ConnectionString.txt");
            using (var conn = new MySqlConnection(connstr)) 
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "UPDATE departments Set Name = (@departmentName) where name = @OldName;";
                cmd.Parameters.AddWithValue("OldName", DepartmentName);
                cmd.Parameters.AddWithValue("departmentName", changeTo);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

