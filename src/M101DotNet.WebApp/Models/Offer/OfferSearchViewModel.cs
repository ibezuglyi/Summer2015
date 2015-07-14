using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Offer
{
    public class OfferSearchViewModel
    {
        public OfferSearchModel SearchParams { get; set; }
        public OfferListViewModel Offers { get; set; }

        public OfferSearchViewModel() 
        {
            SearchParams = new OfferSearchModel();
            Offers = new OfferListViewModel();
        }

        public OfferSearchViewModel(OfferSearchModel searchParams, OfferListViewModel offers)
        {
            SearchParams = searchParams;
            Offers = offers;
        }
    }
}