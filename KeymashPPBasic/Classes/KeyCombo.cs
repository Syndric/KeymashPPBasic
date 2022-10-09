using System;
namespace KeymashPPBasic.Classes
{
    public class KeyCombo
    {
        public string Combo = null;
        public int Count = 0;
        public KeyCombo(string combo, int count)
        {
            this.Combo = combo;
            this.Count = count;
        }
    }
}
