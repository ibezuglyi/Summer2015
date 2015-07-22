using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Candidate
{
    public class CandidateSearchViewModel
    {
        public CandidateSearchModel SearchParams { get; set; }
        public ScoredCandidatesListViewModel Candidates { get; set; }

        public CandidateSearchViewModel() 
        {
            SearchParams = new CandidateSearchModel();
            Candidates = new ScoredCandidatesListViewModel();
        }

        public CandidateSearchViewModel(CandidateSearchModel searchParams, ScoredCandidatesListViewModel candidates)
        {
            SearchParams = searchParams;
            Candidates = candidates;
        }

        public bool HasCandidates()
        {
            return Candidates.CandidatesList.Any();
        }
    }
}