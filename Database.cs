using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

/// <summary>
///     Encapsulates operations for registering and unregistering users and
///     updating banking information.
/// </summary>
class Database
{
    readonly uint userid_column_length = 32;
    readonly uint salt_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.SALT_BYTE_SIZE / 3.0);
    readonly uint hash_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.HASH_BYTE_SIZE / 3.0);
    private static readonly Database instance = new Database();

    /// <summary>
    ///     Singleton instance of the database.
    /// </summary>
    public static Database Instance
    {
        get { return instance; }
    }

    private SQLiteConnection connection;

    /// <summary>
    ///     Initializes connection to database and creates tables if they do not already exist.
    /// </summary>
    private Database()
    {
        connection = new SQLiteConnection("Data Source=secure_poker.db");
        connection.Open();

        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            string sql = "";
            sql += "CREATE TABLE IF NOT EXISTS USER_ACCOUNTS (";
            sql += "    USERID      VARCHAR(" + userid_column_length + ") PRIMARY KEY,";
            sql += "    SALT        CHAR(" + salt_column_length + ") NOT NULL,";
            sql += "    HASH        CHAR(" + hash_column_length + ") NOT NULL";
            sql += ");";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            string sql = "";
            sql += "CREATE TABLE IF NOT EXISTS BANK_ACCOUNTS (";
            sql += "    ACCOUNT_NUMBER  INTEGER PRIMARY KEY AUTOINCREMENT,";
            sql += "    USERID          VARCHAR(" + userid_column_length + ") NOT NULL,";
            sql += "    BALANCE         INTEGER NOT NULL,";
            sql += "    FOREIGN KEY(USERID) REFERENCES USER_ACCOUNTS(USERID));";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     Retrieves the hash of the specified user's password.
    /// </summary>
    /// <param name="userid">ID of user</param>
    /// <returns>
    ///     Three strings concatenated with colons in the following order:
    ///         Number of PBKDF2 iterations
    ///         Salt
    ///         Password hash
    ///     The salt and password hash are Base64 encoded.
    /// </returns>
    public string GetHash(string userid)
    {
        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT SALT, HASH FROM USER_ACCOUNTS WHERE USERID=@UserID";
            command.Parameters.AddWithValue("@UserID", userid);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    throw new Exception("Invalid user ID " + userid + ".");
                }

                reader.Read();
                return PasswordHash.PasswordHash.PBKDF2_ITERATIONS + ":" + reader["SALT"] + ":" + reader["HASH"];
            }
        }
    }

    /// <summary>
    ///     Adds (registers) a new user to the database.
    /// </summary>
    /// <param name="userid">new user's user ID</param>
    /// <param name="password">new user's password</param>
    public void AddUser(string userid, string password)
    {
        if (userid.Length > userid_column_length)
        {
            throw new ArgumentException("User ID length may not exceed " + userid_column_length + " characters.", "username");
        }

        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT * FROM USER_ACCOUNTS WHERE USERID=@UserID;";
            command.Parameters.AddWithValue("@UserID", userid);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) { throw new Exception("User ID " + userid + " is already in use."); }
            }
        }

        string[] hash_salt_iter = PasswordHash.PasswordHash.CreateHash(password).Split(new char[]{ ':' });
        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "INSERT INTO USER_ACCOUNTS (USERID, SALT, HASH) VALUES (@UserID, @Salt, @Hash);";
            command.Parameters.AddWithValue("@UserID", userid);
            command.Parameters.AddWithValue("@Salt", hash_salt_iter[PasswordHash.PasswordHash.SALT_INDEX]);
            command.Parameters.AddWithValue("@Hash", hash_salt_iter[PasswordHash.PasswordHash.PBKDF2_INDEX]);
            command.ExecuteNonQuery();
        }

        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "INSERT INTO BANK_ACCOUNTS (USERID, BALANCE) VALUES (@UserID, 0);";
            command.Parameters.AddWithValue("@UserID", userid);
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     Removes (unregisters) identified user from the database.
    /// </summary>
    /// <param name="userid">ID of the user to remove</param>
    public void RemoveUser(string userid)
    {
        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "DELETE FROM BANK_ACCOUNTS WHERE USERID=@UserID;";
            command.Parameters.AddWithValue("@UserID", userid);
            command.ExecuteNonQuery();
        }

        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "DELETE FROM USER_ACCOUNTS WHERE USERID=@UserID;";
            command.Parameters.AddWithValue("@UserID", userid);
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     Loads the information for the specified user's bank account.
    /// </summary>
    /// <param name="userid">ID of user</param>
    /// <returns>
    ///     Reference to a BankAccount that has been initialized with the user's
    ///     bank account information.
    /// </returns>
    public BankAccount LoadBankAccount(string userid)
    {
        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT ACCOUNT_NUMBER, BALANCE FROM BANK_ACCOUNTS WHERE USERID=@UserID;";
            command.Parameters.AddWithValue("@UserID", userid);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    throw new Exception("Invalid User ID " + userid + ".");
                }

                reader.Read();
                return new BankAccount(userid, Convert.ToUInt32(reader["ACCOUNT_NUMBER"]), Convert.ToUInt32(reader["BALANCE"]));
            }
        }
    }

    /// <summary>
    ///     Updates a user's bank account balance.
    /// </summary>
    /// <param name="accountNumber">The number identifying the bank account.</param>
    /// <param name="balance">The new bank account balance.</param>
    public void UpdateAccountBalance(uint accountNumber, uint balance)
    {
        using(SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "UPDATE BANK_ACCOUNTS SET BALANCE=@Balance WHERE ACCOUNT_NUMBER=@AccountNumber;";
            command.Parameters.AddWithValue("@Balance", balance);
            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     Tests the UpdateAccountBalance method.
    /// </summary>
    /// <returns>
    ///     True if the method passes the test, false if it does not.
    /// </returns>
    private static bool TestAccountBalanceUpdate()
    {
        string userid = "jar2119";
        string password = "password";
        Database db = Database.Instance;
        try
        {
            db.AddUser(userid, password);
        }
        catch (Exception ex)
        {
        }
        PlayerAccount player = new PlayerAccount(userid);
        BankAccount bankAccount = player.BankAccount;
        uint b1 = bankAccount.Balance;
        uint amount = 10000;
        bankAccount.deposit(amount);
        uint b2 = bankAccount.Balance;
        player = new PlayerAccount(userid);
        bankAccount = player.BankAccount;
        return bankAccount.Balance == b2;
    }

    public static void Main()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Testing bank account balance update: " + (TestAccountBalanceUpdate() ? "Pass" : "FAIL"));
        }
    }
}