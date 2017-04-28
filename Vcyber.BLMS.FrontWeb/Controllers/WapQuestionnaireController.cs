using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common.City;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class WapQuestionnaireController : Controller
    {
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        private int PAGESIZE_LIST = 10;

        private string imagePath = string.Empty;

        #endregion

        #region ==== 公共属性 ====

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        #endregion

        #region ==== 构造函数 ====

        public WapQuestionnaireController()
        {
        }

        public WapQuestionnaireController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        //
        // GET: /Questionnaire/
        public ActionResult Index(string from = "BM", string source = "blms_wap")
        {
            //问卷类型 0：BM 1：CS
            int _curLinkFrom = from.Equals("CS") ? 1 : 0;
            string _curLinkSource = source;

            ViewBag.curLinkFrom = _curLinkFrom;
            ViewBag.curLinkSource = _curLinkSource;
            //判断用户是否登录
            if (this.User.Identity.IsAuthenticated)
            {
                //搜集当前用户的信息
                ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
                string _curUserId = _curUser.Id;
                string _curUserNickName = _curUser.NickName;
                string _curUserName = _curUser.UserName;
                string _curUserEmail = _curUser.Email;

                ViewBag.curUserId = _curUserId;
                ViewBag.curUserName = _curUserName;
                ViewBag.curUserNickName = string.IsNullOrEmpty(_curUserNickName) ? _curUserName : _curUserNickName;
                ViewBag.curUserEmail = _curUserEmail;
            }
            else
            {
                ViewBag.curUserId = "";
                ViewBag.curUserName = "";
                ViewBag.curUserNickName = "路人";
                ViewBag.curUserEmail = "";
            }
            //数据加载
            Models.QuestionnaireModel Model = new QuestionnaireModel(_curLinkFrom);

            //登录完成后直接开始问卷调查
            ViewBag.Message = "bluemembers网站用户体验调查问卷";
            return View(Model);
        }

        public ActionResult Result(int qid = 0, string from = "BM")
        {

            //问卷类型 0：BM 1：CS
            int _curLinkFrom = from.Equals("CS") ? 1 : 0;

            int _curQid = qid;
            Questionnaire _curQues = null;
            ApplicationUser _curUser = null;
            if (this.User.Identity.IsAuthenticated)
            {
                _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            }
            if (qid == 0)
            {
                _curQues = _AppContext.QuestionnaireApp.GetCurQuestionnaireInfo(_curLinkFrom);
                _curQid = _curQues.Id;
            }
            else
            {
                _curQues = _AppContext.QuestionnaireApp.GetQuestionnatireById(qid, _curLinkFrom);
            }
            ViewData["questionnaire"] = _curQues;
            ViewData["userinfo"] = _curUser;
            List<QuestionnaireWinning> wins = _AppContext.QuestionnaireWinningApp.GetQuestionnaireWinning(_curQid);
            wins.ForEach(f =>
            {
                string phone = f.WPhoneNumber;
                string tphone = phone.Substring(0, 3);
                string bphone = phone.Substring(7);
                f.WPhoneNumber = tphone + "****" + bphone;
            });

            ViewBag.SecondMessage = "恭喜以下中奖者获得蓝缤有你（礼） 爱答题活动奖品";
            ViewBag.Message = "调查问卷获奖名单";
            return View(wins);
        }

        public ActionResult PasserbyInfo()
        {
            QuestionnaireVisitor visitor = new QuestionnaireVisitor();
            if (this.User.Identity.IsAuthenticated)
            {
                //搜集当前用户的信息
                ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
                visitor.VName = _curUser.RealName;
                visitor.Sex = string.IsNullOrEmpty(_curUser.Gender) ? 1 : int.Parse(_curUser.Gender);   //1，男   2，女(如果数据库中数据为null时取默认性别男)
                visitor.PhoneNumber = _curUser.PhoneNumber;
                visitor.Provency = _curUser.Provency;
                visitor.City = _curUser.City;
                visitor.Area = _curUser.Area;
                visitor.MailAddress = _curUser.Address;
                visitor.Email = _curUser.Email;
            }
            ViewData["provinceList"] = CityService.Instance.GetProvince();
            return View(visitor);
        }

        public string GetImagePath(string path)
        {
            string imgPath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImgPath"];
            return string.IsNullOrEmpty(path) ? "" : imgPath + path;
        }

        public ActionResult WapLogin(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public ActionResult WapRegister(string returnUrl, string source)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/WapQuestionnaire/Index";
            }
            ViewBag.returnUrl = returnUrl;
            ViewBag.source = source;
            return View();
        }

        public ActionResult QuestionnaireHistory()
        {
            Dictionary<int, List<Questionnaire>> dic = new Dictionary<int, List<Questionnaire>>();

            var _result = _AppContext.QuestionnaireApp.GetQuestionnaireList(0);

            foreach (var ques in _result)
            {
                if (dic.Keys.Contains(ques.CreateTime.Year))
                {
                    List<Questionnaire> lst = dic[ques.CreateTime.Year];
                    lst.Add(ques);
                }
                else
                {
                    dic.Add(ques.CreateTime.Year, new List<Questionnaire>() { ques });
                }
            }

            return View(dic);
        }

        public ActionResult WapAccountPageProvince()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }
        /// <summary>
        /// 整理答案列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resultvalue"></param>
        /// <returns></returns>
        private List<Answer> GetListAnswers(string id, string resultvalue)
        {
            List<Answer> answers = new List<Answer>();
            answers.Clear();
            resultvalue.Split('}').ToList().ForEach(f =>
            {
                if (!string.IsNullOrWhiteSpace(f) && f.Contains("{"))
                {
                    string[] result = f.Split('{');
                    Answer model = null;
                    int tempCount = answers.Where(t => t.ParentId == int.Parse(result[0])).Count();
                    if (tempCount > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(result[1]))
                        {
                            model = answers.FirstOrDefault(a => a.ParentId == int.Parse(result[0]));
                            model.AContent += "," + result[1].Replace(",", "&#44;");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(result[1]))
                        {
                            model = new Answer();
                            model.MemberId = id;
                            model.ParentId = int.Parse(result[0]);
                            model.AContent = result[1].Replace(",", "&#44;");
                            model.State = 1;
                            answers.Add(model);
                        }
                    }
                }
            });
            return answers;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckQuestionnaireState(int qtype)
        {
            //问卷状态检测（200:正在进行；203：已经结束；201：未开始）*****原来的逻辑
            //问卷状态检测(0:已经删除；1：未开始；2：开始；3：结束)
            int tempState = _AppContext.QuestionnaireApp.GetCurQuestionnaireState(qtype);
            if (tempState > 0)
            {
                int Qid = _AppContext.QuestionnaireApp.GetCurQuestionnaireInfo(qtype).Id;
                //用户登录状态检测(401:已登录；400：未登录)
                if (this.User.Identity.IsAuthenticated)
                {
                    //用户答题状态检测（300：未完成过问卷；301：已完成过问卷）
                    string memberId = this.User.Identity.GetUserId();
                    if (!_AppContext.QuestionnaireRecordApp.IsQuestionnaire(memberId, Qid))
                    {
                        return Json(new { code = 300, msg = "未完成过问卷！" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = 301, msg = "您已经完成过该问卷调查！" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "unlogin" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 201, msg = "not begin" }, JsonRequestBehavior.AllowGet);
            }
            //else if (tempState == 3)
            //{
            //    return Json(new { code = 203, msg = "end" }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { code = 201, msg = "not begin" }, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SaveQuestionnaireResult(string psName, string psSex, string psPhone, string addressProvince, string addressCity, string addressCounty, string psAddress, string psAge, string psEducation, string psCarType, string psEmail, int qid, string resultvalue, string memberId, int _curBlueBeanCount, string linkSource, int linkFrom)
        {
            //生成答案数据列表
            List<Answer> answers = string.IsNullOrWhiteSpace(memberId) ?
                GetListAnswers(psPhone, resultvalue) : GetListAnswers(memberId, resultvalue);
            //保存答案列表
            if (!_AppContext.AnswerApp.AddAnswerRang(answers))
            {
                return Json(new { code = 400, msg = "问卷答案保存失败！" }, JsonRequestBehavior.AllowGet);
            }

            bool _isLogin = !string.IsNullOrWhiteSpace(memberId);
            //List<Answer> answers = GetListAnswers(psPhone, resultvalue);
            //生成陌生人访客记录
            QuestionnaireVisitor visitor = new QuestionnaireVisitor();
            visitor.VName = psName;
            visitor.Sex = string.IsNullOrEmpty(psSex) ? 1 : (psSex.Equals("man") ? 1 : 2);  //1:男   2：女
            visitor.PhoneNumber = psPhone;
            visitor.MailAddress = psAddress;
            visitor.QuestionnaireId = qid;
            visitor.Provency = addressProvince;
            visitor.City = addressCity;
            visitor.Age = psAge;
            visitor.Education = psEducation;
            visitor.CarModels = psCarType;
            visitor.Area = addressCounty;
            visitor.CreateTime = DateTime.Now;
            visitor.IsMember = _isLogin;
            visitor.Email = psEmail;
            visitor.VSource = linkSource;

            visitor.Id = _AppContext.QuestionnaireVisitorApp.AddQuestionnaireVisitor(visitor);
            //会员保存逻辑
            if (_isLogin)
            {
                QuestionnaireRecord record = new QuestionnaireRecord();
                record.MemberId = memberId;
                record.QuestionnaireId = qid;
                record.State = 2;
                record.CreateTime = DateTime.Now;
                record.ContactId = visitor.Id;

                // 保存问卷答案
                if (_AppContext.QuestionnaireRecordApp.AddQuestionnaireRecord(record))
                {
                    if (linkFrom != 1)
                    {
                        //添加蓝豆操作
                        UserblueBean ub = new UserblueBean();
                        ub.userId = memberId;
                        ub.integralSource = Convert.ToInt32(EBRuleType.调查问卷).ToString();
                        ub.value = _curBlueBeanCount;//当前问卷规定的蓝豆
                        ub.usevalue = 0;//此处不用管
                        ub.datastate = 0;
                        ub.remark = EBRuleType.调查问卷.ToString();
                        ub.CreateTime = DateTime.Now;
                        ub.UpdateTime = DateTime.Now;
                        _AppContext.UserBlueBeanApp.Add(ub);
                    }

                    return Json(new { code = 200, msg = "问卷答案保存成功！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 400, msg = "问卷答案保存失败！" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 200, msg = "问卷答案保存成功！" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SubmitResultByCS(string memberId, int qid, string resultValue, string userEmail)
        {
            string decodeStr = HttpUtility.UrlDecode(resultValue);
            //生成答案数据列表
            List<Answer> answers = GetListAnswers(memberId, decodeStr);
            //答题记录表
            QuestionnaireRecord record = new QuestionnaireRecord();
            record.MemberId = memberId;
            record.QuestionnaireId = qid;
            record.State = 2;
            record.CreateTime = DateTime.Now;
            // 保存问卷答案、邮箱地址及答题记录
            if (_AppContext.AnswerApp.AddAnswerRang(answers) &&
                _AppContext.QuestionnaireApp.UpdateMemberShipEmail(memberId, userEmail) && _AppContext.QuestionnaireRecordApp.AddQuestionnaireRecord(record))
            {

                return Json(new { code = 200, msg = "问卷答案保存成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 400, msg = "问卷答案保存失败！" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}