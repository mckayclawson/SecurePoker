using System;

/// <summary>
///     Represents a bank account which a player can deposit money into or
///     withdraw money from.
/// </summary>
class BankAccount
{
    private static readonly Database db = Database.Instance;

    private uint accountNumber;
    private uint balance;
    private string userid;

    /// <summary>
    ///     The unique number identifying this bank account.
    /// </summary>
    public uint AccountNumber
    {
        get { return accountNumber; }
    }

    /// <summary>
    ///     The currently available balance in this bank account, in cents.
    /// </summary>
    public uint Balance
    {
        get { return balance; }
    }

    /// <summary>
    ///     The ID of the user who owns this bank account;
    /// </summary>
    public string UserID
    {
        get { return userid; }
    }

    /// <summary>
    ///     Deposits the specified amount of money into the bank account;
    /// </summary>
    /// <param name="amount">The amount, in cents, to desposit.</param>
    public void deposit(uint amount)
    {
        balance += amount;
        db.UpdateAccountBalance(accountNumber, balance);
    }

    /// <summary>
    ///     Withdraws the psecified amount of money from the bank account if
    ///     sufficient funds are available.
    /// </summary>
    /// <param name="amount">The amount, in cents, to withdraw.</param>
    public void withdraw(uint amount)
    {
        if (amount > balance)
        {
            throw new Exception("Withdraw request exceeds available balance for account " + accountNumber + ".");
        }

        balance -= amount;
        db.UpdateAccountBalance(accountNumber, balance);
    }

    /// <summary>
    ///     Initializes a bank account with a number and balance.
    /// </summary>
    /// <param name="owner">The player who owns the bank account.</param>
    /// <param name="balance">The currently available balance in the account, in cents.</param>
    public BankAccount(string userid, uint accountNumber, uint balance)
    {
        this.accountNumber = accountNumber;
        this.balance = balance;
        this.userid = userid;
    }

    /// <summary>
    ///     Converts an amount of money in cents to dollars and cents.
    /// </summary>
    /// <param name="amount">The amount of money in cents.</param>
    /// <param name="dollars">The whole dollar amount.</param>
    /// <param name="cents">The remaining amount in cents.</param>
    public static void FromCents(uint amount, out uint dollars, out uint cents)
    {
        dollars = amount / 100;
        cents = amount % 100;
    }

    /// <summary>
    ///     Converts an amount of money in cents to a formatted dollars and
    ///     cents string.
    /// </summary>
    /// <param name="amount">The amount of money in cents.</param>
    /// <returns></returns>
    public static string AmtToString(uint amount)
    {
        uint dollars, cents;
        FromCents(amount, out dollars, out cents);
        string amt_string = "$" + dollars + ".";
        if (cents < 10) { amt_string += "0"; }
        amt_string += cents;
        return amt_string;
    }
}
