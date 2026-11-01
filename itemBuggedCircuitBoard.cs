using System;

namespace TextShot
{
    public class BuggedCircuitBoard : Item
    {
        public BuggedCircuitBoard() : base("Bugged Circuit Board", "Swaps your and the dealer's inventories")
        {}
        
        override public void ExecuteEffect()
        {
            base.ExecuteEffect();

            // Store Player's inventory in temp
            Inventory tempInventory = Program.p.Inventory;

            // Overwrite Player's with Dealer's
            Program.p.Inventory = Program.d.Inventory;

            // Set Dealer's to temp
            Program.d.Inventory = tempInventory;
        }
    }
}