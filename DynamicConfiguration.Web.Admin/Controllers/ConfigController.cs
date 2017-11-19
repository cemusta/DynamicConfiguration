using System.Collections.Generic;
using System.Linq;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace DynamicConfiguration.Web.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/Config")]
    public class ConfigController : Controller
    {
        private readonly IConfigurationRepository _repo;

        public ConfigController(IConfigurationRepository repository)
        {
            _repo = repository;
        }

        [HttpGet(Name = "GetAll")]
        public List<Configuration> Get()
        {            
            return _repo.Repository.All().ToList();
        }

        // GET: api/Config/5
        [HttpGet("{id}", Name = "Get")]
        public Configuration Get(string id)
        {
            var objectId = new ObjectId(id);
            return _repo.Repository.GetById(objectId);
        }
        
        // POST: api/Config
        [HttpPost]
        public void Post([FromBody]Configuration value)
        {
            _repo.Repository.Add(value);
        }
        
        // PUT: api/Config/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]Configuration value)
        {
            var objectId = new ObjectId(id);
            _repo.Repository.Update(value, objectId);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var objectId = new ObjectId(id);
            _repo.Repository.Delete(objectId);
        }
    }
}
