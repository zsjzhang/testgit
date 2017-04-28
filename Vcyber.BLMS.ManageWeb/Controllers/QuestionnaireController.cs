using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using System.IO;
using System.Text;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.ManageWeb.Models;
using System.Data;
using Vcyber.BLMS.Common.City;


namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class QuestionnaireController : Controller
    {
        //
        // GET: /Questionnaire/

        /// <summary>
        /// 调查问卷后台首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 调查问卷列表
        /// </summary>
        /// <param name="index">第几页</param>
        /// <param name="size">每页多少行</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public ActionResult PartialPage(int index = 1, int size = 10)
        {
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理"); // User.IsInRole("CS问卷管理");
            EQuestionnaireType qType = EQuestionnaireType.BM;
            if (IsCs)
                qType = EQuestionnaireType.CS;
            int total = 0;
            var qList = _AppContext.QuestionnaireApp.selectQuestionnaire(qType, "", "desc", index, size, out total);
            int count = (int)Math.Ceiling((double)total / (double)size);
            ViewBag.Total = total;
            ViewBag.PageIndex = index;
            ViewBag.PrePage = index > 1 ? (index - 1) : 1;
            ViewBag.NextPage = index < count ? (index + 1) : count;
            ViewBag.EndPage = count;
            return PartialView(qList);
        }

        /// <summary>
        /// 删除调查问卷
        /// </summary>
        /// <param name="id">问卷id</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            _AppContext.QuestionnaireApp.Delete(id);
            return this.Redirect("/Questionnaire/Index");
        }

        public ActionResult Over(int id)
        {
            _AppContext.QuestionnaireApp.UpdateState(id, 3);
            return this.Redirect("/Questionnaire/Index");
        }

        /// <summary>
        /// 创建/修改问卷
        /// </summary>
        /// <param name="id">问卷id（若创建则为0，若修改则为修改问卷的id）</param>
        /// <returns></returns>
        public ActionResult Create(int id = 0)
        {
            Questionnaire entity = new Questionnaire();
            entity.BeginTime = DateTime.Now;
            entity.EndTime = DateTime.Now;
            ViewBag.IsCs=_AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理");

            if (id > 0)
            {
                entity = _AppContext.QuestionnaireApp.SelectSingle(id);
                //ViewBag.QuestionnaireId = id;
            }
            return View(entity);
        }

        /// <summary>
        /// 创建/修改问卷
        /// </summary>
        /// <param name="entity">问卷实体</param>
        /// <returns></returns>
        [HttpPost]
       
        public ActionResult Create(Questionnaire entity)
        {
            entity.Image = FilterStr.FormatHTML(entity.Image == null ? "" : entity.Image);
            entity.SyImage = FilterStr.FormatHTML(entity.SyImage == null ? "" : entity.SyImage);
            int result = -2;
            //如果问卷id大于0，则为修改
            if (entity.Id > 0)
            {
                entity.CreateTime = DateTime.Now;
                if (_AppContext.QuestionnaireApp.Edit(entity))
                {
                    result = entity.Id;
                }
            }
            else
            {
                bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理"); // User.IsInRole("CS问卷管理"); 
                EQuestionnaireType qType = EQuestionnaireType.BM;
                if (IsCs)
                    qType = EQuestionnaireType.CS;
                entity.QType = Convert.ToInt32(qType);
                QuestionnaireManage qmEntity = new QuestionnaireManage();
                qmEntity.UserId = this.HttpContext.User.Identity.GetUserId();
                qmEntity.IsFirst = true;
                qmEntity.State = Convert.ToInt32(EState.Normal);
                result = _AppContext.QuestionnaireApp.Create(entity, qmEntity);
            }
            return this.Redirect("/Questionnaire/AddQuestion/" + result);
        }

        public ActionResult WinningIndex()
        {
            return View();
        }

        public ActionResult WinningPartialPage(int id = 0, int index = 1, int size = 10)
        {
            int total = 0;
            var qList = _AppContext.QuestionnaireWinningApp.SelectQuestionnaireWinning(id, index, size, out total);
            int count = (int)Math.Ceiling((double)total / (double)size);
            ViewBag.Total = total;
            ViewBag.PageIndex = index;
            ViewBag.PrePage = index > 1 ? (index - 1) : 1;
            ViewBag.NextPage = index < count ? (index + 1) : count;
            ViewBag.EndPage = count;
            return PartialView(qList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportWinning()
        {
            HttpPostedFileBase file = Request.Files["file"];
            if (Request.Form["qId"] != null && Request.Form["qId"].ToString() != "")
            {
                int qId = Convert.ToInt32(Request.Form["qId"]);
                if (file != null)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../UploadImg"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                    if (Path.GetExtension(filePath) != ".xls" && Path.GetExtension(filePath) != ".xlsx")
                    {
                        return RedirectToAction("WinningIndex", "Questionnaire");
                    }

                    file.SaveAs(filePath);
                    DataTable dt = Common.NPOIHelper<QuestionnaireWinning>.ReadExcel(filePath, Path.GetExtension(filePath));
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            string name = dr["获奖人姓名"].ToString();
                            string phoneNumber = dr["获奖人手机号"].ToString();
                            string prize = dr["奖品"].ToString();
                            QuestionnaireWinning qwEntity = new QuestionnaireWinning();
                            qwEntity.QuestionnaireId = qId;
                            qwEntity.WName = name;
                            qwEntity.WPhoneNumber = phoneNumber;
                            qwEntity.Prize = prize;
                            _AppContext.QuestionnaireWinningApp.Create(qwEntity);
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("WinningIndex", "Questionnaire");
                        }
                    }
                }
            }

            return RedirectToAction("WinningIndex", "Questionnaire");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSelect(string beginTime, int id)
        {
            DateTime bt = Convert.ToDateTime(beginTime);
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理"); // User.IsInRole("CS问卷管理");
            EQuestionnaireType qType = EQuestionnaireType.BM;
            if (IsCs)
                qType = EQuestionnaireType.CS;
            return Content(_AppContext.QuestionnaireApp.CreateSelect(bt, id, Convert.ToInt32(qType)).ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateState(int id, int state)
        {
            return Content(_AppContext.QuestionnaireApp.UpdateState(id, state).ToString());
        }

        /// <summary>
        /// 上传主视觉图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadImage()
        {
            HttpContextBase context = this.HttpContext;

            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                HttpPostedFileBase file = context.Request.Files[0];
                string extendsName = Path.GetExtension(file.FileName);

                if (!this.imgExtends.Contains(extendsName.ToLower()) || file.ContentLength > 10485760)
                {
                    return Content("");
                }

                string newFileName = Guid.NewGuid().ToString("N") + extendsName;

                if (!Directory.Exists(HttpContext.Server.MapPath("/UploadImg")))
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("/UploadImg"));
                }

                file.SaveAs(Path.Combine(HttpContext.Server.MapPath("/UploadImg"), newFileName));
                return Content("/UploadImg/" + newFileName);
            }
            return Content("");
        }

        /// <summary>
        /// 添加问题
        /// </summary>
        /// <returns></returns>
       
        public ActionResult AddQuestion(int id)
        {
            ViewBag.ParentId = id;
            ViewBag.IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理");
            return View();
        }

        /// <summary>
        /// 添加问题
        /// </summary>
        /// <param name="entity">问题实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(int type, int sort, int parentId)
        {
            Question entity = new Question();
            entity.Type = type;
            if ((EQuestionType)type == EQuestionType.JzChildren)
            {
                entity.QContent = EnumExtension.GetDiscribe((EQuestionType)type) + sort;
            }
            else
            {
                entity.QContent = EnumExtension.GetDiscribe((EQuestionType)type);
            }
            entity.ParentId = parentId;
            entity.IsRequired = true;
            entity.Sort = sort;
            entity.State = Convert.ToInt32(EState.Normal);
            if ((EQuestionType)type == EQuestionType.JzMessage)
            {
                entity.Cycle = 1;
            }
            int result = _AppContext.QuestionApp.Create(entity);
            entity.Id = result;
            List<Question> qList = new List<Question>();
            List<Option> oList = new List<Option>();
            if (result > 0)
            {
                if (entity.Type == Convert.ToInt32(EQuestionType.JzRadio) || entity.Type == Convert.ToInt32(EQuestionType.JzCheck))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Question qEntity = new Question();
                        qEntity.Type = Convert.ToInt32(EQuestionType.JzChildren);
                        qEntity.QContent = EnumExtension.GetDiscribe(EQuestionType.JzChildren) + (i + 1);
                        qEntity.ParentId = result;
                        qEntity.IsRequired = false;
                        qEntity.Sort = i + 1;
                        qEntity.State = Convert.ToInt32(EState.Normal);
                        int qResult = _AppContext.QuestionApp.Create(qEntity);
                        qEntity.Id = qResult;
                        qList.Add(qEntity);
                        if (qResult <= 0)
                        {
                            return Content("");
                        }
                    }
                }
                if (entity.Type == Convert.ToInt32(EQuestionType.Radio) || entity.Type == Convert.ToInt32(EQuestionType.Check) || entity.Type == Convert.ToInt32(EQuestionType.JzRadio) || entity.Type == Convert.ToInt32(EQuestionType.JzCheck) || entity.Type == Convert.ToInt32(EQuestionType.Sort))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Option oEntity = new Option();
                        oEntity.OContent = EnumExtension.GetDiscribe(EChildrenType.Option) + (i + 1);
                        oEntity.PartentId = result;
                        oEntity.Sort = i + 1;
                        oEntity.State = Convert.ToInt32(EState.Normal);
                        oEntity.OType = Convert.ToInt32(EChildrenType.Option);
                        int oResult = _AppContext.OptionApp.Create(oEntity);
                        oEntity.Id = oResult;
                        oList.Add(oEntity);
                        if (oResult <= 0)
                        {
                            return Content("");
                        }
                    }
                }
                if (entity.Type == Convert.ToInt32(EQuestionType.Satisfied))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Option oEntity = new Option();
                        oEntity.OContent = (i + 1).ToString();
                        oEntity.PartentId = result;
                        oEntity.Sort = i + 1;
                        oEntity.State = Convert.ToInt32(EState.Normal);
                        int oResult = _AppContext.OptionApp.Create(oEntity);
                        oEntity.Id = oResult;
                        oList.Add(oEntity);
                        if (oResult <= 0)
                        {
                            return Content("");
                        }
                    }
                }
                if (entity.Type == Convert.ToInt32(EQuestionType.JzMessage))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Option oEntity = new Option();
                        oEntity.OContent = EnumExtension.GetDiscribe(EChildrenType.tkOption) + (i + 1);
                        oEntity.PartentId = result;
                        oEntity.Sort = i + 1;
                        oEntity.State = Convert.ToInt32(EState.Normal);
                        oEntity.OType = Convert.ToInt32(EChildrenType.tkOption);
                        int oResult = _AppContext.OptionApp.Create(oEntity);
                        oEntity.Id = oResult;
                        oList.Add(oEntity);
                        if (oResult <= 0)
                        {
                            return Content("");
                        }
                    }

                }

            }
            else
            {
                return Content("");
            }
            return Content(CreateQuestion(entity, oList, qList));
        }

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="entity">选项实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOption(int parentId, int sort, int oType, int type)
        {
            Option entity = new Option();
            if ((EQuestionType)type == EQuestionType.Satisfied)
            {
                if (oType == 0)
                {
                    int result = _AppContext.OptionApp.MaxOption(parentId) + 1;
                    entity.OContent = result.ToString();
                    entity.Sort = result;
                }
                else
                {
                    entity.OContent = "选项" + sort;
                    entity.Sort = sort;
                }
            }
            else
            {
                entity.OContent = "选项" + sort;
                entity.Sort = sort;
            }

            entity.PartentId = parentId;
            entity.State = Convert.ToInt32(EState.Normal);
            entity.OType = oType;
            return Content(_AppContext.OptionApp.Create(entity).ToString());
        }

        [HttpPost]
        public ActionResult AddChildrenOption(string content, int parentId)
        {
            bool result = false;
            Option entity = new Option();
            entity.OContent = content;
            entity.Sort = 0;
            entity.PartentId = parentId;
            entity.State = Convert.ToInt32(EState.Normal);
            entity.OType = Convert.ToInt32(EChildrenType.childrenOption);
            if (_AppContext.OptionApp.Create(entity) > 0)
            {
                result = _AppContext.OptionApp.EditType(Convert.ToInt32(EChildrenType.dpOption), parentId);
            }
            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult DeleteMaxOption(int parentId)
        {
            return Content(_AppContext.OptionApp.DeleteMaxOption(parentId).ToString());
        }

        [HttpPost]
        public ActionResult EditQuestionTextIsBefore(bool textIsBefore, int id)
        {
            return Content(_AppContext.QuestionApp.EditQuestionTextIsBefore(textIsBefore, id).ToString());
        }

        [HttpPost]
        public ActionResult AddCycleOption(int id, int count)
        {
            return Content(_AppContext.QuestionApp.AddCycleOption(id, count).ToString());
        }

        [HttpPost]
        public ActionResult CopyQuestion(int id)
        {
            Question entity = _AppContext.QuestionApp.GetById(id);
            entity.Sort = entity.Sort + 1;
            int result = _AppContext.QuestionApp.Create(entity);
            return Content("");
        }

        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="entity">问题实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(string content, int id)
        {
            return Content(_AppContext.QuestionApp.EditContent(content, id).ToString());
        }

        [HttpPost]
        public ActionResult EditOptionType(int type, int id)
        {
            return Content(_AppContext.OptionApp.EditType(type, id).ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestionSort(int sort, int id, int parentId, int ySort)
        {
            return Content(_AppContext.QuestionApp.EditSort(sort, id, parentId, ySort).ToString());
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        /// <param name="isRequired"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestionIsRequired(bool isRequired, int id)
        {
            return Content(_AppContext.QuestionApp.EditIsRequired(isRequired, id).ToString());
        }

        /// <summary>
        /// 是否选中后，才可以填写填空
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQuestionIsChecked(bool isChecked, int id)
        {
            return Content(_AppContext.QuestionApp.EditQuestionIsChecked(isChecked, id).ToString());
        }


        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuestion(int Id, int parentId, int sort)
        {
            return Content(_AppContext.QuestionApp.Delete(Id, parentId, sort).ToString());
        }

        /// <summary>
        /// 修改选项
        /// </summary>
        /// <param name="entity">选项实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOption(string oContent, int oId)
        {
            return Content(_AppContext.OptionApp.EditContent(oContent, oId).ToString());
        }

        /// <summary>
        /// 修改选项排序
        /// </summary>
        /// <param name="sort">排序</param>
        /// <param name="id">选项id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOptionSort(int sort, int id, int parentId, int ySort)
        {
            return Content(_AppContext.OptionApp.EditSort(sort, id, parentId, ySort).ToString());
        }

        public ActionResult ExportData()
        {

            return View();
        }

        /// <summary>
        /// 删除选项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOption(int Id, int parentId, int sort)
        {
            return Content(_AppContext.OptionApp.Delete(Id, parentId, sort).ToString());
        }

        [HttpPost]
        public ActionResult EditValueType(int vType, int id)
        {
            return Content(_AppContext.OptionApp.EditValueType(vType, id).ToString());
        }

        /// <summary>
        /// 预览页面
        /// </summary>
        /// <param name="index">问卷id</param>
        /// <returns></returns>
        public ActionResult PreviewIndex(int id = 0)
        {
            ViewBag.curLinkFrom = 0;
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理");
            if (IsCs)
            {
                ViewBag.curLinkFrom = 1;
            }
            QuestionnaireModel Model = new QuestionnaireModel(id);
            return View(Model);
        }

        public ActionResult PasserbyInfo()
        {
            QuestionnaireVisitor visitor = new QuestionnaireVisitor();
            ViewData["provinceList"] = CityService.Instance.GetProvince();
            return View(visitor);
        }

        #region ==== 私有方法 ====

        private string imgExtends = ".bmp.png.jpeg.jpg.gif";

        private string CreateQuestion(Question question, List<Option> oList, List<Question> qList)
        {
            StringBuilder qStr = new StringBuilder();
            qStr.AppendLine("<li class=\"module\">");
            qStr.AppendLine("      <input type=\"hidden\" value=\"" + question.Type + "\" class=\"q_type\" />");
            qStr.AppendLine("      <input type=\"hidden\" value=\"" + question.Id + "\" class=\"q_id\" />");
            qStr.AppendLine("     <div class=\"topic_type\">");
            qStr.AppendLine("         <div class=\"topic_type_menu\">");
            qStr.AppendLine("              <div class=\"setup-group\">");
            qStr.AppendFormat("                  <h4>Q{0}</h4>", question.Sort);
            qStr.AppendLine("                    <a class=\"up_cq\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"right\" title=\"上移本题\">");
            qStr.AppendLine("                          <i class=\"up-icon-active\"></i>");
            qStr.AppendLine("                    </a>");
            qStr.AppendLine("                    <a class=\"down_cq\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"right\" title=\"下移本题\">");
            qStr.AppendLine("                          <i class=\"down-icon-active\"></i>");
            qStr.AppendLine("                    </a>");
            //qStr.AppendLine("                    <a class=\"Bub\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"right\" title=\"复制题目\">");
            //qStr.AppendLine("                          <i class=\"copy-icon-active\"></i>");
            //qStr.AppendLine("                    </a>");
            qStr.AppendLine("                    <a class=\"Del\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"right\" title=\"删除题目\">");
            qStr.AppendLine("                          <i class=\"del2-icon-active\"></i>");
            qStr.AppendLine("                    </a>");
            qStr.AppendLine("              </div>");
            qStr.AppendLine("         </div>");
            qStr.AppendLine("         <div class=\"topic_type_con\">");
            qStr.AppendLine("              <div class=\"Drag_area\">");
            qStr.AppendLine("                  <div class=\"th4 T_edit q_title\" name=\"question\">" + question.QContent + "</div>");
            qStr.AppendLine("              </div>");
            qStr.AppendLine("              <ul class=\"unstyled\">");
            qStr.AppendLine(CreateQuestionChildren(qList, oList, (EQuestionType)question.Type));
            qStr.AppendLine("              </ul>");
            if (question.Type == Convert.ToInt32(EQuestionType.Radio) || question.Type == Convert.ToInt32(EQuestionType.Check) || question.Type == Convert.ToInt32(EQuestionType.JzMessage) || question.Type == Convert.ToInt32(EQuestionType.Sort) || question.Type == Convert.ToInt32(EQuestionType.Satisfied))
            {
                qStr.AppendLine("              <input type=\"hidden\" class=\"sort\" value=\"" + oList.Count + "\" />");
                qStr.AppendLine("              <div class=\"operationH\">");
                if (question.Type == Convert.ToInt32(EQuestionType.JzMessage))
                {
                    qStr.AppendLine("                  <a class=\"cq_add_tk\" href=\"javascript:;\">");
                }
                else
                {
                    qStr.AppendLine("                  <a class=\"cq_add\" href=\"javascript:;\">");
                }
                qStr.AppendLine("                      <i class=\"add-icon-active\"></i>");
                qStr.AppendLine("                  </a>");
                if (question.Type == Convert.ToInt32(EQuestionType.JzMessage))
                {
                    qStr.AppendLine("               <label>循环次数：</label>");
                    qStr.AppendLine("               <label class=\"T_edit_count\">1</label>");
                }
                qStr.AppendLine("                  <input type=\"checkbox\" class=\"isRequired\" checked=\"true\" name=\"isRequired\" />必填");
                if (question.Type == Convert.ToInt32(EQuestionType.Radio) || question.Type == Convert.ToInt32(EQuestionType.Check) || question.Type == Convert.ToInt32(EQuestionType.Satisfied))
                {
                    qStr.AppendLine("               <a class=\"cq_add_tk\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"添加填空选项\">");
                    qStr.AppendLine("                   <i class=\"clone-icon-active\"></i>");
                    qStr.AppendLine("               </a>");
                }
                if (question.Type == Convert.ToInt32(EQuestionType.JzMessage))
                {
                    qStr.AppendLine("           <input type=\"checkbox\" class=\"TextIsBefore\" name=\"TextIsBefore\" />文字前置");
                }
                qStr.AppendLine("              </div>");
            }
            else
            {
                qStr.AppendLine("              <div class=\"operationH\">");
                if (question.Type == Convert.ToInt32(EQuestionType.Satisfied))
                {
                    qStr.AppendLine("               <a class=\"cq_add\" href=\"javascript:;\">");
                    qStr.AppendLine("                   <i class=\"add-icon-active\"></i>");
                    qStr.AppendLine("               </a>");
                    qStr.AppendLine("               <a class=\"Del_sz\" href=\"javascript:;\"> <i class=\"del2-icon-active\"></i></a>");
                }
                qStr.AppendLine("                  <input type=\"checkbox\" class=\"isRequired\" checked=\"true\" name=\"isRequired\" />必填");
                qStr.AppendLine("              </div>");
            }
            qStr.AppendLine("         </div>");
            qStr.AppendLine("     </div>");
            qStr.AppendLine("</li>");
            return qStr.ToString();
        }

        private string CreateQuestionChildren(List<Question> qList, List<Option> oList, EQuestionType type)
        {
            StringBuilder qcStr = new StringBuilder();
            switch (type)
            {
                case EQuestionType.Radio:
                case EQuestionType.Check:
                case EQuestionType.Satisfied:
                case EQuestionType.JzMessage:
                case EQuestionType.Sort:
                    qcStr.AppendLine(CreateOption(oList, type));
                    break;
                case EQuestionType.Judge:
                    qcStr.AppendLine("<li>");
                    qcStr.AppendLine("  <input type=\"radio\" value=\"\" name=\"radio\" />");
                    qcStr.AppendLine("  <label class=\"T_edit_min_j\" name=\"option\">对</label>");
                    qcStr.AppendLine("</li>");
                    qcStr.AppendLine("<li>");
                    qcStr.AppendLine("  <input type=\"radio\" value=\"\" name=\"radio\" />");
                    qcStr.AppendLine("  <label class=\"T_edit_min_j\" name=\"option\">错</label>");
                    qcStr.AppendLine("</li>");
                    break;
                case EQuestionType.Message:
                    qcStr.AppendLine("<li>");
                    qcStr.AppendLine("  <textarea></textarea>");
                    qcStr.AppendLine("</li>");
                    break;
                case EQuestionType.FillText:
                    qcStr.AppendLine("<li>");
                    qcStr.AppendLine("  <input type=\"text\" name=\"\" />");
                    qcStr.AppendLine("</li>");
                    break;
                case EQuestionType.JzRadio:
                case EQuestionType.JzCheck:
                    qcStr.AppendLine(CreateJzOption(qList, oList, type));
                    break;
                default: break;
            }
            return qcStr.ToString();
        }

        private string CreateJzOption(List<Question> qList, List<Option> oList, EQuestionType type)
        {
            string questionType = string.Empty;
            if (type == EQuestionType.JzRadio)
            {
                questionType = "radio";
            }
            else
            {
                questionType = "checkbox";
            }
            StringBuilder jzStr = new StringBuilder();
            jzStr.AppendLine("<li>");
            jzStr.AppendLine("  <div class=\"matrix\">");
            jzStr.AppendLine("      <table class=\"table table-bordered td-Tc\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 881px;\">");
            jzStr.AppendLine("          <tbody>");
            for (int i = 0; i < qList.Count + 1; i++)
            {
                if (i == 0)
                {
                    jzStr.AppendLine("<tr>");
                    for (int j = 0; j < oList.Count + 1; j++)
                    {
                        if (j == 0)
                        {
                            jzStr.AppendLine("<td width=\"150.20000000000002px\"> </td>");
                        }
                        else
                        {
                            jzStr.AppendFormat("<td class=\"T_edit_min\" width=\"300.40000000000003px\" menutype=\"col\" name=\"option\">{0}</td>", oList[j - 1].OContent);
                        }
                    }
                    jzStr.AppendLine("</tr>");
                }
                else
                {
                    jzStr.AppendLine("<tr class=\"Ed_tr\">");
                    for (int j = 0; j < oList.Count + 1; j++)
                    {
                        if (j == 0)
                        {
                            jzStr.AppendFormat("    <td class=\"T_edit\" menutype=\"row\" name=\"row\" style=\"text-align:left;\">{0}</td>", qList[i - 1].QContent);
                        }
                        else
                        {
                            jzStr.AppendLine("      <td>");
                            jzStr.AppendFormat("        <input type=\"{0}\" name=\"\">", questionType);
                            jzStr.AppendLine("      </td>");
                        }
                    }
                    jzStr.AppendLine("</tr>");
                }
            }
            jzStr.AppendLine("          </tbody>");
            jzStr.AppendLine("      </table>");
            jzStr.AppendLine("  </div>");
            jzStr.AppendLine("</li>");
            return jzStr.ToString();
        }

        private string CreateOption(List<Option> oList, EQuestionType type)
        {
            string questionType = string.Empty;
            StringBuilder opStr = new StringBuilder();
            if (type == EQuestionType.Satisfied)
            {
                opStr.AppendLine("<li>");
                for (int i = 0; i < oList.Count; i++)
                {
                    opStr.AppendLine("<label class=\"T_edit_min_f\">" + (i + 1) + "</label>");
                }
                opStr.AppendLine("</li>");
            }
            else
            {
                if (type == EQuestionType.Radio)
                {
                    questionType = "radio";
                }
                else
                {
                    questionType = "checkbox";
                }

                for (int i = 0; i < oList.Count; i++)
                {
                    opStr.AppendLine("<li>");
                    if (type != EQuestionType.JzMessage)
                    {
                        if (type == EQuestionType.Sort)
                        {
                            opStr.AppendLine("  <label>（" + (i + 1).ToString() + "）</label>");
                        }
                        else
                        {
                            opStr.AppendLine("  <input type=\"" + questionType + "\" value=\"\" name=\"radio\" />");
                        }
                    }
                    opStr.AppendLine("  <label class=\"T_edit_min\" name=\"option\">" + oList[i].OContent + "</label>");
                    opStr.AppendLine("  <input type=\"hidden\" value=\"" + oList[i].Id + "\" class=\"o_id\" />");
                    if (type == EQuestionType.JzMessage)
                    {
                        opStr.AppendLine("  <input type=\"text\" value=\"\" />");
                    }
                    opStr.AppendLine("  <a class=\"up_cq_option\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"上移选项\"> <i class=\"up-icon-active\"></i></a>");
                    opStr.AppendLine("  <a class=\"down_cq_option\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"下移选项\"> <i class=\"down-icon-active\"></i></a>");
                    opStr.AppendLine("  <a class=\"Del_option\" href=\"javascript:;\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"删除选项\"> <i class=\"del2-icon-active\"></i></a>");
                    opStr.AppendLine("</li>");
                }
            }
            return opStr.ToString();
        }

        #endregion

    }
}