using System;
using System.Collections.Generic;

namespace TextShot
{
    public class BuggedCircuitBoard : Item
    {
        public BuggedCircuitBoard() : base("Bugged Circuit Board", "Steal one item from the other participant")
        {}

        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            Participant thief;
            Participant victim;

            // Determine who is stealing from who based on the turn
            if (Program.PlayerTurn)
            {
                thief = Program.p;
                victim = Program.d;
            }
            else
            {
                thief = Program.d;
                victim = Program.p;
            }

            // Check if the victim actually has items to steal
            if (victim.Inventory.Items.Count == 0)
            {
                Console.WriteLine($"{victim.Name} has no items to steal!");
                return;
            }

            Console.WriteLine($"{victim.Name}'s items:");

            // If it is the Player's turn, use the acronym selection
            if (Program.PlayerTurn)
            {
                Dictionary<string, string> itemAcronyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Can of Beer",            "CB" },
                    { "Expired Medicine",       "EM" },
                    { "Hand Cuffs",             "HC" },
                    { "Bugged Circuit Board",   "BC" },
                    { "Infected bullet",        "IB" },
                    { "Magnifying Glass",       "MG" },
                    { "Syringe Shot",           "SS" }
                };

                Dictionary<string, string> availableOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // Loop through the dealer's inventory and print available acronyms
                foreach (Item item in victim.Inventory.Items)
                {
                    if (itemAcronyms.ContainsKey(item.Name))
                    {
                        string acronym = itemAcronyms[item.Name];
                        
                        if (!availableOptions.ContainsKey(acronym))
                        {
                            Console.WriteLine($"Type {acronym} to steal {item.Name}");
                            availableOptions.Add(acronym, item.Name);
                        }
                    }
                }

                Console.WriteLine("\nType the acronym of the item you want to steal:");
                string input = Console.ReadLine().Trim().ToLower();
                Console.WriteLine(); // Buffer

                if (availableOptions.ContainsKey(input))
                {
                    string fullName = availableOptions[input];
                    Item stolenItem = victim.Inventory.FetchItem(fullName);

                    if (stolenItem != null)
                    {
                        stolenItem.ExecuteEffect();
                        Console.WriteLine($"You successfully used the {stolenItem.Name}!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. The opportunity was wasted!");
                }
            }
            else
            {
                // Dealer logic (Dealer steals a random item)
                Random r = new Random();
                int randomIndex = r.Next(victim.Inventory.Items.Count);
                
                Item stolenItem = victim.Inventory.Items[randomIndex];
                victim.Inventory.FetchItem(stolenItem.Name); // Remove it from player
                thief.Inventory.AddItem(stolenItem);         // Add it to dealer

                Console.WriteLine($"The Dealer stole your {stolenItem.Name}!");
            }
        }
    }
}