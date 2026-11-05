using System;

namespace TextShot
{
    public class InfectedBullet : Item
    {
        public InfectedBullet() : base("Infected Bullet", "Changes the current bullet to double damage")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // Sets the current bullet to double damage
            Program.InfectedIsBullet = true;
        }
    }
}