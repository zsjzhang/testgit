using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class ApproveRecordStorage: IApproveRecordStorager
    {
        public bool CreateApproveRecord(ApproveRecord record)
        {
            string sql = "Insert into ApprovalRecord(ItemType,ItemId,CreateTime,UpdateTime,IsApproval,ApprovalMemo,OperatorId,OperatorName) values(@ItemType,@ItemId,@CreateTime,@UpdateTime,@IsApproval,@ApprovalMemo ,@OperatorId,@OperatorName)";
            return DbHelp.Execute(sql, new
            {
                @IsApproval = record.IsApproval,
                @ApprovalMemo = record.ApprovalMemo,
                @OperatorId = record.OperatorId,
                @OperatorName = record.OperatorName,
                @CreateTime = DateTime.Now,
                @UpdateTime = DateTime.Now,
                @ItemType = record.ItemType,
                @ItemId = record.ItemId
            }) > 0;
        }
        public bool UpdateStatus(ApproveRecord record) 
        {
            string sql = "Update ApprovalRecord set IsApproval =@IsApproval,ApprovalMemo=@ApprovalMemo,OperatorId=@OperatorId,OperatorName=@OperatorName,UpdateTime=@UpdateTime where ItemType=@ItemType and ItemId=@ItemId";
            return DbHelp.Execute(sql, new { @IsApproval = record.IsApproval,
                                             @ApprovalMemo = record.ApprovalMemo,
                                             @OperatorId = record.OperatorId,
                                             @OperatorName = record.OperatorName,
                                             @UpdateTime = DateTime.Now,
                                             @ItemType = record.ItemType,
                                             @ItemId = record.ItemId
            }) > 0;

        }

        public IEnumerable<ApproveRecord> GetApproveRecordList(int itemId, string itemType) 
        {
            string sql = "SELECT * FROM ApprovalRecord where ItemType=@ItemType and ItemId=@ItemId ORDER BY UpdateTime desc";

            return DbHelp.Query<ApproveRecord>(sql, new { @ItemType = itemType, @ItemId=itemId });
        }

        public ApproveRecord GetLatestRecord(int itemId, string itemType)
        {
            string sql = "SELECT top 1 * FROM ApprovalRecord where ItemType=@ItemType and ItemId=@ItemId ORDER BY UpdateTime desc";

            return DbHelp.QueryOne<ApproveRecord>(sql, new { @ItemType = itemType, @ItemId = itemId });
        }
    }
}
