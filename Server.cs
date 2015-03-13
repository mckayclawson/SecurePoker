using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

class Server
{
    private List<PlayerAccount> accounts;

    struct UserInfo
    {
        public string userName;
        public string password;
    }

    public Bank Bank
    {
        get { return Bank.Instance; }
    }

    private Server()
    {
        accounts = new List<PlayerAccount>();
    }

    public void startSession(int sessionid)
    {
    }

    public void endSession(int sessionid)
    {
    }

    public bool addUser(string userid, string userName, string password)
    {
        return false;
    }

    public bool removeUser(string userid)
    {
        return false;
    }

    public static void Main()
    {
        TcpListener connection = new TcpListener(Dns.GetHostAddresses("localhost")[0], PORT_NUMBER);
        connection.Start();

        const int BUFFER_SIZE = 1000;
        char[] buffer = new char[BUFFER_SIZE];

        Server server = new Server();
        while (true)
        {
            TcpClient client = connection.AcceptTcpClient();
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            reader.Read(buffer, 0, client.Available);

            string bufferData = new string(buffer);

            string[] lines = bufferData.Split('\n');

            foreach(string line in lines) {
                if(line.StartsWith("json=")) {
                    string jsonData = line.Substring(5);
                    jsonData = WebUtility.UrlDecode(jsonData);

                    UserInfo deserializedInfo = JsonConvert.DeserializeObject<UserInfo>(jsonData);

                    System.Console.WriteLine(deserializedInfo.userName);
                    System.Console.WriteLine(deserializedInfo.password);
                }
            }
        }

        connection.Stop();
    }

    private static readonly Server instance = new Server();
    private const int PORT_NUMBER = 5000;
}