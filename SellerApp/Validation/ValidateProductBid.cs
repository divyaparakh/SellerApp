using Common;
using DataAccess.DB;
using DataAccess.Models;
using System;
using System.Collections.Generic;

namespace SellerApp
{
    public struct ValidateProductBid
    {
        public string ConnectionString;
        public List<Error> AddValidate(ProductBid productBid)
        {
            List<Error> errors = new List<Error>();
            DBAccess<Product> dbAccess = new DBAccess<Product>(ConnectionString);
            var product = dbAccess.Get(productBid.ProductId);
            if (product == null)
                errors.Add(new Error { Field = "Product", Message = ErrorMessage.ProductBidExist });
            else
            {
                DateTime.TryParse(product.BidEndDate, out DateTime dateTime);
                if (dateTime < DateTime.Now)
                    errors.Add(new Error { Field = "BidEndDate", Message = ErrorMessage.ProductBidExpired });
                DBAccess<ProductBid> dbProductBid = new DBAccess<ProductBid>(ConnectionString);
                var ProductBids = dbProductBid.GetAll(productBid.Email, "Email");

                if (ProductBids.Count >= 1)
                    errors.Add(new Error { Field = "User", Message = ErrorMessage.UserBidExist });

                if (string.IsNullOrEmpty(productBid.FirstName))
                    errors.Add(new Error { Field = "FirstName", Message = ErrorMessage.EmptyFirstName });
                else if (productBid.FirstName.Length < 5)
                    errors.Add(new Error { Field = "FirstName", Message = ErrorMessage.MinFirstNameLength });
                else if (productBid.FirstName.Length > 30)
                    errors.Add(new Error { Field = "FirstName", Message = ErrorMessage.MaxFirstNameLength });

                if (string.IsNullOrEmpty(productBid.LastName))
                    errors.Add(new Error { Field = "LastName", Message = ErrorMessage.EmptyLastName });
                else if (productBid.FirstName.Length < 5)
                    errors.Add(new Error { Field = "LastName", Message = ErrorMessage.MinLastNameLength });
                else if (productBid.FirstName.Length > 25)
                    errors.Add(new Error { Field = "LastName", Message = ErrorMessage.MaxLastNameLength });

                if (string.IsNullOrEmpty(productBid.Email))
                    errors.Add(new Error { Field = "Email", Message = ErrorMessage.EmptyEmail });
                else if (!IsValidEmail(productBid.Email))
                    errors.Add(new Error { Field = "Email", Message = ErrorMessage.InvalidEmail });

                if (string.IsNullOrEmpty(productBid.Phone))
                    errors.Add(new Error { Field = "Phone", Message = ErrorMessage.EmptyPhone });
                else if (productBid.Phone.Length != 10)
                    errors.Add(new Error { Field = "Phone", Message = ErrorMessage.InvalidMobile });

                if (!double.TryParse(productBid.BidAmount, out double price))
                    errors.Add(new Error { Field = "BidAmount", Message = ErrorMessage.InvalidPrice });

            }

            return errors;
        }
        public List<Error> UpdateCheck(ProductBid productBid)
        {
            List<Error> errors = new List<Error>();
            DBAccess<Product> dbAccess = new DBAccess<Product>(ConnectionString);
            var product = dbAccess.Get(productBid.ProductId);

            DateTime.TryParse(product.BidEndDate, out DateTime dateTime);
            if (dateTime < DateTime.Now)
                errors.Add(new Error { Field = "BidEndDate", Message = ErrorMessage.ProductBidExpired });

            if (string.IsNullOrEmpty(productBid.Email))
                errors.Add(new Error { Field = "Email", Message = ErrorMessage.EmptyEmail });
            else if (!IsValidEmail(productBid.Email))
                errors.Add(new Error { Field = "Email", Message = ErrorMessage.InvalidEmail });

            if (!double.TryParse(productBid.BidAmount, out double price))
                errors.Add(new Error { Field = "BidAmount", Message = ErrorMessage.InvalidPrice });

            return errors;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
