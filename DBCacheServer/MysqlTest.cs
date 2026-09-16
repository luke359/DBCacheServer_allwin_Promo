using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using Protocol;
using System.Linq;

namespace DBCacheServer
{
    public enum OpertionCode
    {
        select,
        insert,
        replace,
        update1,
        update2,
        get,
        delete,
        specProc,
        insertUpdate,
        runsql,
    }

    public class OperTionDBBox
    {
        public OpertionCode Type;
        public string TableName;
        public Dictionary<string, string> Data;
        public string Col;
        public string Value;
        public string Where;
        public string Order;
        public int MachineUID = 0;
        public GameServerCode GameServerCode;


        public OperTionDBBox(OpertionCode type, string tablename, Dictionary<string, string> data, string col, string value)
        {
            this.Type = type;
            this.TableName = tablename;
            this.Data = data;
            this.Col = col;
            this.Value = value;
        }

        public OperTionDBBox(OpertionCode type, string tablename, Dictionary<string, string> data, string where)
        {
            this.Type = type;
            this.TableName = tablename;
            this.Data = data;
            this.Where = where;
        }

        public OperTionDBBox(OpertionCode type, string tablename, Dictionary<string, string> data)
        {
            this.Type = type;
            this.TableName = tablename;
            this.Data = data;
        }

        public OperTionDBBox(OpertionCode type, string tablename, string where)
        {
            this.Type = type;
            this.TableName = tablename;
            this.Where = where;
        }

        public OperTionDBBox(OpertionCode type, GameServerCode gameServerCode, int machineUID, string date, Dictionary<string, string> data)
        {
            this.Type = type;
            this.Value = date;
            this.Data = data;
            this.MachineUID = machineUID;
            this.GameServerCode = gameServerCode;
        }
    }


    public class MysqlTest
    {
        private MySqlConnection dbConnection;

        private string connstr = "";
        private string _sql = "";
        public string sqlcontext { get { return _sql; } }
        private MySqlCommand m_cmd = null;
        private MySqlDataReader m_reader = null;

        static string host = "";
        static string id = "";
        static string dbpwd = "";
        static string database = "";

        public MysqlTest()
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
                MyConsole.WriteLine(e.StackTrace);
            }

            try
            {
                //this.connstr = "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";SslMode=None" + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;";
                this.connstr = "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";SslMode=Disabled" + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;";
                //this.connstr = "server=" + host + ";uid=" + id + ";pwd=" + dbpwd + ";database=" + database + ";allowpublickeyretrieval=true" + ";charset=utf8;Allow User Variables=True;"; //取消 SSLMode
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

        public int insertUpdate(string tablename, Dictionary<string,string> updatedata)
        {
            //取更新的所有鍵
            string field1 = "";
            string field2 = "";
            
            string field3 = "";

            //INSERT INTO users (id, name, age) VALUES
            //(1, 'John Doe', 30),
            //(2, 'Jane Doe', 31),
            //(3, 'Peter Smith', 29)
            //ON DUPLICATE KEY UPDATE name = VALUES(name), age = VALUES(age);

            foreach (string key in updatedata.Keys)
            {
                field1 += "," + key;
                field2 += "," + updatedata[key];
                field3 += ", " + key + " = VALUES(" + key + ")";
            }
            this._sql = "INSERT INTO " + tablename + " (" + field1.Trim(',') + ") VALUES (" + field2.Trim(',') + ") ON DUPLICATE KEY UPDATE " + field3.Trim(',');
            //MyConsole.WriteLine("insertUpdate字串:" + this._sql); //UNDONE: Show insertUpdate字串

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    foreach (string key in updatedata.Keys)
                    {
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }

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
                    MyConsole.WriteLine("Test insertUpdate " + tablename);
                }
                finally
                {
                    closeHandle();
                }
                return -1;
            }

            return -1;
        }

        /// <summary>直接執行SQL字串</summary>
        public int runsql(string tablename, string sql)
        {
            //取更新的所有鍵
            this._sql = sql;
            //MyConsole.WriteLine("runsql字串:" + this._sql); //UNDONE: Show runsql字串

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    //foreach (string key in updatedata.Keys)
                    //{
                    //    m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    //}

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test runsql " + tablename + ", SQL=[" + sql + "]");
                }
                finally
                {
                    closeHandle();
                }
                return -1;
            }

            return -1;
        }

        /// <summary>
        /// 插入表單
        /// </summary>
        /// <param name="tablname"></param>
        /// <param name="updatedata"></param>
        /// <returns></returns>
        public int insert(string tablename, Dictionary<string,string> updatedata)
        {

            //取更新的所有鍵
            string field1 = "";
            string field2 = "";

            foreach (string key in updatedata.Keys)
            {
                field1 += "," + key;
                field2 += "," + updatedata[key];
            }
            this._sql = "INSERT INTO " + tablename + " (" + field1.Trim(',') + ") VALUE (" + field2.Trim(',') + ")";

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    foreach (string key in updatedata.Keys)
                    {
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }

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
                    MyConsole.WriteLine("Test insert1 " + tablename);
                }
                finally
                {
                    closeHandle();
                }
                return -1;
            }

            return -1;
        }

        /// <summary>
        /// 插入表單(一次寫入多筆)
        /// </summary>
        /// <param name="tablname"></param>
        /// <param name="updatedata"></param>
        /// <returns></returns>
        public int insert(string tablename, List<Dictionary<string, string>> updatedata)
        {

            string order = "";

            //取更新的所有鍵
            string field1 = "";
            string field2 = "";
            int a = 0;
            int b = 0;

            field1 += "(";

            foreach (KeyValuePair<string, string> d in updatedata[0])
            {
                if (a != 0)
                {
                    field1 += ",";
                }

                field1 += d.Key;

                a = 1;
            }

            field1 += ")";

            foreach (Dictionary<string, string> d in updatedata)
            {
                if (b != 0)
                {
                    field2 += ",";
                }

                field2 += "(";

                int c = 0;

                foreach (KeyValuePair<string, string> v in d)
                {
                    if (c != 0)
                    {
                        field2 += ",";
                    }

                    field2 += v.Value;

                    c = 1;
                }

                field2 += ")";

                b = 1;
            }

            this._sql = "INSERT INTO " + tablename + " " + field1 + " VALUE " + field2;

            order = this._sql;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    
                    int result = m_cmd.ExecuteNonQuery();

                    if (result > 0)  //一次寫入多筆, 返回值會大於1, 且不需要取得最後的主Key值 2021/08/06
                    {
                        return result;
                        //string prid = this.getPrimary(tablename);
                        //
                        //if (prid != "")
                        //{
                        //    var data = this.get(tablename, prid, "", prid + " desc");
                        //
                        //    return int.Parse(data[prid]);
                        //}
                        //else
                        //{
                        //    return 1;
                        //}
                    }

                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test insert2 " + tablename);
                    MyConsole.WriteLine("sql order:" + order);
                }
                finally
                {
                    closeHandle();
                }
                return -1;
            }

            return -1;
        }

        /// <summary>
        /// 插入表單(一次寫入多筆)
        /// </summary>
        /// <param name="tablname"></param>
        /// <param name="updatedata"></param>
        /// <returns></returns>
        public int insert(string tablename, string field1, string field2)
        {
            this._sql = "INSERT INTO " + tablename + " " + field1 + " VALUE " + field2;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;

                    int result = m_cmd.ExecuteNonQuery();

                    if (result > 0) //一次寫入多筆, 返回值會大於1, 且不需要取得最後的主Key值 2021/08/06
                    {
                        return result;
                        //string prid = this.getPrimary(tablename);
                        //
                        //if (prid != "")
                        //{
                        //    var data = this.get(tablename, prid, "", prid + " desc");
                        //
                        //    return int.Parse(data[prid]);
                        //}
                        //else
                        //{
                        //    return 1;
                        //}
                    }

                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test inser3 " + tablename);
                }
                finally
                {
                    closeHandle();
                }
                return -1;
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
            this._sql = "INSERT INTO " + tablename + " " + updatedata;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
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
                    MyConsole.WriteLine("Test insert4 " + tablename);
                }
                finally
                {
                    closeHandle();
                }
                return 0;
            }

            return 0;
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
            //取更新的所有鍵
            string field1 = "";
            string field2 = "";

            foreach (string key in updatedata.Keys)
            {
                field1 += "," + key;
                field2 += "," + updatedata[key];
            }
            this._sql = "REPLACE INTO " + tablename + " (" + field1.Trim(',') + ") VALUE (" + field2.Trim(',') + ")";
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    foreach (string key in updatedata.Keys)
                    {
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test replace " + tablename);
                }
                finally
                {
                    this.closeHandle();
                }
                return 0;
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
            this._sql = "select " + (fields != "" ? fields : " * ") + " from " + tablename + " " + (where != "" ? (" where " + where) : "") + " " + (order != "" ? (" order by " + order) : "") + (limit != "" ? (" limit " + limit) : "");
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                List<Dictionary<string, string>> datalist = new List<Dictionary<string, string>>();
                
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
                    MyConsole.WriteLine("Test select " + tablename);
                    datalist = null;
                }
                finally
                {
                    this.closeHandle();
                }
                return datalist;
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
            //取要更新的所有鍵
            string filed1 = "";
            foreach (string key in updatedata.Keys)
            {
                filed1 += "," + key + "=" + updatedata[key];
            }

            this._sql = "UPDATE " + tablename + " SET " + filed1.Trim(',') + " WHERE " + col + "=" + value;
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    foreach (string key in updatedata.Keys)
                    {
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }

                    m_cmd.CommandText = this._sql;

                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test update1 " + tablename);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        public int update(string tablename, Dictionary<string, string> updatedata, string where = "")
        {
            // 取要更新的所有鍵
            string filed1 = "";
            foreach (string key in updatedata.Keys)
            {
                filed1 += "," + key + "=" + updatedata[key];
            }

            this._sql = "UPDATE " + tablename + " SET " + filed1.Trim(',') + " WHERE " + where;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    foreach (string key in updatedata.Keys)
                    {
                        m_cmd.Parameters.AddWithValue(key.ToLower(), updatedata[key]);
                    }

                    m_cmd.CommandText = this._sql;
                    
                    int result = m_cmd.ExecuteNonQuery();
                    
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test update2 " + tablename);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        
        /// <summary>
        /// 更新表單
        /// </summary>
        /// <returns></returns>
        public int batchupdata(List<OperTionDBBox> data)
        {
            string tablename = data[0].TableName;
            string order = BatchUpdataSqlString(data);

            if (string.IsNullOrEmpty(order))
                return 0;

            this._sql = order;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    
                    m_cmd.CommandText = this._sql;

                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test batchupdata " + tablename);
                    MyConsole.WriteLine("sql order:" + order);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }

        public string BatchUpdataSqlString(List<OperTionDBBox> data)
        {
            //這版使用 string 組合字串
            string order = "";

            Dictionary<string, Dictionary<string, string>> value = new Dictionary<string, Dictionary<string, string>>();

            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            int b = 0;
            int c = 0;

            foreach (OperTionDBBox t in data)
            {
                foreach (KeyValuePair<string, string> i in t.Data)
                {
                    if (value.ContainsKey(i.Key))
                    {
                        if (value[i.Key].ContainsKey(t.Value))
                        {
                            value[i.Key][t.Value] = i.Value;
                        }
                        else
                        {
                            value[i.Key].Add(t.Value, i.Value);
                        }
                    }
                    else
                    {
                        Dictionary<string, string> pairs = new Dictionary<string, string>();
                        pairs.Add(t.Value, i.Value);
                        value.Add(i.Key, pairs);
                    }
                }
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> v in value)
            {
                if (c != 0)
                {
                    filed2 += ",";
                }

                filed2 += v.Key + " = CASE " + Col + " ";

                foreach (KeyValuePair<string, string> n in v.Value)
                {
                    filed2 += "WHEN " + n.Key + " THEN " + n.Value + " ";

                    if (c == 0)
                    {
                        if (b != 0)
                        {
                            filed3 += ",";
                        }

                        filed3 += n.Key;

                        b = 1;
                    }
                }

                filed2 += "END";

                c = 1;
            }

            filed3 += ")";

            order = filed1 + filed2 + filed3;

            return order;
        }

        public string BatchUpdataSqlString_New(List<OperTionDBBox> data)
        {
            //這版使用 StringBuilder 組合字串
            if (data == null || data.Count == 0)
                return string.Empty;

            string tableName = data[0].TableName;
            string col = data[0].Col;

            // column -> { pkValue -> fieldValue }
            var valueMap = new Dictionary<string, Dictionary<string, string>>();
            // 用 LinkedHashSet 概念保留所有唯一的 pk 值（保持順序）
            var pkValues = new List<string>();
            var pkSet = new HashSet<string>();

            foreach (OperTionDBBox t in data)
            {
                // 收集唯一的 pk 值
                if (pkSet.Add(t.Value))
                    pkValues.Add(t.Value);

                foreach (KeyValuePair<string, string> i in t.Data)
                {
                    if (!valueMap.TryGetValue(i.Key, out var innerDict))
                    {
                        innerDict = new Dictionary<string, string>();
                        valueMap[i.Key] = innerDict;
                    }
                    // 後蓋前（同一 pk 出現多次，取最後一筆）
                    innerDict[t.Value] = i.Value;
                }
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("UPDATE ").Append(tableName).Append(" SET ");

            bool firstCol = true;
            foreach (KeyValuePair<string, Dictionary<string, string>> v in valueMap)
            {
                if (!firstCol) sb.Append(',');
                firstCol = false;

                sb.Append(v.Key).Append(" = CASE ").Append(col).Append(' ');
                foreach (KeyValuePair<string, string> n in v.Value)
                {
                    sb.Append("WHEN ").Append(n.Key)
                      .Append(" THEN ").Append(n.Value).Append(' ');
                }
                sb.Append("END");
            }

            // WHERE IN 從所有唯一 pk 值建構，不再依賴第一欄位
            sb.Append(" WHERE ").Append(col).Append(" IN (");
            sb.Append(string.Join(",", pkValues));
            sb.Append(')');

            return sb.ToString();
        }

        public int UpdataUserData(List<OperTionDBBox> data)
        {
            return GameAccount(data);
        }


        public int GameAccount(List<OperTionDBBox> data)
        {
            string tablename = data[0].TableName;
            //string order = BatchUpdataSqlString(data);
            string order = BatchUpdataSqlString_New(data);

            if (string.IsNullOrEmpty(order))
                return 0;

            //this._sql = order;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    
                    //m_cmd.CommandText = this._sql;
                    m_cmd.CommandText = order;

                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("MysqlTest: " + tablename);
                    MyConsole.WriteLine("sql order:" + order);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }


        #region 押分機
        /// <summary>更新表單(內外帳)</summary>
        public int XiyouAccount(List<OperTionDBBox> data)
        {
            Dictionary<string, Dictionary<string, string>> value = new Dictionary<string, Dictionary<string, string>>();

            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",GrandSlam" + " = CASE " + Col + " ";
            string context19 = ",SameColor" + " = CASE " + Col + " ";
            string context20 = ",SameAnimal" + " = CASE " + Col + " ";
            string context21 = ",Quartet" + " = CASE " + Col + " ";
            string context22 = ",GiveLamp" + " = CASE " + Col + " ";
            string context23 = ",Bonus" + " = CASE " + Col + " ";
            string context24 = ",FourPeat" + " = CASE " + Col + " ";
            string context25 = ",ThreePeat" + " = CASE " + Col + " ";
            string context26 = ",TwoPeat" + " = CASE " + Col + " ";
            string context27 = ",SeventyTwo" + " = CASE " + Col + " ";
            string context28 = ",TwinAnimal" + " = CASE " + Col + " ";
            string context29 = ",RA" + " = CASE " + Col + " ";
            string context30 = ",GA" + " = CASE " + Col + " ";
            string context31 = ",YA" + " = CASE " + Col + " ";
            string context32 = ",RB" + " = CASE " + Col + " ";
            string context33 = ",GB" + " = CASE " + Col + " ";
            string context34 = ",YB" + " = CASE " + Col + " ";
            string context35 = ",RC" + " = CASE " + Col + " ";
            string context36 = ",GC" + " = CASE " + Col + " ";
            string context37 = ",YC" + " = CASE " + Col + " ";
            string context38 = ",RD" + " = CASE " + Col + " ";
            string context39 = ",GD" + " = CASE " + Col + " ";
            string context40 = ",YD" + " = CASE " + Col + " ";
            string context41 = ",BPTotalBet" + " = CASE " + Col + " ";
            string context42 = ",BPTotalWin" + " = CASE " + Col + " ";
            string context43 = ",BPTotalSurplus" + " = CASE " + Col + " ";
            string context44 = ",BPGameTimes" + " = CASE " + Col + " ";
            string context45 = ",BZ" + " = CASE " + Col + " ";
            string context46 = ",BX" + " = CASE " + Col + " ";
            string context47 = ",BH" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GrandSlam"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SameColor"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SameAnimal"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Quartet"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiveLamp"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bonus"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FourPeat"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThreePeat"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TwoPeat"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SeventyTwo"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TwinAnimal"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RA"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GA"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YA"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RB"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GB"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YB"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RC"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GC"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YC"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RD"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GD"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YD"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPTotalBet"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPTotalWin"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPTotalSurplus"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPGameTimes"] + " ";
                context45 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BZ"] + " ";
                context46 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BX"] + " ";
                context47 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BH"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";
            context45 += "END";
            context46 += "END";
            context47 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41 + context42 + context43 + context44 + context45 +
                context46 + context47;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;

                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test " + TableName);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單(日帳)</summary>
        public int XiyouDetail(List<OperTionDBBox> data)
        {
            Dictionary<string, Dictionary<string, string>> value = new Dictionary<string, Dictionary<string, string>>();

            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",GrandSlam" + " = CASE " + Col + " ";
            string context19 = ",SameColor" + " = CASE " + Col + " ";
            string context20 = ",SameAnimal" + " = CASE " + Col + " ";
            string context21 = ",Quartet" + " = CASE " + Col + " ";
            string context22 = ",GiveLamp" + " = CASE " + Col + " ";
            string context23 = ",Bonus" + " = CASE " + Col + " ";
            string context24 = ",FourPeat" + " = CASE " + Col + " ";
            string context25 = ",ThreePeat" + " = CASE " + Col + " ";
            string context26 = ",TwoPeat" + " = CASE " + Col + " ";
            string context27 = ",SeventyTwo" + " = CASE " + Col + " ";
            string context28 = ",TwinAnimal" + " = CASE " + Col + " ";
            string context29 = ",RA" + " = CASE " + Col + " ";
            string context30 = ",GA" + " = CASE " + Col + " ";
            string context31 = ",YA" + " = CASE " + Col + " ";
            string context32 = ",RB" + " = CASE " + Col + " ";
            string context33 = ",GB" + " = CASE " + Col + " ";
            string context34 = ",YB" + " = CASE " + Col + " ";
            string context35 = ",RC" + " = CASE " + Col + " ";
            string context36 = ",GC" + " = CASE " + Col + " ";
            string context37 = ",YC" + " = CASE " + Col + " ";
            string context38 = ",RD" + " = CASE " + Col + " ";
            string context39 = ",GD" + " = CASE " + Col + " ";
            string context40 = ",YD" + " = CASE " + Col + " ";
            string context41 = ",BPTotalBet" + " = CASE " + Col + " ";
            string context42 = ",BPTotalWin" + " = CASE " + Col + " ";
            string context43 = ",BPTotalSurplus" + " = CASE " + Col + " ";
            string context44 = ",BPGameTimes" + " = CASE " + Col + " ";
            string context45 = ",BZ" + " = CASE " + Col + " ";
            string context46 = ",BX" + " = CASE " + Col + " ";
            string context47 = ",BH" + " = CASE " + Col + " ";
            string context48 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GrandSlam"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SameColor"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SameAnimal"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Quartet"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiveLamp"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bonus"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FourPeat"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThreePeat"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TwoPeat"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SeventyTwo"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TwinAnimal"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RA"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GA"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YA"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RB"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GB"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YB"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RC"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GC"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YC"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RD"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GD"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YD"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPTotalBet"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPTotalWin"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPTotalSurplus"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BPGameTimes"] + " ";
                context45 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BZ"] + " ";
                context46 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BX"] + " ";
                context47 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BH"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";
            context45 += "END";
            context46 += "END";
            context47 += "END";
            context48 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41 + context42 + context43 + context44 + context45 +
                context46 + context47 + context48;

            this._sql = filed1 + filed2 + filed3;
            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;

                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test " + TableName);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }

        /// <summary>更新表單</summary>
        public int SuperKingDerbyDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",JP5_Times" + " = CASE " + Col + " ";
            string context19 = ",JP4_Times" + " = CASE " + Col + " ";
            string context20 = ",JP3_Times" + " = CASE " + Col + " ";
            string context21 = ",JP2_Times" + " = CASE " + Col + " ";
            string context22 = ",H6_Times" + " = CASE " + Col + " ";
            string context23 = ",H5_Times" + " = CASE " + Col + " ";
            string context24 = ",H4_Times" + " = CASE " + Col + " ";
            string context25 = ",H3_Times" + " = CASE " + Col + " ";
            string context26 = ",D1000_Times" + " = CASE " + Col + " ";
            string context27 = ",D500_Times" + " = CASE " + Col + " ";
            string context28 = ",D200_Times" + " = CASE " + Col + " ";
            string context29 = ",D175_Times" + " = CASE " + Col + " ";
            string context30 = ",D125_Times" + " = CASE " + Col + " ";
            string context31 = ",D100_Times" + " = CASE " + Col + " ";
            string context32 = ",D80_Times" + " = CASE " + Col + " ";
            string context33 = ",D60_Times" + " = CASE " + Col + " ";
            string context34 = ",D30_Times" + " = CASE " + Col + " ";
            string context35 = ",D20_Times" + " = CASE " + Col + " ";
            string context36 = ",D10_Times" + " = CASE " + Col + " ";
            string context37 = ",D8_Times" + " = CASE " + Col + " ";
            string context38 = ",D5_Times" + " = CASE " + Col + " ";
            string context39 = ",D4_Times" + " = CASE " + Col + " ";
            string context40 = ",D3_Times" + " = CASE " + Col + " ";
            string context41 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP5_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP4_Times"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP3_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP2_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H6_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H5_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H4_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H3_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D1000_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D500_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D200_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D175_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D125_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D100_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D80_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D60_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D30_Times"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D20_Times"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D10_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D8_Times"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D5_Times"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D4_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D3_Times"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int FerrariDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",Lotto_Times" + " = CASE " + Col + " ";
            string context19 = ",Lotto_Win" + " = CASE " + Col + " ";
            string context20 = ",Bingo_Times" + " = CASE " + Col + " ";
            string context21 = ",Bingo_Win" + " = CASE " + Col + " ";
            string context22 = ",Grand_Times" + " = CASE " + Col + " ";
            string context23 = ",BigFour_Times" + " = CASE " + Col + " ";
            string context24 = ",BigThree_Times" + " = CASE " + Col + " ";
            string context25 = ",Bonus_Times" + " = CASE " + Col + " ";
            string context26 = ",JackPot_Times" + " = CASE " + Col + " ";
            string context27 = ",ShootLight_Times" + " = CASE " + Col + " ";
            string context28 = ",RF_Times" + " = CASE " + Col + " ";
            string context29 = ",GF_Times" + " = CASE " + Col + " ";
            string context30 = ",YF_Times" + " = CASE " + Col + " ";
            string context31 = ",RP_Times" + " = CASE " + Col + " ";
            string context32 = ",GP_Times" + " = CASE " + Col + " ";
            string context33 = ",YP_Times" + " = CASE " + Col + " ";
            string context34 = ",RT_Times" + " = CASE " + Col + " ";
            string context35 = ",GT_Times" + " = CASE " + Col + " ";
            string context36 = ",YT_Times" + " = CASE " + Col + " ";
            string context37 = ",RL_Times" + " = CASE " + Col + " ";
            string context38 = ",GL_Times" + " = CASE " + Col + " ";
            string context39 = ",YL_Times" + " = CASE " + Col + " ";
            string context40 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lotto_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lotto_Win"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bingo_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bingo_Win"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Grand_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BigFour_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BigThree_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bonus_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JackPot_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ShootLight_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RF_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GF_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YF_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RP_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GP_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YP_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RT_Times"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GT_Times"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YT_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RL_Times"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GL_Times"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YL_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int DrollMonkeyDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",JP5_Times" + " = CASE " + Col + " ";
            string context19 = ",JP4_Times" + " = CASE " + Col + " ";
            string context20 = ",JP3_Times" + " = CASE " + Col + " ";
            string context21 = ",JP2_Times" + " = CASE " + Col + " ";
            string context22 = ",H6_Times" + " = CASE " + Col + " ";
            string context23 = ",H5_Times" + " = CASE " + Col + " ";
            string context24 = ",H4_Times" + " = CASE " + Col + " ";
            string context25 = ",H3_Times" + " = CASE " + Col + " ";
            string context26 = ",D1000_Times" + " = CASE " + Col + " ";
            string context27 = ",D500_Times" + " = CASE " + Col + " ";
            string context28 = ",D200_Times" + " = CASE " + Col + " ";
            string context29 = ",D175_Times" + " = CASE " + Col + " ";
            string context30 = ",D125_Times" + " = CASE " + Col + " ";
            string context31 = ",D100_Times" + " = CASE " + Col + " ";
            string context32 = ",D80_Times" + " = CASE " + Col + " ";
            string context33 = ",D60_Times" + " = CASE " + Col + " ";
            string context34 = ",D30_Times" + " = CASE " + Col + " ";
            string context35 = ",D20_Times" + " = CASE " + Col + " ";
            string context36 = ",D10_Times" + " = CASE " + Col + " ";
            string context37 = ",D8_Times" + " = CASE " + Col + " ";
            string context38 = ",D5_Times" + " = CASE " + Col + " ";
            string context39 = ",D4_Times" + " = CASE " + Col + " ";
            string context40 = ",D3_Times" + " = CASE " + Col + " ";
            string context41 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP5_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP4_Times"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP3_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JP2_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H6_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H5_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H4_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["H3_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D1000_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D500_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D200_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D175_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D125_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D100_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D80_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D60_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D30_Times"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D20_Times"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D10_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D8_Times"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D5_Times"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D4_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["D3_Times"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();
                    
                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int Bmw3DDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",Special_Times" + " = CASE " + Col + " ";
            string context19 = ",SuperBonus_Times" + " = CASE " + Col + " ";
            string context20 = ",LuckyShoot_Times" + " = CASE " + Col + " ";
            string context21 = ",Celebrate_Times" + " = CASE " + Col + " ";
            string context22 = ",MaxOdds_Times" + " = CASE " + Col + " ";
            string context23 = ",RBenz_Times" + " = CASE " + Col + " ";
            string context24 = ",GBenz_Times" + " = CASE " + Col + " ";
            string context25 = ",YBenz_Times" + " = CASE " + Col + " ";
            string context26 = ",RBMW_Times" + " = CASE " + Col + " ";
            string context27 = ",GBMW_Times" + " = CASE " + Col + " ";
            string context28 = ",YBMW_Times" + " = CASE " + Col + " ";
            string context29 = ",RAudi_Times" + " = CASE " + Col + " ";
            string context30 = ",GAudi_Times" + " = CASE " + Col + " ";
            string context31 = ",YAudi_Times" + " = CASE " + Col + " ";
            string context32 = ",RJetta_Times" + " = CASE " + Col + " ";
            string context33 = ",GJetta_Times" + " = CASE " + Col + " ";
            string context34 = ",YJetta_Times" + " = CASE " + Col + " ";
            string context35 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Special_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SuperBonus_Times"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LuckyShoot_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Celebrate_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MaxOdds_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RBenz_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GBenz_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YBenz_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RBMW_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GBMW_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YBMW_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RAudi_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GAudi_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YAudi_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RJetta_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GJetta_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YJetta_Times"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int AstonMartinDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",Special_Times" + " = CASE " + Col + " ";
            string context19 = ",SuperBonus_Times" + " = CASE " + Col + " ";
            string context20 = ",LuckyShoot_Times" + " = CASE " + Col + " ";
            string context21 = ",Celebrate_Times" + " = CASE " + Col + " ";
            string context22 = ",FireChance_Times" + " = CASE " + Col + " ";
            string context23 = ",MaxOdds_Times" + " = CASE " + Col + " ";
            string context24 = ",RF_Times" + " = CASE " + Col + " ";
            string context25 = ",GF_Times" + " = CASE " + Col + " ";
            string context26 = ",YF_Times" + " = CASE " + Col + " ";
            string context27 = ",RA_Times" + " = CASE " + Col + " ";
            string context28 = ",GA_Times" + " = CASE " + Col + " ";
            string context29 = ",YA_Times" + " = CASE " + Col + " ";
            string context30 = ",RP_Times" + " = CASE " + Col + " ";
            string context31 = ",GP_Times" + " = CASE " + Col + " ";
            string context32 = ",YP_Times" + " = CASE " + Col + " ";
            string context33 = ",RR_Times" + " = CASE " + Col + " ";
            string context34 = ",GR_Times" + " = CASE " + Col + " ";
            string context35 = ",YR_Times" + " = CASE " + Col + " ";
            string context36 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Special_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SuperBonus_Times"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LuckyShoot_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Celebrate_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireChance_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MaxOdds_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RF_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GF_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YF_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RA_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GA_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YA_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RP_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GP_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YP_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RR_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GR_Times"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YR_Times"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int SuperAltynDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",Special_Times" + " = CASE " + Col + " ";
            string context19 = ",SuperBonus_Times" + " = CASE " + Col + " ";
            string context20 = ",LuckyShoot_Times" + " = CASE " + Col + " ";
            string context21 = ",Celebrate_Times" + " = CASE " + Col + " ";
            string context22 = ",FireChance_Times" + " = CASE " + Col + " ";
            string context23 = ",MaxOdds_Times" + " = CASE " + Col + " ";
            string context24 = ",RC_Times" + " = CASE " + Col + " ";
            string context25 = ",GC_Times" + " = CASE " + Col + " ";
            string context26 = ",YC_Times" + " = CASE " + Col + " ";
            string context27 = ",RE_Times" + " = CASE " + Col + " ";
            string context28 = ",GE_Times" + " = CASE " + Col + " ";
            string context29 = ",YE_Times" + " = CASE " + Col + " ";
            string context30 = ",RL_Times" + " = CASE " + Col + " ";
            string context31 = ",GL_Times" + " = CASE " + Col + " ";
            string context32 = ",YL_Times" + " = CASE " + Col + " ";
            string context33 = ",RT_Times" + " = CASE " + Col + " ";
            string context34 = ",GT_Times" + " = CASE " + Col + " ";
            string context35 = ",YT_Times" + " = CASE " + Col + " ";
            string context36 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Special_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SuperBonus_Times"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LuckyShoot_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Celebrate_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireChance_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MaxOdds_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RC_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GC_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YC_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RE_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GE_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YE_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RL_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GL_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YL_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RT_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GT_Times"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YT_Times"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int MonkeyKing2Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",GrandSlam" + " = CASE " + Col + " ";
            string context19 = ",HalfCountry" + " = CASE " + Col + " ";
            string context20 = ",AllColor" + " = CASE " + Col + " ";
            string context21 = ",SameColor" + " = CASE " + Col + " ";
            string context22 = ",SameAnimal" + " = CASE " + Col + " ";
            string context23 = ",Quartet" + " = CASE " + Col + " ";
            string context24 = ",GiveLamp" + " = CASE " + Col + " ";
            string context25 = ",Bonus" + " = CASE " + Col + " ";
            string context26 = ",Gold" + " = CASE " + Col + " ";
            string context27 = ",FourPeat" + " = CASE " + Col + " ";
            string context28 = ",ThreePeat" + " = CASE " + Col + " ";
            string context29 = ",TwoPeat" + " = CASE " + Col + " ";
            string context30 = ",TwinAnimal" + " = CASE " + Col + " ";
            string context31 = ",SeventyTwo" + " = CASE " + Col + " ";
            string context32 = ",RA" + " = CASE " + Col + " ";
            string context33 = ",GA" + " = CASE " + Col + " ";
            string context34 = ",YA" + " = CASE " + Col + " ";
            string context35 = ",RB" + " = CASE " + Col + " ";
            string context36 = ",GB" + " = CASE " + Col + " ";
            string context37 = ",YB" + " = CASE " + Col + " ";
            string context38 = ",RC" + " = CASE " + Col + " ";
            string context39 = ",GC" + " = CASE " + Col + " ";
            string context40 = ",YC" + " = CASE " + Col + " ";
            string context41 = ",RD" + " = CASE " + Col + " ";
            string context42 = ",GD" + " = CASE " + Col + " ";
            string context43 = ",YD" + " = CASE " + Col + " ";
            string context44 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GrandSlam"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HalfCountry"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AllColor"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SameColor"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SameAnimal"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Quartet"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiveLamp"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bonus"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Gold"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FourPeat"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThreePeat"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TwoPeat"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TwinAnimal"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SeventyTwo"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RA"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GA"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YA"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RB"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GB"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YB"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RC"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GC"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YC"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RD"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GD"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YD"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41 + context42 + context43 + context44;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int FIFA2022Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",Special_Times" + " = CASE " + Col + " ";
            string context19 = ",LuckyShoot_Times" + " = CASE " + Col + " ";
            string context20 = ",Celebrate_Times" + " = CASE " + Col + " ";
            string context21 = ",RB_Times" + " = CASE " + Col + " ";
            string context22 = ",GB_Times" + " = CASE " + Col + " ";
            string context23 = ",YB_Times" + " = CASE " + Col + " ";
            string context24 = ",RS_Times" + " = CASE " + Col + " ";
            string context25 = ",GS_Times" + " = CASE " + Col + " ";
            string context26 = ",YS_Times" + " = CASE " + Col + " ";
            string context27 = ",RA_Times" + " = CASE " + Col + " ";
            string context28 = ",GA_Times" + " = CASE " + Col + " ";
            string context29 = ",YA_Times" + " = CASE " + Col + " ";
            string context30 = ",RU_Times" + " = CASE " + Col + " ";
            string context31 = ",GU_Times" + " = CASE " + Col + " ";
            string context32 = ",YU_Times" + " = CASE " + Col + " ";
            string context33 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Special_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LuckyShoot_Times"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Celebrate_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RB_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GB_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YB_Times"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RS_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GS_Times"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YS_Times"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RA_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GA_Times"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YA_Times"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RU_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GU_Times"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YU_Times"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int BigSmallDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",TripleSix_Times" + " = CASE " + Col + " ";
            string context19 = ",TripleSix_Win" + " = CASE " + Col + " ";
            string context20 = ",TripleOne_Times" + " = CASE " + Col + " ";
            string context21 = ",TripleOne_Win" + " = CASE " + Col + " ";
            string context22 = ",Big_Times" + " = CASE " + Col + " ";
            string context23 = ",Big_Win" + " = CASE " + Col + " ";
            string context24 = ",Small_Times" + " = CASE " + Col + " ";
            string context25 = ",Small_Win" + " = CASE " + Col + " ";
            string context26 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TripleSix_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TripleSix_Win"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TripleOne_Times"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TripleOne_Win"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Big_Times"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Big_Win"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Small_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Small_Win"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        #endregion

        #region 魚機
        /// <summary>更新表單</summary>
        public int OceanKingDetail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";
            string context18 = ",ThunderDragon_Times" + " = CASE " + Col + " ";
            string context19 = ",ThunderDragon_Bet" + " = CASE " + Col + " ";
            string context20 = ",ThunderDragon_Win" + " = CASE " + Col + " ";
            string context21 = ",PurpleDragon_Times" + " = CASE " + Col + " ";
            string context22 = ",PurpleDragon_Bet" + " = CASE " + Col + " ";
            string context23 = ",PurpleDragon_Win" + " = CASE " + Col + " ";
            string context24 = ",Phoenix_Times" + " = CASE " + Col + " ";
            string context25 = ",Phoenix_Bet" + " = CASE " + Col + " ";
            string context26 = ",Phoenix_Win" + " = CASE " + Col + " ";
            string context27 = ",Mermaid_Times" + " = CASE " + Col + " ";
            string context28 = ",Mermaid_Bet" + " = CASE " + Col + " ";
            string context29 = ",Mermaid_Win" + " = CASE " + Col + " ";
            string context30 = ",Behemoth_Times" + " = CASE " + Col + " ";
            string context31 = ",Behemoth_Bet" + " = CASE " + Col + " ";
            string context32 = ",Behemoth_Win" + " = CASE " + Col + " ";
            string context33 = ",GiantCrocodile_Times" + " = CASE " + Col + " ";
            string context34 = ",GiantCrocodile_Bet" + " = CASE " + Col + " ";
            string context35 = ",GiantCrocodile_Win" + " = CASE " + Col + " ";
            string context36 = ",GiantOctopus_Times" + " = CASE " + Col + " ";
            string context37 = ",GiantOctopus_Bet" + " = CASE " + Col + " ";
            string context38 = ",GiantOctopus_Win" + " = CASE " + Col + " ";
            string context39 = ",GiantCrab_Times" + " = CASE " + Col + " ";
            string context40 = ",GiantCrab_Bet" + " = CASE " + Col + " ";
            string context41 = ",GiantCrab_Win" + " = CASE " + Col + " ";
            string context42 = ",FireTurtle_Times" + " = CASE " + Col + " ";
            string context43 = ",FireTurtle_Bet" + " = CASE " + Col + " ";
            string context44 = ",FireTurtle_Win" + " = CASE " + Col + " ";
            string context45 = ",FireDragon_Times" + " = CASE " + Col + " ";
            string context46 = ",FireDragon_Bet" + " = CASE " + Col + " ";
            string context47 = ",FireDragon_Win" + " = CASE " + Col + " ";
            string context48 = ",JellyFish_Times" + " = CASE " + Col + " ";
            string context49 = ",JellyFish_Bet" + " = CASE " + Col + " ";
            string context50 = ",JellyFish_Win" + " = CASE " + Col + " ";
            string context51 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context55 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context56 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context57 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context58 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context59 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context60 = ",LungPanCrab_Times" + " = CASE " + Col + " ";
            string context61 = ",LungPanCrab_Bet" + " = CASE " + Col + " ";
            string context62 = ",LungPanCrab_Win" + " = CASE " + Col + " ";
            string context63 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context64 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context65 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context66 = ",UnicornWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",UnicornWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",UnicornWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context70 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context71 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context72 = ",Shark_Times" + " = CASE " + Col + " ";
            string context73 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context74 = ",Shark_Win" + " = CASE " + Col + " ";
            string context75 = ",PufferGold_Times" + " = CASE " + Col + " ";
            string context76 = ",PufferGold_Bet" + " = CASE " + Col + " ";
            string context77 = ",PufferGold_Win" + " = CASE " + Col + " ";
            string context78 = ",MoorishIdolGold_Times" + " = CASE " + Col + " ";
            string context79 = ",MoorishIdolGold_Bet" + " = CASE " + Col + " ";
            string context80 = ",MoorishIdolGold_Win" + " = CASE " + Col + " ";
            string context81 = ",ClownFishGold_Times" + " = CASE " + Col + " ";
            string context82 = ",ClownFishGold_Bet" + " = CASE " + Col + " ";
            string context83 = ",ClownFishGold_Win" + " = CASE " + Col + " ";
            string context84 = ",PufferBig_Times" + " = CASE " + Col + " ";
            string context85 = ",PufferBig_Bet" + " = CASE " + Col + " ";
            string context86 = ",PufferBig_Win" + " = CASE " + Col + " ";
            string context87 = ",MoorishIdolBig_Times" + " = CASE " + Col + " ";
            string context88 = ",MoorishIdolBig_Bet" + " = CASE " + Col + " ";
            string context89 = ",MoorishIdolBig_Win" + " = CASE " + Col + " ";
            string context90 = ",ClownFishBig_Times" + " = CASE " + Col + " ";
            string context91 = ",ClownFishBig_Bet" + " = CASE " + Col + " ";
            string context92 = ",ClownFishBig_Win" + " = CASE " + Col + " ";
            string context93 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context94 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context95 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context96 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context97 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context98 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context99 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context100 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context101 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context102 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context103 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context104 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context105 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context106 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context107 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context108 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context109 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context111 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context112 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context113 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context114 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context115 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context116 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context117 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context118 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context119 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context120 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context121 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context122 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context123 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context124 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context125 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context126 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context127 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context128 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context129 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context130 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context131 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Bet"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Win"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Bet"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Win"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Bet"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Win"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Bet"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Win"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Bet"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Win"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrocodile_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrocodile_Bet"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrocodile_Win"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Bet"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Win"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrab_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrab_Bet"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrab_Win"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireTurtle_Times"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireTurtle_Bet"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireTurtle_Win"] + " ";
                context45 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireDragon_Times"] + " ";
                context46 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireDragon_Bet"] + " ";
                context47 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireDragon_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";
            context45 += "END";
            context46 += "END";
            context47 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41 + context42 + context43 + context44 + context45 +
                context46 + context47 + context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 +
                context132;


            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing2Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context21 = ",PurpleDragon_Times" + " = CASE " + Col + " ";
            string context22 = ",PurpleDragon_Bet" + " = CASE " + Col + " ";
            string context23 = ",PurpleDragon_Win" + " = CASE " + Col + " ";
            string context24 = ",Phoenix_Times" + " = CASE " + Col + " ";
            string context25 = ",Phoenix_Bet" + " = CASE " + Col + " ";
            string context26 = ",Phoenix_Win" + " = CASE " + Col + " ";
            string context27 = ",Mermaid_Times" + " = CASE " + Col + " ";
            string context28 = ",Mermaid_Bet" + " = CASE " + Col + " ";
            string context29 = ",Mermaid_Win" + " = CASE " + Col + " ";

            string context48 = ",JellyFish_Times" + " = CASE " + Col + " ";
            string context49 = ",JellyFish_Bet" + " = CASE " + Col + " ";
            string context50 = ",JellyFish_Win" + " = CASE " + Col + " ";
            string context51 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context55 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context56 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context57 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context58 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context59 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context60 = ",LungPanCrab_Times" + " = CASE " + Col + " ";
            string context61 = ",LungPanCrab_Bet" + " = CASE " + Col + " ";
            string context62 = ",LungPanCrab_Win" + " = CASE " + Col + " ";
            string context63 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context64 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context65 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context66 = ",UnicornWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",UnicornWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",UnicornWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context70 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context71 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context72 = ",Shark_Times" + " = CASE " + Col + " ";
            string context73 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context74 = ",Shark_Win" + " = CASE " + Col + " ";
            string context75 = ",PufferGold_Times" + " = CASE " + Col + " ";
            string context76 = ",PufferGold_Bet" + " = CASE " + Col + " ";
            string context77 = ",PufferGold_Win" + " = CASE " + Col + " ";
            string context78 = ",MoorishIdolGold_Times" + " = CASE " + Col + " ";
            string context79 = ",MoorishIdolGold_Bet" + " = CASE " + Col + " ";
            string context80 = ",MoorishIdolGold_Win" + " = CASE " + Col + " ";
            string context81 = ",ClownFishGold_Times" + " = CASE " + Col + " ";
            string context82 = ",ClownFishGold_Bet" + " = CASE " + Col + " ";
            string context83 = ",ClownFishGold_Win" + " = CASE " + Col + " ";
            string context84 = ",PufferBig_Times" + " = CASE " + Col + " ";
            string context85 = ",PufferBig_Bet" + " = CASE " + Col + " ";
            string context86 = ",PufferBig_Win" + " = CASE " + Col + " ";
            string context87 = ",MoorishIdolBig_Times" + " = CASE " + Col + " ";
            string context88 = ",MoorishIdolBig_Bet" + " = CASE " + Col + " ";
            string context89 = ",MoorishIdolBig_Win" + " = CASE " + Col + " ";
            string context90 = ",ClownFishBig_Times" + " = CASE " + Col + " ";
            string context91 = ",ClownFishBig_Bet" + " = CASE " + Col + " ";
            string context92 = ",ClownFishBig_Win" + " = CASE " + Col + " ";
            string context93 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context94 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context95 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context96 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context97 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context98 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context99 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context100 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context101 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context102 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context103 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context104 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context105 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context106 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context107 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context108 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context109 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context111 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context112 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context113 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context114 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context115 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context116 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context117 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context118 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context119 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context120 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context121 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context122 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context123 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context124 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context125 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context126 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context127 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context128 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context129 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context130 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context131 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Bet"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Win"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Bet"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Win"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Bet"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + 
                context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + 
                context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 +
                context132;


            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing3Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context18 = ",ThunderDragon_Times" + " = CASE " + Col + " ";
            string context19 = ",ThunderDragon_Bet" + " = CASE " + Col + " ";
            string context20 = ",ThunderDragon_Win" + " = CASE " + Col + " ";
            string context30 = ",Behemoth_Times" + " = CASE " + Col + " ";
            string context31 = ",Behemoth_Bet" + " = CASE " + Col + " ";
            string context32 = ",Behemoth_Win" + " = CASE " + Col + " ";
            string context36 = ",GiantOctopus_Times" + " = CASE " + Col + " ";
            string context37 = ",GiantOctopus_Bet" + " = CASE " + Col + " ";
            string context38 = ",GiantOctopus_Win" + " = CASE " + Col + " ";

            string context48 = ",JellyFish_Times" + " = CASE " + Col + " ";
            string context49 = ",JellyFish_Bet" + " = CASE " + Col + " ";
            string context50 = ",JellyFish_Win" + " = CASE " + Col + " ";
            string context51 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context55 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context56 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context57 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context58 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context59 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context60 = ",LungPanCrab_Times" + " = CASE " + Col + " ";
            string context61 = ",LungPanCrab_Bet" + " = CASE " + Col + " ";
            string context62 = ",LungPanCrab_Win" + " = CASE " + Col + " ";
            string context63 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context64 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context65 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context66 = ",UnicornWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",UnicornWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",UnicornWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context70 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context71 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context72 = ",Shark_Times" + " = CASE " + Col + " ";
            string context73 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context74 = ",Shark_Win" + " = CASE " + Col + " ";
            string context75 = ",PufferGold_Times" + " = CASE " + Col + " ";
            string context76 = ",PufferGold_Bet" + " = CASE " + Col + " ";
            string context77 = ",PufferGold_Win" + " = CASE " + Col + " ";
            string context78 = ",MoorishIdolGold_Times" + " = CASE " + Col + " ";
            string context79 = ",MoorishIdolGold_Bet" + " = CASE " + Col + " ";
            string context80 = ",MoorishIdolGold_Win" + " = CASE " + Col + " ";
            string context81 = ",ClownFishGold_Times" + " = CASE " + Col + " ";
            string context82 = ",ClownFishGold_Bet" + " = CASE " + Col + " ";
            string context83 = ",ClownFishGold_Win" + " = CASE " + Col + " ";
            string context84 = ",PufferBig_Times" + " = CASE " + Col + " ";
            string context85 = ",PufferBig_Bet" + " = CASE " + Col + " ";
            string context86 = ",PufferBig_Win" + " = CASE " + Col + " ";
            string context87 = ",MoorishIdolBig_Times" + " = CASE " + Col + " ";
            string context88 = ",MoorishIdolBig_Bet" + " = CASE " + Col + " ";
            string context89 = ",MoorishIdolBig_Win" + " = CASE " + Col + " ";
            string context90 = ",ClownFishBig_Times" + " = CASE " + Col + " ";
            string context91 = ",ClownFishBig_Bet" + " = CASE " + Col + " ";
            string context92 = ",ClownFishBig_Win" + " = CASE " + Col + " ";
            string context93 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context94 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context95 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context96 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context97 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context98 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context99 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context100 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context101 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context102 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context103 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context104 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context105 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context106 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context107 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context108 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context109 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context111 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context112 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context113 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context114 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context115 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context116 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context117 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context118 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context119 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context120 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context121 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context122 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context123 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context124 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context125 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context126 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context127 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context128 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context129 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context130 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context131 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Bet"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Win"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Bet"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Win"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Bet"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + 
                context30 + context31 + context32 + context36 +
                context37 + context38 + 
                context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 +
                context132;


            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing4Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context27 = ",Mermaid_Times" + " = CASE " + Col + " ";
            string context28 = ",Mermaid_Bet" + " = CASE " + Col + " ";
            string context29 = ",Mermaid_Win" + " = CASE " + Col + " ";
            string context36 = ",GiantOctopus_Times" + " = CASE " + Col + " ";
            string context37 = ",GiantOctopus_Bet" + " = CASE " + Col + " ";
            string context38 = ",GiantOctopus_Win" + " = CASE " + Col + " ";
            string context42 = ",FireTurtle_Times" + " = CASE " + Col + " ";
            string context43 = ",FireTurtle_Bet" + " = CASE " + Col + " ";
            string context44 = ",FireTurtle_Win" + " = CASE " + Col + " ";

            string context48 = ",JellyFish_Times" + " = CASE " + Col + " ";
            string context49 = ",JellyFish_Bet" + " = CASE " + Col + " ";
            string context50 = ",JellyFish_Win" + " = CASE " + Col + " ";
            string context51 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context55 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context56 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context57 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context58 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context59 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context60 = ",LungPanCrab_Times" + " = CASE " + Col + " ";
            string context61 = ",LungPanCrab_Bet" + " = CASE " + Col + " ";
            string context62 = ",LungPanCrab_Win" + " = CASE " + Col + " ";
            string context63 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context64 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context65 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context66 = ",UnicornWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",UnicornWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",UnicornWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context70 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context71 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context72 = ",Shark_Times" + " = CASE " + Col + " ";
            string context73 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context74 = ",Shark_Win" + " = CASE " + Col + " ";
            string context75 = ",PufferGold_Times" + " = CASE " + Col + " ";
            string context76 = ",PufferGold_Bet" + " = CASE " + Col + " ";
            string context77 = ",PufferGold_Win" + " = CASE " + Col + " ";
            string context78 = ",MoorishIdolGold_Times" + " = CASE " + Col + " ";
            string context79 = ",MoorishIdolGold_Bet" + " = CASE " + Col + " ";
            string context80 = ",MoorishIdolGold_Win" + " = CASE " + Col + " ";
            string context81 = ",ClownFishGold_Times" + " = CASE " + Col + " ";
            string context82 = ",ClownFishGold_Bet" + " = CASE " + Col + " ";
            string context83 = ",ClownFishGold_Win" + " = CASE " + Col + " ";
            string context84 = ",PufferBig_Times" + " = CASE " + Col + " ";
            string context85 = ",PufferBig_Bet" + " = CASE " + Col + " ";
            string context86 = ",PufferBig_Win" + " = CASE " + Col + " ";
            string context87 = ",MoorishIdolBig_Times" + " = CASE " + Col + " ";
            string context88 = ",MoorishIdolBig_Bet" + " = CASE " + Col + " ";
            string context89 = ",MoorishIdolBig_Win" + " = CASE " + Col + " ";
            string context90 = ",ClownFishBig_Times" + " = CASE " + Col + " ";
            string context91 = ",ClownFishBig_Bet" + " = CASE " + Col + " ";
            string context92 = ",ClownFishBig_Win" + " = CASE " + Col + " ";
            string context93 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context94 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context95 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context96 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context97 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context98 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context99 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context100 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context101 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context102 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context103 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context104 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context105 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context106 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context107 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context108 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context109 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context111 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context112 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context113 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context114 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context115 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context116 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context117 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context118 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context119 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context120 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context121 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context122 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context123 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context124 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context125 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context126 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context127 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context128 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context129 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context130 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context131 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Bet"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Win"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Bet"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantOctopus_Win"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireTurtle_Times"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireTurtle_Bet"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireTurtle_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + 
                context27 +
                context28 + context29 + context36 +
                context37 + context38 + context42 + context43 + context44 +
                context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 +
                context132;


            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing5Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context24 = ",Phoenix_Times" + " = CASE " + Col + " ";
            string context25 = ",Phoenix_Bet" + " = CASE " + Col + " ";
            string context26 = ",Phoenix_Win" + " = CASE " + Col + " ";
            string context30 = ",Behemoth_Times" + " = CASE " + Col + " ";
            string context31 = ",Behemoth_Bet" + " = CASE " + Col + " ";
            string context32 = ",Behemoth_Win" + " = CASE " + Col + " ";
            string context39 = ",GiantCrab_Times" + " = CASE " + Col + " ";
            string context40 = ",GiantCrab_Bet" + " = CASE " + Col + " ";
            string context41 = ",GiantCrab_Win" + " = CASE " + Col + " ";

            string context48 = ",JellyFish_Times" + " = CASE " + Col + " ";
            string context49 = ",JellyFish_Bet" + " = CASE " + Col + " ";
            string context50 = ",JellyFish_Win" + " = CASE " + Col + " ";
            string context51 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context55 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context56 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context57 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context58 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context59 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context60 = ",LungPanCrab_Times" + " = CASE " + Col + " ";
            string context61 = ",LungPanCrab_Bet" + " = CASE " + Col + " ";
            string context62 = ",LungPanCrab_Win" + " = CASE " + Col + " ";
            string context63 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context64 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context65 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context66 = ",UnicornWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",UnicornWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",UnicornWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context70 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context71 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context72 = ",Shark_Times" + " = CASE " + Col + " ";
            string context73 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context74 = ",Shark_Win" + " = CASE " + Col + " ";
            string context75 = ",PufferGold_Times" + " = CASE " + Col + " ";
            string context76 = ",PufferGold_Bet" + " = CASE " + Col + " ";
            string context77 = ",PufferGold_Win" + " = CASE " + Col + " ";
            string context78 = ",MoorishIdolGold_Times" + " = CASE " + Col + " ";
            string context79 = ",MoorishIdolGold_Bet" + " = CASE " + Col + " ";
            string context80 = ",MoorishIdolGold_Win" + " = CASE " + Col + " ";
            string context81 = ",ClownFishGold_Times" + " = CASE " + Col + " ";
            string context82 = ",ClownFishGold_Bet" + " = CASE " + Col + " ";
            string context83 = ",ClownFishGold_Win" + " = CASE " + Col + " ";
            string context84 = ",PufferBig_Times" + " = CASE " + Col + " ";
            string context85 = ",PufferBig_Bet" + " = CASE " + Col + " ";
            string context86 = ",PufferBig_Win" + " = CASE " + Col + " ";
            string context87 = ",MoorishIdolBig_Times" + " = CASE " + Col + " ";
            string context88 = ",MoorishIdolBig_Bet" + " = CASE " + Col + " ";
            string context89 = ",MoorishIdolBig_Win" + " = CASE " + Col + " ";
            string context90 = ",ClownFishBig_Times" + " = CASE " + Col + " ";
            string context91 = ",ClownFishBig_Bet" + " = CASE " + Col + " ";
            string context92 = ",ClownFishBig_Win" + " = CASE " + Col + " ";
            string context93 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context94 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context95 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context96 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context97 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context98 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context99 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context100 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context101 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context102 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context103 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context104 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context105 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context106 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context107 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context108 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context109 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context111 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context112 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context113 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context114 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context115 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context116 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context117 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context118 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context119 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context120 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context121 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context122 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context123 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context124 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context125 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context126 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context127 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context128 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context129 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context130 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context131 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Bet"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Phoenix_Win"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Bet"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Behemoth_Win"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrab_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrab_Bet"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrab_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + 
                context24 + context25 + context26 + 
                context30 + context31 + context32 + 
                context39 + context40 + context41 + 
                context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 +
                context132;


            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing6Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context18 = ",ThunderDragon_Times" + " = CASE " + Col + " ";
            string context19 = ",ThunderDragon_Bet" + " = CASE " + Col + " ";
            string context20 = ",ThunderDragon_Win" + " = CASE " + Col + " ";
            string context21 = ",PurpleDragon_Times" + " = CASE " + Col + " ";
            string context22 = ",PurpleDragon_Bet" + " = CASE " + Col + " ";
            string context23 = ",PurpleDragon_Win" + " = CASE " + Col + " ";
            string context33 = ",GiantCrocodile_Times" + " = CASE " + Col + " ";
            string context34 = ",GiantCrocodile_Bet" + " = CASE " + Col + " ";
            string context35 = ",GiantCrocodile_Win" + " = CASE " + Col + " ";

            string context48 = ",JellyFish_Times" + " = CASE " + Col + " ";
            string context49 = ",JellyFish_Bet" + " = CASE " + Col + " ";
            string context50 = ",JellyFish_Win" + " = CASE " + Col + " ";
            string context51 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context55 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context56 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context57 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context58 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context59 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context60 = ",LungPanCrab_Times" + " = CASE " + Col + " ";
            string context61 = ",LungPanCrab_Bet" + " = CASE " + Col + " ";
            string context62 = ",LungPanCrab_Win" + " = CASE " + Col + " ";
            string context63 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context64 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context65 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context66 = ",UnicornWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",UnicornWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",UnicornWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context70 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context71 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context72 = ",Shark_Times" + " = CASE " + Col + " ";
            string context73 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context74 = ",Shark_Win" + " = CASE " + Col + " ";
            string context75 = ",PufferGold_Times" + " = CASE " + Col + " ";
            string context76 = ",PufferGold_Bet" + " = CASE " + Col + " ";
            string context77 = ",PufferGold_Win" + " = CASE " + Col + " ";
            string context78 = ",MoorishIdolGold_Times" + " = CASE " + Col + " ";
            string context79 = ",MoorishIdolGold_Bet" + " = CASE " + Col + " ";
            string context80 = ",MoorishIdolGold_Win" + " = CASE " + Col + " ";
            string context81 = ",ClownFishGold_Times" + " = CASE " + Col + " ";
            string context82 = ",ClownFishGold_Bet" + " = CASE " + Col + " ";
            string context83 = ",ClownFishGold_Win" + " = CASE " + Col + " ";
            string context84 = ",PufferBig_Times" + " = CASE " + Col + " ";
            string context85 = ",PufferBig_Bet" + " = CASE " + Col + " ";
            string context86 = ",PufferBig_Win" + " = CASE " + Col + " ";
            string context87 = ",MoorishIdolBig_Times" + " = CASE " + Col + " ";
            string context88 = ",MoorishIdolBig_Bet" + " = CASE " + Col + " ";
            string context89 = ",MoorishIdolBig_Win" + " = CASE " + Col + " ";
            string context90 = ",ClownFishBig_Times" + " = CASE " + Col + " ";
            string context91 = ",ClownFishBig_Bet" + " = CASE " + Col + " ";
            string context92 = ",ClownFishBig_Win" + " = CASE " + Col + " ";
            string context93 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context94 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context95 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context96 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context97 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context98 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context99 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context100 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context101 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context102 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context103 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context104 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context105 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context106 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context107 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context108 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context109 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context111 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context112 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context113 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context114 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context115 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context116 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context117 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context118 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context119 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context120 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context121 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context122 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context123 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context124 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context125 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context126 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context127 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context128 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context129 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context130 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context131 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Bet"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Win"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Bet"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Win"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrocodile_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrocodile_Bet"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCrocodile_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JellyFish_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LungPanCrab_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["UnicornWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferGold_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolGold_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishGold_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PufferBig_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdolBig_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFishBig_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + 
                context33 + context34 + context35 + 
                context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 +
                context132;


            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing7Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context18 = ",RockSkull_Times" + " = CASE " + Col + " ";
            string context19 = ",RockSkull_Bet" + " = CASE " + Col + " ";
            string context20 = ",RockSkull_Win" + " = CASE " + Col + " ";
            string context21 = ",VampireKing_Times" + " = CASE " + Col + " ";
            string context22 = ",VampireKing_Bet" + " = CASE " + Col + " ";
            string context23 = ",VampireKing_Win" + " = CASE " + Col + " ";
            string context24 = ",GhostShip_Times" + " = CASE " + Col + " ";
            string context25 = ",GhostShip_Bet" + " = CASE " + Col + " ";
            string context26 = ",GhostShip_Win" + " = CASE " + Col + " ";
            string context27 = ",PurpleDragon_Times" + " = CASE " + Col + " ";
            string context28 = ",PurpleDragon_Bet" + " = CASE " + Col + " ";
            string context29 = ",PurpleDragon_Win" + " = CASE " + Col + " ";
            string context30 = ",IcePhoenix_Times" + " = CASE " + Col + " ";
            string context31 = ",IcePhoenix_Bet" + " = CASE " + Col + " ";
            string context32 = ",IcePhoenix_Win" + " = CASE " + Col + " ";
            string context33 = ",FireDragon_Times" + " = CASE " + Col + " ";
            string context34 = ",FireDragon_Bet" + " = CASE " + Col + " ";
            string context35 = ",FireDragon_Win" + " = CASE " + Col + " ";
            string context36 = ",GeneralLobster_Times" + " = CASE " + Col + " ";
            string context37 = ",GeneralLobster_Bet" + " = CASE " + Col + " ";
            string context38 = ",GeneralLobster_Win" + " = CASE " + Col + " ";
            string context39 = ",GiantSquid_Times" + " = CASE " + Col + " ";
            string context40 = ",GiantSquid_Bet" + " = CASE " + Col + " ";
            string context41 = ",GiantSquid_Win" + " = CASE " + Col + " ";
            string context42 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context43 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context44 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context45 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context46 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context47 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context48 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context49 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context50 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context51 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context52 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context53 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context54 = ",GiantNemo_Times" + " = CASE " + Col + " ";
            string context55 = ",GiantNemo_Bet" + " = CASE " + Col + " ";
            string context56 = ",GiantNemo_Win" + " = CASE " + Col + " ";
            string context57 = ",GiantCoralFish_Times" + " = CASE " + Col + " ";
            string context58 = ",GiantCoralFish_Bet" + " = CASE " + Col + " ";
            string context59 = ",GiantCoralFish_Win" + " = CASE " + Col + " ";
            string context60 = ",GiantPuffer_Times" + " = CASE " + Col + " ";
            string context61 = ",GiantPuffer_Bet" + " = CASE " + Col + " ";
            string context62 = ",GiantPuffer_Win" + " = CASE " + Col + " ";
            string context63 = ",HumpbackWhale_Times" + " = CASE " + Col + " ";
            string context64 = ",HumpbackWhale_Bet" + " = CASE " + Col + " ";
            string context65 = ",HumpbackWhale_Win" + " = CASE " + Col + " ";
            string context66 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context67 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context68 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context69 = ",Shark_Times" + " = CASE " + Col + " ";
            string context70 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context71 = ",Shark_Win" + " = CASE " + Col + " ";
            string context72 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context73 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context74 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context75 = ",Stingray_Times" + " = CASE " + Col + " ";
            string context76 = ",Stingray_Bet" + " = CASE " + Col + " ";
            string context77 = ",Stingray_Win" + " = CASE " + Col + " ";
            string context78 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context79 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context80 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context81 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context82 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context83 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context84 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context85 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context86 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context87 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context88 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context89 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context90 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context91 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context92 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context93 = ",YellowTang_Times" + " = CASE " + Col + " ";
            string context94 = ",YellowTang_Bet" + " = CASE " + Col + " ";
            string context95 = ",YellowTang_Win" + " = CASE " + Col + " ";
            string context96 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context97 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context98 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context99 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context100 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context101 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context102 = ",MoorishIdol_Times" + " = CASE " + Col + " ";
            string context103 = ",MoorishIdol_Bet" + " = CASE " + Col + " ";
            string context104 = ",MoorishIdol_Win" + " = CASE " + Col + " ";
            string context105 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context106 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context107 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context108 = ",Bat_Times" + " = CASE " + Col + " ";
            string context109 = ",Bat_Bet" + " = CASE " + Col + " ";
            string context110 = ",Bat_Win" + " = CASE " + Col + " ";
            string context111 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";
                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RockSkull_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RockSkull_Bet"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RockSkull_Win"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["VampireKing_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["VampireKing_Bet"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["VampireKing_Win"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GhostShip_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GhostShip_Bet"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GhostShip_Win"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Bet"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Win"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IcePhoenix_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IcePhoenix_Bet"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IcePhoenix_Win"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireDragon_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireDragon_Bet"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FireDragon_Win"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GeneralLobster_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GeneralLobster_Bet"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GeneralLobster_Win"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantSquid_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantSquid_Bet"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantSquid_Win"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context45 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context46 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context47 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantNemo_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantNemo_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantNemo_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCoralFish_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCoralFish_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCoralFish_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantPuffer_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantPuffer_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantPuffer_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HumpbackWhale_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HumpbackWhale_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HumpbackWhale_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Stingray_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["YellowTang_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoorishIdol_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bat_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bat_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Bat_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";
            context45 += "END";
            context46 += "END";
            context47 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41 + context42 + context43 + context44 + context45 +
                context46 + context47 + context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        /// <summary>更新表單</summary>
        public int OceanKing8Detail(List<OperTionDBBox> data)
        {
            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalBet" + " = CASE " + Col + " ";
            string context2 = ",TotalWin" + " = CASE " + Col + " ";
            string context3 = ",TotalSurplus" + " = CASE " + Col + " ";
            string context4 = ",GameTimes" + " = CASE " + Col + " ";
            string context5 = ",WinTimes" + " = CASE " + Col + " ";
            string context6 = ",Super_Times" + " = CASE " + Col + " ";
            string context7 = ",Super_Bet" + " = CASE " + Col + " ";
            string context8 = ",Super_Win" + " = CASE " + Col + " ";
            string context9 = ",Mega_Times" + " = CASE " + Col + " ";
            string context10 = ",Mega_Bet" + " = CASE " + Col + " ";
            string context11 = ",Mega_Win" + " = CASE " + Col + " ";
            string context12 = ",Major_Times" + " = CASE " + Col + " ";
            string context13 = ",Major_Bet" + " = CASE " + Col + " ";
            string context14 = ",Major_Win" + " = CASE " + Col + " ";
            string context15 = ",Minor_Times" + " = CASE " + Col + " ";
            string context16 = ",Minor_Bet" + " = CASE " + Col + " ";
            string context17 = ",Minor_Win" + " = CASE " + Col + " ";

            string context18 = ",Buddha_Times" + " = CASE " + Col + " ";
            string context19 = ",Buddha_Bet" + " = CASE " + Col + " ";
            string context20 = ",Buddha_Win" + " = CASE " + Col + " ";
            string context21 = ",Poseidon_Times" + " = CASE " + Col + " ";
            string context22 = ",Poseidon_Bet" + " = CASE " + Col + " ";
            string context23 = ",Poseidon_Win" + " = CASE " + Col + " ";
            string context24 = ",IceDragon_Times" + " = CASE " + Col + " ";
            string context25 = ",IceDragon_Bet" + " = CASE " + Col + " ";
            string context26 = ",IceDragon_Win" + " = CASE " + Col + " ";
            string context27 = ",GoldenSpiderCrab_Times" + " = CASE " + Col + " ";
            string context28 = ",GoldenSpiderCrab_Bet" + " = CASE " + Col + " ";
            string context29 = ",GoldenSpiderCrab_Win" + " = CASE " + Col + " ";
            string context30 = ",IcePhoenix_Times" + " = CASE " + Col + " ";
            string context31 = ",IcePhoenix_Bet" + " = CASE " + Col + " ";
            string context32 = ",IcePhoenix_Win" + " = CASE " + Col + " ";
            string context33 = ",Mermaid_Times" + " = CASE " + Col + " ";
            string context34 = ",Mermaid_Bet" + " = CASE " + Col + " ";
            string context35 = ",Mermaid_Win" + " = CASE " + Col + " ";
            string context36 = ",ThunderDragon_Times" + " = CASE " + Col + " ";
            string context37 = ",ThunderDragon_Bet" + " = CASE " + Col + " ";
            string context38 = ",ThunderDragon_Win" + " = CASE " + Col + " ";
            string context39 = ",ThunderCrab_Times" + " = CASE " + Col + " ";
            string context40 = ",ThunderCrab_Bet" + " = CASE " + Col + " ";
            string context41 = ",ThunderCrab_Win" + " = CASE " + Col + " ";
            string context42 = ",BombCrab_Times" + " = CASE " + Col + " ";
            string context43 = ",BombCrab_Bet" + " = CASE " + Col + " ";
            string context44 = ",BombCrab_Win" + " = CASE " + Col + " ";
            string context45 = ",DrillCrab_Times" + " = CASE " + Col + " ";
            string context46 = ",DrillCrab_Bet" + " = CASE " + Col + " ";
            string context47 = ",DrillCrab_Win" + " = CASE " + Col + " ";
            string context48 = ",LaserCrab_Times" + " = CASE " + Col + " ";
            string context49 = ",LaserCrab_Bet" + " = CASE " + Col + " ";
            string context50 = ",LaserCrab_Win" + " = CASE " + Col + " ";
            string context51 = ",Lightning_Times" + " = CASE " + Col + " ";
            string context52 = ",Lightning_Bet" + " = CASE " + Col + " ";
            string context53 = ",Lightning_Win" + " = CASE " + Col + " ";
            string context54 = ",Tornato_Times" + " = CASE " + Col + " ";
            string context55 = ",Tornato_Bet" + " = CASE " + Col + " ";
            string context56 = ",Tornato_Win" + " = CASE " + Col + " ";
            string context57 = ",HumpbackWhale_Times" + " = CASE " + Col + " ";
            string context58 = ",HumpbackWhale_Bet" + " = CASE " + Col + " ";
            string context59 = ",HumpbackWhale_Win" + " = CASE " + Col + " ";
            string context60 = ",KillerWhale_Times" + " = CASE " + Col + " ";
            string context61 = ",KillerWhale_Bet" + " = CASE " + Col + " ";
            string context62 = ",KillerWhale_Win" + " = CASE " + Col + " ";
            string context63 = ",Shark_Times" + " = CASE " + Col + " ";
            string context64 = ",Shark_Bet" + " = CASE " + Col + " ";
            string context65 = ",Shark_Win" + " = CASE " + Col + " ";
            string context66 = ",GiantPuffer_Times" + " = CASE " + Col + " ";
            string context67 = ",GiantPuffer_Bet" + " = CASE " + Col + " ";
            string context68 = ",GiantPuffer_Win" + " = CASE " + Col + " ";
            string context69 = ",GiantNemo_Times" + " = CASE " + Col + " ";
            string context70 = ",GiantNemo_Bet" + " = CASE " + Col + " ";
            string context71 = ",GiantNemo_Win" + " = CASE " + Col + " ";
            string context72 = ",GiantCoralFish_Times" + " = CASE " + Col + " ";
            string context73 = ",GiantCoralFish_Bet" + " = CASE " + Col + " ";
            string context74 = ",GiantCoralFish_Win" + " = CASE " + Col + " ";
            string context75 = ",Mobula_Times" + " = CASE " + Col + " ";
            string context76 = ",Mobula_Bet" + " = CASE " + Col + " ";
            string context77 = ",Mobula_Win" + " = CASE " + Col + " ";
            string context78 = ",SawtoothShark_Times" + " = CASE " + Col + " ";
            string context79 = ",SawtoothShark_Bet" + " = CASE " + Col + " ";
            string context80 = ",SawtoothShark_Win" + " = CASE " + Col + " ";
            string context81 = ",Turtle_Times" + " = CASE " + Col + " ";
            string context82 = ",Turtle_Bet" + " = CASE " + Col + " ";
            string context83 = ",Turtle_Win" + " = CASE " + Col + " ";
            string context84 = ",AnglerFish_Times" + " = CASE " + Col + " ";
            string context85 = ",AnglerFish_Bet" + " = CASE " + Col + " ";
            string context86 = ",AnglerFish_Win" + " = CASE " + Col + " ";
            string context87 = ",Octopus_Times" + " = CASE " + Col + " ";
            string context88 = ",Octopus_Bet" + " = CASE " + Col + " ";
            string context89 = ",Octopus_Win" + " = CASE " + Col + " ";
            string context90 = ",SwordFish_Times" + " = CASE " + Col + " ";
            string context91 = ",SwordFish_Bet" + " = CASE " + Col + " ";
            string context92 = ",SwordFish_Win" + " = CASE " + Col + " ";
            string context93 = ",Lobster_Times" + " = CASE " + Col + " ";
            string context94 = ",Lobster_Bet" + " = CASE " + Col + " ";
            string context95 = ",Lobster_Win" + " = CASE " + Col + " ";
            string context96 = ",FlatFish_Times" + " = CASE " + Col + " ";
            string context97 = ",FlatFish_Bet" + " = CASE " + Col + " ";
            string context98 = ",FlatFish_Win" + " = CASE " + Col + " ";
            string context99 = ",Pterois_Times" + " = CASE " + Col + " ";
            string context100 = ",Pterois_Bet" + " = CASE " + Col + " ";
            string context101 = ",Pterois_Win" + " = CASE " + Col + " ";
            string context102 = ",Puffer_Times" + " = CASE " + Col + " ";
            string context103 = ",Puffer_Bet" + " = CASE " + Col + " ";
            string context104 = ",Puffer_Win" + " = CASE " + Col + " ";
            string context105 = ",Carp_Times" + " = CASE " + Col + " ";
            string context106 = ",Carp_Bet" + " = CASE " + Col + " ";
            string context107 = ",Carp_Win" + " = CASE " + Col + " ";
            string context108 = ",ClownFish_Times" + " = CASE " + Col + " ";
            string context109 = ",ClownFish_Bet" + " = CASE " + Col + " ";
            string context110 = ",ClownFish_Win" + " = CASE " + Col + " ";
            string context111 = ",FlyingFish_Times" + " = CASE " + Col + " ";
            string context112 = ",FlyingFish_Bet" + " = CASE " + Col + " ";
            string context113 = ",FlyingFish_Win" + " = CASE " + Col + " ";
            string context114 = ",PurpleDragon_Times" + " = CASE " + Col + " ";
            string context115 = ",PurpleDragon_Bet" + " = CASE " + Col + " ";
            string context116 = ",PurpleDragon_Win" + " = CASE " + Col + " ";
            string context117 = ",GeneralLobster_Times" + " = CASE " + Col + " ";
            string context118 = ",GeneralLobster_Bet" + " = CASE " + Col + " ";
            string context119 = ",GeneralLobster_Win" + " = CASE " + Col + " ";
            string context120 = ",GiantSquid_Times" + " = CASE " + Col + " ";
            string context121 = ",GiantSquid_Bet" + " = CASE " + Col + " ";
            string context122 = ",GiantSquid_Win" + " = CASE " + Col + " ";
            string context123 = ",DragonBoat_Times" + " = CASE " + Col + " ";
            string context124 = ",DragonBoat_Bet" + " = CASE " + Col + " ";
            string context125 = ",DragonBoat_Win" + " = CASE " + Col + " ";
            string context126 = ",MoonRabbit_Times" + " = CASE " + Col + " ";
            string context127 = ",MoonRabbit_Bet" + " = CASE " + Col + " ";
            string context128 = ",MoonRabbit_Win" + " = CASE " + Col + " ";
            string context129 = ",LionDance_Times" + " = CASE " + Col + " ";
            string context130 = ",LionDance_Bet" + " = CASE " + Col + " ";
            string context131 = ",LionDance_Win" + " = CASE " + Col + " ";
            string context132 = ",RecDate" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalBet"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalSurplus"] + " ";
                context4 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GameTimes"] + " ";
                context5 += "WHEN " + data[index].Value + " THEN " + data[index].Data["WinTimes"] + " ";
                context6 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Times"] + " ";
                context7 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Bet"] + " ";
                context8 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Super_Win"] + " ";
                context9 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Times"] + " ";
                context10 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Bet"] + " ";
                context11 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mega_Win"] + " ";
                context12 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Times"] + " ";
                context13 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Bet"] + " ";
                context14 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Major_Win"] + " ";
                context15 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Times"] + " ";
                context16 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Bet"] + " ";
                context17 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Minor_Win"] + " ";

                context18 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Buddha_Times"] + " ";
                context19 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Buddha_Bet"] + " ";
                context20 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Buddha_Win"] + " ";
                context21 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Poseidon_Times"] + " ";
                context22 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Poseidon_Bet"] + " ";
                context23 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Poseidon_Win"] + " ";
                context24 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IceDragon_Times"] + " ";
                context25 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IceDragon_Bet"] + " ";
                context26 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IceDragon_Win"] + " ";
                context27 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GoldenSpiderCrab_Times"] + " ";
                context28 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GoldenSpiderCrab_Bet"] + " ";
                context29 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GoldenSpiderCrab_Win"] + " ";
                context30 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IcePhoenix_Times"] + " ";
                context31 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IcePhoenix_Bet"] + " ";
                context32 += "WHEN " + data[index].Value + " THEN " + data[index].Data["IcePhoenix_Win"] + " ";
                context33 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Times"] + " ";
                context34 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Bet"] + " ";
                context35 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mermaid_Win"] + " ";
                context36 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Times"] + " ";
                context37 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Bet"] + " ";
                context38 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderDragon_Win"] + " ";
                context39 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Times"] + " ";
                context40 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Bet"] + " ";
                context41 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ThunderCrab_Win"] + " ";
                context42 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Times"] + " ";
                context43 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Bet"] + " ";
                context44 += "WHEN " + data[index].Value + " THEN " + data[index].Data["BombCrab_Win"] + " ";
                context45 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Times"] + " ";
                context46 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Bet"] + " ";
                context47 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DrillCrab_Win"] + " ";
                context48 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Times"] + " ";
                context49 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Bet"] + " ";
                context50 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LaserCrab_Win"] + " ";
                context51 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lightning_Times"] + " ";
                context52 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lightning_Bet"] + " ";
                context53 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lightning_Win"] + " ";
                context54 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Tornato_Times"] + " ";
                context55 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Tornato_Bet"] + " ";
                context56 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Tornato_Win"] + " ";
                context57 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HumpbackWhale_Times"] + " ";
                context58 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HumpbackWhale_Bet"] + " ";
                context59 += "WHEN " + data[index].Value + " THEN " + data[index].Data["HumpbackWhale_Win"] + " ";
                context60 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Times"] + " ";
                context61 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Bet"] + " ";
                context62 += "WHEN " + data[index].Value + " THEN " + data[index].Data["KillerWhale_Win"] + " ";
                context63 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Times"] + " ";
                context64 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Bet"] + " ";
                context65 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Shark_Win"] + " ";
                context66 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantPuffer_Times"] + " ";
                context67 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantPuffer_Bet"] + " ";
                context68 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantPuffer_Win"] + " ";
                context69 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantNemo_Times"] + " ";
                context70 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantNemo_Bet"] + " ";
                context71 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantNemo_Win"] + " ";
                context72 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCoralFish_Times"] + " ";
                context73 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCoralFish_Bet"] + " ";
                context74 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantCoralFish_Win"] + " ";
                context75 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Times"] + " ";
                context76 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Bet"] + " ";
                context77 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Mobula_Win"] + " ";
                context78 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SawtoothShark_Times"] + " ";
                context79 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SawtoothShark_Bet"] + " ";
                context80 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SawtoothShark_Win"] + " ";
                context81 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Times"] + " ";
                context82 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Bet"] + " ";
                context83 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Turtle_Win"] + " ";
                context84 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Times"] + " ";
                context85 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Bet"] + " ";
                context86 += "WHEN " + data[index].Value + " THEN " + data[index].Data["AnglerFish_Win"] + " ";
                context87 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Times"] + " ";
                context88 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Bet"] + " ";
                context89 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Octopus_Win"] + " ";
                context90 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Times"] + " ";
                context91 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Bet"] + " ";
                context92 += "WHEN " + data[index].Value + " THEN " + data[index].Data["SwordFish_Win"] + " ";
                context93 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Times"] + " ";
                context94 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Bet"] + " ";
                context95 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Lobster_Win"] + " ";
                context96 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlatFish_Times"] + " ";
                context97 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlatFish_Bet"] + " ";
                context98 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlatFish_Win"] + " ";
                context99 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Times"] + " ";
                context100 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Bet"] + " ";
                context101 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Pterois_Win"] + " ";
                context102 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Times"] + " ";
                context103 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Bet"] + " ";
                context104 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Puffer_Win"] + " ";
                context105 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Carp_Times"] + " ";
                context106 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Carp_Bet"] + " ";
                context107 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Carp_Win"] + " ";
                context108 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Times"] + " ";
                context109 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Bet"] + " ";
                context110 += "WHEN " + data[index].Value + " THEN " + data[index].Data["ClownFish_Win"] + " ";
                context111 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Times"] + " ";
                context112 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Bet"] + " ";
                context113 += "WHEN " + data[index].Value + " THEN " + data[index].Data["FlyingFish_Win"] + " ";
                context114 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Times"] + " ";
                context115 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Bet"] + " ";
                context116 += "WHEN " + data[index].Value + " THEN " + data[index].Data["PurpleDragon_Win"] + " ";
                context117 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GeneralLobster_Times"] + " ";
                context118 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GeneralLobster_Bet"] + " ";
                context119 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GeneralLobster_Win"] + " ";
                context120 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantSquid_Times"] + " ";
                context121 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantSquid_Bet"] + " ";
                context122 += "WHEN " + data[index].Value + " THEN " + data[index].Data["GiantSquid_Win"] + " ";
                context123 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DragonBoat_Times"] + " ";
                context124 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DragonBoat_Bet"] + " ";
                context125 += "WHEN " + data[index].Value + " THEN " + data[index].Data["DragonBoat_Win"] + " ";
                context126 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoonRabbit_Times"] + " ";
                context127 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoonRabbit_Bet"] + " ";
                context128 += "WHEN " + data[index].Value + " THEN " + data[index].Data["MoonRabbit_Win"] + " ";
                context129 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LionDance_Times"] + " ";
                context130 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LionDance_Bet"] + " ";
                context131 += "WHEN " + data[index].Value + " THEN " + data[index].Data["LionDance_Win"] + " ";
                context132 += "WHEN " + data[index].Value + " THEN " + data[index].Data["RecDate"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";
            context4 += "END";
            context5 += "END";
            context6 += "END";
            context7 += "END";
            context8 += "END";
            context9 += "END";
            context10 += "END";
            context11 += "END";
            context12 += "END";
            context13 += "END";
            context14 += "END";
            context15 += "END";
            context16 += "END";
            context17 += "END";
            context18 += "END";
            context19 += "END";
            context20 += "END";
            context21 += "END";
            context22 += "END";
            context23 += "END";
            context24 += "END";
            context25 += "END";
            context26 += "END";
            context27 += "END";
            context28 += "END";
            context29 += "END";
            context30 += "END";
            context31 += "END";
            context32 += "END";
            context33 += "END";
            context34 += "END";
            context35 += "END";
            context36 += "END";
            context37 += "END";
            context38 += "END";
            context39 += "END";
            context40 += "END";
            context41 += "END";
            context42 += "END";
            context43 += "END";
            context44 += "END";
            context45 += "END";
            context46 += "END";
            context47 += "END";
            context48 += "END";
            context49 += "END";
            context50 += "END";
            context51 += "END";
            context52 += "END";
            context53 += "END";
            context54 += "END";
            context55 += "END";
            context56 += "END";
            context57 += "END";
            context58 += "END";
            context59 += "END";
            context60 += "END";
            context61 += "END";
            context62 += "END";
            context63 += "END";
            context64 += "END";
            context65 += "END";
            context66 += "END";
            context67 += "END";
            context68 += "END";
            context69 += "END";
            context70 += "END";
            context71 += "END";
            context72 += "END";
            context73 += "END";
            context74 += "END";
            context75 += "END";
            context76 += "END";
            context77 += "END";
            context78 += "END";
            context79 += "END";
            context80 += "END";
            context81 += "END";
            context82 += "END";
            context83 += "END";
            context84 += "END";
            context85 += "END";
            context86 += "END";
            context87 += "END";
            context88 += "END";
            context89 += "END";
            context90 += "END";
            context91 += "END";
            context92 += "END";
            context93 += "END";
            context94 += "END";
            context95 += "END";
            context96 += "END";
            context97 += "END";
            context98 += "END";
            context99 += "END";
            context100 += "END";
            context101 += "END";
            context102 += "END";
            context103 += "END";
            context104 += "END";
            context105 += "END";
            context106 += "END";
            context107 += "END";
            context108 += "END";
            context109 += "END";
            context110 += "END";
            context111 += "END";
            context112 += "END";
            context113 += "END";
            context114 += "END";
            context115 += "END";
            context116 += "END";
            context117 += "END";
            context118 += "END";
            context119 += "END";
            context120 += "END";
            context121 += "END";
            context122 += "END";
            context123 += "END";
            context124 += "END";
            context125 += "END";
            context126 += "END";
            context127 += "END";
            context128 += "END";
            context129 += "END";
            context130 += "END";
            context131 += "END";
            context132 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3 + context4 + context5 + context6 + context7 + context8 + context9 +
                context10 + context11 + context12 + context13 + context14 + context15 + context16 + context17 + context18 +
                context19 + context20 + context21 + context22 + context23 + context24 + context25 + context26 + context27 +
                context28 + context29 + context30 + context31 + context32 + context33 + context34 + context35 + context36 +
                context37 + context38 + context39 + context40 + context41 + context42 + context43 + context44 + context45 +
                context46 + context47 + context48 + context49 + context50 + context51 + context52 + context53 + context54 +
                context55 + context56 + context57 + context58 + context59 + context60 + context61 + context62 + context63 +
                context64 + context65 + context66 + context67 + context68 + context69 + context70 + context71 + context72 +
                context73 + context74 + context75 + context76 + context77 + context78 + context79 + context80 + context81 +
                context82 + context83 + context84 + context85 + context86 + context87 + context88 + context89 + context90 +
                context91 + context92 + context93 + context94 + context95 + context96 + context97 + context98 + context99 +
                context100 + context101 + context102 + context103 + context104 + context105 + context106 + context107 +
                context108 + context109 + context110 + context111 + context112 + context113 + context114 + context115 +
                context116 + context117 + context118 + context119 + context120 + context121 + context122 + context123 +
                context124 + context125 + context126 + context127 + context128 + context129 + context130 + context131 + 
                context132;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();

                    m_cmd.CommandText = this._sql;
                    int result = m_cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
            }

            return -1;
        }
        #endregion


        /// <summary>
        /// 更新表單
        /// </summary>
        public int UpdataUserGameData(List<OperTionDBBox> data)
        {
            Dictionary<string, Dictionary<string, string>> value = new Dictionary<string, Dictionary<string, string>>();

            string TableName = data[0].TableName;
            string Col = data[0].Col;

            string filed1 = "UPDATE " + TableName + " SET ";
            string filed2 = "";
            string filed3 = " WHERE " + Col + " IN (";

            string context1 = "TotalWin" + " = CASE " + Col + " ";
            string context2 = ",Balanceafter" + " = CASE " + Col + " ";
            string context3 = ",JpAccountUID" + " = CASE " + Col + " ";

            for (int index = 0; index < data.Count; index++)
            {
                context1 += "WHEN " + data[index].Value + " THEN " + data[index].Data["TotalWin"] + " ";
                context2 += "WHEN " + data[index].Value + " THEN " + data[index].Data["Balanceafter"] + " ";
                context3 += "WHEN " + data[index].Value + " THEN " + data[index].Data["JpAccountUID"] + " ";

                if (index != 0)
                {
                    filed3 += ",";
                }

                filed3 += data[index].Value;
            }

            context1 += "END";
            context2 += "END";
            context3 += "END";

            filed3 += ")";

            filed2 += context1 + context2 + context3;

            this._sql = filed1 + filed2 + filed3;

            //安全時才可以訪問共享資源,否則掛起,檢測到安全並訪問的同時會上鎖
            this.reOpen();

            if (dbConnection.State == ConnectionState.Open)
            {
                try
                {
                    m_cmd = dbConnection.CreateCommand();



                    m_cmd.CommandText = this._sql;

                    int result = m_cmd.ExecuteNonQuery();

                    return result;
                }
                catch (Exception ex)
                {
                    this.errorMsg(ex);
                    MyConsole.WriteLine("Test UpdataUserGame" + TableName);
                }
                finally
                {
                    this.closeHandle();
                }
                return -1;
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
            this._sql = "DELETE FROM " + tablename + " WHERE " + where;

            this.reOpen();

            try
            {
                m_cmd = dbConnection.CreateCommand();

                m_cmd.CommandText = this._sql;
                int result = m_cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                this.errorMsg(ex);
                MyConsole.WriteLine("Test delete" + tablename);
            }
            finally
            {
                this.closeHandle();
            }
            return -1;
        }


        /************************************************************************/
        /* 如果连接已经关闭就重新连接记录集是打开状态的关闭*/
        /************************************************************************/
        private void reOpen()
        {
            try
            {
                this.closeHandle();
                // 防止網路或其他情況下連接斷開時重新連接
                if (dbConnection == null || dbConnection.State != ConnectionState.Open)
                {
                    if (dbConnection != null)
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                        dbConnection = null;
                    }
                    dbConnection = new MySqlConnection(this.connstr);
                    dbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("reOpen:" + ex.Message);
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
                if (m_reader != null)
                {
                    if (m_reader.IsClosed == false)
                    {
                        m_reader.Close();
                    }
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

        public void CloseCoon()
        {
            try
            {
                if (m_cmd != null)
                {
                    m_cmd.Dispose();
                }
                if (m_reader != null)
                {
                    if (m_reader.IsClosed == false)
                    {
                        m_reader.Close();
                    }
                    
                    m_reader.Dispose();
                }
                if (dbConnection != null)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
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
                dbConnection = null;
            }
        }

        public ConnectionState GetStatus()
        {
            return dbConnection.State;
        }
     }
}
