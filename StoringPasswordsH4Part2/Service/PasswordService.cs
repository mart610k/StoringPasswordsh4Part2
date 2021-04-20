using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using StoringPasswordsH4Part2.DTO;
using System.Linq;
using MySql.Data.MySqlClient;



namespace StoringPasswordsH4Part2.Service
{
    class PasswordService
    {
        RNGCryptoServiceProvider rngSecure = new RNGCryptoServiceProvider();
        MysqlConfigurationObject mysql;
        int WorkCount { get; set; }

        /// <summary>
        /// Gets the mysql connection settings and the amount of work there should be put in each time a hash is calculated
        /// </summary>
        public PasswordService()
        {
            mysql = FileService.GetDatabaseConfig();

            WorkCount = 1000;
        }

        /// <summary>
        /// Registers a user from an ID and password, handles the whole situation from start to finish
        /// </summary>
        /// <param name="userId">Username of the user</param>
        /// <param name="plaintextPass">plain text of the password</param>
        /// <returns>if the registration was sucessfull</returns>
        public bool RegisterUser(string userId, string plaintextPass)
        {
            UserPasswordObject userPasswordObject = HashPassword(plaintextPass);

            return CreateUserAndPassword(userId, userPasswordObject);
        }

        /// <summary>
        /// Create the user and saves the password, salt and user id on the database
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="userPassword">Object consisting of salt and password</param>
        /// <returns>if the action was sucessfull</returns>
        public bool CreateUserAndPassword(string userId, UserPasswordObject userPassword)
        {
            MySqlConnection conn = new MySqlConnection(mysql.ConnectionString);

            MySqlCommand comm = conn.CreateCommand();

            bool success = false;

            try
            {
                conn.Open();
                comm.CommandText = "INSERT INTO userpassword(ID,Password,Salt) value (@ID,@Password,@Salt);";


                comm.Parameters.AddWithValue("@ID", userId);
                comm.Parameters.AddWithValue("@Password", Convert.ToBase64String(userPassword.HashedPassword));
                comm.Parameters.AddWithValue("@Salt", Convert.ToBase64String(userPassword.Salt));


                comm.ExecuteNonQuery();
                success = true;
            }
            catch
            {

            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return success;
        }


        /// <summary>
        /// Hashes the incoming password, if incoming bytes in salt it will use those bytes as part of the hash
        /// </summary>
        /// <param name="plaintextPassword">password in plain text to encrypt</param>
        /// <param name="salt">if salt is provided it will hash using the salt, otherwise it will generate a new salt</param>
        /// <returns>the password and salt used for the hashed password</returns>
        public UserPasswordObject HashPassword(string plaintextPassword, byte[] salt = null)
        {
            int saltSize = 16;
            if(salt == null)
            {
                salt = GenerateSalt(saltSize);
            }

            byte[] hashedBytes;

            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(plaintextPassword, salt))
            {
                hashGenerator.IterationCount = WorkCount;
                hashedBytes = hashGenerator.GetBytes(64);
            }

            return new UserPasswordObject(hashedBytes, salt);
        }

        /// <summary>
        /// Generates a salt using the secure random generator
        /// </summary>
        /// <param name="saltSize">The salt size to generate</param>
        /// <returns>Filled byte array with random bytes</returns>
        private byte[] GenerateSalt(int saltSize)
        {
            byte[] saltBytes = new byte[saltSize];

            rngSecure.GetBytes(saltBytes);

            return saltBytes;
        }

        /// <summary>
        /// verifies the password coming in to the hashed password and salt incoming in the method
        /// </summary>
        /// <param name="plainPassword">the plain password of the user</param>
        /// <param name="passwordObject">the hashed version of the password and salt</param>
        /// <returns>if the password and hashed password was equal</returns>
        public bool VerifyPassword(string plainPassword,UserPasswordObject passwordObject)
        {
            if(passwordObject.HashedPassword.Length == HashPassword(plainPassword, passwordObject.Salt).HashedPassword.Length &&
                passwordObject.HashedPassword.SequenceEqual(HashPassword(plainPassword, passwordObject.Salt).HashedPassword))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the user credentials twoards the database returns if the user was verified
        /// </summary>
        /// <param name="userID">the users ID</param>
        /// <param name="plainPassword">Plain password of6 the user</param>
        /// <returns>If the users credentials was verified</returns>
        public bool CheckUserCredentials(string userID, string plainPassword)
        {
            UserPasswordObject userPasswordObject = GetUserPassword(userID);

            if (userPasswordObject == null)
            {
                return false;
            }
            bool verifyPasswordResult = VerifyPassword(plainPassword, userPasswordObject);

            if (verifyPasswordResult)
            {
                return true;
            }
            else
            {
                return false;
            }
                   
        }

        /// <summary>
        /// Gets a users hashed password
        /// </summary>
        /// <param name="userId">The user ID to get the hashed password</param>
        /// <returns>null if the user does not exist otherwise the data existing on the database</returns>
        public UserPasswordObject GetUserPassword(string userId)
        {
            MySqlConnection conn = new MySqlConnection(mysql.ConnectionString);

            MySqlCommand comm = conn.CreateCommand();

            UserPasswordObject userPasswordObject = null;

            try
            {
                conn.Open();

                comm.CommandText = "SELECT Password,Salt FROM userpassword WHERE ID = @ID LIMIT 1;";


                comm.Parameters.AddWithValue("@ID", userId);

                MySqlDataReader reader = comm.ExecuteReader();

                while(reader.Read()){
                    userPasswordObject = new UserPasswordObject(
                        Convert.FromBase64String(reader.GetString("Password")),
                        Convert.FromBase64String(reader.GetString("Salt"))
                        );
                }
                

            }
            catch
            {

            }
            finally
            {
                if(conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return userPasswordObject;
        }
    }
}
