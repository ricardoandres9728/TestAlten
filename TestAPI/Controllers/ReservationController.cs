using Microsoft.AspNetCore.Mvc;
using TestAPI.Services;
using TestDAL.Models;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService ReservationService;
        public ReservationController(IReservationService reservationService)
        {
            //Service injection, Scopped in Program.cs
            ReservationService = reservationService;
        }

        // GET: api/<Reservation>?code={code}
        [HttpGet]
        public IActionResult Get([FromQuery] string? code = null)
        {

            if (code == null)
            {
                return Ok(ReservationService.Get());
            }
            else
            {
                return Ok(ReservationService.Get(code));
            }
        }

        // GET api/<Reservation>/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Reservation reservation = ReservationService.Get(id);

            if (reservation.Id != 0)
            {
                return Ok(reservation);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<Reservation>
        [HttpPost]
        public IActionResult Post([FromBody] Reservation reservation)
        {
            //Validates if dates of reservation are valid in order to insert
            List<string> errors = ReservationService.ValidateDates(reservation.StartReservation, reservation.EndReservation).ToList();

            if(errors.Count == 0)
            {
                ReservationService.Save(reservation);
                return Ok();
            }
            else
            {
                return BadRequest(errors);
            }

            
        }

        // PUT api/<Reservation>/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Reservation reservation)
        {
            //Validates if dates of reservation are valid in order to update
            List<string> errors = ReservationService.ValidateDates(reservation.StartReservation, reservation.EndReservation, id).ToList();

            if (errors.Count == 0)
            {
                ReservationService.Update(id, reservation);
                return Ok();
            }
            else
            {
                return BadRequest(errors);
            }
        }

        // DELETE api/<Reservation>/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ReservationService.Delete(id);
            return Ok();
        }
    }
}
