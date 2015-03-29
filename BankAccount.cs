class BankAccount
{
    private string userid;
    private uint balance;
    public string UserID
    {
        get { return userid; }
    }

    public uint Balance
    {
        get { return balance; }
        set { balance = value; }
    }

    public BankAccount(string userid, uint balance)
    {
        this.userid = userid;
        this.balance = balance;
    }
}
