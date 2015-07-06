using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;
using WebApp.Models;
using WebApp.Models.Account;

namespace WebApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private JobContext dbContext;

        public ApplicationService()
        {
            dbContext = new JobContext();
        }

        public async Task<RecruiterUser> GetRecruterByEmailAsync(string email)
        {
            var user = await dbContext.RecruiterUsers.Find(x => x.Email == email).SingleOrDefaultAsync();
            return user;
        }
        public async Task<CandidateUser> GetCandidateByEmailAsync(string email)
        {
            var user = await dbContext.CandidateUsers.Find(x => x.Email == email).SingleOrDefaultAsync();
            return user;
        }

        public async Task<RecruiterUser> GetRecruiterByIdAsync(string id)
        {
            var recruiter = await dbContext.RecruiterUsers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return recruiter;
        }

        public async Task<CandidateUser> GetCandidateByIdAsync(string id)
        {
            var candidate = await dbContext.CandidateUsers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return candidate;
        }

        public async Task CreateRecruiterUserAsync(RegisterModel model)
        {
            var user = new RecruiterUser
            {
                Name = model.Name,
                Email = model.Email,
            };

            user.Password = GenerateHashPassword(model.Password, user);
            await dbContext.RecruiterUsers.InsertOneAsync(user);
        }

        public async Task UpdateRecruiterUserAsync(RecruiterUser model, string id)
        {
            var filter = Builders<RecruiterUser>.Filter.Eq(r => r.Id, id);
            var update = Builders<RecruiterUser>
                .Update
                .Set(r => r.CompanyDescription, model.CompanyDescription)
                .Set(r => r.CompanyName, model.CompanyName)
                .Set(r => r.Surname, model.Surname);

            await dbContext.RecruiterUsers.UpdateOneAsync(filter, update);
        }


        public string GenerateHashPassword(string password, User user)
        {
            SHA1 sha1 = SHA1.Create();
            string dataToHash = user.Name + password + user.Email;
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(dataToHash));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        public async Task CreateCandidateUserAsync(RegisterModel model)
        {
            var user = new CandidateUser
            {
                Name = model.Name,
                Email = model.Email,
            };

            user.Password = GenerateHashPassword(model.Password, user);
            await dbContext.CandidateUsers.InsertOneAsync(user);
        }

        public async Task UpdateCandidateUserAsync(CandidateUser model, string id)
        {            
            var filter = Builders<CandidateUser>.Filter.Eq(r => r.Id, id);
            var update = Builders<CandidateUser>
                .Update
                .Set(r => r.ExperienceDescription, model.ExperienceDescription)
                .Set(r => r.ExperienceInYears, model.ExperienceInYears)
                .Set(r => r.LastName, model.LastName)
                .Set(r => r.Surname, model.Surname)
                .Set(r => r.Salary, model.Salary);

            await dbContext.CandidateUsers.UpdateOneAsync(filter, update);
        }
    }
}