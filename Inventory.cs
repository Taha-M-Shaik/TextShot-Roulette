using System;
using System.Collections.Generic;

namespace TextShot
{
    public class Inventory
    {
        private List<Item> _items;
        private Random r;

        public Inventory()
        {
            _items = new List<Item>();
            r = new Random();
        }

        // Program.cs can read the list
        public List<Item> Items 
        { 
            get { return _items; } 
        }

        // Adds an item to the inventory
        public void AddItem(Item item)
        {
            if (item != null)
            {
                _items.Add(item);
                if (Program.Debug) Console.WriteLine($"Debug {item.Name} added to inventory");
            }
        }

        // Fetches (removes and returns) an item by name
        public Item FetchItem(string itemName)
        {
            Item foundItem = _items.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            
            if (foundItem != null)
            {
                _items.Remove(foundItem);
                if (Program.Debug) Console.WriteLine($"Debug {foundItem.Name} removed from inventory");
            }
            else
            {
                if (Program.Debug) Console.WriteLine($"Debug Item '{itemName}' not found in inventory");
            }

            return foundItem;
        }

        // Reads and prints all items currently in the inventory
        public void ReadItems()
        {
            Console.WriteLine("---- Inventory ----");
            if (_items.Count == 0)
            {
                Console.WriteLine("Your inventory is currently empty");
                return;
            }

            foreach (Item item in _items)
            {
                Console.WriteLine($"- {item.Name}:   {item.Description}");
            }
            Console.WriteLine("-------------------\n");
        }

        public void ClearOutInventory()
        {
            _items.Clear();
        }

        // Populates the inventory with a random assortment of items
        public void PopulateInventory(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int randomChoice = r.Next(7);
                Item newItem = null;

                switch (randomChoice)
                {
                    case 0:
                        newItem = new BuggedCircuitBoard();
                        break;
                    case 1:
                        newItem = new CanOfBeer();
                        break;
                    case 2:
                        newItem = new ExpiredMedicine();
                        break;
                    case 3:
                        newItem = new HandCuffs();
                        break;
                    case 4:
                        newItem = new InfectedBullet();
                        break;
                    case 5:
                        newItem = new MagnifyingGlass();
                        break;
                    case 6:
                        newItem = new SyringeShot();
                        break;
                }

                AddItem(newItem);
            }

            if (Program.Debug) Console.WriteLine($"Debug Populated inventory with {amount} random items");
        }
    }
}