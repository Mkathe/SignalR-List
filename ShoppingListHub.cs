using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace SignalR_With_Angular;

public class ShoppingListHub : Hub
{
    private static readonly ConcurrentDictionary<string, List<string>> _groupIdShoppingLists = new();

    public async Task AddItem(string shoppingListId, string itemName)
    {
        if (_groupIdShoppingLists.TryGetValue(shoppingListId, out var list))
        {
            list.Add(itemName);
            await Clients.Group(shoppingListId).SendAsync("ReceiveShoppingList", list);
        }
    }

    public async Task UpdateItem(string shoppingListId, string itemName, string newItemName)
    {
        if (_groupIdShoppingLists.TryGetValue(shoppingListId, out var list))
        {
            list.Remove(itemName);
            list.Add(newItemName);
            await Clients.Caller.SendAsync("ReceiveShoppingList", list);
        }
    }
    public async Task RemoveItem(string shoppingListId, string itemName)
    {
        if (_groupIdShoppingLists.TryGetValue(shoppingListId, out var list))
        {
            list.Remove(itemName);
            await Clients.Group(shoppingListId).SendAsync("ReceiveShoppingList", list);
        }
    }

    public async Task JoinShoppingList(string shoppingListId)
    {
        if (_groupIdShoppingLists.TryGetValue(shoppingListId, out var list))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, shoppingListId);
            await Clients.Caller.SendAsync("JoinShoppingList", shoppingListId, list);
        }
    }
    public async Task CreateShoppingList()
    {
        string shoppingListId = Guid.NewGuid().ToString();
        _groupIdShoppingLists.TryAdd(shoppingListId, []);
        await Groups.AddToGroupAsync(Context.ConnectionId, shoppingListId);
        await Clients.Caller.SendAsync("ShoppingListCreated", shoppingListId);
    }
}