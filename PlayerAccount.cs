class PlayerAccount
{
    private string userid;
    private string userName;
    private BankAccount bankAccount;

    public Wallet wallet {get;}
    public UserName username {get;}

    public PlayerAccount(string userid, string userName, BankAccount bankAccount)
    {
        this.userid = userid;
        this.username = userName;
        this.bankAccount = bankAccount;
    }    
}
