using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IO;

namespace PoalimBackEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public void GetByIdAndUpdate(PoalimContract contractObj)
        {
            var poalimConfig = JsonConvert.DeserializeObject<PoalimConfig>(File.ReadAllText(@"./config.json"));
            var mongoClient = new MongoClient(poalimConfig.DbUrl);
            var db = mongoClient.GetDatabase(poalimConfig.DbName);
            var dbObj = db.GetCollection<PoalimDBO>(poalimConfig.CollectionName).Find(x => x._id == contractObj.Id).FirstOrDefault();
            dbObj._t = contractObj.Type.ToString();
            dbObj._v = contractObj.Value;
            db.GetCollection<PoalimDBO>(poalimConfig.CollectionName).ReplaceOne(Builders<PoalimDBO>.Filter.Where(x => x._id == contractObj.Id), dbObj);
        }

        public class PoalimContract
        {
            public string Id { get; set; }
            public PoalimType Type { get; set; }
            public string Value { get; set; }
        }

        public class PoalimDBO
        {
            public string _id { get; set; }
            public string _t { get; set; }
            public string _v { get; set; }
        }

        public enum PoalimType
        {
            JobInterview,
            Mingling
        }

        public class PoalimConfig
        {
            public string DbName { get; set; }
            public string DbUrl { get; set; }
            public string CollectionName { get; set; }
        }
    }
}
