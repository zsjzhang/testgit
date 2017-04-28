using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IInvoiceForReserveRepository
    {
        int Insert(InvoiceForReserve entity);
        int InsertUserProofRecord(InvoiceForReserve entity);
        bool UpdateUserProofRecord(InvoiceForReserve entity);
        
        bool UpdateProofStatus(string id, string status);
        bool DeleteProofInfo(string id);
        InvoiceForReserve GetProofInfoById(string id);
        IEnumerable<InvoiceForReserve> GetSearch(string phone, string mlevel, string paperwork, string identitynumber, string status, string createTime, string end, int pageCount, int currentPage, out int totalCount);
    }
}
