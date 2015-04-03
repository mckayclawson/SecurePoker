using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

class Server
{
    private static readonly Server instance = new Server();
    private static readonly uint port_number = 5000;
    private static readonly Database db = Database.Instance;

    public static Server Instance
    {
        get { return instance; }
    }

    public static uint PortNumber
    {
        get { return port_number; }
    }

    // Note: password should probably actually be an encrypted password that will be decrypted, then checked
    struct UserInfo
    {
        public string userid;
        public string password;

        public UserInfo(string userid, string password)
        {
            this.userid = userid;
            this.password = password;
        }
    }

    /* Collection of active player accounts. */
    private List<PlayerAccount> accounts;

    /// <summary>
    /// 
    /// </summary>
    private Server()
    {
        accounts = new List<PlayerAccount>();
    }

    public bool authenticate(string userid, string password)
    {
        string hash = db.getHash(userid);
        return PasswordHash.PasswordHash.ValidatePassword(password, db.getHash(userid));
    }

    /// <summary>
    /// Starts a new session.
    /// </summary>
    /// <returns>
    ///     a unique, 32-bit, positive integer ID to refer to the session
    /// </returns>
    public int startSession()
    {
        // TODO: start a session, return ID
        return -1;
    }

    /// <summary>
    /// Ends the identified session.
    /// </summary>
    /// <param name="sessionid">identifier of session to end</param>
    /// <returns>
    ///     true if session was successfully ended, false if it was not
    ///     (for example, in the case of an invalid identifier)
    /// </returns>
    public bool endSession(uint sessionid)
    {
        // TODO: check whether sessionid is valid
        // TODO: end identified session
        return false;
    }

    /// <summary>
    ///     Authenticates the identified user and adds them to identified
    ///     session upon success.
    /// </summary>
    /// <param name="userid">ID of user</param>
    /// <param name="userName">user's username</param>
    /// <param name="password">user's password</param>
    /// <param name="sessionid">ID of session to add user to</param>
    /// <returns>
    ///     true if the user was successfully authenticated and added to
    ///     the session, flase if they were not
    /// </returns>
    public bool addUser(string userid, string password, uint sessionid)
    {
        return false;
    }

    /// <summary>
    /// Removes the identified user from the identified session.
    /// </summary>
    /// <param name="userid">ID of user to remove</param>
    /// <param name="sessionid">ID of session to remove user from</param>
    /// <returns>
    ///     true if the user was successfully removed from the identified
    ///     session, false otherwise
    /// </returns>
    public bool removeUser(string userid, uint sessionid)
    {
        return false;
    }

    /// <summary>
    ///     The main server loop listens for connections and, upon accepting
    ///     them, processes incoming requests.
    /// </summary>
    public static void _Main()
    {
        TcpListener connection = new TcpListener(Dns.GetHostAddresses("localhost")[0], Convert.ToInt32(Server.PortNumber));
        connection.Start();

        const int buffer_size = 1024;
        char[] buffer = new char[buffer_size];

        Server server = new Server();
        while (true)
        {
            TcpClient client = connection.AcceptTcpClient();
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            reader.Read(buffer, 0, client.Available);

            string[] lines = new string(buffer).Split('\n');
            foreach (string line in lines)
            {
                if (line.StartsWith("json="))
                {
                    string jsonData = line.Substring(5);
                    jsonData = WebUtility.UrlDecode(jsonData);

                    UserInfo deserializedInfo = JsonConvert.DeserializeObject<UserInfo>(jsonData);

                    System.Console.WriteLine(deserializedInfo.userid);
                    System.Console.WriteLine(deserializedInfo.password);
                }
            }

            reader.DiscardBufferedData();
        }
    }
}