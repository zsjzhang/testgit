using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ActivityResult
    {
        /// <summary>
        /// 参与结果（-1已参与、0虚拟、1实物）
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息记录
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// 奖品信息
        /// </summary>
        public PrizesInfo PrizesInfo { get; set; }
        /// <summary>
        /// 获奖信息
        /// </summary>
        public WinningInfo WinningInfo { get; set; }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="code">（0已参与、1实物、2虚拟物品）</param>
        /// <param name="message"></param>
        public void Info(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        /// <summary>
        /// 记录结果
        /// </summary>
        /// <param name="join"></param>
        /// <param name="prize"></param>
        /// <param name="winning"></param>
        public void Result(PrizesInfo prize, WinningInfo winning)
        {
            this.PrizesInfo = prize;
            this.WinningInfo = winning;
        }
    }
}
