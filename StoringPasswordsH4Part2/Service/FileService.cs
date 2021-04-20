using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using StoringPasswordsH4Part2.DTO;

namespace StoringPasswordsH4Part2.Service
{

    class FileService
    {

        /// <summary>
        /// Pulls the database config from the file system to be used for the authentication towards the database
        /// </summary>
        /// <returns>the object with infomation the required data values in a connection string</returns>
        public static MysqlConfigurationObject GetDatabaseConfig()
        {
            return new MysqlConfigurationObject("localhost", "StoringPasswordsH4", "StoringPasswordsH4", "StoringPasswordsH4");
        }
    }
}
