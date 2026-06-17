using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContract.DTOs;
using ServiceContract.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Person> AddPerson(Person person)
        {
            await this._dbContext.Persons.AddAsync(person);
            await this._dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            this._dbContext.Persons.RemoveRange(this._dbContext.Persons.Where(person => person.PersonID == personId));
            int affectedRows = await this._dbContext.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await this._dbContext.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate)
        {
            return await this._dbContext.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid id)
        {
            return await this._dbContext.Persons.Include("Country").FirstOrDefaultAsync(person => person.PersonID == id);
        }

        public async Task<Person?> UpdatePerson(Person requestPerson)
        {
            Person? matchingPerson = await this._dbContext.Persons.Include("Country").FirstOrDefaultAsync(person => person.PersonID == requestPerson.PersonID);

            if (matchingPerson == null)
            {
                return null;
            }

            matchingPerson.Address = requestPerson.Address;
            matchingPerson.Gender = requestPerson.Gender;
            matchingPerson.CountryID = requestPerson.CountryID;
            matchingPerson.DateOfBirth = requestPerson.DateOfBirth;
            matchingPerson.PersonName = requestPerson.PersonName;
            matchingPerson.ReceiveNewsLetters = requestPerson.ReceiveNewsLetters;
            matchingPerson.Email = requestPerson.Email;

            await this._dbContext.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
