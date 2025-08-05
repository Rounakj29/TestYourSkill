using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestController : ControllerBase
    {
        private readonly IUserAndAiMediatorSeervice _mediatorSeService;
        private readonly string VerificationKey = "Tacos";
        public UserRequestController(IUserAndAiMediatorSeervice mediatorSeService)
        {
            _mediatorSeService = mediatorSeService;
        }

        // GET: api/<UserRequestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserRequestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserRequestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequests value)
        {
            if (value == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            var result = await _mediatorSeService.ProcessUserRequestAsync(value);

            return Ok(result);
        }
        [HttpPost("VerifyAccess")]
        public async Task<IActionResult> VerifyAccess(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Request body cannot be null.");
            }
            var result = value.Equals(VerificationKey, StringComparison.OrdinalIgnoreCase)
                ? "Access Granted"
                : "Access Denied";
            bool isAccessGranted = result.Equals("Access Granted", StringComparison.OrdinalIgnoreCase);
            
            return Ok(isAccessGranted);
        }

        // PUT api/<UserRequestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserRequestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
