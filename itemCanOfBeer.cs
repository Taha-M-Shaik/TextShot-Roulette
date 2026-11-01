using System;

namespace TextShot
{
    public class CanOfBeer : Item
    {
        public CanOfBeer() : base("Can Of Beer", "Removes the current bullet without using it")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // Read current bullet
            Console.Write("Bullet ejected: ");
            Program.Gun.PrintCurBullet();

            // Eject curent bullet
            Program.Gun.EjectCurBullet();
        }
    }
}