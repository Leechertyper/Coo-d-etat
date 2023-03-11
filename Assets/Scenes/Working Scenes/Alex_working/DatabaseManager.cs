using System;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region VARIABLES

    [Header("Database Properties")]
    public string Host = "db.cs.usask.ca";
    public string User = "coo_d_etat_user";
    public string Password = "2kkj23hvuiio32hlasjvkaf";
    public string Database = "coo_d_etat";

    #endregion
    private MySqlConnection conn = null;
    #region UNITY METHODS

    private void Start()
    {
        conn = Connect();
    }

    #endregion

    #region METHODS
    /***
    *Opens a connection to the database (ONLY WORKS WITH VPN OR ON CAMPUS)
    *@param:None
    *@return: MySqlConnection - The connection to the database that is opened
    *@Post: Connection is opened and returned
    *Uses the host, user, password, database strings to build a connection to the database and returns the connection
    *Absolutely unsafe/unsecure (Replace with a interface to a server which then interfaces with the database in this way when available.)
    *
    ***/
    private MySqlConnection Connect()
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = Host;
        builder.UserID = User;
        builder.Password = Password;
        builder.Database = Database;

        try
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
            {
                connection.Open();
                print("MySQL - Opened Connection");
                return connection;
            }
        }
        catch (MySqlException exception)
        {   
            
            print(exception.Message);
            return null;
        }
    }
    /***
    *Closes the connection to the database, Call this before closing the game.
    *@param: None
    *@return: None
    *@Post: Connection to database Closed
    ***/
    public void CloseConnection()
    {
        conn.Close();
    }
    /***
    *Updates a value of a variable using a string and a float value
    *@param:string variable - The name of the variable from the database, float value - the value you wish to commit to the database
    *@return:None
    *@Post:Database is updated with a new value
    ***/
    public void UpdateValue(string variable, float value)
    {
        string sql = "UPDATE coo_d_etat.GameBalance SET value = " + value + " WHERE variableName = " + variable +";"; 
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }
    /***
    *Retrieves a value from the database using a string
    *@param: string variable - the Name of the variable from the database
    *@return: float - value associated with the variable
    *@Post:None
    ***/
    public float GetValue(string variable)
    {
        string sql = "SELECT value FROM coo_d_etat.GameBalance WHERE variableName = " + variable;
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        object result = cmd.ExecuteScalar();
        if (result != null)
        {
            float r = Convert.ToSingle(result);
            return r;
        }
        else
        {
            Debug.Log("Got nothing back from database.");
            return -9999f;
        }
    }

    #endregion
}