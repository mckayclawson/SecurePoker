class Database
{
    private static readonly Database instance = new Database();
    public static Database Instance
    {
        get { return instance; }
    }

    // TODO: need member variables for database connection, transactions,
    // statements, everything necessary for state

    private Database()
    {
        // TODO: connect to database
    }

    /// <summary>
    /// Adds (registers) a new user to the database.
    /// </summary>
    /// <param name="userName">new user's username</param>
    /// <param name="password">new user's password</param>
    /// <param name="userid">unique string identifying user</param>
    /// <returns>
    ///     true if the new user was successfully added ot the database,
    ///     false if they were not
    /// </returns>
    public bool insertUser(string userName, string password, out string userid)
    {
        // TODO: generate a salt to store in the record for the new user
        // TODO: compute the hash of the password plus the salt to store in the
        // record
        // TODO: generate unique user ID
        userid = "null";

        // TODO: create bank account record for new user

        return false;
    }

    /// <summary>
    /// Removes (unregisters) identified user from the database.
    /// </summary>
    /// <param name="userid">ID of the user to remove</param>
    /// <returns>
    ///     true if the identified user's record was successfully removed
    ///     from the database, false if it was not
    /// </returns>
    public bool deleteUser(string userid)
    {
        // TODO: delete user's records from BankAccoutn and Player databases
        return false;
    }
}