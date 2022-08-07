using DataService.Models;
using DataService.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExposeDataService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineStreamController : ControllerBase
    {
        private readonly StoreService _storeService;
        public MachineStreamController(StoreService storeService)
        {
            _storeService = storeService;
        }
        // GET: api/<MachineStreamController>
        [HttpGet]
        public async Task<List<MachineStream>> Get()
        {
            return await _storeService.GetAsync();
        }

        // GET api/<MachineStreamController>/5
        [HttpGet("{id}")]
        public async Task<MachineStream?> Get(string id)
        {
            var machineStream = await _storeService.GetAsync(id);
            return machineStream;
        }

        // POST api/<MachineStreamController>
        [HttpPost]
        public async Task Post([FromBody] MachineStream value)
        {
            await _storeService.CreateAsync(value);
        }

        // PUT api/<MachineStreamController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] MachineStream value)
        {
            await _storeService.UpdateAsync(id, value);
        }

        // DELETE api/<MachineStreamController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _storeService.RemoveAsync(id);
        }
    }
}
