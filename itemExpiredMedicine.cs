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
            bool effect = r.NextDouble() >= 0.5;

            // Effect participant
            if (Program.PlayerTurn)
            {
                if (effect)
                {
                    Program.p.Health += 2;
                    Console.WriteLine("The medicine healed you!");
                    if (Program.Debug) { Console.WriteLine("Debug Healed Player 2 from Expired Medicine"); }
                }
                else
                {
                    Program.p.Health--;
                    Console.WriteLine("The medicine damaged you!");
                    if (Program.Debug) { Console.WriteLine("Debug Damaged Player 1 from Expired Medicine"); }
                }

                // Cap health
                if (Program.p.Health > Program.MaxHealth)
                {
                    Program.p.Health = Program.MaxHealth;
                    Console.WriteLine("But your health was already at max");
                    if (Program.Debug) { Console.WriteLine("Debug Player Health capped out"); }
                }
            }
            else
            {
                if (effect)
                {
                    Program.d.Health += 2;
                    Console.WriteLine("The medicine healed the Dealer!");
                    if (Program.Debug) { Console.WriteLine("Debug Healed Dealer 2 from Expired Medicine"); }
                }
                else
                {
                    Program.d.Health--;
                    Console.WriteLine("The medicine damaged the Dealer!");
                    if (Program.Debug) { Console.WriteLine("Debug Damaged Dealer 1 from Expired Medicine"); }
                }

                // Cap health
                if (Program.p.Health > Program.MaxHealth)
                {
                    Program.p.Health = Program.MaxHealth;
                    Console.WriteLine("But the Dealer's health was already at max");
                    if (Program.Debug) { Console.WriteLine("Debug Player Health capped out"); }
                }
            }

            // Print Healths
            Console.WriteLine($"Your health is now {Program.p.Health} \nThe Dealer's health is now {Program.d.Health}");
        }
    }
}