using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Entities;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class TestController : Controller
    {
        private IApplicationService _applicationService;
        private IDatabaseService _databaseService;
        private List<string> _skillNames;

        public TestController(IApplicationService applicationService, IDatabaseService databaseService)
        {
            _applicationService = applicationService;
            _databaseService = databaseService;
            _skillNames = new List<string>()
            {
                "Pascal", "Delphi", "Assembler", "C++", "C", "C#", "HTML", 
                "CSS", "JavaScript", "jQuery", "Python", "Scala", ".NET",
                "ASP.NET MVC", "WebForms", "EmberJs", "AngularJS", "ReactJS"
            };
        }


        [HttpGet]
        public async Task<ActionResult> GenerateRecruiterTestSet()
        {
            int numberOfRecruiters = 100;
            int numberOfOffersForRecruiter = 5;
            Random random = new Random();
            string recruiterName = GenerateRandomString(random);
            for (int i = 0; i < numberOfRecruiters; i++)
            {
                var recruiterUser = await CreateAndInsertRecruiterUser(recruiterName, i);
                var recruiterId = await GetRecruiterId(recruiterUser);
                GenerateOffersForRecruiter(numberOfOffersForRecruiter, recruiterName, i, recruiterId, random);
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GenerateCandidateTestSet()
        {
            int numberOfCandidates = 100;
            Random random = new Random();
            string candidateName = GenerateRandomString(random);
            for (int i = 0; i < numberOfCandidates; i++)
            {
                await CreateAndInsertCandidateUser(candidateName, i, random);
            }
            return View();
        }

        private async Task CreateAndInsertCandidateUser(string candidateName, int candidateNumber, Random random)
        {
            var skills = GenerateSkillList(random);
            var candidateUser = new CandidateUser() 
            {
                Name = candidateName + candidateNumber,
                Email = candidateName + candidateNumber + "@" + candidateName + ".pl",
                ModificationDate = DateTime.UtcNow,
                Salary = random.Next(1000, 20000),
                ExperienceInYears = random.Next(1,51),
                Skills = skills,
            };
            SetPassword(candidateUser, "candidate");
            await _databaseService.InsertCaniddateUserAsync(candidateUser);
        }

        private async Task<RecruiterUser> CreateAndInsertRecruiterUser(string name, int recruiterNumber)
        {
            var recruiterUser = new RecruiterUser()
            {
                Name = name + recruiterNumber,
                Email = name + recruiterNumber + "@" + name + ".pl",
            };
            SetPassword(recruiterUser, "recruiter");
            await _databaseService.InsertRecruiterUserAsync(recruiterUser);
            return recruiterUser;
        }

        private string GenerateRandomString(Random random)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            var randomString = new string
                (Enumerable.Repeat(chars, 10)
                            .Select(r => r[random.Next(r.Count())])
                            .ToArray());
            return randomString;
        }

        private void GenerateOffersForRecruiter(int numberOfOffersForRecruiter, string recruiterName, int recuiterNumber, string recruiterId, Random random)
        {
            for (int j = 0; j < numberOfOffersForRecruiter; j++)
            {
                var skillList = GenerateSkillList(random);
                var offer = new JobOffer()
                {
                    RecruiterId = recruiterId,
                    ModificationDate = DateTime.UtcNow,
                    Name = "Recruiter " + recruiterName + recuiterNumber + ": Offer " + j,
                    Salary = random.Next(1000, 20000),
                    Skills = skillList,
                };
                _databaseService.InsertJobOfferAsync(offer);
            }
        }

        private void SetPassword(User recruiterUser, string password)
        {
            var hashedPassword = _applicationService.GenerateHashPassword(password, recruiterUser);
            recruiterUser.Password = hashedPassword;
        }

        private async Task<string> GetRecruiterId(RecruiterUser recruiterUser)
        {
            var recruiter = await _databaseService.GetRecruterByEmailAsync(recruiterUser.Email);
            return recruiter.Id;
        }

        private List<Skill> GenerateSkillList(Random random)
        {
            var numberOfSkills = random.Next(1, 11);
            var shuffledSkills = _skillNames.OrderBy(r => random.Next());
            var skillSubsequence = shuffledSkills.Take(numberOfSkills);
            var skillList = new List<Skill>();
            foreach (var skillName in skillSubsequence)
            {
                var skill = new Skill(skillName, random.Next(1, 11));
                skillList.Add(skill);
            }
            return skillList;
        }
	}
}