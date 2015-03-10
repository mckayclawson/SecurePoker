using System.Net;
using System.Net.Sockets;

class Client
{
    private string url;
    private int port;
    private PlayerAccount playerAccount;
    private TcpClient connection;

    public string ServerURL
    {
        get { return url; }
    }

    public int Port
    {
        get { return port; }
    }

    public PlayerAccount PlayerAccount
    {
        get { return playerAccount; }
    }

    public bool authenticate(string userName, string password)
    {
        return false;
    }

    public void move()
    {
    }

    public Client(string url, int port, PlayerAccount playerAccount)
    {
        this.url = url;
        this.port = port;
        this.playerAccount = playerAccount;
        connection = new TcpClient(url, port);
    }
}