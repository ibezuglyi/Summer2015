using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Candidate
{
    public class CandidatesListViewModel
    {
        public List<CandidateViewModel> CandidatesList { get; set; }

        public CandidatesListViewModel()
        {
            CandidatesList = new List<CandidateViewModel>();
        }

        public CandidatesListViewModel(List<CandidateViewModel> offersList)
        {
            CandidatesList = offersList;
        }
    }
}