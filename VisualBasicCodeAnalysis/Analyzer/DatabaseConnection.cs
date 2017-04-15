using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBasicCodeAnalysis;

namespace VisualBasicCodeAnalysis.Analyzer
{
    public class DatabaseConnection
    {
        private string dbFileName;
        private SQLiteFactory sqlFactory;
        private SQLiteConnection connection;
        public void StartConnection()
        {
            dbFileName = @"E:\testdb.db";
            SQLiteConnection con = new SQLiteConnection();
            if (!File.Exists(dbFileName))
            {
               SQLiteConnection.CreateFile(dbFileName); 
            }
            sqlFactory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            using (connection = (SQLiteConnection) sqlFactory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + dbFileName;
                connection.Open();
                NonExecuteQuery("CREATE TABLE example (id INTEGER PRIMARY KEY, value TEXT);");
                connection.Close();
            }
        }

        public void CreateOrUpdateDatabase()
        {
            dbFileName = @"E:\testdb.db";
            SQLiteConnection con = new SQLiteConnection();
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }
            sqlFactory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            using (connection = (SQLiteConnection)sqlFactory?.CreateConnection())
            {
                if (connection != null)
                {
                    connection.ConnectionString = "Data Source = " + dbFileName;
                    connection.Open();
                    NonExecuteQuery(
                        "CREATE TABLE Function (id INTEGER PRIMARY KEY, name TEXT, ret_type TEXT, type_param TEXT, count_line INTEGER," + 
                        "def_file TEXT, def_offset INTEGER, dec_file TEXT, is_useful INTEGER, udb_id INTEGER);");
                    NonExecuteQuery("CREATE TABLE gVar (id INTEGER PRIMARY KEY, name TEXT, type TEXT, def_file TEXT, def_offset INTEGER);");
                    NonExecuteQuery("CREATE TABLE Func_Func_Link (id_parent INGETER, id_child INTEGER);");
                    connection.Close();
                }
            }
        }

        public int NonExecuteQuery(string query)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(this.connection);
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                
                return 0;
            }
        }

        public void NonExecuteQueryForInsertFunc(List<VisualBasicAnalysis.FuncStruct> functionStruct)
        {
            try
            {
                dbFileName = @"E:\testdb.db";
                SQLiteConnection con = new SQLiteConnection();
                if (!File.Exists(dbFileName))
                {
                   // SQLiteConnection.CreateFile(dbFileName);
                }
                sqlFactory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                using (connection = (SQLiteConnection)sqlFactory.CreateConnection())
                {
                    connection.ConnectionString = "Data Source = " + dbFileName;
                    connection.Open();
                    
              
                int i = 1;
                foreach (var func in functionStruct)
                {
                    SQLiteCommand command = new SQLiteCommand(connection)
                    {
                        CommandText = "INSERT INTO Function (id, name, ret_type, type_param, count_line, def_file," +
                                      " def_offset, dec_file) VALUES (@id,@name,@return_type,@type_param,@count_line,@def_file,@def_offset,@dec_file)"
                    };
                    command.Parameters.Add(new SQLiteParameter("@id", i) );
                    command.Parameters.Add(new SQLiteParameter("@name", func.Name));
                    command.Parameters.Add(new SQLiteParameter("@return_type", func.ReturnType));
                    command.Parameters.Add(new SQLiteParameter("@type_param", func.TypeParam));
                    command.Parameters.Add(new SQLiteParameter("@count_line", func.CountLine));
                    command.Parameters.Add(new SQLiteParameter("@def_file", func.DefFile));
                    command.Parameters.Add(new SQLiteParameter("@def_offset", func.DefOffset));
                    command.Parameters.Add(new SQLiteParameter("@dec_file", func.DefFile));
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    i++;
                }
                    connection.Close();
                }

            }
            catch (Exception e)
            {

                
            }
        }

        public void NonExecuteQueryForInsertFuncToFunc(List<VisualBasicAnalysis.FuncToFuncLinkStruct> funcToFunc)
        {
            try
            {
                dbFileName = @"E:\testdb.db";
                SQLiteConnection con = new SQLiteConnection();
                if (!File.Exists(dbFileName))
                {
                    // SQLiteConnection.CreateFile(dbFileName);
                }
                sqlFactory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                using (connection = (SQLiteConnection)sqlFactory.CreateConnection())
                {
                    connection.ConnectionString = "Data Source = " + dbFileName;
                    connection.Open();


                    
                    foreach (var func in funcToFunc)
                    {
                        SQLiteCommand command = new SQLiteCommand(connection)
                        {
                            CommandText = "INSERT INTO Func_Func_Link (id_parent, id_child ) VALUES (@id_parent,@id_child)"
                        };
                        command.Parameters.Add(new SQLiteParameter("@id_parent", func.ParentFuncId ));
                        command.Parameters.Add(new SQLiteParameter("@id_child", func.ChildFuncId));
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                       
                    }

                    foreach (var function in VisualBasicAnalysis.FunctionList)
                    {
                        SQLiteCommand command = new SQLiteCommand(connection)
                        {
                            CommandText = "INSERT INTO Function (id, name) VALUES (@id,@name)"
                        };
                        command.Parameters.Add(new SQLiteParameter("@id", function.Key));
                        command.Parameters.Add(new SQLiteParameter("@name", function.Value.Name));
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    
                    connection.Close();
                }

            }
            catch (Exception e)
            {


            }
        }

        public void NonExecuteQueryForInsertVar(List<VisualBasicAnalysis.VarStruct> varStruct)
        {
            try
            {
                dbFileName = @"E:\testdb.db";
                SQLiteConnection con = new SQLiteConnection();
                if (!File.Exists(dbFileName))
                {
                    // SQLiteConnection.CreateFile(dbFileName);
                }
                sqlFactory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                using (connection = (SQLiteConnection)sqlFactory.CreateConnection())
                {
                    connection.ConnectionString = "Data Source = " + dbFileName;
                    connection.Open();


                    int i = 1;
                    foreach (var varS in varStruct)
                    {
                        SQLiteCommand command = new SQLiteCommand(connection)
                        {
                            CommandText = "INSERT INTO gVar (id, name, type, def_file," +
                                          " def_offset) VALUES (@id,@name,@type,@def_file,@def_offset)"
                        };
                        command.Parameters.Add(new SQLiteParameter("@id", varS.Id));
                        command.Parameters.Add(new SQLiteParameter("@name", varS.Name));
                        command.Parameters.Add(new SQLiteParameter("@type", varS.Type));
                        command.Parameters.Add(new SQLiteParameter("@def_file", varS.DefFile));
                        command.Parameters.Add(new SQLiteParameter("@def_offset", varS.DefOffset));
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                        i++;
                    }
                    connection.Close();
                }

            }
            catch (Exception e)
            {


            }
        }

        public SQLiteDataReader Query(string query)
        {
            SQLiteCommand command = new SQLiteCommand(this.connection);
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
        }
    }
}
