using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Connection : IEntity
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;

        [Column("connection_id")]
        public string ConnectionId { get; set; }

        [Column("account_id")]
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
