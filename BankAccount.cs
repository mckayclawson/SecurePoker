class BankAccount
{
    private string userid;
    private double balance;

    publci BankAccount(string userid, double balance)
    {
        this.userid = userid;
        this.balance = balance;
    }

    public string UserID
    {
        get { return userid; }
    }

    public double Balance
    {
        get { return balance; }
    }
}