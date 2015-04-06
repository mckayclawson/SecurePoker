using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

class Database
{
    readonly uint userid_column_length = 32;
    readonly uint salt_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.SALT_BYTE_SIZE / 3.0);
    readonly uint hash_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.HASH_BYTE_SIZE / 3.0);
    private static readonly Database instance = new Database();
    public static Database Instance
    {
        get { return instance; }
    }

    private SQLiteConnection connection;

    /// <summary>
    /// Initializes connection to database and creates tables if they do not already exist.
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
    /// Retrieves the hash of the specified user's password.
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
    /// Adds (registers) a new user to the database.
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
    /// Removes (unregisters) identified user from the database.
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

    // sloppy test code, please ignore
    public static void Main()
    {
        //string orig_id = "jar2119";
        //string unique_id = orig_id;
        //string password = "notarealorgoodpassword";
        //string wrongpassword = "wrongpassword";
        //Database db = Database.Instance;

        //uint i = 0;
        //while (true)
        //{
        //    try
        //    {
        //        db.AddUser(unique_id, password);
        //        break;
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine(unique_id + " already in use.");
        //        unique_id = orig_id + i++;
        //    }
        //}

        //Console.WriteLine("Added user " + unique_id);
        //Server server = Server.Instance;
        //Console.WriteLine("Authenticating password: " + (server.authenticate(unique_id, password) == true ? "Pass" : "FAIL"));
        //Console.WriteLine("Authenticating wrong password: " + (server.authenticate(unique_id, wrongpassword) == false ? "Pass" : "FAIL"));
        //PlayerAccount player = new PlayerAccount(unique_id);
        PlayerAccount player = new PlayerAccount("jar211926");
        Console.WriteLine(player.UserID + "\'s bank account number is " + player.BankAccount.AccountNumber);// + " and balance is $" + player.BankAccount.Balance / 100 + "." + player.BankAccount.Balance % 100);
        uint amt = 10000;
        player.BankAccount.deposit(amt);
        Console.WriteLine("Deposited $" + amt / 100 + "." + amt % 100);
        Console.WriteLine("Balance is now $" + player.BankAccount.Balance / 100 + "." + player.BankAccount.Balance % 100);
        player.BankAccount.withdraw(amt);
        Console.WriteLine("Withdrew $" + amt / 100 + "." + amt % 100);
        Console.WriteLine("Balance is now $" + player.BankAccount.Balance / 100 + "." + player.BankAccount.Balance % 100);
        amt = 500;
        player.BankAccount.deposit(amt);
        Console.WriteLine("Deposited $" + amt / 100 + "." + amt % 100);
        try
        {
            amt = player.BankAccount.Balance * 2;
            player.BankAccount.withdraw(amt);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Attempted to withdraw $" + amt / 100 + "." + amt % 100 + " from account with balance $" + player.BankAccount.Balance / 100 + "." + player.BankAccount.Balance % 100);
        }
        Console.WriteLine("Amount remains at $" + player.BankAccount.Balance / 100 + "." + player.BankAccount.Balance % 100 + ", as before.");
    }
}