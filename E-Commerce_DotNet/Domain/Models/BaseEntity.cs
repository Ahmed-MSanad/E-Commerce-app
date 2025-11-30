namespace Domain.Models
{// Parent for all Entities {Domain Models} => {SQL Server DB } => StoreDbContext
    public class BaseEntity<Tkey>
    {
        public Tkey Id { get; set; } = default!;
    }
}
