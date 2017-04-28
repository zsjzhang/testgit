namespace Vcyber.BLMS.Entity
{
    public class BlueMemberPrize
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Total { get; set; }

        public int LeftNum { get; set; }

        public bool IsDeleted { get; set; }
        public string PrizeType { get; set; }
    }
}