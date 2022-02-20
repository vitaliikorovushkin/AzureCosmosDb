using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebToDo.Models;
using WebToDo.Services;

namespace WebToDo.Controllers
{
    public class ItemController : Controller
    {
        private readonly ICosmosDb cosmosDb;

        public ItemController(ICosmosDb cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View(await this.cosmosDb.GetItemsAsync("select * from c"));
        }
        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }
        [ActionName("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] Item item)
        {
            if (ModelState.IsValid)
            {
                item.Id = Guid.NewGuid().ToString();
                //item.IsComplited = false;
                await this.cosmosDb.AddItemAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [ActionName("Edit")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            Item item = await this.cosmosDb.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync([FromForm] Item item)
        {
            if (ModelState.IsValid)
            {
                await this.cosmosDb.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            Item item = await this.cosmosDb.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(Item item)
        {
            await this.cosmosDb.DeleteItemAsync(item.Id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            return View(await this.cosmosDb.GetItemAsync(id));
        }
    }
}
