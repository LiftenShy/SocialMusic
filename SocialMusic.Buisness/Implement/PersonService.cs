
using System;
using System.Collections.Generic;
using SocialMusic.Buisness.Abstract;
using SocialMusic.Data.Abstract;
using SocialMusic.Models.EntityModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMusic.Buisness
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person> _personRepository;

        public PersonService(IRepository<Person> persoRepository)
        {
            _personRepository = persoRepository;
        }

        public async Task<List<Person>> GetAll()
        {
            return await _personRepository.GetAll();
        }

        public async Task<Person> GetById(int id)
        {
            return await _personRepository.GetById(id);
        }

        public async Task<Person> GetByName(string name)
        {
            return await _personRepository.Table.FirstOrDefaultAsync(p => p.Name.Contains(name));
        }

        public async Task Update(Person newPerson, int id)
        {
            var updatePerson = _personRepository.GetById(id).Result;
            updatePerson = newPerson;
            await _personRepository.Update(updatePerson);
        }

        public async Task Insert(Person person)
        {
            if (!_personRepository.Table.Any(p => p.Name == person.Name))
            {
                await _personRepository.Insert(person);
            }
            else
            {
               throw new Exception("Name already have");
            }
        }

        public async Task Remove(int id)
        {
            if (_personRepository.Table.Any(p => p.Id == id))
            {
                await _personRepository.Delete(_personRepository.Table.FirstOrDefault(p => p.Id == id));
            }
            else
            {
                throw new Exception("Don't have user");
            }
        }
    }
}
