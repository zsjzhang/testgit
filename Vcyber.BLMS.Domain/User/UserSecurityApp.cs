using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using log4net;
using log4net.Repository.Hierarchy;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;


namespace Vcyber.BLMS.Domain
{
    public class UserSecurityApp : IUserSecurityApp
    {

        private static ILog logger = LogManager.GetLogger(typeof(UserSecurityApp));

        //获取用户安全概况
        public UserSecurityInfo GetSecurityInfo(string userGuid)
        {
            //获取用户信息
            var user = _DbSession.UserStorager.SelectOne(userGuid);

            //获取用户密保问题
            var questions = _DbSession.UserPwQuestionStorager.Select(user.Id);

            UserSecurityInfo info = new UserSecurityInfo
            {
                PhoneNumberConfirmed = user.PhoneNumberConfirmed == 1,
                IdentityConfirmed = user.IdentityConfirmed == 1,
                EmailConfirmed = user.EmailConfirmed == 1,
                SecurityQuestion = questions.Count() > 0,
                SecurityLevel = "中"
            };

            int count = 0;

            if (info.PhoneNumberConfirmed) count++;
            if (info.IdentityConfirmed) count++;
            if (info.EmailConfirmed) count++;
            if (info.SecurityQuestion) count++;

            if (count >= 2)
                info.SecurityLevel = "高";

            return info;

        }




        //预留－验证支付密码
        public ReturnResult ValidatePayPassword(string userId, string payPassword)
        {
            var result = new ReturnResult { IsSuccess = true };

            //添加验证逻辑

            return result;
        }





        #region 密保相关

        //创建或修改密保
        public ReturnResult CreateQuestionAndAnswer(List<UserPwQuestion> questions, int userId)
        {
            var returnResult = new ReturnResult { IsSuccess = true };

            //三项问题及答案
            if (questions.Count < 3)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "密保必须三项问题及答案";
                return returnResult;
            }

            //查询用户设定问题
            var items = _DbSession.UserPwQuestionStorager.Select(userId);

            using (TransactionScope scope = new TransactionScope())
            {
                //删除已经设定的问题及答案
                if (items.Count() > 0)
                    _DbSession.UserPwQuestionStorager.Delete(userId);

                //重新设定问题及答案
                foreach (var question in questions)
                {
                    question.UserId = userId;
                    bool isSuccess = _DbSession.UserPwQuestionStorager.Add(question);

                    if (!isSuccess)
                    {
                        returnResult.IsSuccess = false;
                        returnResult.Message = "重新设定密保失败";
                        return returnResult;
                    }
                }

                scope.Complete();
                return returnResult;
            }
        }

        //验证密保
        public ReturnResult ValidateQuestionAndAnswer(List<UserPwQuestion> questions, int userId)
        {
            var returnResult = new ReturnResult { IsSuccess = true };

            //三项问题及答案
            if (questions.Count < 3)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "密保必须三项问题及答案";
                return returnResult;
            }

            //查询用户设定问题
            var items = _DbSession.UserPwQuestionStorager.Select(userId);

            //验证
            foreach (var item in items)
            {
                var isYes = questions.Where(x => x.PwId == item.PwId && x.Answer == item.Answer).Count() > 0;

                if (!isYes)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "密保问题及答案验证不通过";
                    return returnResult;
                }
            }

            return returnResult;
        }

        //查询用户设定的密保
        public IEnumerable<UserPwQuestion> SelectQuestionAndAnswer(int userId)
        {
            return _DbSession.UserPwQuestionStorager.Select(userId);
        }

        //查询密保问题
        public IEnumerable<PwQuestion> SelectQuestion()
        {
            return _DbSession.PwQuestionStorager.Select();
        }

        #endregion

        #region 验证码相关

        //发送手机验证码
        public ReturnResult SendMobileVerifyCode(string phoneNumber, int number, string dataSource)
        {
            //初始化返回结果
            var returnResult = new ReturnResult { IsSuccess = true };


            int countDaylimit = _AppContext.ValidateCodeApp.GetCount(phoneNumber);
            if (countDaylimit >= 5)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "超出今日验证码请求次数，请联系客服或管理员";
                return returnResult;
            }

            //int count1 = _AppContext.ValidateCodeApp.InMinuteValidateCount(phoneNumber);

            //if (count1 >= 1)
            //{
            //    returnResult.IsSuccess = false;
            //    returnResult.Message = "1分钟内只能发送一次验证码，请稍后再试";
            //    return returnResult;
            //}

            //int count2 = _AppContext.ValidateCodeApp.InHourValidateCount(phoneNumber);

            //if (count2 >= 3)
            //{
            //    returnResult.IsSuccess = false;
            //    returnResult.Message = "1小时内只能发送三次验证码，请稍后再试";
            //    return returnResult;
            //}

            //int count3 = _AppContext.ValidateCodeApp.GetValidateCountByPhoneNumber(phoneNumber);
            //if(count3 >= 30)
            //{
            //    returnResult.IsSuccess = false;
            //    returnResult.Message = "您的验证码请求总次数超出，请联系客服或管理员";
            //    return returnResult;
            //}

            //获取指定位数的随机验证码
            string code = SMSHelper.GetValidateCode(number);

            //发送验证码
            SmsApp sms = new SmsApp();
            var result = sms.SendSMS(phoneNumber, string.Format("您的验证码是：{0}，3分钟有效。请您尽快登录，切勿转发或泄漏他人。", code));

            //是否发送成功
            if (!result.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            //保存数据
            bool isSuccess = _DbSession.ValidateCodeStorager.Add(new ValidateCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                DataSource = dataSource
            });

            //保存失败的情况
            if (!isSuccess)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "验证码数据存储失败";
                return returnResult;
            }

            return returnResult;
        }
        //保存手机验证码
        public bool SaveMobileVerifyCode(string phoneNumber, string code, string dataSource)
        {
           //保存数据
            return _DbSession.ValidateCodeStorager.Add(new ValidateCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                DataSource = dataSource
            });
        }

        //验证手机验证码是否正确
        public ReturnResult ValidateMobileVerifyCode(string phoneNumber, string verifyCode)
        {
            //初始化返回结果
            var returnResult = new ReturnResult { IsSuccess = true };

            //查询验证码
            var code = _DbSession.ValidateCodeStorager.Select(phoneNumber);

           // logger.InfoFormat("查询验证码是否正确，手机号={0}， 数据库验证码={1}，用户post={2}", phoneNumber, code.Code,verifyCode);

            //todo:tsunuen-万能验证码-begin
            //if (verifyCode == "0000")
            //{
            //    return returnResult;
            //}
            //todo:tsunuen-万能验证码-end

            //验证正确性
            if (code == null || code.Code != verifyCode)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "验证码错误";
                return returnResult;
            }

            //验证是否过期
            if (DateTime.Now > code.CreateTime.AddMinutes(3))
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "验证码有效期已过，请重新发送";
                return returnResult;
            }

            return returnResult;
        }

        public ReturnResult SendVerifyCodeWithMessage(string phoneNumber, int number, string message)
        {
            //初始化返回结果
            var returnResult = new ReturnResult { IsSuccess = true };

            int count = _AppContext.ValidateCodeApp.GetCount(phoneNumber);

            if (count >= 10)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "超出今日短信发送次数";
                return returnResult;
            }

            //获取指定位数的随机验证码
            string code = SMSHelper.GetValidateCode(number);

            //发送验证码
            SmsApp sms = new SmsApp();
            var result = sms.SendSMS(phoneNumber, string.Format("{1}。您的验证码是： {0}， 请立即使用。", code, message));

            //是否发送成功
            if (!result.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            //保存数据
            bool isSuccess = _DbSession.ValidateCodeStorager.Add(new ValidateCode
            {
                PhoneNumber = phoneNumber,
                Code = code
            });

            //保存失败的情况
            if (!isSuccess)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "验证码数据存储失败";
                return returnResult;
            }

            return returnResult;
        }

        #endregion


        public ReturnResult FindPassword(string phoneNumber,string  dataSource)
        {
            var returnResult = new ReturnResult { IsSuccess = true };
            int countDaylimit = _AppContext.ValidateCodeApp.GetCount(phoneNumber);
            if (countDaylimit >= 5)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "超出今日验证码请求次数，请联系客服或管理员";
                return returnResult;
            }
                SmsApp sms = new SmsApp();
                string code = SMSHelper.GetValidateCode(4);
                var result=  sms.SendSMS(Entity.Enum.ESmsType.找回密码, phoneNumber, new string[] { code });
                if (!result.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "短信发送失败";
                    return result;
                }


                //保存数据
                bool isSuccess = _DbSession.ValidateCodeStorager.Add(new ValidateCode
                {
                    PhoneNumber = phoneNumber,
                    Code = code,
                    DataSource = dataSource
                });

                //保存失败的情况
                if (!isSuccess)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "验证码数据存储失败";
                    return returnResult;
                }
                return returnResult;

        }

        public ReturnResult ChangePassword(string  phoneNumber,string dataSource)
        {
            var returnResult = new ReturnResult { IsSuccess = true };
            int countDaylimit = _AppContext.ValidateCodeApp.GetCount(phoneNumber);
            if (countDaylimit >= 5)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "超出今日验证码请求次数，请联系客服或管理员";
                return returnResult;
            }
            SmsApp sms = new SmsApp();
            string code = SMSHelper.GetValidateCode(4);
            var result = sms.SendSMS(Entity.Enum.ESmsType.重置密码, phoneNumber, new string[] { code });
            if (!result.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            //保存数据
            bool isSuccess = _DbSession.ValidateCodeStorager.Add(new ValidateCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                DataSource = dataSource
            });

            //保存失败的情况
            if (!isSuccess)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "验证码数据存储失败";
                return returnResult;
            }
            return returnResult;
        }



        public ReturnResult RegisterAcount(string phoneNumber, string dataSource)
        {
            var returnResult = new ReturnResult { IsSuccess = true };
            int countDaylimit = _AppContext.ValidateCodeApp.GetCount(phoneNumber);
            if (countDaylimit >= 5)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "超出今日验证码请求次数，请联系客服或管理员";
                return returnResult;
            }
            SmsApp sms = new SmsApp();
            string code = SMSHelper.GetValidateCode(4);
            var result = sms.SendSMS(Entity.Enum.ESmsType.提交注册_发送验证码, phoneNumber, new string[] { code });
            if (!result.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            //保存数据
            bool isSuccess = _DbSession.ValidateCodeStorager.Add(new ValidateCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                DataSource = dataSource
            });

            //保存失败的情况
            if (!isSuccess)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "验证码数据存储失败";
                return returnResult;
            }
            return returnResult;
        }
        public ReturnResult BindMobile()
        {
            throw new NotImplementedException();
        }

        public ReturnResult ChangeBindMobile()
        {
            throw new NotImplementedException();
        }

        public ReturnResult BindEmail()
        {
            throw new NotImplementedException();
        }

        public ReturnResult ChangeBindEmail()
        {
            throw new NotImplementedException();
        }

        public ReturnResult SendConfirmEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
