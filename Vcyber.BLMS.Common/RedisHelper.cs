using Newtonsoft.Json;
using RedisBoost;
using RedisBoost.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Vcyber.BLMS.Common
{
    public class RedisExtend : IDisposable
    {
        private IRedisClient _client;
        public RedisExtend()
        {
            RedisClient.DefaultSerializer = new JsonSerializer();
        }
        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();
        }

        public void Connect()
        {
            var connectionString = System.Configuration.ConfigurationManager.AppSettings["Cache_Redis_Configuration"];
            var connectionQuery = connectionString.Split(':');
            _client = RedisClient.ConnectAsync(connectionQuery[0], int.Parse(connectionQuery[1])).Result;
        }
        /// <summary>  
        /// 将指定项目(数组)存储到Set  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="redisClient">Redis客户端实例</param>  
        /// <param name="key">Set键值</param>  
        /// <param name="items">需要存储在此Set中的Item数组</param>  
        /// <returns>存储成功的项目个数</returns>  
        public long SaveSet<T>(string key, T[] items)
        {
            var task = _client.SAddAsync<T>(key, items);
            task.Wait();
            return task.Result;
        }

        /// <summary>  
        /// 得到指定Set中的项目  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="redisClient">Redis客户端实例</param>  
        /// <param name="setName">Set键值</param>  
        /// <returns>此Set中的所有项目，以数组的形式返回</returns>  
        public T[] GetSet<T>(string key)
        {
            List<T> items = new List<T>();

            var t = _client.ExecuteAsync("smembers", key);
            t.Wait();

            if (t.Result.AsMultiBulk().Count() > 0)
            {
                foreach (var item in t.Result.AsMultiBulk())
                {
                    items.Add(item.As<T>());
                }
            }
            return items.ToArray<T>();
        }

        public bool SetContainsItem(string setId, string item)
        {
            var t = _client.SIsMemberAsync<string>(setId, item);
            t.Wait();
            return t.Result == 1 ? true : false;
        }
        public long Incr(string key)
        {
            var t = _client.IncrAsync(key);
            t.Wait();
            return t.Result;
        }
        public long Decr(string key)
        {
            var t = _client.DecrAsync(key);
            t.Wait();
            return t.Result;
        }
        public void Set<T>(string key, T value)
        {
            _client.SetAsync<T>(key, value).Wait();
        }
        /// <summary>
        /// 向list类型数据添加成员，向列表顶部(左侧)添加
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public void AddItemToListLeft(string list, string item)
        {
            _client.LPushAsync(list, Encoding.Default.GetBytes(item)).Wait();
        }
        /// <summary>
        /// 设置hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void SetHashField(string key, string field, string value)
        {
            _client.HSetAsync<string, string>(key, field, value).Wait();
        }
        /// <summary>
        /// 获取key,返回string格式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueString(string key)
        {
            var result = _client.GetAsync(key).Result;
            var value = string.Empty;
            if (!result.IsNull)
            {
                value = Encoding.Default.GetString(result.Value);
            }
            return value;
        }
        /// <summary>
        /// 向集合中添加数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="set"></param>
        public void GetItemToSet(string item, string set)
        {
            SaveSet<string>(item, new string[] { set });
        }
        /// <summary>
        /// 向集合中添加数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="set"></param>
        public string RPop(string key)
        {
            var t = _client.RPopAsync(key);
            t.Wait();
            var value = string.Empty;
            if (!t.Result.IsNull)
            {
                value = Encoding.Default.GetString(t.Result.Value);
            }
            return value;
        }
        /// <summary>
        /// 向集合中添加数据
        /// </summary>
        public string BrPop(string key)
        {
            var t = _client.BrPopAsync(300, key);
            t.Wait();
            var value = string.Empty;
            if (!t.Result.IsNull)
            {
                var r = t.Result;
                var array = r.AsArray<string>();
                if (array.Length > 0)
                {
                    value = array[1];
                }
            }
            return value;
        }
        /// <summary>
        /// 查询哈希set
        /// </summary>
        public string[] HGetAll(string key)
        {
            var t = _client.HGetAllAsync(key);
            t.Wait();
            var value = new string[] { };
            if (!t.Result.IsNull)
            {                
                value = t.Result.AsArray<string>();
            }
            return value;
        }
        public T Hget<T>(string key, string field)
        {
            var t = _client.HGetAsync<string>(key, field);
            T obj = default(T);
            t.Wait();
            var value = string.Empty;
            if (!t.Result.IsNull)
            {
                obj = t.Result.As<T>();
            }
            return obj;
        }

        /// <summary>  
        /// 将指定项目(数组)存储到Set  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="redisClient">Redis客户端实例</param>  
        /// <param name="key">Set键值</param>  
        /// <param name="items">需要存储在此Set中的Item数组</param>  
        /// <returns>存储成功的项目个数</returns>  
        public string GetHashSet(string key, string field)
        {
            var task = _client.HGetAsync<string>(key, field);
            //task.Wait();
            return task.Result.IsNull ? "" : Encoding.UTF8.GetString(task.Result.Value);
        }

        public string Hget(string key, string field)
        {
            var t = _client.HGetAsync<string>(key, field);
            t.Wait();
            var value = string.Empty;
            if (!t.Result.IsNull)
            {
                value = t.Result.As<string>();
            }
            return value;
        }
    }
    public class JsonSerializer : BasicRedisSerializer
    {
        internal static readonly byte[] Null = new byte[] { 0 };
        internal const string DatetimeFormat = "yyyy-MM-dd'T'HH:mm:ss.fffffff";

        #region Serialization

        public override byte[] Serialize(object value)
        {
            if (value == null) return Null;

            var type = value.GetType();

            if (type == typeof(string))
                return SerializeString(value.ToString());
            if (type == typeof(byte[]))
                return value as byte[];
            if (type.IsEnum)
                return SerializeString(value.ToString());
            if (type == typeof(DateTime))
                return SerializeString((value as IFormattable).ToString(DatetimeFormat, CultureInfo.InvariantCulture));
            if (type == typeof(Guid))
                return SerializeString(value.ToString());
            if (type == typeof(int) || type == typeof(long) || type == typeof(byte) || type == typeof(short) ||
                type == typeof(uint) || type == typeof(ulong) || type == typeof(sbyte) || type == typeof(ushort) ||
                type == typeof(bool) || type == typeof(decimal) || type == typeof(double) || type == typeof(char))
                return SerializeString((value as IConvertible).ToString(CultureInfo.InvariantCulture));

            var result = SerializeComplexValue(value);

            if (result.SequenceEqual(Null))
                throw new SerializationException("Serializer returned unexpected result. byte[]{0} value is reserved for NULL");

            return result;
        }

        protected virtual byte[] SerializeComplexValue(object value)
        {
            string json = JsonConvert.SerializeObject(value);
            return Encoding.UTF8.GetBytes(json);
        }

        #endregion

        #region Deserialization

        public override object Deserialize(Type type, byte[] value)
        {
            if (value == null || value.SequenceEqual(Null)) return null;

            if (type == typeof(string))
                return DeserializeToString(value);
            if (type == typeof(byte[]))
                return value;
            if (type.IsEnum)
                return DeserializeToEnum(DeserializeToString(value), type);
            if (type == typeof(DateTime))
                return DateTime.ParseExact(DeserializeToString(value), DatetimeFormat, CultureInfo.InvariantCulture);
            if (type == typeof(Guid))
                return Guid.Parse(DeserializeToString(value));
            if (type == typeof(int))
                return int.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(long))
                return long.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(byte))
                return byte.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(short))
                return short.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(uint))
                return uint.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(ulong))
                return ulong.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(sbyte))
                return sbyte.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(ushort))
                return ushort.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(bool))
                return bool.Parse(DeserializeToString(value));
            if (type == typeof(decimal))
                return decimal.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(double))
                return double.Parse(DeserializeToString(value), CultureInfo.InvariantCulture);
            if (type == typeof(char))
                return DeserializeToString(value)[0];

            return DeserializeComplexValue(type, value);
        }

        protected override object DeserializeComplexValue(Type type, byte[] value)
        {
            string json = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject(json, type);
        }

        #endregion

        private static object DeserializeToEnum(string value, Type enumType)
        {
            try
            {
                return Enum.Parse(enumType, value, true);
            }
            catch (Exception ex)
            {
                throw new SerializationException("Invalid enum value. Enum type: " + enumType.Name, ex);
            }
        }

        private static byte[] SerializeString(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        private static string DeserializeToString(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}
