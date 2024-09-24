namespace Domain.Entities
{
    /// <summary>
    /// type of MC: event MC/ wedding MC/ ...
    /// </summary>
    public class MCType : BaseEntity
    {
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
