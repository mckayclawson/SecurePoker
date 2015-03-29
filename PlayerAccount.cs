class PlayerAccount
{
    private string userid;
    private string userName;
    private BankAccount account;
    private Wallet wallet;

    public Wallet Wallet
    {
        get { return wallet; }
    }

    // TODO: Wallet is not necessary. Why not store however much of a player's money is not in a player's bank account in a member variable?
    // For example:
    // private uint moneyToBet;

    public string UserName
    {
        get { return userName; }
    }

    public PlayerAccount(string userid, string userName, BankAccount account)
    {
        this.userid = userid;
        this.userName = userName;
        this.account = account;

        wallet = new Wallet();
        wallet.Cents = (int)(account.Balance * 100.0);
    }    
}
