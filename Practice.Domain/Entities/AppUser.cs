using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Practice.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public int CityID { get; set; }
        public int CountryID { get; set; }
        public string ShippingAddress { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

        //public void ConvertShippingAddressToDbFormat(List<string> ShippingAddresses)
        //{
        //    string result = "";
        //    foreach(string address in ShippingAddresses)
        //    {
        //        result += address + ",";
        //    }
        //}

        //public string[] GetShippingAddresses()
        //{
        //    string[] result = ShippingAddress.Split(',');
        //    return result;
        //}
    }
}
