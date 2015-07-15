using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Offer
{
    public class OfferSearchViewModel
    {
        public OfferSearchModel SearchParams { get; set; }
        public ScoredOfferListViewModel Offers { get; set; }

        public OfferSearchViewModel() 
        {
            SearchParams = new OfferSearchModel();
            Offers = new ScoredOfferListViewModel();
        }

        public OfferSearchViewModel(OfferSearchModel searchParams, ScoredOfferListViewModel offers)
        {
            SearchParams = searchParams;
            Offers = offers;
        }
    }
}