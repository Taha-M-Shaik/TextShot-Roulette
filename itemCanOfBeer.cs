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
            bool wasLive = Program.Gun.ReadCurBullent();
            Console.WriteLine($"You popped a {(wasLive ? "LIVE" : "DUD")} bullet out of the gun!");
            
            // Eject current bullet
            Program.Gun.EjectCurBullet();
            
            // Dealer logs the ejected bullet and runs deductions
            Program.d.RemoveTopBullet(wasLive);
        }
    }
}