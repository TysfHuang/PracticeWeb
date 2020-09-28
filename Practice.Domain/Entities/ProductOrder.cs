using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Practice.Domain.Entities
{
    public class ProductOrder
    {
        public int ID { get; set; }
        public string ReceiverName { get; set; }
        public string AppUserID { get; set; }   //因Identity的ID為字串型態，因此這邊宣告為string
        public DateTime Date { get; set; }
        public string ProductList { get; set; }
        public string ShippingAddress { get; set; }

        public virtual AppUser AppUser { get; set; }

        public void ConvertProductListToDbFormat(int ProductID, string ProductName, int Quantity, int Price)
        {
            if (ProductList == null)
                ProductList = "";
            ProductList += ProductID.ToString() + ":" + ProductName + "," + Quantity.ToString() + "," + Price.ToString() + "&";
        }

        public List<List<string>> GetDetailFromProductList()
        {
            string[] dataList = ProductList.Split('&');
            List<List<string>> allList = new List<List<string>>();
            foreach (string data in dataList)
            {
                if (data == "")
                    continue;
                List<string> tempList = new List<string>();
                int index1 = data.IndexOf(':');
                int index2 = data.IndexOf(',');
                int index3 = data.IndexOf(',', index2 + 1);
                tempList.Add(data.Substring(0, index1));                            //Product ID
                tempList.Add(data.Substring(index1 + 1, index2 - index1 - 1));      //Product Name
                tempList.Add(data.Substring(index2 + 1, index3 - index2 - 1));      //Product Quantity
                tempList.Add(data.Substring(index3 + 1, data.Length - index3 - 1)); //Product Price
                allList.Add(tempList);
            }
            return allList;
        }
    }
}
