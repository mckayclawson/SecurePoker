class BankAccount
{
    private string userid;
    private double balance;
    public string UserID
    {
        get { return userid; }
    }

    public double Balance
    {
        get { return balance; }
        set { balance = value; }
    }

    public BankAccount(string userid, double balance)
    {
        this.userid = userid;
        this.balance = balance;
    }
}
