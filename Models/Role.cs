using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("t_roles")]
    [Index(nameof(Name), IsUnique = true)]
    public class Role : IEntity
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(25)]
        public string Name { get; set; }

        public virtual List<Account> Accounts { get; set; } = new();
    }
}
