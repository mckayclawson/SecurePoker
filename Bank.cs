using System.Collections.Generic;

class Bank
{
    private List<BankAccount> accounts;
    private static readonly Bank instance = new Bank();

    private Bank()
    {
        accounts = new List<BankAccount>();

        // TODO: load account information from database and create BankAccount objects
        // establish database connection, start transaction
        // while there are still accounts to be created
        //    query database for information for next account
        //    use data from response to initialize a new BankAccount object
        //    add the new BankAccount to the accounts list
        // end while
    }

    public static Bank Instance
    {
        get { return instance; }
    }

    public bool withdraw(BankAccount account, double amount)
    {
        if (account.Balance < amount)
        {
            return false;
        }

        account.Balance -= amount;
        return true;
    }

    public void deposit(BankAccount account, double amount)
    {
        account.Balance += amount;
    }

    private const double FIXED_MONEY = 100000.00;
}
