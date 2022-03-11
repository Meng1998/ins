using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallTools.Ser
{
    class pgsql
    {
        private static NpgsqlConnection conn;
        public static Boolean GetSql(String sql, String sqlstr)
        {
            try
            {
                conn = new NpgsqlConnection(sqlstr);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                NpgsqlCommand cmdy = new NpgsqlCommand(sql, conn);
                cmdy.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                conn.Close();
                if (ex.Data["Code"].ToString() == "42P06"|| ex.Data["MessageText"].ToString().Contains("已经存在"))
                    return true;
                else
                    return false;
            }
        }
    }
}
