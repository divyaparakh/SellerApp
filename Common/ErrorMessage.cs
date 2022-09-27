
namespace Common
{
    public static class ErrorMessage
    {
        public const string InvalidSeller = "Invalid Seller selected";
        public const string InvalidCategory = "Invalid Category";
        public const string InvalidPrice = "Invalid Price Entered";
        public const string InvalidBidDate = "Invalid Bid End Date Entered";
        public const string SmallBidDate = "Bid End Date must be of future date";
        public const string CannotDeleteBid = "Product cannot be deleted after Bid End Date";
        public const string CannotDeleteProduct = "Product containing bid cannot be deleted";
        public const string MinProductLength = "Product Name must be greater than 5 characters";
        public const string MaxProductLength = "Product Name must be less than 30 characters";
        public const string MinFirstNameLength = "First Name must be greater than 5 characters";
        public const string MaxFirstNameLength = "First Name must be less than 30 characters";
        public const string MinLastNameLength = "Last Name must be greater than 5 characters";
        public const string MaxLastNameLength = "Last Name must be less than 25 characters";
        public const string EmptyProduct = "Product Name must be defined";
        public const string EmptyFirstName = "First Name must be defined";
        public const string EmptyLastName = "Last Name must be defined";
        public const string EmptyEmail = "Email must be defined";
        public const string InvalidEmail = "Invalid Email entered";
        public const string EmptyPhone = "Mobile no. must be defined";
        public const string InvalidMobile = "Invalid Mobile entered";
        public const string ProductBidExpired = "Product bid is expired";
        public const string ProductBidExist = "Product bid does not exist";
        public const string UserBidExist = "User with same product bid already exist";
    }
}
