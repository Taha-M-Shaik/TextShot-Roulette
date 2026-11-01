using System;
using System.Collections.Generic;

namespace TextShot
{
    class Program
    {
        // ================================
        // State Machine, Pre Turn Decisions & Turn storage
        // ================================

        // Game States
        public enum State
        {
            Start,              // Start of game
            Name,               // Entering Name
            ExplainGame,        // Explains the game
            RoundStart,         // Start of Round, Set Health, Empty Inventories
            LoadGun,            // Load gun
            PreTurnDecisions,   // Any events that need to be processed before next turn, then deciding whose turn
            PlayerTurn,         // Player chooses between using item or shooting gun
            DealerTurn,         // Start of Dealer's Turn
            PickItem,           // Pick Item
            DealerDecide,       // Dealer Decision Algorithm
            ShootGun,           // Use the gun to shoot, discard bullet
            RoundEnd,           // End of Round
            GameOver            // Show Stats, end program
        };

        public static State CurrentState = State.Start;
        public static State PrevState = State.Start;

        // Pre Turn Decisions
        public static bool HandCuffed = false; // Keeps track if handcuffs is used
        public static bool PlayerTurn = true; // Keeps track of whose turn it is, true = Player's turn

        // Gun
        public static Gun Gun = null;
        public static bool GunFrom  = false; // Who its coming from, True = Player
        public static bool GunTo    = false; // Who its shooting,    True = Player

        // Participants
        public static Player p;
        public static Dealer d;

        public static string input;

        // Player & Dealer Health
        public static int MaxHealth;


        // =====================
        // Debug
        public static bool Debug = false;



        public static void Main(string[] args)
        {
            // ================================
            // Initialise Variables
            // ================================

            // =====================
            // Game Variables

            // New Random
            Random r = new Random();


            // =====================
            // Player & Dealer Variables

            // Player Command
            string Command = "";

            // Player Name
            string PlayerName = "";



            // ================================
            // Game Loop
            // ================================

            // Item test = new InfectedBullet();
            while (true)
            {
                // State Change Detector
                if (CurrentState != PrevState)
                {
                    PrevState = CurrentState;
                    if (Debug) { Console.WriteLine($"\nDebug State changed to {CurrentState}\n"); }
                }



                // ===============================
                // Test Code (Priority)
                // ===============================

                // Test Code here



                // ================================
                // State Machine
                // ================================

                // ================================
                // Start
                if (CurrentState == State.Start)
                {
                    // Start Game
                    CurrentState = State.Name;

                    // Add a few lines to seperate
                    Console.WriteLine("\n\n\n");
                }


                // ================================
                // Name
                else if (CurrentState == State.Name)
                {
                    // Enter Name
                    Console.WriteLine("Enter Name:");
                    PlayerName = Console.ReadLine();
                    if (Debug) { Console.WriteLine($"Debug Name Entered: {PlayerName}"); }

                    // Dealer Greeting Message
                    Console.WriteLine($"\nGreetings, {PlayerName}\nI hope you know how to play (Y/n)");

                    // Dev Breakpoint
                    if (PlayerName == "dev") { break; }

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


                // ================================
                // Explain Game
                else if (CurrentState == State.ExplainGame)
                {
                    Console.WriteLine("You and I take turns using a Gun.\nThe gun has a random assortment of live bullets and dud bullets.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("Each turn, you can use any items if we have any.\nItems can help your chances of survival, or decrease mine.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("You can look at your items from your inventory.\n(Press Enter to continue)");
                    Console.ReadLine();
                    Console.WriteLine("You can also look at the my items (the dealer's items).\nYou can also look at our healths.\n(Press Enter to continue)");
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


                // ================================
                // Round Start
                else if (CurrentState == State.RoundStart)
                {
                    // New Random
                    r = new Random();

                    // Set MaxHealth
                    MaxHealth = r.Next(3,5);

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
                    p.Inventory.PopulateInventory(InveAmount);
                    d.Inventory.PopulateInventory(InveAmount);

                    // Transition to LoadGun
                    Console.ReadLine();
                    CurrentState = State.LoadGun;
                }


                // ================================
                // Load Gun
                else if (CurrentState == State.LoadGun)
                {
                    // Load the gun
                    int numOfBullets = r.Next(2, 8);
                    Gun = new Gun(numOfBullets);
                    Gun.Load(numOfBullets);

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


                // ================================
                // Pre Turn Decisions
                else if (CurrentState == State.PreTurnDecisions)
                {
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

                    // Determine whose turn it is
                    if (PlayerTurn)
                    {
                        CurrentState = State.PlayerTurn;
                    }
                    else
                    {
                        CurrentState = State.DealerTurn;
                    }
                }


                // ================================
                // Player Turn
                else if (CurrentState == State.PlayerTurn)
                {
                    // Establish your turn
                    Console.WriteLine("\nIt's your turn");
                    PlayerTurn = true;
                    
                    // Mini State machine in the player's turn
                    while (true)
                    {
                        if (Debug) { Console.WriteLine("Debug Currently inside PlayerTurn State Machine\n"); }

                        // Show avilable Commands
                        Console.WriteLine("Options:\n");
                        Console.WriteLine("g: use gun");
                        Console.WriteLine("ui: use your items");
                        Console.WriteLine("di: view dealer's items");
                        Console.WriteLine("h: look at health");

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
                            Console.WriteLine($"You have {p.Health} health and the dealer has {d.Health} health\n");
                        } 
                    }
                }


                // ================================
                // Dealer Turn
                else if (CurrentState == State.DealerTurn)
                {
                    // Establish your turn
                    Console.WriteLine("\nIt's your turn");

                    // Use SyringeShot (test)
                    try
                    {
                        Item s = d.Inventory.FetchItem("Syringe Shot");
                        s.ExecuteEffect();
                        if (Debug) { Console.WriteLine("Debug DEALER USED SYRINGE!!!"); }
                    }
                    finally { /*nothing lol*/ }

                    break; // work in progress
                }


                // ===============================
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
                        CurrentState = State.PlayerTurn;
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
                            CurrentState = State.PlayerTurn;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid input or you don't have that item. Please try again.");
                    }
                }


                // ================================
                // Dealer Decisions
                else if (CurrentState == State.DealerDecide)
                {
                    break; // work in progress
                }


                // ================================
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

                    // Remove Health
                    if (isLive)
                    {
                        if (GunFrom) { p.Health--; } else { d.Health--; }
                    }

                    // Eject Bullet
                    Gun.EjectCurBullet();

                    // Print new Bullet and Bullets left
                    if (Debug) { Console.WriteLine($"Debug New Bullet: {isLive}"); }

                    // Return to Pre-Turn Decisions
                    PlayerTurn = !PlayerTurn; // Flip turn
                    CurrentState = State.PreTurnDecisions;
                }


                // ================================
                // End Round
                else if (CurrentState == State.RoundEnd)
                {
                    break; // work in progress
                }


                // ================================
                // Game over
                else if (CurrentState == State.GameOver)
                {
                    break; // work in progress
                }
            }
        }
    }
}