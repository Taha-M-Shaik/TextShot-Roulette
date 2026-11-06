using System;
using System.Collections.Generic;

namespace TextShot
{
    public enum BulletKnowledge
    {
        Unknown,
        Live,
        Dud
    }

    public class Dealer : Participant
    {
        public List<BulletKnowledge> KnownBullets { get; private set; }
        
        // Track the exact number of remaining bullets of each type
        private int _knownLive;
        private int _knownDud;

        public Dealer(string name, int health) : base(name, health)
        {
            KnownBullets = new List<BulletKnowledge>();
        }

        // Pass the gun's exact initial loadout when the round starts
        public void ClearKnowledge()
        {
            KnownBullets.Clear();
            _knownLive = 0;
            _knownDud = 0;
        }

        public void InitialiseKnowledge(int liveCount, int dudCount)
        {
            ClearKnowledge();
            _knownLive = liveCount;
            _knownDud = dudCount;
            int totalBullets = liveCount + dudCount;
            
            for (int i = 0; i < totalBullets; i++)
            {
                KnownBullets.Add(BulletKnowledge.Unknown);
            }
            if (Program.Debug) { Console.WriteLine($"Debug Dealer initialized memory: {_knownLive} Live, {_knownDud} Dud."); }
            
            DeduceRemainingBullets();
        }

        public void LearnCurrentBullet(bool isLive)
        {
            if (KnownBullets.Count > 0)
            {
                KnownBullets[0] = isLive ? BulletKnowledge.Live : BulletKnowledge.Dud;
                if (Program.Debug) { Console.WriteLine($"Debug Dealer learned current bullet is {KnownBullets[0]}"); }
            }
        }

        // Call this whenever a bullet is fired or ejected by an item
        public void RemoveTopBullet(bool wasLive)
        {
            if (KnownBullets.Count > 0)
            {
                KnownBullets.RemoveAt(0);
                
                // Decrement the correct tracked pool
                if (wasLive) { _knownLive--; }
                else { _knownDud--; }
                
                if (Program.Debug) { Console.WriteLine($"Debug Dealer saw a {(wasLive ? "Live" : "Dud")} bullet leave. Remaining - Live: {_knownLive}, Dud: {_knownDud}"); }
                
                // Run the deduction algorithm on the remaining bullets
                DeduceRemainingBullets();
            }
        }

        public void PrintKnowledge()
        {
            Console.Write("Debug Dealer knows: ");
            for (int i = 0; i < KnownBullets.Count; i++)
            {
                Console.Write($"Debug Bullet {i}: {KnownBullets[i]} ");
            }
            Console.WriteLine();
        }

        private void DeduceRemainingBullets()
        {
            // If there are no duds left, every remaining bullet MUST be live
            if (_knownDud == 0 && _knownLive > 0)
            {
                for (int i = 0; i < KnownBullets.Count; i++)
                {
                    if (KnownBullets[i] == BulletKnowledge.Unknown)
                    {
                        KnownBullets[i] = BulletKnowledge.Live;
                    }
                }
                if (Program.Debug) { Console.WriteLine("Debug Dealer deduced all remaining bullets are LIVE."); }
            }
            // If there are no lives left, every remaining bullet MUST be dud
            else if (_knownLive == 0 && _knownDud > 0)
            {
                for (int i = 0; i < KnownBullets.Count; i++)
                {
                    if (KnownBullets[i] == BulletKnowledge.Unknown)
                    {
                        KnownBullets[i] = BulletKnowledge.Dud;
                    }
                }
                if (Program.Debug) { Console.WriteLine("Debug Dealer deduced all remaining bullets are DUD."); }
            }
        }
    }
}