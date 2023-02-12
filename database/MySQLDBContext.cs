using System;
using MySqlConnector;

namespace ZiekenhuisDataGenerator.database
{
    public class MySQLDBContext
    {
        private MySqlConnection connection;
        private static MySQLDBContext? instance = null;

        private MySqlTransaction? transaction = null;

        public MySQLDBContext()
        {
            Console.WriteLine("Opening connection");
            connection = new MySqlConnection();
            // FIXME: get configuration information from project instead of hardcoding it.
            connection.ConnectionString =
                "Server=localhost;Database=ziekenhuisdata;Uid=ziekenhuis;Pwd=ziekenhuis";
            try
            {
                connection.Open();
                Console.WriteLine("===============================================");
                Console.WriteLine("- Database: {0}", connection.Database);
                Console.WriteLine("- Version:  {0}", connection.ServerVersion);
                Console.WriteLine("===============================================");
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static MySQLDBContext Instance
        {
            get
            {
                if (instance == null)
                {
                    Console.WriteLine("*** Creating singleton instance!");
                    instance = new MySQLDBContext();
                }

                return instance;
            }
        } //Instance getter

        public MySqlCommand ConstructStatement(string sql)
        {
            if (this.connection == null)
            {
                throw new Exception("?? Connection not set");
            }

            return new MySqlCommand(sql, instance?.connection);
        }


        public void StartTransaction()
        {
            this.transaction = this.connection.BeginTransaction();
        }

        public void FinishTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
            }
        }
    }
}