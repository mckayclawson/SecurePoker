class Wallet
{
    private int cents;
    public int Cents
    {
        get { return cents; }
        set
        {
            if (value >= 0) { cents = value; }
        }
    }
}