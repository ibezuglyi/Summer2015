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
    }
}