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
            command.Prepare();
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
            command.Prepare();
            command.ExecuteNonQuery();
        }
    }

    public string getHash(string userid)
    {
        using (SQLiteCommand command = new SQLiteCommand(connection))
        {
            command.CommandText = "SELECT SALT, HASH FROM USER_ACCOUNTS WHERE USERID=\"UserID\";";
            command.Prepare();
            SQLiteParameter userid_param = command.CreateParameter();
            userid_param.ParameterName = "@UserID";
            command.Parameters.Add(userid_param);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return "";
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
    public void addUser(string userid, string password)
    {
        if (userid.Length > userid_column_length)
        {
            throw new ArgumentException("User ID length may not exceed " + userid_column_length + " characters.", "username");
        }

        string sql;
        SQLiteCommand command;

        sql = "SELECT * FROM USER_ACCOUNTS WHERE USERID=\"" + userid + "\" LIMIT 1;";
        command = new SQLiteCommand(sql, connection);
        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                throw new Exception("User ID " + userid + " is already in use.");   // maybe some derived exception type, perhaps a custom one, should be thrown here
            }
        }

        string[] hash_salt_iter = PasswordHash.PasswordHash.CreateHash(password).Split(new char[] { ':' });

        sql = "INSERT INTO USER_ACCOUNTS (USERID, SALT, HASH) VALUES (\"" + userid + "\", \"" + hash_salt_iter[1] + "\", \"" + hash_salt_iter[2] + "\");";
        command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();

        sql = "INSERT INTO BANK_ACCOUNTS (USERID, BALANCE) VALUES (\"" + userid + "\", 0);"; // should the initial account balance be zero, some provided value, or some generated value?
        command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Removes (unregisters) identified user from the database.
    /// </summary>
    /// <param name="userid">ID of the user to remove</param>
    public void removeUser(string userid)
    {
        string sql = "DELETE FROM BANK_ACCOUNTS WHERE USERID=\"" + userid + "\";";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();

        sql = "DELETE FROM USER_ACCOUNTS WHERE USERID=\"" + userid + "\";";
        command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    public static void Main(string[] args)
    {
        string orig_id = "jar2119";
        string userid = orig_id;
        string password = "notarealorgoodpassword";
        string wrongpassword = "wrongpassword";
        Database db = Database.Instance;

        uint i = 0;
        while (true)
        {
            try
            {
                db.addUser(userid, password);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                userid = orig_id + i++;
            }
        }

        try
        {
            db.removeUser(userid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        Server server = Server.Instance;
        Console.WriteLine("Authenticating password: " + (server.authenticate(userid, password) == true ? "Success!" : "FAIL"));
        Console.WriteLine("Authenticating wrong password: " + (server.authenticate(userid, wrongpassword) == false ? "Success!" : "FAIL"));

        userid += i++;
        try
        {
            Console.WriteLine("Attempting to remove non-existing user " + userid + ".");
            db.removeUser(userid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}