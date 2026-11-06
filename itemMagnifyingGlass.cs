using System;

namespace TextShot
{
    public class MagnifyingGlass : Item
    {
        public MagnifyingGlass() : base("Magnifying Glass", "See what bullet is in the cartridge")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // Inside MagnifyingGlass ExecuteEffect()
            bool isLive = Program.Gun.ReadCurBullent();
            
            if (Program.PlayerTurn)
            {
                Console.WriteLine($"You examine the chamber... it's a {(isLive ? "LIVE" : "DUD")} bullet.");
            }
            else
            {
                // If dealer uses it, they log the knowledge
                Program.d.LearnCurrentBullet(isLive);
                Console.WriteLine("The Dealer examines the chamber...");
            }
        }
    }
}