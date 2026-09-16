using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using Protocol;

namespace DBCacheServer
{
    public partial class MysqlAcess
    {
        private static MysqlAcess myAcess = null;
        private static Mutex m_mutex = null;    //多線程時互斥鎖
        private MySqlConnection dbConnection;

        private string connstr = "";
        private string _sql = "";
        private MySqlCommand m_cmd = null;
        private MySqlDataReader m_reader = null;

        static string host = "";
        static string id = "";
        static string dbpwd = "";
        static string database = "";
        static string writeConnection = "";
        private static readonly object _selectLock = new object();

        private MysqlAcess()
        {
            try
            {
                string connectionFile = Environment.GetEnvironmentVariable("DB_CACHE_SQL_CONNECTION_FILE");
                if (string.IsNullOrWhiteSpace(connectionFile))
                    connectionFile = "SqlConnection.txt";

                using (StreamReader sr = new StreamReader(connectionFile))
                {
                    String text;

                    while ((text = sr.ReadLine()) != null)
                    {
                        string[] words = text.Split(':');

                        switch (words[0])
                        {
                            case "mysqlConnectionWrite":
                            case "mysqlConnection":
                                writeConnection = words.Length > 1 ? words[1] : "";
                                break;
                            case "DBIP":
                                host = words[1];
                                break;
                            case "DBID":
                                id = words[1];
                                break;
                            case "DBPWD":
                                dbpwd = words[1];
                                break;
                            case "DBDataBase":
                                database = words[1];
                                break;
                        }
                    }

                    sr.Close();
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteLine("The file could not be read:");
                MyConsole.WriteLine(e.Message);
            }

            try
            {

                m_mutex = new Mutex();
                //this.connstr = "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";SslMode=None" + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;";
                this.connstr = string.IsNullOrWhiteSpace(writeConnection)
                    ? "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";SslMode=Disabled" + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;"
                    : writeConnection;
                dbConnection = new MySqlConnection(this.connstr);
                //開啟sql連線
                dbConnection.Open();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MyConsole.WriteLine("無法連線到伺服器");
                        break;
                    case 1:
                        MyConsole.WriteLine("使用者帳號密碼錯誤");
                        break;
                }
            }
        }

        static public MysqlAcess GetInstance()
        {
            if (myAcess == null)
            {
                myAcess = new MysqlAcess();
            }
            return myAcess;
        }

        public T ExecuteInTransaction<T>(Func<MySqlConnection, MySqlTransaction, T> action)
        {
            using (var connection = new MySqlConnection(connstr))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        T result = action(connection, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Each statement runs in its own autocommit transaction, so this method can never
        // observe an uncommitted outbox row from the deposit transaction.
        public T ExecuteInReadCommitted<T>(Func<MySqlConnection, T> action)
        {
            using (var connection = new MySqlConnection(connstr))
            {
                connection.Open();
                using (var command = new MySqlCommand("SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED", connection))
                    command.ExecuteNonQuery();
                return action(connection);
            }
        }

        public void ExecuteInReadCommitted(Action<MySqlConnection> action)
        {
            ExecuteInReadCommitted(connection =>
            {
                action(connection);
                return 0;
            });
        }

        private void ReConnect()
        {
            MyConsole.WriteLine("MysqlAcess Reconnect");

            try
            {

                m_mutex = new Mutex();
                //this.connstr = "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";SslMode=None" + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;";
                this.connstr = "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";SslMode=Disabled" + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;";
                dbConnection = new MySqlConnection(this.connstr);
                //開啟sql連線
                dbConnection.Open();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MyConsole.WriteLine("無法連線到伺服器");
                        break;
                    case 1:
                        MyConsole.WriteLine("使用者帳號密碼錯誤");
                        break;
                }
            }
        }

        /// <summary>
        /// 獲取主鍵
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        private string getPrimary(string tablename = "")
        {
            string key = "";

            reOpen();

            try
            {
                m_cmd = new MySqlCommand("SHOW COLUMNS FROM " + tablename, dbConnection);
                m_reader = m_cmd.ExecuteReader();

                //如果有數據就輸出
                if (m_reader.HasRows)
                {
                    //逐行讀取數據
                    while (m_reader.Read())
                    {
                        string zizeng = m_reader.GetString("Extra");
                        if (zizeng == "auto_increment")
                        {
                            key = m_reader.GetString("Field");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
            }
            finally
            {
                this.closeHandle();
            }
            return key;
        }

        /// <summary>
        /// 插入表單
        /// </summary>
        /// <param name="tablname"></param>
        /// <param name="updatedata"></param>
        /// <returns></returns>
        public int insert(string tablename, Dictionary<string, string> updatedata)
        {
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取更新的所有鍵
                    string field1 = "";
                    string field2 = "";

                    foreach (string key in updatedata.Keys)
                    {
                        field1 += "," + key;
                        field2 += "," + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "INSERT INTO " + tablename + " (" + field1.Trim(',') + ") VALUE (" + field2.Trim(',') + ")";
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        string prid = this.getPrimary(tablename);
                        if (prid != "")
                        {
                            var data = this.get(tablename, prid, "", prid + " desc");
                            return int.Parse(data[prid]);
                        }
                        else
                        {
                            return 1;
                        }
                    }

                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);

                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            m_mutex.WaitOne();

            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取更新的所有鍵
                    string field1 = "";
                    string field2 = "";

                    foreach (string key in updatedata.Keys)
                    {
                        field1 += "," + key;
                        field2 += "," + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "INSERT INTO " + tablename + " (" + field1.Trim(',') + ") VALUE (" + field2.Trim(',') + ")";
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        string prid = this.getPrimary(tablename);
                        if (prid != "")
                        {
                            var data = this.get(tablename, prid, "", prid + " desc");
                            return int.Parse(data[prid]);
                        }
                        else
                        {
                            return 1;
                        }
                    }

                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);

                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }

                return -1;
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            return -1;
        }

        #region 沒用到的
        /// <summary>
        /// 插入表單
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="updatedata"></param>
        /// <returns></returns>
        public int insert(string tablename, string updatedata)
        {
            
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    this._sql = "INSERT INTO " + tablename + " " + updatedata;
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    if (result == 1)
                    {
                        string prid = this.getPrimary(tablename);
                        if (prid != "")
                        {
                            var data = this.get(tablename, prid, "", prid + " desc");
                            return int.Parse(data[prid]);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess insert " + tablename);

                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    this._sql = "INSERT INTO " + tablename + " " + updatedata;
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    if (result == 1)
                    {
                        string prid = this.getPrimary(tablename);
                        if (prid != "")
                        {
                            var data = this.get(tablename, prid, "", prid + " desc");
                            return int.Parse(data[prid]);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess insert " + tablename);

                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
                return -1;
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            return -1;
        }
        #endregion

        /// <summary>
        /// 替換表單
        /// </summary>
        /// <param name="tablname"></param>
        /// <param name="updatedata"></param>
        /// <returns></returns>
        public int replace(string tablename, Dictionary<string, string> updatedata)
        {
            
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取更新的所有鍵
                    string field1 = "";
                    string field2 = "";

                    foreach (string key in updatedata.Keys)
                    {
                        field1 += "," + key;
                        field2 += "," + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "REPLACE INTO " + tablename + " (" + field1.Trim(',') + ") VALUE (" + field2.Trim(',') + ")";
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess replace " + tablename);
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
                return 0;
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            return 0;
        }

        /// <summary>
        /// 查詢表單
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fields"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> select(string tablename = "", string fields = "", string where = "", string order = "", string limit = "")
        {
            List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
            
            string _sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + (limit != "" ? (" limit " + limit) : "");
            
            try
            {
                using (var conn = new MySqlConnection(this.connstr))
                {
                    conn.Open();

                    using (var cmd = new MySqlCommand(_sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //逐行讀取數據
                            while (reader.Read())
                            {
                                Dictionary<string, string> coldata = new Dictionary<string, string>();
                                //取的所有欄位
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string filedname = reader.GetName(i).Trim();
                                    string value;
                                    if (!reader.IsDBNull(reader.GetOrdinal(filedname)))
                                    {
                                        coldata.Add(filedname, reader.GetValue(filedname).ToString());
                                        //value = m_reader.GetString(filedname);
                                    }
                                    else
                                    {
                                        coldata.Add(filedname, "");
                                        value = "";
                                    }
                                    //coldata.Add(filedname, value);
                                }
                                datalist.Add(coldata);
                            }

                            return datalist;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 你可以將錯誤記錄到檔案、console 或 log 系統
                MyConsole.WriteLine($"[Select] SQL錯誤: {ex.Message}");
                MyConsole.WriteLine(ex.StackTrace);
                // 若需要你可以回傳 null 或空集合
                return datalist;
            }

            return datalist;
            

            //lock (_selectLock)
            //{
            //    List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();

            //    this.reOpen();

            //    if (dbConnection.State == ConnectionState.Open)
            //    {

            //        this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + (limit != "" ? (" limit " + limit) : "");

            //        try
            //        {
            //            m_cmd = new MySqlCommand(this._sql, dbConnection);
            //            m_reader = m_cmd.ExecuteReader();

            //            if (m_reader.HasRows)
            //            {
            //                //逐行讀取數據
            //                while (m_reader.Read())
            //                {
            //                    Dictionary<string, string> coldata = new Dictionary<string, string>();
            //                    //取的所有欄位
            //                    for (int i = 0; i < m_reader.FieldCount; i++)
            //                    {
            //                        string filedname = m_reader.GetName(i).Trim();
            //                        string value;
            //                        if (!m_reader.IsDBNull(m_reader.GetOrdinal(filedname)))
            //                        {
            //                            coldata.Add(filedname, m_reader.GetValue(filedname).ToString());
            //                            //value = m_reader.GetString(filedname);
            //                        }
            //                        else
            //                        {
            //                            coldata.Add(filedname, "");
            //                            value = "";
            //                        }
            //                        //coldata.Add(filedname, value);
            //                    }
            //                    datalist.Add(coldata);
            //                }

            //                return datalist;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            this.errorMsg(ex);
            //            MyConsole.WriteLine("Access select " + tablename);
            //            datalist = null;
            //        }
            //        finally
            //        {
            //            this.closeHandle();
            //        }
            //    }

            //    ReConnect();

            //    this.reOpen();

            //    if (dbConnection.State == ConnectionState.Open)
            //    {

            //        this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + (limit != "" ? (" limit " + limit) : "");

            //        try
            //        {
            //            m_cmd = new MySqlCommand(this._sql, dbConnection);
            //            m_reader = m_cmd.ExecuteReader();

            //            if (m_reader.HasRows)
            //            {
            //                //逐行讀取數據
            //                while (m_reader.Read())
            //                {
            //                    Dictionary<string, string> coldata = new Dictionary<string, string>();
            //                    //取的所有欄位
            //                    for (int i = 0; i < m_reader.FieldCount; i++)
            //                    {
            //                        string filedname = m_reader.GetName(i).Trim();
            //                        string value;
            //                        if (!m_reader.IsDBNull(m_reader.GetOrdinal(filedname)))
            //                        {
            //                            coldata.Add(filedname, m_reader.GetValue(filedname).ToString());
            //                            //value = m_reader.GetString(filedname);
            //                        }
            //                        else
            //                        {
            //                            coldata.Add(filedname, "");
            //                            value = "";
            //                        }
            //                        //coldata.Add(filedname, value);
            //                    }
            //                    datalist.Add(coldata);
            //                }

            //                return datalist;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            this.errorMsg(ex);
            //            MyConsole.WriteLine("Access select " + tablename);
            //            datalist = null;
            //        }
            //        finally
            //        {
            //            this.closeHandle();
            //        }
            //    }

            //    return null;
            //}


            ////安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            //m_mutex.WaitOne();

            //this.reOpen();

            //if (dbConnection.State == ConnectionState.Open)
            //{
            //    List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
            //    this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + (limit != "" ? (" limit " + limit) : "");
            //    try
            //    {
            //        m_cmd = new MySqlCommand(this._sql, dbConnection);
            //        m_reader = m_cmd.ExecuteReader();
            //        //如果有數據就輸出
            //        if (m_reader.HasRows)
            //        {
            //            //逐行讀取數據
            //            while (m_reader.Read())
            //            {
            //                Dictionary<string, string> coldata = new Dictionary<string, string>();
            //                //取的所有欄位
            //                for (int i = 0; i < m_reader.FieldCount; i++)
            //                {
            //                    string filedname = m_reader.GetName(i).Trim();
            //                    string value;
            //                    if (!m_reader.IsDBNull(m_reader.GetOrdinal(filedname)))
            //                    {
            //                        coldata.Add(filedname, m_reader.GetValue(filedname).ToString());
            //                        //value = m_reader.GetString(filedname);
            //                    }
            //                    else
            //                    {
            //                        coldata.Add(filedname, "");
            //                        value = "";
            //                    }
            //                    //coldata.Add(filedname, value);
            //                }
            //                datalist.Add(coldata);
            //            }

            //            return datalist;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        this.errorMsg(ex);
            //        MyConsole.WriteLine("Acess select "+ tablename);
            //        datalist = null;
            //    }
            //    finally
            //    {
            //        this.closeHandle();
            //        m_mutex.ReleaseMutex();
            //    }
            //}
            //else
            //{
            //    m_mutex.ReleaseMutex();
            //}

            //ReConnect();

            ////安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            //m_mutex.WaitOne();
            //this.reOpen();

            //if (dbConnection.State == ConnectionState.Open)
            //{
            //    List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
            //    this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + (limit != "" ? (" limit " + limit) : "");
            //    try
            //    {
            //        m_cmd = new MySqlCommand(this._sql, dbConnection);
            //        m_reader = m_cmd.ExecuteReader();
            //        //如果有數據就輸出
            //        if (m_reader.HasRows)
            //        {
            //            //逐行讀取數據
            //            while (m_reader.Read())
            //            {
            //                Dictionary<string, string> coldata = new Dictionary<string, string>();
            //                //取的所有欄位
            //                for (int i = 0; i < m_reader.FieldCount; i++)
            //                {
            //                    string filedname = m_reader.GetName(i).Trim();
            //                    string value;
            //                    if (!m_reader.IsDBNull(m_reader.GetOrdinal(filedname)))
            //                    {
            //                        coldata.Add(filedname, m_reader.GetValue(filedname).ToString());
            //                        //value = m_reader.GetString(filedname);
            //                    }
            //                    else
            //                    {
            //                        coldata.Add(filedname, "");
            //                        value = "";
            //                    }
            //                    //coldata.Add(filedname, value);
            //                }
            //                datalist.Add(coldata);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        this.errorMsg(ex);
            //        MyConsole.WriteLine("Acess select " + tablename);
            //        datalist = null;
            //    }
            //    finally
            //    {
            //        this.closeHandle();
            //        m_mutex.ReleaseMutex();
            //    }
            //    return datalist;
            //}
            //else
            //{
            //    m_mutex.ReleaseMutex();
            //}

            //return null;
        }

        public List<Dictionary<string, string>> selectdesc(string tablename = "", string fields = "", string where = "", string order = "", string limit = "")
        {
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
                this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + " DESC " + (limit != "" ? (" limit " + limit) : "");
                try
                {
                    m_cmd = new MySqlCommand(this._sql, dbConnection);
                    m_reader = m_cmd.ExecuteReader();
                    //如果有數據就輸出
                    if (m_reader.HasRows)
                    {
                        //逐行讀取數據
                        while (m_reader.Read())
                        {
                            Dictionary<string, string> coldata = new Dictionary<string, string>();
                            //取的所有欄位
                            for (int i = 0; i < m_reader.FieldCount; i++)
                            {
                                string filedname = m_reader.GetName(i).Trim();
                                string value;
                                if (!m_reader.IsDBNull(m_reader.GetOrdinal(filedname)))
                                {
                                    coldata.Add(filedname, m_reader.GetValue(filedname).ToString());
                                    //value = m_reader.GetString(filedname);
                                }
                                else
                                {
                                    coldata.Add(filedname, "");
                                    value = "";
                                }
                                //coldata.Add(filedname, value);
                            }
                            datalist.Add(coldata);
                        }

                        return datalist;
                    }
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess select " + tablename);
                    datalist = null;
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
                this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + " DESC " + (limit != "" ? (" limit " + limit) : "");
                try
                {
                    m_cmd = new MySqlCommand(this._sql, dbConnection);
                    m_reader = m_cmd.ExecuteReader();
                    //如果有數據就輸出
                    if (m_reader.HasRows)
                    {
                        //逐行讀取數據
                        while (m_reader.Read())
                        {
                            Dictionary<string, string> coldata = new Dictionary<string, string>();
                            //取的所有欄位
                            for (int i = 0; i < m_reader.FieldCount; i++)
                            {
                                string filedname = m_reader.GetName(i).Trim();
                                string value;
                                if (!m_reader.IsDBNull(m_reader.GetOrdinal(filedname)))
                                {
                                    coldata.Add(filedname, m_reader.GetValue(filedname).ToString());
                                    //value = m_reader.GetString(filedname);
                                }
                                else
                                {
                                    coldata.Add(filedname, "");
                                    value = "";
                                }
                                //coldata.Add(filedname, value);
                            }
                            datalist.Add(coldata);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess select " + tablename);
                    datalist = null;
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
                return datalist;
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            return null;
        }


        /// <summary>
        /// 更新表單
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="updatedata"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int update(string tablename, Dictionary<string, string> updatedata, string col = "", string value = "")
        {
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取要更新的所有鍵
                    string filed1 = "";
                    foreach (string key in updatedata.Keys)
                    {
                        filed1 += "," + key + "=" + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "UPDATE " + tablename + " SET " + filed1.Trim(',') + " WHERE " + col + "=" + value;

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess upata " + tablename);
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取要更新的所有鍵
                    string filed1 = "";
                    foreach (string key in updatedata.Keys)
                    {
                        filed1 += "," + key + "=" + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "UPDATE " + tablename + " SET " + filed1.Trim(',') + " WHERE " + col + "=" + value;

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess upata " + tablename);
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
                return -1;
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            return -1;
        }
        public int update(string tablename, Dictionary<string, string> updatedata, string where = "")
        {
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();

            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取要更新的所有鍵
                    string filed1 = "";
                    foreach (string key in updatedata.Keys)
                    {
                        filed1 += "," + key + "=" + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "UPDATE " + tablename + " SET " + filed1.Trim(',') + " WHERE " + where;

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess upata2 " + tablename);
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            m_mutex.WaitOne();

            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    //取要更新的所有鍵
                    string filed1 = "";
                    foreach (string key in updatedata.Keys)
                    {
                        filed1 += "," + key + "=" + updatedata[key];
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    this._sql = "UPDATE " + tablename + " SET " + filed1.Trim(',') + " WHERE " + where;

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Acess upata2 " + tablename);
                }
                finally
                {
                    this.closeHandle();
                    m_mutex.ReleaseMutex();
                }
                return -1;
            }
            else
            {
                m_mutex.ReleaseMutex();
            }

            return -1;
        }

        /// <summary>
        /// 返回一筆紀錄
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fields"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Dictionary<string, string> get(string tablename = "", string fields = "", string where = "", string order = "")
        {
            var dict = this.select(tablename, fields, where, order, "1");
            if (dict.Count > 0)
            {
                return dict[0];
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 刪除紀錄
        /// </summary>
        public int delete(string tablename, string where)
        {
            m_mutex.WaitOne();
            this.reOpen();

            try
            {
                m_cmd = dbConnection.CreateCommand();
                this._sql = "DELETE FROM " + tablename + " WHERE " + where;

                m_cmd.CommandText = this._sql;
                int result = m_cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
                MyConsole.WriteLine("Acess delete " + tablename);
            }
            finally
            {
                this.closeHandle();
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            m_mutex.WaitOne();
            this.reOpen();

            try
            {
                m_cmd = dbConnection.CreateCommand();
                this._sql = "DELETE FROM " + tablename + " WHERE " + where;

                m_cmd.CommandText = this._sql;
                int result = m_cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
                MyConsole.WriteLine("Acess delete " + tablename);
            }
            finally
            {
                this.closeHandle();
                m_mutex.ReleaseMutex();
            }
            return -1;
        }


        /************************************************************************/
        /* 如果连接已经关闭就重新连接记录集是打开状态的关闭*/
        /************************************************************************/
        private void reConn()
        {
            try
            {
                //防止網路或其他情況下連接斷開時重新連接
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection = new MySqlConnection(this.connstr);
                    dbConnection.Open();
                }
                this.closeHandle();
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
            }
        }

        private void reOpen()
        {
            try
            {
                //防止網路或其他情況下連接斷開時重新連接
                if (dbConnection.State != ConnectionState.Open)
                {
                    //dbConnection = new MySqlConnection(this.connstr);
                    dbConnection.Open();
                }
                this.closeHandle();
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
            }
        }

        private void errorMsg(Exception ex = null)
        {
            if (ex != null)
            {
                MyConsole.WriteLine(ex.Message);
                MyConsole.WriteLine(ex.StackTrace);
            }
        }

        private void closeHandle()
        {
            try
            {
                if (m_cmd != null)
                {
                    m_cmd.Dispose();
                }
                if (m_reader != null && m_reader.IsClosed == false)
                {
                    m_reader.Close();
                    m_reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
            }
            finally
            {
                m_cmd = null;
                m_reader = null;
            }
        }
    }
}
