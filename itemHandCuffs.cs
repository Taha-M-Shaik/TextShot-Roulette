using System;

namespace TextShot
{
    public class HandCuffs : Item
    {
        public HandCuffs() : base("Hand Cuffs", "Skips the next player's turn")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // Set handcuffs to true
            Program.HandCuffed = true;
        }
    }
}