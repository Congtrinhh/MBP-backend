using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Entity representing a client review of an MC.
    /// </summary>
    public class ClientReviewMC : BaseEntity
    {

        /// <summary>
        /// Foreign key to the client.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Foreign key to the MC.
        /// </summary>
        public int? MCId { get; set; }

        /// <summary>
        /// Foreign key to the contract.
        /// </summary>
        public int? ContractId { get; set; }

        /// <summary>
        /// Client's rating of the MC's professional skills (scale from 1 to 5).
        /// </summary>
        [Required]
        public int ProPoint { get; set; }

        /// <summary>
        /// Client's rating of the MC's work attitude (scale from 1 to 5).
        /// </summary>
        [Required]
        public int AttitudePoint { get; set; }

        /// <summary>
        /// Indicates whether the MC was punctual.
        /// </summary>
        public bool? IsPunctual { get; set; }

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
