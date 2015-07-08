using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Offer
{
    public class OfferViewModel
    {
        public OfferModel Offer { get; set; }
        public string Id { get; set; }
        public string IdRecruiter { get; set; }

        public OfferViewModel() 
        {
            Offer = new OfferModel();
        }
        public OfferViewModel(OfferModel offer, string id, string idRecruiter)
        {
            Offer = offer;
            Id = id;
            IdRecruiter = idRecruiter;
        }
    }
}