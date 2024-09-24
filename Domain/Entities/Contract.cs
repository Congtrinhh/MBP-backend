namespace Domain.Entities
{
    /// <summary>
    /// A contract between a client and a MC
    /// </summary>
    public class Contract : BaseEntity
    {
        public int McId { get; set; }
        public int ClientId { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DepositPercentage { get; set; }
        public string Place { get; set; } = string.Empty;
        public bool HasPayAll { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsConfirmed { get; set; }
        public string RefuseReason { get; set; } = string.Empty;
    }
}
