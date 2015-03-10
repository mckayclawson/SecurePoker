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

    public string UserName
    {
        get { return userName; }
    }

    public PlayerAccount(string userid, string userName, BankAccount account)
    {
        this.userid = userid;
        this.userName = userName;
        this.account = account;
    }    
}
