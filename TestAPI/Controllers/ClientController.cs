using Microsoft.AspNetCore.Mvc;
using TestAPI.Services;
using TestDAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService ClientService;
        public ClientController(IClientService clientService)
        {
            //Service injection, Scopped in Program.cs
            ClientService = clientService;
        }

        // GET: api/<Client>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ClientService.Get());
        }

        // GET api/<Client>/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Client client = ClientService.Get(id);

            if(client.Id != 0)
            {
                return Ok(client);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<Client>
        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            ClientService.Save(client);
            return Ok();
        }

        // PUT api/<Client>/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            ClientService.Update(id, client);
            return Ok();
        }

        // DELETE api/<Client>/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ClientService.Delete(id);
            return Ok();
        }
    }
}
