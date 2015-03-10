using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class Server
{
    private List<PlayerAccount> accounts;
    private TcpListener connection;

    public Bank Bank
    {
        get { return Bank.Instance; }
    }

    private Server()
    {
        accounts = new List<PlayerAccount>();
        connection = new TcpListener(Dns.GetHostAddresses("localhost")[0], PORT_NUMBER);
    }

    public void start()
    {
        connection.Start();
        const int BUFFER_SIZE = 100;
        byte[] buffer = new byte[BUFFER_SIZE];
        TcpClient player = connection.AcceptTcpClient();
        player.GetStream().Read(buffer, 0, BUFFER_SIZE);
        System.Console.WriteLine(buffer);
        connection.Stop();
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
    }

    private static readonly Server instance = new Server();
    private const int PORT_NUMBER = 5000;
}