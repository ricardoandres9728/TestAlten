using System.Text.Json.Serialization;
using TestAPI.Converters;

namespace TestDAL.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartReservation { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndReservation { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ReservationDate { get; set; }

        public string? Code { get; set; }

        public ReservationState State { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public virtual Client? Client { get; set; }

    }

    public enum ReservationState
    {
        Active,

        Inactive,

        Canceled,

    }

    public enum PaymentMethod
    {
        Cash,

        Transfer,

        CredidCard
    }
}
