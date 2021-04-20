using System;
using System.Collections.Generic;
using System.Text;

namespace StoringPasswordsH4Part2.DTO
{
    class MysqlConfigurationObject
    {
        public string Server { private set; get; }

        public string Database { private set; get; }

        public string Uid { private set; get; }

        public string Pwd { private set; get; }

        /// <summary>
        /// Uses the other properties to build up a connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3};",
                    Server,
                    Database,
                    Uid,
                    Pwd);
            }
        }

        public MysqlConfigurationObject(string server, string database, string uid, string pwd)
        {
            Server = server;
            Database = database;
            Uid = uid;
            Pwd = pwd;
        }
    }
}
