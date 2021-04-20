using System;
using System.Collections.Generic;
using System.Text;

namespace StoringPasswordsH4Part2.DTO
{
    /// <summary>
    /// Maintains the passwords and salt in one location for use in other parts of the code.
    /// </summary>
    class UserPasswordObject
    {
        public byte[] HashedPassword { get; private set; }
        public byte[] Salt { get; private set; }

        public UserPasswordObject(byte[] hashedPassword, byte[] salt)
        {
            HashedPassword = hashedPassword;

            Salt = salt;
        }

    }
}
