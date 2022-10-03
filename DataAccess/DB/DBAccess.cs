using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Security.Authentication;

namespace DataAccess.DB
{
    public class DBAccess<T> : IDBAccess<T>
    {
        //private const string ConnectionString = "mongodb://localhost:27017";
        private const string DBName = "Usecase";
        private MongoClient Client;
        private IMongoDatabase db;
        public DBAccess(string ConnectionString)
        {
            Client = new MongoClient(ConnectionString);
            //Client.Settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            db = Client.GetDatabase(DBName);
        }
        private string GetCollectionName()
        {
            if (typeof(T).Name == "Seller")
                return "Seller";
            else if (typeof(T).Name == "Product")
                return "Product";
            else if (typeof(T).Name == "ProductBid")
                return "ProductBid";
            else
                return "";
        }

        private IMongoCollection<T> Context()
        {
            return db.GetCollection<T>(GetCollectionName());
        }
        public T Get(object Id, string Prop = "Id")
        {
            var filter = Builders<T>.Filter.Eq(Prop, Id);
            return Context().Find(filter).FirstOrDefault();
        }
        public T Get(ObjectId Id)
        {
            var filter = Builders<T>.Filter.Eq("Id", Id);
            return Context().Find(filter).FirstOrDefault();
        }
        public List<T> GetAll()
        {
            return Context().AsQueryable().ToList();
        }
        public IMongoQueryable GetAllQueryable()
        {
            return Context().AsQueryable();
        }
        public IMongoQueryable GetAllQueryable(object Id, string Prop = "Id")
        {
            return Context().AsQueryable().Where(x=>x.GetType().GetProperty(Prop).GetValue(x) == Id);
        }
        public List<T> GetAll(object Id, string Prop = "Id")
        {
            var filter = Builders<T>.Filter.Eq(Prop, Id);
            return Context().Find(filter).ToList();
        }
        public async void Insert(T obj)
        {
            await Context().InsertOneAsync(obj);
        }
        public async void Update(T obj, string Prop = "Id")
        {
            object val = obj.GetType().GetProperty(Prop).GetValue(obj, null);
            var filter = Builders<T>.Filter.Eq(Prop, val);
            await Context().ReplaceOneAsync(filter, obj, new ReplaceOptions { IsUpsert = true });
        }
        public async void Delete(object Id, string Prop = "Id")
        {
            var filter = Builders<T>.Filter.Eq(Prop, Id);
            await Context().DeleteOneAsync(filter);
        }
    }
}
