using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms.VisualStyles;

namespace MoneyManager
{
    internal class Sql
    {
        private string _oldconnectionstring;
        private string _connectionstring;

        public Sql()
        {

        }
        public Sql(string oldconnectionstring, string connectionstring)
        {
            setOldConnectionString(oldconnectionstring);
            setConnectionString(connectionstring);
        }

        public void setOldConnectionString(string oldconnectionstring)
        {
            if (oldconnectionstring == null)
                throw new ArgumentNullException("Old connection string is null");
            else
            {
                _oldconnectionstring = oldconnectionstring;
            }
        }
        public string getOldConnectionString()
        {
            return _oldconnectionstring;
        }
        public void setConnectionString(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("Connection string is null");
            else
            {
                _connectionstring = connectionString;
            }
        }
        public string getConnectionString()
        {
            return _connectionstring;
        }

        public string CheckSqlConnection()
        {
            SqlConnection connection = new SqlConnection(_oldconnectionstring);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();

                }
            }
            return "200";

        }
        public string CheckDBConnections()
        {
            SqlConnection connection = new SqlConnection(_oldconnectionstring);
            try
            {
                connection.Open();
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
            finally
            {
                if(connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return "200";
        }
        public async Task CreateDB(RichTextBox rtb)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_oldconnectionstring))
                {
                    await connection.OpenAsync();
                    string se = "CREATE DATABASE transacsdb";
                    SqlCommand cmd = new SqlCommand(se, connection);
                    await cmd.ExecuteNonQueryAsync();
                    rtb.Text += "DB CREATED" + "\n";
                }
            }
            catch (SqlException ex) 
            {
                rtb.Text += ex.Message + "\n";
                //return ex.Message;
            }
            
            //return "DB CREATED";
        }
        public async Task CreateTable(RichTextBox rtb)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string Se = $"CREATE TABLE Transac(" +
                        $"Id INT PRIMARY KEY IDENTITY(1,1)," +
                        $"Type NVARCHAR(100) NOT NULL, " +
                        $"Name NVARCHAR(100) NOT NULL, " +
                        $"Description NVARCHAR(MAX), " +
                        $"Amount MONEY NOT NULL, " +
                        $"Date DATETIME NOT NULL," +
                        $"OldMoney MONEY NOT NULL," +
                        $"NewMoney MONEY NOT NULL," +
                        $"Active INT NOT NULL)";
                    SqlCommand cmd = new SqlCommand(Se, connection);
                    await cmd.ExecuteNonQueryAsync();
                    rtb.Text += "TABLE CREATED" + "\n";
                }
            }
            catch (SqlException ex)
            {
                rtb.Text += ex.Message + "\n";
                //ex.Message;
            }
            //return "TABLE CREATED";
        }
        public async Task<int> getMinId()
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string SE = "SELECT MIN(Id) FROM Transac WHERE Active=1";
                    SqlCommand cmd = new SqlCommand(SE, connection);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }    
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }
        public async Task<int> getMaxId()
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string SE = "SELECT MAX(Id) FROM Transac WHERE Active=1";
                    SqlCommand cmd = new SqlCommand(SE, connection);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
            catch (SqlException ex) 
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }
        public async Task<double> getLatestMoneyValue()
        {
            double money = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string SE = "SELECT MAX(Id) FROM Transac WHERE Active=1";
                    SqlCommand tmp = new SqlCommand(SE, connection);
                    int id = Convert.ToInt32(tmp.ExecuteScalar());
                    string se2 = $"SELECT NewMoney FROM Transac WHERE Id={id}";
                    SqlCommand cmd = new SqlCommand(se2, connection);
                    money = Convert.ToDouble(cmd.ExecuteScalar());
                    Console.WriteLine(money);
                    return money;
                }
            }
            catch (SqlException ex) 
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return money;
        }
        public async Task<List<Transaction>> GetTenTransacs(int index, bool flag = true)
        {
            List<Transaction> translist = new List<Transaction>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string SE = "";
                    if (flag)
                    {
                        SE = $"SELECT * FROM (SELECT TOP 10 * FROM Transac WHERE Id<={index} AND Active = 1 ORDER BY ID DESC) t ORDER BY ID";
                    }
                    else
                    {
                        SE = $"SELECT * FROM (SELECT TOP 10 * FROM Transac WHERE Id>={index} AND Active = 1 ORDER BY ID) t ORDER BY ID";
                    }
                     
                    SqlCommand cmd = new SqlCommand(SE, connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine(index);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader.GetValue(0)} Type: {reader.GetValue(1)} Name: {reader.GetValue(2)} Description: {reader.GetValue(3)} Amount: {reader.GetValue(4)} Date: {reader.GetValue(5)} OldMoney: {reader.GetValue(6)} NewMoney: {reader.GetValue(7)} Active: {reader.GetValue(8)}");
                            translist.Add(new Transaction(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), Convert.ToDouble(reader.GetValue(4)), Convert.ToDateTime(reader.GetValue(5))));
                        }
                        return translist;
                        //Console.WriteLine($"{reader.getValue(0)}");
                    }
                }
            }
            catch (SqlException ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return translist;
        }
        public async Task<bool> UpdateActive(int id, string type, int active)
        {
            bool flag = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string SE = "";
                    if(type == "Income")
                    {
                        SE = $"UPDATE Transac SET NewMoney = NewMoney - (SELECT Amount FROM Transac WHERE Id = {id} and Active=1) WHERE Id = (SELECT MAX(Id) FROM Transac WHERE Active=1)";
                    }
                    else if(type == "Spending")
                    {
                        SE = $"UPDATE Transac SET NewMoney = NewMoney + (SELECT Amount FROM Transac WHERE Id = {id} and Active=1) WHERE Id = (SELECT MAX(Id) FROM Transac WHERE Active=1)";
                    }
                    SqlCommand cmd2 = new SqlCommand(SE, connection);
                    await cmd2.ExecuteNonQueryAsync();
                    SE = $"UPDATE Transac SET Active = {active} WHERE Id = {id} And Active=1";
                    SqlCommand cmd = new SqlCommand(SE, connection);
                    await cmd.ExecuteNonQueryAsync();
                    flag = true;
                    return flag;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return flag;
            }
            
            
        }
        public async Task<int> Insert(string type, string name, string description, double amount, DateTime date, double oldmoney, double newmoney, int active)
        {
            int id = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    string SE = $"sp_InsertTr";
                    SqlCommand cmd = new SqlCommand(SE, connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@type", type));
                    cmd.Parameters.Add(new SqlParameter("@name", name));
                    cmd.Parameters.Add(new SqlParameter("@description", description));
                    cmd.Parameters.Add(new SqlParameter("@amount", amount));
                    cmd.Parameters.Add(new SqlParameter("@date", date));
                    cmd.Parameters.Add(new SqlParameter("@oldmoney", oldmoney));
                    cmd.Parameters.Add(new SqlParameter("@newmoney", newmoney));
                    cmd.Parameters.Add(new SqlParameter("@active", active));
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            return id;
        }
        public async Task CreateInsertProcedure(RichTextBox rtb)
        {
            try
            {
                string proc = @"CREATE PROCEDURE [dbo].[sp_InsertTr]
                @type nvarchar(100),
                @name nvarchar(100),
                @description nvarchar(MAX),
                @amount money,
                @date datetime,
                @oldmoney money,
                @newmoney money,
                @active int
            
            AS
                INSERT INTO Transac (Type, Name, Description, Amount, Date, OldMoney, NewMoney, Active)
                VALUES(@type, @name, @description, @amount, @date, @oldmoney, @newmoney, @active)

                SELECT SCOPE_IDENTITY()
            GO";
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand(proc, connection);
                    await cmd.ExecuteNonQueryAsync();
                    rtb.Text += "INSERT PROCEDURE CREATED" + "\n";
                }
            }
            catch (SqlException ex) 
            {
                rtb.Text = ex.Message + "\n";
            }
        }
        public async Task CreateGetProcedure(RichTextBox rtb)
        {
            try
            {
                string proc = @"CREATE PROCEDURE [dbo].[sp_getTr]
                AS
                    SELECT * FROM Transacs
                GO";
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand(proc, connection);
                    await cmd.ExecuteNonQueryAsync();
                    rtb.Text += "GET PROCEDURE CREATED" + "\n"; 
                }
            }
            catch (SqlException ex) 
            {
                rtb.Text = ex.Message + "\n";
            }
        }
    }
    
}
