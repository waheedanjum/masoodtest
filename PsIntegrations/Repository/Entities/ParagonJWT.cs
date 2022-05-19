using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class ParagonJWT : BaseEntity
    {
        public string Token { get; set; }
        public decimal ExpireAt { get; set; }
    }
}
