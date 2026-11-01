using System;
using System.Collections.Generic;

namespace TextShot
{
    public class Gun
    {
        private List<bool> _bullets;
        private int _liveBullets;
        private int _dudBullets;
        Random r = new Random();

        public Gun(int n)
        {
            _bullets = new List<bool>();
        }

        public int LiveBullets
        {
            get { return _liveBullets; }
        }

        public int DudBullets
        {
            get { return _dudBullets; }
        }

        public List<bool> Bullets
        {
            get { return _bullets; }
        }

        public void Load(int n)
        {
            // Use backing fields (_bullets) instead of read-only properties (Bullets)
            _bullets = new List<bool>();
            if (Program.Debug) { Console.WriteLine($"Debug Loaded {n} bullets"); }

            // Guarantee at least 1 live and 1 dud
            _liveBullets = r.Next(1, n);
            _dudBullets = n - _liveBullets;

            // Add the chosen amount of live bullets first, then duds
            for (int i = 0; i < n; i++)
            {
                _bullets.Add(i < _liveBullets); 
            }
        }

        public void Shuffle()
        {
            // Shuffle the list to randomize bullet order
            for (int i = 0; i < _bullets.Count; i++)
            {
                int randomIndex = r.Next(i, _bullets.Count);
                bool temp = _bullets[i];
                _bullets[i] = _bullets[randomIndex];
                _bullets[randomIndex] = temp;
                
                if (Program.Debug) { Console.WriteLine($"Debug Bullet {i}: {_bullets[i]}"); }
            }
        }

        public void PrintLoadout()
        {
            // Print amount of live bullets
            if (_liveBullets == 1)
            { Console.Write($"1 live bullet, "); }
            else
            { Console.Write($"{_liveBullets} live bullets, "); }

            // Print amount of dud bullets
            if (_dudBullets == 1)
            { Console.WriteLine($"1 dud bullet"); }
            else
            { Console.WriteLine($"{_dudBullets} dud bullets"); }
        }

        public bool ReadCurBullent() // Read current bullet
        {
            if (_bullets.Count > 0)
            {
                return _bullets[0];
            }
            else
            {
                return false; // Required return path if the gun is empty
            }
        }

        public void PrintCurBullet() // Print current bullet
        {
            if (ReadCurBullent())
            {
                Console.WriteLine("Live");
            }
            else
            {
                Console.WriteLine("Dud");
            }
        }

        public void EjectCurBullet() // Eject current bullet
        {
            if (_bullets.Count > 0)
            {
                // Check if the bullet being ejected is live or dud
                bool isLive = _bullets[0];
                
                // Remove the bullet from the start of the list
                _bullets.RemoveAt(0);
                
                // Decrement the correct counter
                if (isLive) 
                {
                    _liveBullets--;
                }
                else
                {
                    _dudBullets--;
                }

                if (Program.Debug) { Console.WriteLine($"Debug Bullet Ejected. Remaining: {_bullets.Count}"); Console.Write("Debug "); PrintLoadout(); }
            }
        }
    }
}