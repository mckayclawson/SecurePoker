using System;

class BankAccount
{
    private static readonly Database db = Database.Instance;

    private uint accountNumber;
    private uint balance;
    private string userid;

    /// <summary>
    /// The unique number identifying this bank account.
    /// </summary>
    public uint AccountNumber
    {
        get { return accountNumber; }
    }

    /// <summary>
    /// The currently available balance in this bank account, in cents.
    /// </summary>
    public uint Balance
    {
        get { return balance; }
    }

    public string UserID
    {
        get { return userid; }
    }

    public void deposit(uint amount)
    {
        balance += amount;
        db.UpdateAccountBalance(accountNumber, amount);
    }
    public void withdraw(uint amount)
    {
        if (amount > balance)
        {
            throw new Exception("Withdraw request exceeds available balance for account " + accountNumber + ".");
        }

        balance -= amount;
        db.UpdateAccountBalance(accountNumber, amount);
    }

    /// <summary>
    /// Initializes a bank account with a number and balance.
    /// </summary>
    /// <param name="owner">The player who owns the bank account.</param>
    /// <param name="balance">The currently available balance in the account, in cents.</param>
    public BankAccount(string userid, uint accountNumber, uint balance)
    {
        this.accountNumber = accountNumber;
        this.balance = balance;
        this.userid = userid;
    }
}
