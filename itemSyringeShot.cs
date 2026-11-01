using System;

namespace TextShot
{
    public class SyringeShot : Item
    {
        public SyringeShot() : base("Syringe Shot", "Heals 1 point")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();
        }
    }
}