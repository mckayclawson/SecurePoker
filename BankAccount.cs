class BankAccount
{
    private string userid;
    private uint balance;
    public string UserID
    {
        get { return userid; }
    }

    /// <summary>
    /// The current account balance.
    /// </summary>
    public uint Balance
    {
        get { return balance; }
        set { balance = value; }
    }

    public BankAccount(string userid, uint initialBalance)
    {
        this.userid = userid;
        this.balance = initialBalance;
    }
}
