//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vcyber.BLMS.FrontWeb.Models.BBS
{
    using System;
    using System.Collections.Generic;
    
    public partial class BBSGuestBook
    {
        public BBSGuestBook()
        {
            this.BBSComment = new HashSet<BBSComment>();
        }
    
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string G_Content { get; set; }
        public Nullable<System.DateTime> G_Time { get; set; }
        public string G_Title { get; set; }
        public string G_HF_Content { get; set; }
        public Nullable<int> G_Recycle { get; set; }
        public int G_ReadCount { get; set; }
        public int G_ResponseCount { get; set; }
        public string LastUpdateMemberName { get; set; }
        public System.DateTime LastUpdateTime { get; set; }
        public int Column_Id { get; set; }
        public bool G_approved { get; set; }
        public bool G_isTop { get; set; }
    
        public virtual BBSColumns BBSColumns { get; set; }
        public virtual ICollection<BBSComment> BBSComment { get; set; }
        public virtual BBSMember BBSMember { get; set; }
    }
}