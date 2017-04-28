using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public sealed class DbHelp
    {
        #region ==== 构造函数 ====

        private DbHelp()
        { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                con.Open();
                return con.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="totalCount"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryMultiple<T>(string sql, out int totalCount, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                con.Open();
                var multi = con.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
                totalCount = multi.Read<int>().Single();
                return multi.Read<T>();
            }
        }

        public static T QueryOne<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            var dataResult = Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            return dataResult != null && dataResult.Count() > 0 ? dataResult.ToList<T>()[0] : null;
        }


        public static TReturn QueryOne<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) where TReturn : class
        {
            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                con.Open();
                return con.Query<TFirst, TSecond, TThird, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType).SingleOrDefault();
            }
        }

        public static int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {

            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                con.Open();
                return con.Execute(sql, param, transaction, commandTimeout, commandType);
            }
        }

        public static T ExecuteScalar<T>(string sql, object param = null)
        {
            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                con.Open();
                return con.ExecuteScalar<T>(sql, param);
            }
        }


        public static T ExecuteScalarWithTran<T>(string sql, object param = null, bool userTran = false)
        {
            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                con.Open();
                IDbTransaction dbTransaction = null;
                if (userTran)
                {
                    dbTransaction = con.BeginTransaction();
                }
                try
                {
                    var result = con.ExecuteScalar<T>(sql, param, dbTransaction);
                    if (userTran)
                    {
                        dbTransaction.Commit();
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    if (userTran)
                    {
                        dbTransaction.Rollback();
                    }
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static T Execute<T>(string command, Dictionary<string, object> paras)
        {
            using (IDbConnection con = DbConnectionFactory.CreateConnection())
            {
                IDbCommand com = con.CreateCommand();
                com.CommandText = command;
                com.CommandType = CommandType.StoredProcedure;

                if (paras != null)
                {
                    foreach (var item in paras.Keys)
                    {
                        IDbDataParameter para = com.CreateParameter();
                        para.Value = paras[item];
                        para.ParameterName = item;
                        com.Parameters.Add(para);
                    }
                }

                con.Open();
                return (T)com.ExecuteScalar();
            }
        }

        #endregion
    }
}
