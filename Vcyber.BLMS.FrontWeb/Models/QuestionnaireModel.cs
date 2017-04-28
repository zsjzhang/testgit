using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class QuestionnaireModel
    {
        public QuestionnaireModel(int qType)
        {
            Questionnaire QNModel = _AppContext.QuestionnaireApp.GetCurQuestionnaireInfo(qType);
            if (QNModel == null)
            {
                QNModel = _AppContext.QuestionnaireApp.GetLastQuestionnatire(qType);
                if (QNModel == null) return;
            }
            QuestionnaireId = QNModel.Id;
            QuestionnaireTheme = QNModel.Theme;
            QuestionnaireAlteTheme = QNModel.AlternateTheme;
            QuestionnaireBTime = QNModel.BeginTime;
            QuestionnaireETime = QNModel.EndTime;
            QuestionnaireCTime = QNModel.CreateTime;
            QuestionnaireRemark = System.Web.HttpUtility.UrlDecode(QNModel.Remarks);
            string imgPath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImgPath"];
            QuestionnaireImage = string.IsNullOrEmpty(QNModel.Image) ? "" : imgPath + QNModel.Image;
            QuestionnaireState = QNModel.State;
            BlueBeanCount = QNModel.BlueBeanCount;
            listQuestion = BindQuestion(QNModel.Id);

        }
        public int QuestionnaireId { get; set; }
        public string QuestionnaireTheme { get; set; }
        public string QuestionnaireAlteTheme { get; set; }
        public string QuestionnaireImage { get; set; }
        public DateTime QuestionnaireBTime { get; set; }
        public DateTime QuestionnaireETime { get; set; }
        public DateTime QuestionnaireCTime { get; set; }
        public int BlueBeanCount { get; set; }

        /// <summary>
        /// 1:未开始；2:开始；3：结束
        /// </summary>
        public int QuestionnaireState { get; set; }
        public List<QuestionModel> listQuestion { get; set; }
        public string QuestionnaireRemark { get; set; }
        List<QuestionModel> BindQuestion(int pid)
        {
            List<QuestionModel> result = new List<QuestionModel>();
            _AppContext.QuestionApp.GetQuestionByPId(pid).ForEach(f =>
            {
                QuestionModel mode = new QuestionModel()
                {
                    QuestionId = f.Id,
                    QuestionTitle = f.QContent,
                    QuestionType = f.Type,
                    QuestionState = f.State,
                    QuestionSort = f.Sort,
                    IsRequired = f.IsRequired,
                    QuestionCycle = f.Cycle,
                    QuestionTextIsBefore = f.TextIsBefore

                };
                //单选或多选
                if (f.Type == 0 || f.Type == 1)
                {
                    mode.ListOption = _AppContext.OptionApp.GetOptionByQId(f.Id);
                }
                if (f.Type == 4 || f.Type == 5)
                {
                    mode.ListChildQurstion = BindQuestion(f.Id);
                    mode.ListOption = _AppContext.OptionApp.GetOptionByQId(f.Id);
                }
                if (f.Type == 7)    //满意度调查题
                {
                    mode.ListOption = _AppContext.OptionApp.GetOptionByQId(f.Id);
                }
                if (f.Type == 8)    //多选填空题
                {
                    mode.ListOption = _AppContext.OptionApp.GetOptionByQId(f.Id);
                }
                if (f.Type == 9)    //排序题
                {
                    mode.ListOption = _AppContext.OptionApp.GetOptionByQId(f.Id);
                }
                result.Add(mode);
            });
            return result;
        }
    }
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public int QuestionType { get; set; }
        public int QuestionState { get; set; }
        public int QuestionSort { get; set; }
        public bool QuestionTextIsBefore { get; set; }
        public List<QuestionModel> ListChildQurstion { get; set; }
        /// <summary>
        /// 是否为必填
        /// </summary>
        public bool IsRequired { get; set; }
        public int QuestionCycle { get; set; }
        public List<Vcyber.BLMS.Entity.Option> ListOption { get; set; }

    }
}