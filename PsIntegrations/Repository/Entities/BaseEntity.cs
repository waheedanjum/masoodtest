using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class GeneralBaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
    public class BaseEntity : GeneralBaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
