using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestDAL.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Identification { get; set; }

        [JsonIgnore]
        public virtual ICollection<Reservation>? Reservations { get; set; }

    }
}
