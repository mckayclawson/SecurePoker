class Bank
{
    private BankAccount[] bankAccounts;
    private const double FIXED_MONEY = 100000.00;
    private static Bank theBank = null;

    private Bank()
    {
        // TODO: create accounts
        
    }

    public Bank getBank()
    {
        if (theBank == null)
        {
            theBank = new Bank();
        }

        return theBank;
    }

    public boolean withdraw(BankAccount account, double amount)
    {
        return false;
    }

    public void deposit(BankAccount account, double amount)
    {
    }
}
