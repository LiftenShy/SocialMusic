using System;
using Microsoft.AspNetCore.Mvc;
using SocialMusic.Models.EntityModels;
using System.Diagnostics;
using System.Threading.Tasks;
using SocialMusic.Buisness.Abstract;

namespace SocialMusic.API.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
            if (_personService == null)
            {
                Debug.WriteLine($"Null is {nameof(_personService)}");
            }
            else
            {
                Debug.WriteLine($"PersonService {nameof(_personService)}");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            return new JsonResult(await _personService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            return new JsonResult(await _personService.GetById(id));
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody] Person person)
        {
            try
            {
                if (person != null)
                {
                    await _personService.Insert(person);
                    return new JsonResult("Succsess insert");
                }
                return new JsonResult("person is null");
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Person newPerson)
        {
            await _personService.Update(newPerson, id);
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _personService.Remove(id);
                    return new JsonResult("Succsess delete");
                }
                return new JsonResult("id is zero");
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }
    }
}
