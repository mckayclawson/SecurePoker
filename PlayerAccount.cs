class PlayerAccount
{
    private string userid;
    private string userName;
    private BankAccount bankAccount;
    private Wallet wallet;

    public Wallet Wallet
    {
        get { retunr wallet; }
    }

    public UserID
    {
        get { return userid; }
    }

    public UserName
    {
        get { return userName; }
    }

    public PlayerAccount(string userid, string userName, BankAccount bankAccount)
    {
        this.userid = userid;
        this.username = userName;
        this.bankAccount = bankAccount;
    }    
}