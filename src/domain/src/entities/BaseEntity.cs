
namespace domain.src.entities;
public abstract class BaseEntity
{
    public Guid Id { get; set; }
     public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; }="System";

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}