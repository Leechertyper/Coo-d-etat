using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using System.Net;


using System.Net.Sockets;
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
    private bool _hostFound;
    private void Awake()
    {
        try 
        {
            int x = System.Net.Dns.GetHostAddresses(Host).Length;
            _hostFound = true;
        }
        catch (SocketException exception)
        {
            _hostFound = false;
        }
        if(PlayerPrefs.GetInt("BalanceDataBase") == 1)
        {
            if(_hostFound){
                conn = Connect();
            }
            else
            {
                Debug.Log("Host not found");
            }
            
        }
    }

    void Start()
    {
        try 
        {
            int x = System.Net.Dns.GetHostAddresses(Host).Length;
            _hostFound = true;
             Debug.Log("Host found");
        }
        catch (SocketException exception)
        {
            _hostFound = false;
             Debug.Log("Host not found");
        }
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

    public bool GetHostFound()
    {
        return _hostFound;
    }

    /***
    *Updates a value of a variable using a string and a float value
    *@param:string variable - The name of the variable from the database, float value - the value you wish to commit to the database
    *@return:None
    *@Post:Database is updated with a new value
    ***/
    public void UpdateSteps(string variable, float value)
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
                string sql = "UPDATE `coo_d_etat`.`GameBalance` SET `steps` = '" + value + "' WHERE (`variableName` = '" +variable +"');"; 
                //Debug.Log(sql);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
        }
        catch (MySqlException exception)
        {   
            Debug.Log("Error updating value");
            Debug.Log(exception.Message);
        }
        
    }
    /***
    *Retrieves steps from the database using a string
    *@param: string variable - the Name of the variable from the database
    *@return: float - steps - the steps taken from zero for climbing up a sigmoid function
    *@Post:None
    ***/
    public float GetSteps(string variable)
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = Host;
        builder.UserID = User;
        builder.Password = Password;
        builder.Database = Database;

        object result = null;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
            {
                connection.Open();
                string sql = "SELECT steps FROM `coo_d_etat`.`GameBalance` WHERE (`variableName` = '" +variable +"');";
                //Debug.Log(sql);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                result = cmd.ExecuteScalar();
            }
        }
        catch (MySqlException exception)
        {   
            
            print(exception.Message);
        }
        
        if (result != null)
        {
            float r = Convert.ToSingle(result);
            //Debug.Log("Got " + r + " from GetSteps");
            return r;
        }
        else
        {
            Debug.Log("Got nothing back from database. Replacing with -9999");
            return -9999f;
        }
    }
    /***
    *Retrieves a maximum value from the database using a string
    *@param: string variable - the Name of the variable from the database
    *@return: float - maximum value associated with the variable
    *@Post:None
    ***/
    public float GetMaxValue(string variable)
    {
         MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = Host;
        builder.UserID = User;
        builder.Password = Password;
        builder.Database = Database;

        object result = null;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
            {
                connection.Open();
                string sql = "SELECT maximumValue FROM `coo_d_etat`.`GameBalance` WHERE (`variableName` = '" +variable +"');";
                //Debug.Log(sql);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                result = cmd.ExecuteScalar();
            }
        }
        catch (MySqlException exception)
        {   
            
            print(exception.Message);
        }
        
        if (result != null)
        {
            float r = Convert.ToSingle(result);
            //Debug.Log("Got " + r + " from GetMaxValue");
            return r;
        }
        else
        {
            Debug.Log("Got nothing back from database. Replacing with -9999");
            return -9999f;
        }
    }

    /***
    *Retrieves a minimum value from the database using a string
    *@param: string variable - the Name of the variable from the database
    *@return: float - minimum value associated with the variable
    *@Post:None
    ***/
    public float GetMinValue(string variable)
    {
         MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = Host;
        builder.UserID = User;
        builder.Password = Password;
        builder.Database = Database;

        object result = null;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
            {
                connection.Open();
                string sql = "SELECT minValue FROM `coo_d_etat`.`GameBalance` WHERE (`variableName` = '" +variable +"');";
                //Debug.Log(sql);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                result = cmd.ExecuteScalar();
            }
        }
        catch (MySqlException exception)
        {   
            
            print(exception.Message);
        }
        
        if (result != null)
        {
            float r = Convert.ToSingle(result);
            //Debug.Log("Got " + r + " from GetMinValue");
            return r;
        }
        else
        {
            Debug.Log("Got nothing back from database. Replacing with 1");
            return 1f;
        }
    }
    /***
    *Inserts/updates a highscore in the table
    *@param: string name - the Name of the player (3 characters), int score - the score given
    *@return: None
    *@Post:The Highscores table in the database is updated to reflect the score
    ***/
    public void SubmitHighScore(string name, int score)
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
                string query = "SELECT COUNT(*) FROM `coo_d_etat`.`Highscores` WHERE ('name' = " + name+");";
                MySqlCommand queryCMD = new MySqlCommand(query, connection);
                object queryResult = queryCMD.ExecuteScalar();
                int r = Convert.ToInt32(queryResult);
                string sql = "";
                if(r > 0)
                {
                    sql = "UPDATE `coo_d_etat`.`Highscores` SET `score` = '"+score+"' WHERE (`name` = '"+name+"');";
                }
                else
                {
                    sql = "INSERT INTO `coo_d_etat`.`Highscores` (`name`, `score`) VALUES ('"+name+"', '"+score+"');"; 
                }
                //Debug.Log(sql);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
        }
        catch (MySqlException exception)
        {   
            Debug.Log("Error updating value");
            Debug.Log(exception.Message);
        }
        
    }

    /***
    *Retrieves the top 10 scores from the database
    *@param: None
    *@return: (strint, int)[10] a size 10 array with 10 tuples with name in string form then an int 32 as the score
    *@Post:None
    ***/

    public (string, int)[] GetHighScore()
    {
         MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = Host;
        builder.UserID = User;
        builder.Password = Password;
        builder.Database = Database;

        MySqlDataReader read = null;
       (string, int)[] result = new (string, int)[10];
        try
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
            {
                connection.Open();
                string sql = "SELECT * FROM coo_d_etat.Highscores order by score desc limit 10;";
                //Debug.Log(sql);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                read = cmd.ExecuteReader();
                int i = 0;
                while(read.Read())
                {
                    string name = read.GetString(0);
                    int score = read.GetInt32(1);
                    result[i] = (name, score);
                    i++;
                }
            }
        }
        catch (MySqlException exception)
        {   
            
            print(exception.Message);
        }
        
        return result;
    }

    #endregion
}