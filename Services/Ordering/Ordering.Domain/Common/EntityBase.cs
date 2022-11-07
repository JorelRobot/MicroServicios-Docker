namespace Ordering.Domain.Common
{
    public class EntityBase
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string? LastMidifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
