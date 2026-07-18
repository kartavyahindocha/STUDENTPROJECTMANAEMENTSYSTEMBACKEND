namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    public abstract class BaseModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
