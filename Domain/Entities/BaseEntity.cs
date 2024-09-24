using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Base entity class for all entities
    /// </summary>
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public BaseEntity()
        {
                
        }
    }
}
