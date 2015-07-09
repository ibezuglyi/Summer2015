using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Offer
{
    public class OfferViewModelList
    {
        public List<OfferViewModel> OffersList { get; set; }

        public OfferViewModelList()
        {
            OffersList = new List<OfferViewModel>();
        }

        public OfferViewModelList(List<OfferViewModel> offersList)
        {
            OffersList = offersList;
        }
    }
}