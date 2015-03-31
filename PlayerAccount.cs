class PlayerAccount
{
    private string userid;
    private string userName;
    private BankAccount account;

    public string UserName
    {
        get { return userName; }
    }

    public bool isBankrupt()
    {
        return account.Balance == 0;
    }

    public PlayerAccount(string userid, string userName, BankAccount account)
    {
        this.userid = userid;
        this.userName = userName;
        this.account = account;
    }    
}
