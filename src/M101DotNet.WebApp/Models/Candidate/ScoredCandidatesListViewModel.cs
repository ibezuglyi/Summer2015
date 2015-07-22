using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Candidate
{
    public class ScoredCandidatesListViewModel
    {
        public List<ScoredCandidateViewModel> CandidatesList { get; set; }

        public ScoredCandidatesListViewModel()
        {
            CandidatesList = new List<ScoredCandidateViewModel>();
        }

        public ScoredCandidatesListViewModel(List<ScoredCandidateViewModel> candidatesList)
        {
            CandidatesList = candidatesList;
        }

        public bool HasCandidates()
        {
            return CandidatesList.Count > 0;
        }
    }
}