using System;

namespace TextShot
{
    public class SyringeShot : Item
    {
        public SyringeShot() : base("Syringe Shot", "Heals 1 health point (doesn't exceed max health)")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // Determine whose turn it is
            if (Program.PlayerTurn)
            {
                // Heal Player 1
                Program.p.Health += 1;
                if (Program.Debug) { Console.WriteLine("Debug Healed Player 1"); }

                // Ensure it doesn't go over MaxHealth
                if (Program.p.Health > Program.MaxHealth)
                {
                    Program.p.Health = Program.MaxHealth;
                    if (Program.Debug) { Console.WriteLine("Debug Player Health capped out"); }
                }
                Console.WriteLine($"You heal yourself, you are now at {Program.p.Health} health");
            }
            else
            {
                // Heal Dealer 1
                Program.d.Health += 1;
                if (Program.Debug) { Console.WriteLine("Debug Healed Dealer 1"); }

                // Ensure it doesn't go over MaxHealth
                if (Program.d.Health > Program.MaxHealth)
                {
                    Program.d.Health = Program.MaxHealth;
                    if (Program.Debug) { Console.WriteLine("Debug Dealer Health capped out"); }
                }
                Console.WriteLine($"The Dealer heals themselves, they are now at {Program.p.Health} health");
            }
        }   
    }
}