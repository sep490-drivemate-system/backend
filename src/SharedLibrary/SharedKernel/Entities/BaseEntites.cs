using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Entities
{
    public abstract class BaseEntites
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        [Column("create_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
