class Client
{
    private string url;
    private int port;
    private PlayerAccount playerAccount;

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

    public boolean authenticate(string userName, string password)
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
    }
}