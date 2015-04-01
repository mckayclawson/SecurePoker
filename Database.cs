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
    readonly uint username_column_length = 16;
    readonly uint salt_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.SALT_BYTE_SIZE / 3.0);
    readonly uint hash_column_length = 4u * (uint)Math.Ceiling(PasswordHash.PasswordHash.HASH_BYTE_SIZE / 3.0);

    private SQLiteConnection connection;

    private Database()
    {
        connection = new SQLiteConnection(@"Data Source=D:\Users\Jordan\Desktop\secure_poker.db; Version=3");
        connection.Open();
        string sql = "";
        sql += "CREATE TABLE IF NOT EXISTS USER_ACCOUNT_INFO (";
        sql += "    USERID      VARCHAR(" + userid_column_length + ") PRIMARY KEY,";
        sql += "    USERNAME    VARCHAR(" + username_column_length + ") NOT NULL,";
        sql += "    SALT        CHAR(" + salt_column_length + ") NOT NULL,";
        sql += "    HASH        CHAR(" + hash_column_length + ") NOT NULL";
        sql += ");";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
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
    public bool insertUser(string username, string password, out string userid)
    {
        if (username.Length > username_column_length)
        {
            throw new ArgumentException("Username length may not exceed " + username_column_length + " characters.", "username");
        }

        string sql;
        SQLiteCommand command;

        userid = username;
        uint i = 0;

        while (true)
        {
            sql = "SELECT * FROM USER_ACCOUNT_INFO WHERE USERID=\"" + userid + "\";";
            command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                userid = username + i++;
            }
            else if (userid.Length <= userid_column_length)
            {
                break;
            }
        }

        string[] hash_salt_iter = PasswordHash.PasswordHash.CreateHash(password).Split(new char[] { ':' });

        sql = "INSERT INTO USER_ACCOUNT_INFO (USERID, USERNAME, SALT, HASH) VALUES (\"" + userid + "\", \"" + username + "\", \"" + hash_salt_iter[1] + "\", \"" + hash_salt_iter[2] + "\");";
        System.Console.WriteLine("SQL Statement: " + sql);
        command = new SQLiteCommand(sql, connection);
        
        if (command.ExecuteNonQuery() == 1)
        {
            return true;
        }
        else
        {
            userid = "";
            return false;
        }
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
        string userid;
        bool result = Database.Instance.insertUser("jar2119", "notarealorgoodpassword", out userid);
        if (result)
        {
            Console.WriteLine("record inserted");
            Console.WriteLine("unique userid: " + userid);
        }
        else
        {
            Console.WriteLine("record not inserted");
            if (userid.Equals(""))
            {
                Console.WriteLine("userid is empty string as expected");
            }
            else
            {
                Console.WriteLine("userid is not empty string");
                Console.WriteLine("unexpected value: " + userid);
            }
        }
    }
}