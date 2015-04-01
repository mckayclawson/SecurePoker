using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

class Database
{
    private static readonly Database instance = new Database();
    public static Database Instance
    {
        get { return instance; }
    }

    readonly uint userid_column_length = 32;
    readonly uint salt_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.SALT_BYTE_SIZE / 3.0);
    readonly uint hash_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.HASH_BYTE_SIZE / 3.0);

    private SQLiteConnection connection;

    private Database()
    {
        connection = new SQLiteConnection(@"Data Source=C:\Users\Jordan\Documents\Visual Studio 2013\Projects\SecurePoker\SecurePoker\bin\Debug\secure_poker.db; Version=3");
        connection.Open();
        string sql = "";
        sql += "CREATE TABLE IF NOT EXISTS USER_ACCOUNT_INFO (";
        sql += "    USERID      VARCHAR(" + userid_column_length + ") PRIMARY KEY,";
        sql += "    SALT        CHAR(" + salt_column_length + ") NOT NULL,";
        sql += "    HASH        CHAR(" + hash_column_length + ") NOT NULL";
        sql += ");";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    public bool getHash(string userid, out string hash)
    {
        string sql;
        SQLiteCommand command;

        sql = "SELECT SALT, HASH FROM USER_ACCOUNT_INFO WHERE USERID=\"" + userid + "\";";
        command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows)
        {
            hash = "";
            return false;
        }
        else
        {
            reader.Read();
            hash = PasswordHash.PasswordHash.PBKDF2_ITERATIONS + ":" + reader["SALT"] + ":" + reader["HASH"];
            return true;
        }
    }

    /// <summary>
    /// Adds (registers) a new user to the database.
    /// </summary>
    /// <param name="userid">new user's user ID</param>
    /// <param name="password">new user's password</param>
    /// <returns>
    ///     true if the new user was successfully added to the database,
    ///     false if they were not
    /// </returns>
    public bool insertUser(string userid, string password)
    {
        if (userid.Length > userid_column_length)
        {
            throw new ArgumentException("User ID length may not exceed " + userid_column_length + " characters.", "username");
        }

        string sql;
        SQLiteCommand command;

        sql = "SELECT * FROM USER_ACCOUNT_INFO WHERE USERID=\"" + userid + "\" LIMIT 1;";
        command = new SQLiteCommand(sql, connection);
        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                throw new Exception("User ID " + userid + " is already in use.");
            }
        }

        string[] hash_salt_iter = PasswordHash.PasswordHash.CreateHash(password).Split(new char[] { ':' });

        sql = "INSERT INTO USER_ACCOUNT_INFO (USERID, SALT, HASH) VALUES (\"" + userid + "\", \"" + hash_salt_iter[1] + "\", \"" + hash_salt_iter[2] + "\");";
        command = new SQLiteCommand(sql, connection);
        
        return command.ExecuteNonQuery() == 1;
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

    public static void _Main(string[] args)
    {
        uint i = 0;
        string userid = "jar2119";
        while (true)
        {
            try
            {
                bool result = Database.Instance.insertUser(userid, "notarealorgoodpassword");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userid = "jar2119" + i++;
            }
        }

        Console.WriteLine(Server.Instance.authenticate(userid, "notarealorgoodpassword"));
        Console.WriteLine(Server.Instance.authenticate(userid, "wrongpassword"));
    }
}