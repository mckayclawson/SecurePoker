class PlayerAccount
{
    private static readonly Database db = Database.Instance;

    private BankAccount bankAccount;
    private string userid;

    public BankAccount BankAccount
    {
        get { return bankAccount; }
    }

    public string UserID
    {
        get { return userid; }
    }

    public bool isBankrupt()
    {
        return bankAccount.Balance == 0;
    }

    public PlayerAccount(string userid)
    {
        this.userid = userid;
        bankAccount = db.LoadBankAccount(userid);
    }
}
