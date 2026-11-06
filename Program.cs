using System;
using System.Collections.Generic;

namespace TextShot
{
    class Program
    {
        // ================================
        // Game Variables -- Important Game handling things
        // ================================

        // =====================
        // Game States
        public enum State
        {
            Start,              // Start of game
            Name,               // Entering Name
            ExplainGame,        // Explains the game
            RoundStart,         // Start of Round, Set Health, Empty Inventories
            LoadGun,            // Load gun
            PreTurnDecisions,   // Any events that need to be processed before next turn, then deciding whose turn
            PlayerSTurn,         // Player chooses between using item or shooting gun
            DealerSTurn,         // Start of Dealer's Turn
            PickItem,           // Pick Item
            DealerDecide,       // Dealer Decision Algorithm
            ShootGun,           // Use the gun to shoot, discard bullet
            RoundEnd,           // End of Round
            GameOver            // Show Stats, end program
        };

        public static State CurrentState = State.Start;
        public static State PrevState = State.Start;


        // =====================
        // Pre Turn Decisions && Item info
        public static bool HandCuffed = false; // Keeps track if handcuffs is used
        public static bool InfectedIsBullet = false; // Keeps track if bullet is infected
        public static bool PlayerTurn = true; // Keeps track of whose turn it is, true = Player's turn
        // New Random
        public static Random r;


        // =====================
        // Gun
        public static Gun Gun = null;
        public static bool GunFrom  = false; // Who its coming from, True = Player
        public static bool GunTo = false;    // Who its shooting, True = Player


        // =====================
        // Participants
        public static Player p;
        public static Dealer d;
        public static int MaxHealth;
        public static string DealerTargetItem = ""; // Tells steal items what to target

        // Player inputs
        public static string PlayerName = "";
        public static string input;


        // =====================
        // Debug
        public static bool Debug = false;



        public static void Main(string[] args)
        {
            // ================================
            // Game Loop
            // ================================

            // Item test = new InfectedBullet();
            while (true)
            {
                // State Change Detector
                if (CurrentState != PrevState)
                {
                    if (Debug) { Console.WriteLine($"\nDebug State changed from {PrevState} to {CurrentState}\n"); }
                    PrevState = CurrentState;
                }



                // ===============================
                // Test Code (Priority)
                // ===============================

                // Test Code here



                // ================================
                // State Machine
                // ================================

                // =====================
                // Start
                if (CurrentState == State.Start)
                {
                    // Start Game
                    CurrentState = State.Name;

                    // Add a few lines to seperate
                    Console.WriteLine("\n\n\n");
                }


                // =====================
                // Name
                else if (CurrentState == State.Name)
                {
                    // Enter Name
                    Console.WriteLine("Enter Name:");
                    PlayerName = Console.ReadLine();

                    // Dev Breakpoint
                    if (PlayerName == "dev" || PlayerName == "d") { Debug = true; Console.WriteLine($"Debug Enabled\nDebug Name Entered: {PlayerName}"); }

                    // Dealer Greeting Message
                    Console.WriteLine($"\nGreetings, {PlayerName}\nI hope you know how to play (y/n)");

                    // Transition to RoundStart
                    string knowGame = Console.ReadLine().Trim().ToLower();
                    while (knowGame != "y" && knowGame != "n" && knowGame != "yes" && knowGame != "no")
                    {
                        if (Debug) { Console.WriteLine($"Debug Invalid Input: {knowGame}"); }
                        Console.WriteLine("Please answer yes or no\n no means I will explain the game");
                        knowGame = Console.ReadLine().Trim().ToLower();
                    }
                    if (knowGame == "n" || knowGame == "no")
                    {
                        CurrentState = State.ExplainGame;
                    }
                    else
                    {
                        CurrentState = State.RoundStart;
                    }
                }


                // =====================
                // Explain Game
                else if (CurrentState == State.ExplainGame)
                {
                    Console.WriteLine("You and I take turns using a Gun.\nThe gun has a random assortment of live bullets and dud bullets.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("Each turn, you can use any items if we have any.\nItems can help your chances of survival, or decrease mine.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("You can look at your items from your inventory.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("You can also look at the my items (the Dealer's items).\nYou can also look at our healths.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("You can either shoot me, or yourself.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("If you shoot yourself, and it's not a live bullet (a dud bullet), you don't lose any health and it's your turn again.\nSame goes for me.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("The first one to have their health reach 0 loses\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("Are you ready? (Y/n)");
                    string ready = Console.ReadLine().Trim().ToLower();
                    while (ready != "y" && ready != "n" && ready != "yes" && ready != "no")
                    {
                        if (Debug) { Console.WriteLine($"Debug Invalid Input: {ready}"); }
                        Console.WriteLine("Please answer yes or no.\n No means I will explain the game");
                        ready = Console.ReadLine().Trim().ToLower();
                    }
                    if (ready == "n" || ready == "no")
                    {
                        CurrentState = State.ExplainGame;
                    }
                    CurrentState = State.RoundStart;
                }


                // =====================
                // Round Start
                else if (CurrentState == State.RoundStart)
                {
                    // New Random
                    r = new Random();

                    // Set MaxHealth
                    MaxHealth = r.Next(2, 5);

                    // Create Participants
                    p = new Player(PlayerName, MaxHealth);
                    d = new Dealer("Dealer", MaxHealth);
                    Console.WriteLine($"\nYou and I have {MaxHealth} health");
                    if (Debug) { Console.WriteLine($"Debug Player {MaxHealth} health, Dealer {MaxHealth} health"); }

                    // Empty Inventories
                    p.ClearInventory();
                    d.ClearInventory();

                    // Populate Inventories
                    int InveAmount = r.Next(2, 5);
                    p.Inventory.PopulateInventory(13);
                    d.Inventory.PopulateInventory(InveAmount);

                    // Transition to LoadGun
                    Console.ReadLine();
                    CurrentState = State.LoadGun;
                }


                // =====================
                // Load Gun
                else if (CurrentState == State.LoadGun)
                {
                    // Load the gun
                    int numOfBullets = r.Next(2, 8);
                    Gun = new Gun(numOfBullets);
                    Gun.Load(numOfBullets);

                    // Initialize Dealer's memory
                    d.InitializeKnowledge(Gun.LiveBullets, Gun.DudBullets);

                    // Shuffle the array to randomize bullet order (Fisher-Yates shuffle)
                    Gun.Shuffle();

                    // Print amount of live bullets
                    Gun.PrintLoadout();

                    // Flavour text
                    Console.ReadLine();
                    Console.WriteLine("They enter the gun in an unknown order");

                    // Transition to PreTurnDecisions
                    Console.ReadLine();
                    CurrentState = State.PreTurnDecisions;
                }


                // =====================
                // Pre Turn Decisions
                else if (CurrentState == State.PreTurnDecisions)
                {
                    // New Random
                    r = new Random();
                    if (Debug) { Console.WriteLine("Debug New Random"); }

                    // Determine whose turn it is
                    if (Debug) { Console.WriteLine($"Debug Handcuffs value: {HandCuffed}"); }
                    if (HandCuffed)
                    {
                        // Skip the next player's turn
                        PlayerTurn = !PlayerTurn;
                        if (Debug) { Console.WriteLine($"Debug Handcuffed used up, skipping turn"); }
                        Console.WriteLine("Handcuffs used up, skipping turn");
                        HandCuffed = false;
                    }

                    // =====================
                    // Determine whose turn it is
                    if (PlayerTurn)
                    {
                        CurrentState = State.PlayerSTurn;
                    }
                    else
                    {
                        CurrentState = State.DealerSTurn;
                    }

                    // =====================
                    // Check if gun is empty (overrides turn decisions)
                    if (Gun.Bullets.Count == 0)
                    {
                        // Transition to LoadGun
                        Console.WriteLine("The gun is empty");
                        CurrentState = State.LoadGun;
                        if (Debug) { Console.WriteLine("Debug Gun empty, transition to LoadGun"); }
                    }

                    // =====================
                    // End round if either participant is dead
                    if (p.Health == 0 || d.Health == 0)
                    {
                        // Transition to RoundEnd
                        CurrentState = State.RoundEnd;
                        if (Debug) { Console.WriteLine($"Debug Player {p.Health} health, Dealer {d.Health} health"); }
                    }
                }


                // =====================
                // Player Turn
                else if (CurrentState == State.PlayerSTurn)
                {
                    // Establish your turn
                    Console.WriteLine("\nIt's your turn");
                    PlayerTurn = true;
                    
                    // Mini State machine in the player's turn
                    while (true)
                    {
                        if (Debug) { Console.WriteLine("Debug Currently inside PlayerSTurn State Machine\n"); }

                        // Show avilable Commands
                        Console.WriteLine("Options:\n");
                        Console.WriteLine("g:   use gun");
                        Console.WriteLine("ui:  use your items");
                        Console.WriteLine("di:  view the Dealer's items");
                        Console.WriteLine("h:   look at health");

                        // Get input
                        input = Console.ReadLine().Trim().ToLower();
                        if (Debug) { Console.WriteLine($"Debug Player Input: {input}\n"); }
                        Console.WriteLine(); // Buffer

                        // Process input
                        if (input == "g")
                        {
                            if (Debug) { Console.WriteLine("Debug Inside Gun loop"); }
                            GunFrom = true;
                            Console.WriteLine("Who do you shoot?\nP = Player (You)\nD = Dealer\nC = Cancel\n");
                            input = Console.ReadLine().Trim().ToLower();
                            Console.WriteLine(); // Buffer
                            if (input == "p")
                            {
                                Console.WriteLine("\nYou shoot yourself");
                                GunTo = true;
                                CurrentState = State.ShootGun;
                                break;
                            }
                            else if (input == "d")
                            {
                                Console.WriteLine("\nYou shoot the Dealer");
                                GunTo = false;
                                CurrentState = State.ShootGun;
                                break;
                            }
                            else if (input == "c")
                            {
                                if (Debug) { Console.WriteLine("Debug Cancel Gun loop"); }
                                break;
                            }
                            else
                            {
                                // Invalid input
                                Console.WriteLine("Invalid input");
                                if (Debug) { Console.WriteLine("Debug Invalid input"); }
                                continue;
                            }
                        }
                        else if (input == "ui")
                        {
                            CurrentState = State.PickItem;
                            break;
                        }
                        else if (input == "di")
                        {
                            Console.WriteLine("Dealer's items:");
                            d.PrintInventory();
                        }
                        else if (input == "h")
                        {
                            Console.WriteLine($"You have {p.Health} health and the Dealer has {d.Health} health\n");
                        } 
                    }
                }


                // =====================
                // Player pick items
                else if (CurrentState == State.PickItem)
                {
                    // Map full names to your desired acronyms
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

                    p.PrintInventory();
                    
                    // Temporary dictionary to track which acronyms the player can actually use right now
                    Dictionary<string, string> availableOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    // Loop through the player's actual inventory and print only what they have
                    foreach (Item item in p.Inventory.Items)
                    {
                        if (itemAcronyms.ContainsKey(item.Name))
                        {
                            string acronym = itemAcronyms[item.Name];
                            if (Debug) { Console.WriteLine($"Debug Item: {item.Name}, Acronym: {acronym}"); }
                            
                            // Prevent printing the same option twice if they have duplicates
                            if (!availableOptions.ContainsKey(acronym))
                            {
                                Console.WriteLine($"Type {acronym} to use {item.Name}");
                                availableOptions.Add(acronym, item.Name);
                            }
                        }
                    }

                    if (availableOptions.Count == 0)
                    {
                        Console.WriteLine("- No usable items in inventory.");
                    }

                    Console.WriteLine("\nType the acronym of the item to use, or 'c' (cancel) to go back.");
                    
                    // Get input
                    string input = Console.ReadLine().Trim().ToLower();
                    Console.WriteLine(); // Buffer
                    
                    if (input == "c") // Cancel (go back to player turn)
                    {
                        CurrentState = State.PlayerSTurn;
                    }
                    else if (availableOptions.ContainsKey(input))
                    {
                        // Translate the acronym (e.g. "MG") back to the full name ("Magnifying Glass")
                        string fullName = availableOptions[input];
                        
                        // Fetch and remove the item from the inventory using its full name
                        Item selectedItem = p.Inventory.FetchItem(fullName);

                        if (selectedItem != null)
                        {
                            Console.WriteLine($"\nYou used the {selectedItem.Name}!\n");
                            selectedItem.ExecuteEffect();
                            CurrentState = State.PlayerSTurn;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid input or you don't have that item. Please try again.");
                    }
                }


                // =====================
                // Dealer Turn
                else if (CurrentState == State.DealerSTurn)
                {
                    // Establish your turn
                    Console.WriteLine("\nIt's the Dealer's turn");

                    // Print DealerKnowldege
                    if (Debug) { d.PrintKnowledge(); }

                    // Continue to decisions
                    CurrentState = State.DealerDecide;
                }


                // =====================
                // Dealer Decisions
                else if (CurrentState == State.DealerDecide)
                {
                    // Heal
                    while (true)
                    {
                        // Heal if health is not max
                        if (d.Health < MaxHealth)
                        {
                            if (Debug) { Console.WriteLine("Debug Dealer Decide: needs healing"); }

                            // Check if items are available (Syringe Shot or Expired Medicine)
                            if (d.HasItem("Syringe Shot"))
                            {
                                if (Debug) { Console.WriteLine("Debug Dealer Decide: Using Syringe Shot"); }
                                d.UseItem("Syringe Shot");
                            }
                            else if (d.HasItem("Expired Medicine"))
                            {
                                if (Debug) { Console.WriteLine("Debug Dealer Decide: Using Expired Medicine"); }
                                d.UseItem("Expired Medicine");
                            }

                            // Steal healing item if possible
                            else if (d.HasItem("Bugged Circuit Board") && p.HasItem("Syringe Shot"))
                            {
                                if (Debug) { Console.WriteLine("Debug Dealer Decide: Using Bugged Circuit Board to steal Syringe Shot"); }
                                
                                // Set the target for the item to read
                                Program.DealerTargetItem = "Syringe Shot"; 
                                
                                d.UseItem("Bugged Circuit Board");
                            }
                            else if (d.HasItem("Bugged Circuit Board") && p.HasItem("Expired Medicine"))
                            {
                                if (Debug) { Console.WriteLine("Debug Dealer Decide: Using Bugged Circuit Board to steal Expired Medicine"); }
                                
                                // Set the target for the item to read
                                Program.DealerTargetItem = "Expired Medicine"; 
                                
                                d.UseItem("Bugged Circuit Board");
                            }
                            else
                            {
                                if (Debug) { Console.WriteLine("Debug Dealer Decide: can't heal"); }
                                break;
                            }
                        }
                        else
                        {
                            if (Debug) { Console.WriteLine("Debug Dealer Decide: doesn't need healing"); }
                            break;
                        }
                    }
                }


                // =====================
                // Shoot Gun
                else if (CurrentState == State.ShootGun)
                {
                    // Who shoots who
                    if (Debug)
                    {
                        Console.Write("Debug ");
                        if (GunFrom) { Console.Write("Player"); } else { Console.Write("Dealer"); }
                        Console.Write(" shoots ");
                        if (GunTo) { Console.WriteLine("Player"); } else { Console.WriteLine("Dealer"); }
                    }

                    // Read Bullet
                    bool isLive = Gun.ReadCurBullent();

                    // Print Bullet
                    if (Debug) { Console.WriteLine($"Debug Current Bullet: {isLive}"); }
                    if (isLive) { Console.WriteLine("\nBANG\n\nit was a live bullet"); } else { Console.WriteLine("D\nBANG\n\nit was a dud bullet"); }

                    // Remove Health
                    if (isLive)
                    {
                        if (GunTo) { p.Health--; } else { d.Health--; }

                        // Double Damage from infectedBullet
                        if (InfectedIsBullet)
                        {
                            if (GunTo) { p.Health--; } else { d.Health--; }
                            if (Debug) { Console.WriteLine("Debug Double Damage from Infected Bullet used up"); }
                        }
                    }
                    InfectedIsBullet = false;

                    // Print Health
                    if (Debug) { Console.WriteLine($"Debug Player Health: {p.Health}"); }
                    if (Debug) { Console.WriteLine($"Debug Dealer Health: {d.Health}"); }
                    Console.WriteLine($"Your health is now {Program.p.Health} \nThe Dealer's health is now {Program.d.Health}");

                    // Eject Bullet
                    Gun.EjectCurBullet();

                    // Dealer forgets the bullet that just left the gun
                    d.RemoveTopBullet(isLive);

                    // Print new Bullet and Bullets left
                    if (Debug) { Console.WriteLine($"Debug New Bullet: {isLive}"); }

                    // Return to Pre-Turn Decisions
                    PlayerTurn = !PlayerTurn; // Flip turn
                    CurrentState = State.PreTurnDecisions;
                }


                // =====================
                // End Round
                else if (CurrentState == State.RoundEnd)
                {
                    // Announce Round End
                    Console.WriteLine("Round ended");
                    break; // work in progress
                }


                // =====================
                // Game over
                else if (CurrentState == State.GameOver)
                {
                    break; // work in progress
                }
            }
        }
    }
}