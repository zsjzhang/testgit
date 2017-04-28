using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application.Invoice;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.Invoice
{
    public class InvoiceForReserveApp : IInvoiceForReserveApp
    {
        public int Insert(InvoiceForReserve entity)
        {
            return _DbSession.InvoiceForReserveRepository.Insert(entity);
        }
         public int InsertUserProofRecord(InvoiceForReserve entity)
        {
            return _DbSession.InvoiceForReserveRepository.InsertUserProofRecord(entity);
        }
        public bool UpdateUserProofRecord(InvoiceForReserve entity)
         {
             return _DbSession.InvoiceForReserveRepository.UpdateUserProofRecord(entity);
         }
        public bool UpdateProofStatus(string id, string status )
         {

             return _DbSession.InvoiceForReserveRepository.UpdateProofStatus(id, status);
         }
        public bool DeleteProofInfo(string id)
        {
            return _DbSession.InvoiceForReserveRepository.DeleteProofInfo(id);
        }
        public InvoiceForReserve GetProofInfoById(string id)
        {

            return _DbSession.InvoiceForReserveRepository.GetProofInfoById(id);
        }

        public IEnumerable<InvoiceForReserve> GetSearch(string phone, string mlevel, string paperwork, string identitynumber, string status, string createTime, string end, int pageCount, int currentPage, out int totalCount)
        {
            return _DbSession.InvoiceForReserveRepository.GetSearch(phone,mlevel,paperwork,identitynumber,status,createTime,end,pageCount,currentPage,out totalCount);
        }
        
    }
}
