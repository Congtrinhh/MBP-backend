using Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// a master of ceremony
    /// </summary>
    public class MC : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string NickName { get; set; } = string.Empty;
        public decimal Credit { get; set; }
        public Sex Sex { get; set; }
        public bool IsNewbie { get; set; }
        public string WorkingArea { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
