using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebToDo.Models;

namespace WebToDo.Services
{
    public interface ICosmosDb
    {
        Task<IEnumerable<Item>> GetItemsAsync(string queryString);
        Task<Item> GetItemAsync(string id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(string id, Item item);
        Task DeleteItemAsync(string id);
    }
}
