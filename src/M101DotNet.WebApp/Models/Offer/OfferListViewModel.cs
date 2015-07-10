using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Offer
{
    public class OfferListViewModel
    {
        public List<OfferViewModel> OffersList { get; set; }

        public OfferListViewModel()
        {
            OffersList = new List<OfferViewModel>();
        }

        public OfferListViewModel(List<OfferViewModel> offersList)
        {
            OffersList = offersList;
        }
    }
}