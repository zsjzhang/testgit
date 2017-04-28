using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common.Payment.Model;
using Vcyber.BLMS.Common.Payment.Model.ReponseCreate;
using Vcyber.BLMS.Common.Payment.Model.Request;
using Vcyber.BLMS.Common.Payment.Model.Response;
using Vcyber.BLMS.Common.Payment.Model.ResponseNotify;
using Vcyber.BLMS.Common.Payment.Model.ResponseRefund;
namespace Vcyber.BLMS.Common.Payment
{
    public class PaymentForTmall:IPayment
    {

        public ResponseRefund Refund(RequestRefund obj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 企业付款
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResponseGiveFund GiveFund(RequestGiveFund obj)
        {
            throw new NotImplementedException();
        }

        public ResponseCreate Create(RequestCreate obj)
        {
            //如果金额是100，应该跳转的链接
            var result = "https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.3.cmKogk&id=44232536739&rn=f01d8240d8d928afde8c46ce6322d111&abbucket=8&skuId=3184713456111&scene=taobao_shop ";
            //如果金额是50，应该跳转的链接
            if (obj.TotalFee == 50)
            {
                result = "https://detail.tmall.com/item.htm?spm=a1z10.4-b.w5003-12994442256.2.cmKogk&id=533870432232&rn=eff56e7365ac38eb25caa9cacf22327b&abbucket=5&skuId=3184550976675&scene=taobao_shop ";
            }
            return new ResponseCreate() { result_code = result };
        }


        public ResponseNotify Notify(string content)
        {
            throw new NotImplementedException();
        }

        public Model.ResponseQuery.ResponseQuery Query(RequestQuery obj)
        {
            throw new NotImplementedException();
        }
    }
}
