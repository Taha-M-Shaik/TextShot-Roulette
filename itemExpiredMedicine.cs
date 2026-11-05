using System;

namespace TextShot
{
    public class ExpiredMedicine : Item
    {
        public ExpiredMedicine() : base("Expired Medicine", "50% chance of healing 1 health point, or damages 2 health points (doesn't exceed max health)")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // New random
            Random r = new Random();

            // Heal or damage
            int effect = r.NextDouble() >= 0.5 ? 1 : -2;

            // Effect participant
            if (Program.PlayerTurn)
            {
                Program.p.Health += effect;
                if (Program.Debug) { Console.WriteLine("Debug Healed Player 2"); }

                // Cap health
                if (Program.p.Health > Program.MaxHealth)
                {
                    Program.p.Health = Program.MaxHealth;
                    if (Program.Debug) { Console.WriteLine("Debug Player Health capped out"); }
                }
            }
            else
            {
                Program.d.Health += effect;
                if (Program.Debug) { Console.WriteLine("Debug Healed Dealer 2"); }

                // Cap health
                if (Program.p.Health > Program.MaxHealth)
                {
                    Program.p.Health = Program.MaxHealth;
                    if (Program.Debug) { Console.WriteLine("Debug Player Health capped out"); }
                }
            }
        }
    }
}