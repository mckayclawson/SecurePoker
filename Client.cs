using System;
using System.Net;
using System.Net.Sockets;

class Client
{
    private string url;
    private uint port;
    private PlayerAccount playerAccount;
    private TcpClient connection;

    public string ServerURL
    {
        get { return url; }
    }

    public uint Port
    {
        get { return port; }
    }

    public PlayerAccount PlayerAccount
    {
        get { return playerAccount; }
    }

    public void move()
    {
    }

    public Client(string url, uint port, PlayerAccount playerAccount)
    {
        this.url = url;
        this.port = port;
        this.playerAccount = playerAccount;
        connection = new TcpClient(url, Convert.ToInt32(port));
    }
}