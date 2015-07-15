using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Offer
{
    public class ScoredOfferListViewModel
    {
        public List<ScoredOfferViewModel> ScoredOffersList { get; set; }

        public ScoredOfferListViewModel()
        {
            ScoredOffersList = new List<ScoredOfferViewModel>();
        }

        public ScoredOfferListViewModel(List<ScoredOfferViewModel> scoredOffersList)
        {
            ScoredOffersList = scoredOffersList;
        }
    }
}