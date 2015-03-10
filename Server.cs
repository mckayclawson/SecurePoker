using System.Collections.Generic;

class Server
{
    private List<PlayerAccount> accounts;

    public Bank Bank
    {
        get { return Bank.Instance; }
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
}