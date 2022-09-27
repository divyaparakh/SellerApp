using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;

namespace DataAccess.DB
{
    public interface IDBAccess<T>
    {
        T Get(object Id, string Prop = "Id");
        T Get(ObjectId Id);
        List<T> GetAll();
        List<T> GetAll(object Id, string Prop = "Id");
        IMongoQueryable GetAllQueryable();
        IMongoQueryable GetAllQueryable(object Id, string Prop = "Id");
        void Insert(T obj);
        void Update(T obj, string Prop = "Id");
        void Delete(object Id, string Prop = "Id");
    }
}
