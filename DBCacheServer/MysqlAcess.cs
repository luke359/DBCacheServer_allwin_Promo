using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
        private static readonly object _selectLock = new object();

        private MysqlAcess()
        {
            try
            {
                using (StreamReader sr = new StreamReader("SqlConnection.txt"))
                {
                    String text;

                    while ((text = sr.ReadLine()) != null)
                    {
                        string[] words = text.Split(':');

                        switch (words[0])
                        {
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

        #region 參數化 Insert／Update／Select

        /// <summary>
        /// 以參數化查詢新增一列。表名與欄位名只允許識別字並加上反引號；值綁定為 <c>@set_欄名</c>，不拼進 SQL。
        /// 沿用既有連線與 Mutex；失敗時記錄後重連再試一次，第二次仍失敗則擲出例外。
        /// </summary>
        /// <param name="tableName">資料表名稱。</param>
        /// <param name="data">要寫入的欄位與已格式化字串值。</param>
        /// <returns>影響列數與 <see cref="MySqlCommand.LastInsertedId"/>。</returns>
        public MysqlParameterizedWriteResult InsertParameterized(string tableName, Dictionary<string, string> data)
        {
            ValidateSqlIdentifier(tableName, nameof(tableName));
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count == 0)
            {
                throw new ArgumentException("寫入資料字典不得為空。", nameof(data));
            }

            ValidateColumnMap(data, nameof(data));

            return ExecuteWithMutexRetry("InsertParameterized", tableName, delegate
            {
                return ExecuteInsertParameterized(tableName, data);
            });
        }

        /// <summary>
        /// 以參數化查詢更新列。條件全部為 AND 與相等。值綁定為 <c>@set_欄名</c>／<c>@where_欄名</c>，不拼進 SQL。
        /// </summary>
        /// <param name="tableName">資料表名稱。</param>
        /// <param name="data">要更新的欄位與已格式化字串值。</param>
        /// <param name="where">相等條件字典；不可為 null 或空。</param>
        /// <returns>影響列數。</returns>
        public long UpdateParameterized(string tableName, Dictionary<string, string> data, Dictionary<string, string> where)
        {
            ValidateSqlIdentifier(tableName, nameof(tableName));
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count == 0)
            {
                throw new ArgumentException("寫入資料字典不得為空。", nameof(data));
            }

            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (where.Count == 0)
            {
                throw new ArgumentException("Update 操作必須提供 where 條件。", nameof(where));
            }

            ValidateColumnMap(data, nameof(data));
            ValidateColumnMap(where, nameof(where));

            return ExecuteWithMutexRetry("UpdateParameterized", tableName, delegate
            {
                return ExecuteUpdateParameterized(tableName, data, where);
            });
        }

        /// <summary>
        /// 以參數化查詢讀取列。條件全部為 AND 與相等。值綁定為 <c>@where_欄名</c>，不拼進 SQL。
        /// 沿用既有連線與 Mutex，與 Insert／Update 共用同一把鎖。
        /// </summary>
        /// <param name="tableName">資料表名稱。</param>
        /// <param name="fields">要查詢的欄位清單；null 或空則查詢 <c>*</c>。</param>
        /// <param name="where">相等條件字典；null 或空則不加 WHERE。</param>
        /// <returns>每一列一個欄位名對字串的字典；資料庫 NULL 為空字串。沒有資料時為空清單。</returns>
        public List<Dictionary<string, string>> SelectParameterized(string tableName, IReadOnlyList<string> fields, Dictionary<string, string> where)
        {
            ValidateSqlIdentifier(tableName, nameof(tableName));
            ValidateFieldList(fields, nameof(fields));
            if (where != null)
            {
                ValidateColumnMap(where, nameof(where));
            }

            return ExecuteWithMutexRetry("SelectParameterized", tableName, delegate
            {
                return ExecuteSelectParameterized(tableName, fields, where);
            });
        }

        private MysqlParameterizedWriteResult ExecuteInsertParameterized(string tableName, Dictionary<string, string> data)
        {
            m_cmd = dbConnection.CreateCommand();
            StringBuilder columns = new StringBuilder();
            StringBuilder parameters = new StringBuilder();
            bool first = true;
            foreach (KeyValuePair<string, string> pair in data)
            {
                if (!first)
                {
                    columns.Append(", ");
                    parameters.Append(", ");
                }

                first = false;
                columns.Append('`').Append(pair.Key).Append('`');
                string parameterName = "@set_" + pair.Key;
                parameters.Append(parameterName);
                m_cmd.Parameters.AddWithValue(parameterName, pair.Value);
            }

            this._sql = "INSERT INTO `" + tableName + "` (" + columns + ") VALUES (" + parameters + ")";
            m_cmd.CommandText = this._sql;
            int affectedRows = m_cmd.ExecuteNonQuery();
            return new MysqlParameterizedWriteResult(affectedRows, m_cmd.LastInsertedId);
        }

        private long ExecuteUpdateParameterized(string tableName, Dictionary<string, string> data, Dictionary<string, string> where)
        {
            m_cmd = dbConnection.CreateCommand();
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE `").Append(tableName).Append("` SET ");
            AppendAssignments(sql, m_cmd, data, "@set_");
            sql.Append(" WHERE ");
            AppendEqualsConditions(sql, m_cmd, where, "@where_");
            this._sql = sql.ToString();
            m_cmd.CommandText = this._sql;
            return m_cmd.ExecuteNonQuery();
        }

        private List<Dictionary<string, string>> ExecuteSelectParameterized(string tableName, IReadOnlyList<string> fields, Dictionary<string, string> where)
        {
            m_cmd = dbConnection.CreateCommand();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            if (fields == null || fields.Count == 0)
            {
                sql.Append('*');
            }
            else
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(", ");
                    }

                    sql.Append('`').Append(fields[i]).Append('`');
                }
            }

            sql.Append(" FROM `").Append(tableName).Append('`');
            if (where != null && where.Count > 0)
            {
                sql.Append(" WHERE ");
                AppendEqualsConditions(sql, m_cmd, where, "@where_");
            }

            this._sql = sql.ToString();
            m_cmd.CommandText = this._sql;
            m_reader = m_cmd.ExecuteReader();

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            if (m_reader.HasRows)
            {
                while (m_reader.Read())
                {
                    Dictionary<string, string> row = new Dictionary<string, string>();
                    for (int i = 0; i < m_reader.FieldCount; i++)
                    {
                        string fieldName = m_reader.GetName(i).Trim();
                        row.Add(fieldName, ReadFieldAsInvariantString(m_reader, i));
                    }

                    rows.Add(row);
                }
            }

            return rows;
        }

        private T ExecuteWithMutexRetry<T>(string operationName, string tableName, Func<T> action)
        {
            Exception firstException = null;

            m_mutex.WaitOne();
            try
            {
                this.reOpen();
                if (dbConnection.State == ConnectionState.Open)
                {
                    try
                    {
                        return action();
                    }
                    catch (Exception ex)
                    {
                        this.errorMsg(ex);
                        MyConsole.WriteLine("Acess " + operationName + " " + tableName);
                        firstException = ex;
                    }
                    finally
                    {
                        this.closeHandle();
                    }
                }
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }

            ReConnect();

            m_mutex.WaitOne();
            try
            {
                this.reOpen();
                if (dbConnection.State == ConnectionState.Open)
                {
                    try
                    {
                        return action();
                    }
                    catch (Exception ex)
                    {
                        this.errorMsg(ex);
                        MyConsole.WriteLine("Acess " + operationName + " " + tableName);
                        throw;
                    }
                    finally
                    {
                        this.closeHandle();
                    }
                }

                throw new InvalidOperationException("MySQL 連線未開啟，無法執行參數化 " + operationName + "。", firstException);
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

        private static void AppendAssignments(StringBuilder sql, MySqlCommand command, Dictionary<string, string> values, string parameterPrefix)
        {
            bool first = true;
            foreach (KeyValuePair<string, string> pair in values)
            {
                if (!first)
                {
                    sql.Append(", ");
                }

                first = false;
                string parameterName = parameterPrefix + pair.Key;
                sql.Append('`').Append(pair.Key).Append("` = ").Append(parameterName);
                command.Parameters.AddWithValue(parameterName, pair.Value);
            }
        }

        private static void AppendEqualsConditions(StringBuilder sql, MySqlCommand command, Dictionary<string, string> values, string parameterPrefix)
        {
            bool first = true;
            foreach (KeyValuePair<string, string> pair in values)
            {
                if (!first)
                {
                    sql.Append(" AND ");
                }

                first = false;
                string parameterName = parameterPrefix + pair.Key;
                sql.Append('`').Append(pair.Key).Append("` = ").Append(parameterName);
                command.Parameters.AddWithValue(parameterName, pair.Value);
            }
        }

        private static string ReadFieldAsInvariantString(MySqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return "";
            }

            object raw = reader.GetValue(ordinal);
            if (raw is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
            }

            return Convert.ToString(raw, CultureInfo.InvariantCulture) ?? "";
        }

        private static void ValidateSqlIdentifier(string name, string paramName)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (!IsSafeSqlIdentifier(name))
            {
                throw new ArgumentException("SQL 識別字只能是英數字與底線，且不可從數字開頭。值=" + name, paramName);
            }
        }

        private static void ValidateColumnMap(Dictionary<string, string> values, string paramName)
        {
            foreach (KeyValuePair<string, string> pair in values)
            {
                ValidateSqlIdentifier(pair.Key, paramName);
                if (pair.Value == null)
                {
                    throw new ArgumentException("欄位值不得為 null。欄位=" + pair.Key, paramName);
                }
            }
        }

        private static void ValidateFieldList(IReadOnlyList<string> fields, string paramName)
        {
            if (fields == null || fields.Count == 0)
            {
                return;
            }

            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < fields.Count; i++)
            {
                ValidateSqlIdentifier(fields[i], paramName);
                if (!seen.Add(fields[i]))
                {
                    throw new ArgumentException("查詢欄位重複。欄位=" + fields[i], paramName);
                }
            }
        }

        private static bool IsSafeSqlIdentifier(string name)
        {
            if (name.Length == 0)
            {
                return false;
            }

            char first = name[0];
            if (!(first == '_' || (first >= 'A' && first <= 'Z') || (first >= 'a' && first <= 'z')))
            {
                return false;
            }

            for (int i = 1; i < name.Length; i++)
            {
                char c = name[i];
                if (!(c == '_' || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

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
