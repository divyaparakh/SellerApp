using MongoDB.Driver;
using DataAccess.Models;
using System.Collections.Generic;

namespace DataAccess.DB
{
    public class DBAccess : IDBAccess
    {
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DBName = "Usecase";

        private MongoClient Client;
        private IMongoClient Client1;
        public IMongoDatabase db;
        private string CollectionName { get; set; }
        public DBAccess(IMongoClient Client1, string CollectionName)
        {
            this.Client1 = Client1;
            db = Client1.GetDatabase(DBName);
            this.CollectionName = CollectionName;
        }
        public DBAccess(string CollectionName)
        {
            Client = new MongoClient(ConnectionString);
            db = Client.GetDatabase(DBName);
            this.CollectionName = CollectionName;
        }
        public IMongoCollection<T> Context<T>()
        {
            return db.GetCollection<T>(CollectionName);            
        }
        public T Get<T>(string Id)
        {
            return Context<T>().Find(Id).FirstOrDefault();
        }
        public List<T> GetAll<T>()
        {
            return Context<T>().AsQueryable<T>().ToList();
        }
        public async void Insert<T>(T obj)
        {
            await Context<T>().InsertOneAsync(obj);            
        }
        public async void Update<T>(T obj,string Prop = "Id")
        {
            object val = obj.GetType().GetProperty(Prop).GetValue(obj, null);
            await Context<T>().ReplaceOneAsync(x=> x.GetType().GetProperty(Prop).GetValue(x) == val, obj, new ReplaceOptions { IsUpsert = true });
        }
        public async void Delete<T>(string Id, string Prop = "Id")
        {
            await Context<T>().DeleteOneAsync<T>(x => x.GetType().GetProperty(Prop).GetValue(x).ToString() == Id);
        }
    }
}
