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
using static VisualBasicCodeAnalysis.Analyzer.FullThirdLevelAnalyzer;

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

        public void CreateNewDatabase()
        {
            dbFileName = @"D:\workdb.db";
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
                    NonExecuteQuery("CREATE TABLE Func_Func_Link (id_parent INTEGER, id_child INTEGER);");
                    NonExecuteQuery("CREATE TABLE Line (id_func INTEGER, id_line_in_func INTEGER, id_global INTEGER, begin INTEGER, size INTEGER, is_useful INTEGER, fileName TEXT, funcName TEXT );");
                    NonExecuteQuery("CREATE TABLE Line_line (id_line1 INTEGER, id_line2 INTEGER, flag_return INTEGER, id_func INTEGER);");

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

        public void NonExecuteQueryFromLinSection()
        {
            try
            {
                dbFileName = @"D:\workdb.db";
                SQLiteConnection con = new SQLiteConnection();
                if (File.Exists(dbFileName))
                {
                    // SQLiteConnection.CreateFile(dbFileName);

                    sqlFactory = (SQLiteFactory) DbProviderFactories.GetFactory("System.Data.SQLite");
                    using (connection = (SQLiteConnection) sqlFactory.CreateConnection())
                    {
                        connection.ConnectionString = "Data Source = " + dbFileName;
                        connection?.Open();


                        int i = 1;
                        foreach (var linSection in LinearSectionsList)
                        {
                            SQLiteCommand command = new SQLiteCommand(connection)
                            {
                                CommandText =
                                    "INSERT INTO Line (id_func, id_line_in_func, id_global, begin, size, is_useful," +
                                    " fileName, funcName) VALUES (@id_func, @id_line_in_func, @id_global, @begin, @size, @is_useful, @fileName, @funcName)"
                            };
                            command.Parameters.Add(new SQLiteParameter("@id_func", linSection.Value.FunctionID));
                            command.Parameters.Add(new SQLiteParameter("@id_line_in_func", linSection.Value.LocalID));
                            command.Parameters.Add(new SQLiteParameter("@id_global", linSection.Value.GlobalID));
                            command.Parameters.Add(new SQLiteParameter("@begin", linSection.Value.StartLine));
                            command.Parameters.Add(new SQLiteParameter("@size", linSection.Value.Size));
                            command.Parameters.Add(new SQLiteParameter("@is_useful", linSection.Value.IsUsing));
                            command.Parameters.Add(new SQLiteParameter("@fileName", linSection.Value.FileName));
                            command.Parameters.Add(new SQLiteParameter("@funcName", linSection.Value.FuncName));
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                            i++;
                        }
                        connection.Close();
                    }
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
