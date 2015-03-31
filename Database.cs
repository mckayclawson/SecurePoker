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

    /*
     * At some point we can put these files in a better, permanent location, and just change the strings to reflect that.
     * Should these string be hard coded?
     */
    private static string connectionString = @"Data Source=C:\Data\Users\Jordan\Documents\School\2145\CSCI-455-01\SECURE_POKER_DB; Version=3";
    private static string sqlAbsolutePath = @"C:\Data\Users\Jordan\Documents\School\2145\CSCS-455-01\users.sql";
    private SQLiteConnection connection;

    private Database()
    {
        connection = new SQLiteConnection(connectionString);
        connection.Open();

        string sql = string.Empty;
        try
        {
            /*
             * This is a major security problem: users.sql needs to be protected somehow. Otherwise, someone could simply modify it and do whatever they wanted to.
             * A simple example would be removing "IF NOT EXISTS" from the CREATE TABLE statement to destroy all existing data.
             */
            string[] lines = File.ReadAllLines(sqlAbsolutePath);
            foreach (string line in lines)
            {
                sql += line;
            }
        }
        catch (FileNotFoundException ex)
        {
            /*
             * Now I'm thinking that since the CREATE TABLE statement for the USER_ACCT_INFO table is so short, maybe it should be hard coded.
             * Otherwise, how should this exception be handled?
             */
        }

        if (sql.Length > 0)
        {
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
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
        const int username_column_length = 16;
        if (username.Length > username_column_length)
        {
            throw new ArgumentException("Username length may not exceed " + username_column_length + " characters.", "username");
        }

        // TODO: generate a salt to store in the record for the new user
        // TODO: compute the hash of the password plus the salt to store in the
        // record
        // TODO: generate unique user ID
        userid = "null";
        char[] separator = new char[1];
        separator[0] = ':';
        string[] hash_salt_iter = PasswordHash.PasswordHash.CreateHash(password).Split(separator);

        string sql = "SELECT USERID FROM USER_ACCT_INFO WHERE USERID LIKE " + username + "%";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        userid = username;

        const int userid_column_length = 32;

        uint i = 0;
        while (reader.Read() && userid.Length < userid_column_length)
        {
            if (reader["USERID"].Equals(userid))
            {

            }
        }

        sql = "INSERT INTO USER_ACCT_INFO (USERID, USERNAME, SALT, HASH) VALUES ";

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