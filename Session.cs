using System.Collections.Generic;

class Session
{
    private List<PlayerAccount> players; // players currently in game

    public Session()
    {
        players = new List<PlayerAccount>();
    }
}