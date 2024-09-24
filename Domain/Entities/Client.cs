using Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// client that books MC
    /// </summary>
    public class Client : BaseEntity
    {
        public decimal Credit { get; set; }
        public Sex Sex { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;     
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
