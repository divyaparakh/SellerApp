using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DB
{
    public interface IDBAccess
    {
        public IMongoCollection<T> Context<T>();
        public T Get<T>(string Id);
        public List<T> GetAll<T>();
        public void Insert<T>(T obj);
        public void Update<T>(T obj, string Prop = "Id");
        public void Delete<T>(string Id, string Prop = "Id");
    }
}
