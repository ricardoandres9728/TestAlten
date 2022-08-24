using TestDAL.Models;
using TestDAL;

namespace TestAPI.Services
{
    public class ReservationService : IReservationService
    {
        readonly TestBDContext TestBDContext;

        public ReservationService(TestBDContext testBDContext)
        {
            //BDContext injection, defined in Program.cs
            TestBDContext = testBDContext;
        }

        //Gets all the reservations
        public IEnumerable<Reservation> Get()
        {
            try
            {
                IEnumerable<Reservation>? reservations = TestBDContext.Reservations;
                if (reservations != null)
                    return reservations;
                else
                    return Enumerable.Empty<Reservation>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting reservation. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Finds reservation by ID
        public Reservation Get(int id)
        {
            try
            {
                Reservation reservation = TestBDContext.Reservations?.Find(id) ?? new();
                return reservation;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting reservation by ID. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Finds reservation by reservation code
        public Reservation Get(string code)
        {
            try
            {
                Reservation reservation = (from rsvt in TestBDContext.Reservations where rsvt.Code == code.ToUpper() select rsvt).FirstOrDefault() ?? new();
                return reservation;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting client by ID. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Inserts reservation in database
        public void Save(Reservation reservation)
        {
            try
            {
                //Generates random reservation code
                reservation.Code = GenerateCode();
                //Sets current date as date of creation
                reservation.ReservationDate = DateTime.Now;
                TestBDContext.Add(reservation);
                TestBDContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting reservation. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Updates existing reservation, finds by Id
        public void Update(int id, Reservation reservation)
        {
            try
            {
                Reservation currentInfo = TestBDContext.Reservations?.Find(id) ?? new();

                if (currentInfo.Id != 0)
                {
                    currentInfo.State = reservation.State;
                    currentInfo.StartReservation = reservation.StartReservation;
                    currentInfo.EndReservation = reservation.EndReservation;
                    currentInfo.PaymentMethod = reservation.PaymentMethod;

                    if (reservation.ClientId.HasValue)
                    {
                        currentInfo.ClientId = reservation.ClientId;
                    }
                    TestBDContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting client by ID. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Deletes existing reservation, finds by Id
        public void Delete(int id)
        {
            try
            {
                Reservation reservation = TestBDContext.Reservations?.Find(id) ?? new();

                if (reservation.Id != 0)
                {
                    TestBDContext.Remove(reservation);
                    TestBDContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting client by ID. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        public IEnumerable<string> ValidateDates(DateTime startDate, DateTime endDate, int? id = null)
        {
            List<string> errors = new();

           

            //Validates availability between dates
            IQueryable<Reservation> reservationQuery = (from rsvt in TestBDContext.Reservations
                                        where ((rsvt.StartReservation.Date >= startDate.Date && startDate.Date <= rsvt.EndReservation.Date)
                                        || (rsvt.StartReservation.Date >= endDate.Date && endDate.Date <= rsvt.EndReservation.Date))
                                        && rsvt.State == ReservationState.Active
                                        select rsvt);

            //In case of update, excludes id from query
            if (id.HasValue)
            {
                reservationQuery = reservationQuery.Where(x => x.Id != id);
            }

            Reservation? reservation = reservationQuery.FirstOrDefault();

            if (reservation != null)
            {
                errors.Add("Already exists a reservation in the chosen dates");
            }

            //Validates if the number of days between of the dates isnt bigger than 3
            if ((startDate.Date - endDate.Date).Days > 3)
            {
                errors.Add("The stay can't be longer than 3 days");
            }

            //Validates if the reservation is 30 days ahead of time
            if ((startDate.Date - DateTime.Today.Date).Days > 30)
            {
                errors.Add("The room can't be booked more than 30 days in advance");
            }

            //Validates if the user is selecting a date before today
            if (startDate.Date < DateTime.Today.Date)
            {
                errors.Add("Invalid start date");
            }

            //Validates if the start date is before the end date
            if (startDate.Date >= endDate.Date)
            {
                errors.Add("End date of the reservation can't be before the start date or equal");
            }

            return errors;
        }

        //Method that generates a random reservation code
        private static string GenerateCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[6];
            Random random = new();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string code = new(stringChars);
            return code.ToUpper();
        }
    }

    //Creates interface in order to expose methods to be consummed
    public interface IReservationService
    {
        public IEnumerable<Reservation> Get();
        public Reservation Get(int id);
        public Reservation Get(string code);
        public void Save(Reservation reservation);
        public void Update(int id, Reservation reservation);
        public void Delete(int id);
        public IEnumerable<string> ValidateDates(DateTime startDate, DateTime endDate);
    }
}
