using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 请求webapi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestApi<T> where T : class 
    {
        private Dictionary<string, Task<T>> _dicTask = new Dictionary<string, Task<T>>();
        public void StartRequest()
        {

        }

        /// <summary>
        /// 添加请求
        /// </summary>
        /// <param name="func">调用的方法</param>
        /// <param name="codeList">id列表</param>
        public void AddRequest(Func<string, T> func, ICollection<string> codeList)
        {
            if (codeList == null)
            {
                throw new ArgumentException("参数错误");
            }

            //Task<T>.Factory.StartNew 方法只能传入object 类型的委托参数，要传入string类型的参数，使用下面的方式转化一下
            Func<object, T> f = (o) =>
            {
                return func(o.ToString()); 
            };

            foreach (var code in codeList)
            {
                if (!_dicTask.ContainsKey(code))
                {
                    _dicTask.Add(code, Task<T>.Factory.StartNew(f, code));
                }
            }
        }

        public T EndRequestAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || !_dicTask.ContainsKey(code))
            {
                throw new ArgumentException("参数错误");
            }

            return _dicTask[code].Result;
        }

    }
}
