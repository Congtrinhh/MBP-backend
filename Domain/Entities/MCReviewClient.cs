using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Entity representing an MC's review of a client.
    /// </summary>
    public class MCReviewClient : BaseEntity
    {
        /// <summary>
        /// Foreign key to the MC.
        /// </summary>
        public int? MCId { get; set; }

        /// <summary>
        /// Foreign key to the client.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Foreign key to the contract.
        /// </summary>
        public int? ContractId { get; set; }

        /// <summary>
        /// MC's rating of the client's payment punctuality (scale from 1 to 5).
        /// </summary>
        [Required]
        public int PaymentPunctualPoint { get; set; }

        /// <summary>
        /// Title of the post.
        /// </summary>
        public string Caption { get; set; } = string.Empty;

        /// <summary>
        /// Content of the post.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
