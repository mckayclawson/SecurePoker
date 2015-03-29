using System.Collections.Generic;

class Bank
{
    private static readonly Bank instance = new Bank();
    public static Bank Instance
    {
        get { return instance; }
    }

    private static readonly uint fixed_money = 10000000;
    public static uint FixedMoney
    {
        get { return fixed_money; }
    }

    private List<BankAccount> accounts;

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

    public bool withdraw(BankAccount account, uint amount)
    {
        if (account.Balance < amount)
        {
            return false;
        }

        account.Balance -= amount;
        return true;
    }

    public void deposit(BankAccount account, uint amount)
    {
        account.Balance += amount;
    }
}
