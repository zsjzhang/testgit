using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Domain
{
    public class WinningInfoApp : IWinningInfoApp
    {
        public List<WinningInfo> GetWinningInfosByActivityId(int activityId)
        {
            return _DbSession.WinningInfoStorager.GetWinningInfosByActivityId(activityId).ToList();
        }

        public bool IsWinningByUIdAndActicityId(int activityId, string userTel)
        {
            return _DbSession.WinningInfoStorager.IsWinningByUIdAndActicityId(activityId, userTel);
        }

        public bool AddWinningInfo(WinningInfo entity)
        {
            int offsetnum = _DbSession.WinningInfoStorager.AddWinningInfo(entity);
            if (offsetnum > 0) return true;
            else return false;
        }


        public List<WinningInfo> GetWinningInfoByActivityId(int activityId, PageData pageData, out int total)
        {
            return _DbSession.WinningInfoStorager.GetWinningInfoByActivityId(activityId, pageData, out total).ToList();
        }

        /// <summary>
        /// 根据ID获取中奖信息
        /// 贾锡安2015-09-10 15:36:40
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WinningInfo GetWinningInfo(int id)
        {
            WinningInfo entity = _DbSession.WinningInfoStorager.GetWinningInfo(id);
            return entity;
        }

        /// <summary>
        /// 修改中奖纪录
        /// 贾锡安2015-09-10 15:38:22
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateWinningInfo(WinningInfo entity)
        {
            bool bol = _DbSession.WinningInfoStorager.UpdateWinningInfo(entity);
            return bol;
        }

        /// <summary>
        /// 获取获奖名单（适合前台显示的方法）
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<WinningModel> GetWinningModelsByActivityId(int activityId)
        {
            List<WinningInfo> _wiList = _DbSession.WinningInfoStorager.GetWinningInfosByActivityId(activityId).ToList();
            List<PrizesInfo> _piList = _DbSession.PrizesInfoStorager.GetPrizesInfosByActivity(activityId).ToList();
            List<WinningModel> _retwmList = new List<WinningModel>();
            if (_wiList == null || _wiList.Count == 0 || _piList == null || _piList.Count == 0) { return _retwmList; }
            _wiList.ForEach(w =>
            {
                WinningModel wm = new WinningModel();
                wm.ActivityId = activityId;
                wm.UserId = w.UserId;
                wm.UserName = w.UserName;
                wm.UserPhone = w.UserTel;
                wm.PrizeID = w.PrizesId;
                PrizesInfo _curPI = _piList.SingleOrDefault(f => f.Id == w.PrizesId);
                wm.PrizeImg = _curPI.Img;
                wm.PrizePrice = _curPI.Price;
                wm.PrizeTitle = _curPI.Title;
                _retwmList.Add(wm);
            });
            return _retwmList;
        }


        public Membership GetMembershipByNameAndTel(string name, string tel)
        {
            return _DbSession.WinningInfoStorager.GetMembershipByNameAndTel(name, tel);
        }

        public ReturnResult ImportWinningInfoData(string path)
        {
            var result = new ReturnResult { IsSuccess = true };

            if (string.IsNullOrEmpty(path))
            {
                result.IsSuccess = false;
                result.Message = "导入出错,请选择正确的数据文件";
                return result;
            }

            DataTable dt = NPOIHelper<QuestionnaireWinning>.ReadExcel(path, Path.GetExtension(path));
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string name = dr["获奖人姓名"].ToString();
                    string phoneNumber = dr["获奖人手机号"].ToString();
                    string prize = dr["奖品"].ToString();
                    QuestionnaireWinning qwEntity = new QuestionnaireWinning();
                    //qwEntity.QuestionnaireId = qId;
                    qwEntity.WName = name;
                    qwEntity.WPhoneNumber = phoneNumber;
                    qwEntity.Prize = prize;
                    _AppContext.QuestionnaireWinningApp.Create(qwEntity);
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = "异常：" + ex.Message.ToString(); ;
                    //return RedirectToAction("WinningIndex", "Questionnaire");
                }
            }

            //StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("../" + path), Encoding.Default);
            //String line;
            //while ((line = sr.ReadLine()) != null)
            //{
            //    SNCard card = new SNCard
            //    {
            //        SNCode = line,
            //        Status = (int)ESNCardStatus.Created,
            //        CreateTime = DateTime.Now
            //    };

            //    bool isSuccess = _DbSession.SNCardStorager.AddSNCard(card);

            //    if (!isSuccess)
            //    {
            //        result.IsSuccess = false;
            //        result.Message = "导入卡券出错，请检查文件";
            //        return result;
            //    }
            //}

            return result;
        }


        public List<WinningInfo> GetWinningsByWhere(int activityId, string where)
        {
            return _DbSession.WinningInfoStorager.GetWinningsByWhere(activityId, where).ToList() ;
        }
        public int GetWinningsCount(int activityId)
        {
            return _DbSession.WinningInfoStorager.GetWinningsCount(activityId);
        }
        public List<WinningInfo> GetWinningsList(string[] activityIds, string phonenumber)
        {
            return _DbSession.WinningInfoStorager.GetWinningsList(activityIds, phonenumber).ToList();
        }
        public WinningInfo GetWinningByTelAndActicityId(int activityId, string userTel)
        {
            return _DbSession.WinningInfoStorager.GetWinningByTelAndActicityId(activityId, userTel);
        }
        /// <summary>
        /// 获取单条用户获奖信息by userid
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public WinningInfo GetWinningByUserIdAndActicityId(int activityId, string userId) 
        {
            return _DbSession.WinningInfoStorager.GetWinningByUserIdAndActicityId(activityId, userId);
        }
        public bool IsWinningByActivity(int activityId, string openId)
        {
            return _DbSession.WinningInfoStorager.IsWinningByActivity(activityId, openId);
        }
        /// <summary>
        /// 获取奖品的每天的使用量
        /// </summary>
        /// <param name="activityId">活动ID</param>        
        /// <returns>奖品使用量列表</returns>
        public IEnumerable<PrizesInfo> GetPrizeUse(int activityId)
        {
            return _DbSession.WinningInfoStorager.GetPrizeUse(activityId);
        }
        /// <summary>
        /// 修改中奖纪录
        /// </summary>
        public bool UpdateForUserId(WinningInfo obj)
        {
            return _DbSession.WinningInfoStorager.UpdateForUserId(obj);
        }
        /// <summary>
        /// 修改中奖纪录
        /// </summary>
        public bool UpdateForId(int id, string userName, string phone, string address)
        {
            return _DbSession.WinningInfoStorager.UpdateForId(id, userName, phone, address);
        }
        /// <summary>
        /// 获取所中的记录
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizeId">奖品ID</param>
        /// <returns>中奖记录</returns>
        public WinningInfo GetWinPrize(int activityId, string userId, int prizeId)
        {
            return _DbSession.WinningInfoStorager.GetWinPrize(activityId, userId, prizeId);
        }
    }
}
