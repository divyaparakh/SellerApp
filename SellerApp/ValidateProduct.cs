using Common;
using DataAccess.DB;
using DataAccess.Models;
using System;
using System.Collections.Generic;

namespace SellerApp
{
    public static class ValidateProduct
    {
        static List<string> Categories = new List<string> { "Painting", "Sculptor", "Ornament" };
        public static List<Error> AddValidate(Product product)
        {
            List<Error> errors = new List<Error>();

            if (!Categories.Contains(product.Category))
                errors.Add(new Error { Field = "Category", Message = ErrorMessage.InvalidCategory });
            if (!int.TryParse(product.StartingPrice, out int price))
                errors.Add(new Error { Field = "StartingPrice", Message = ErrorMessage.InvalidPrice });
            
            if (!DateTime.TryParse(product.BidEndDate, out DateTime dateTime))
                errors.Add(new Error { Field = "BidEndDate", Message = ErrorMessage.InvalidBidDate });
            else if (dateTime < DateTime.Now)
                errors.Add(new Error { Field = "BidEndDate", Message = ErrorMessage.SmallBidDate });
            
            if (string.IsNullOrEmpty(product.ProductName))
                errors.Add(new Error { Field = "ProductName", Message = ErrorMessage.EmptyProduct });
            else if (product.ProductName.Length > 5)
                errors.Add(new Error { Field = "ProductName", Message = ErrorMessage.MinProductLength });
            else if (product.ProductName.Length < 30)
                errors.Add(new Error { Field = "ProductName", Message = ErrorMessage.MaxProductLength });

            return errors;
        }
        public static List<Error> DeleteValidate(Product product)
        {
            List<Error> errors = new List<Error>();
            DateTime.TryParse(product.BidEndDate, out DateTime dateTime);
            
            if (DateTime.Now < dateTime)
                errors.Add(new Error { Field = "BidEndDate", Message = ErrorMessage.CannotDeleteBid });

            DBAccess dBAccess = new DBAccess("Product");
            

            return errors;
        }
    }
}
