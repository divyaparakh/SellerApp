using Common;
using DataAnnotationsExtensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DataAccess.Models
{
    public class Seller
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonRequired]
        public string FirstName { get; set; }
        [BsonRequired]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        [BsonRequired]
        [Min(10, ErrorMessage = ErrorMessage.InvalidMobile)]
        [Max(10, ErrorMessage = ErrorMessage.InvalidMobile)]
        [Numeric]
        public string Phone { get; set; }
        [BsonRequired]
        [Email(ErrorMessage = ErrorMessage.InvalidEmail)]
        public string Email { get; set; }
    }
}
