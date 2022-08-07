using DataService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataService.Services
{
    public class StoreService
    {
        private readonly IMongoCollection<MachineStream> machineStreamCollection;

        public StoreService(
            IOptions<DatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            machineStreamCollection = mongoDatabase.GetCollection<MachineStream>(
                DatabaseSettings.Value.StreamCollectionName);
        }

        public async Task<List<MachineStream>> GetAsync() =>
            await machineStreamCollection.Find(_ => true).ToListAsync();

        public async Task<MachineStream?> GetAsync(string id) =>
            await machineStreamCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<MachineStream>> GetByStatusAsync(MachineStreamStatus Status) =>
            await machineStreamCollection.Find(x => x.Status == Status).ToListAsync();

        public async Task CreateAsync(MachineStream newMachineStream) =>
            await machineStreamCollection.InsertOneAsync(newMachineStream);

        public async Task UpdateAsync(string id, MachineStream updatedMachineStream) =>
            await machineStreamCollection.ReplaceOneAsync(x => x.Id == id, updatedMachineStream);

        public async Task RemoveAsync(string id) =>
            await machineStreamCollection.DeleteOneAsync(x => x.Id == id);
    }
}
