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

    #region UNITY METHODS

    private void Start()
    {
        Connect();
    }

    #endregion

    #region METHODS

    private void Connect()
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
            }
        }
        catch (MySqlException exception)
        {
            print(exception.Message);
        }
    }

    #endregion
}