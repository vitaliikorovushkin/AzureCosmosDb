using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using WebToDo.Models;

namespace WebToDo.Services
{
    public class CosmosDbService : ICosmosDb
    {
        private Container conteiner;
        public CosmosDbService(CosmosClient client, string dbName, string conteinerName)
        {
            this.conteiner = client.GetContainer(dbName, conteinerName);
        }
        public async Task AddItemAsync(Item item)
        {
            await this.conteiner.CreateItemAsync(item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await this.conteiner.DeleteItemAsync<Item>(id, new PartitionKey(id));

        }

        public async Task<Item> GetItemAsync(string id)
        {
            try
            {
                //CosmosDb class, not ours
                ItemResponse<Item> response = await this.conteiner.ReadItemAsync<Item>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string queryString)
        {
            var query = this.conteiner.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
            List<Item> items = new List<Item>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                items.AddRange(response.ToList());
            }
            return items;
        }

        public async Task UpdateItemAsync(string id, Item item)
        {
            await this.conteiner.UpsertItemAsync<Item>(item, new PartitionKey(id));

        }
    }
}
