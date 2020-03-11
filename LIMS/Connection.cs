using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Text;


namespace LIMS
{

    /// <summary>
    ///     This class handles all things related to connecting to the database
    ///     there are two constructors
    ///     1. default is used only on program startup to load the json data
    ///     2. Connection(action) for two actions
    ///         i. Connection("open") will open the connection
    ///         ii. Connection("close") will close the connection
    /// </summary>
    public class Connection
    {
        // create the properties we will use when loading the data from the json file
        public static string ServerAddress { get; set; }
        public static string ServerPort { get; set; }
        public static string Database { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }

        public static string connString { get; private set; }

        public static string _filePath = "DB_Config.json";

        // this is the default constructor
        public Connection()
        {
            readJson();
        }

        // this is the constructor that will be used for performing actions
        /// <summary>
        ///     Connection(action) for two actions
        ///         i. Connection("open") will open the connection
        ///         ii. Connection("close") will close the connection
        /// </summary>
        public Connection(string action)
        {
            // decide which action to take
            if (action == "open")
                openConnection();
            else if (action == "close")
                closeConnection();

        }

        public void readJson()
        {
            try
            {
                string JsonData;

                // read the entire json file
                using (var reader = new StreamReader(_filePath))
                {
                    JsonData = reader.ReadToEnd();
                }

                // parse the json data 
                dynamic json = JValue.Parse(JsonData);

                // assigned the parsed data to the specified variable
                ServerAddress = json.Host;
                ServerPort = json.Port;
                Database = json.Database;
                User = json.User;
                Password = json.Password;

                connString = ServerAddress + ":" + ServerPort + ", " + Database + ", " + User + ", " + Password;
            }
            catch (Exception e)
            {
                // unused
            }
        }


        // this method will allow us to quickly open a connection
        public void openConnection()
        {

        }

        // this method will allow us to quickly close a connection
        public void closeConnection()
        {

        }

    }
}
