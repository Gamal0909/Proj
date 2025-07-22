using System;
using System.Linq;

namespace Series.Core
{
    [Serializable]
    public class LevelData
    {
        public bool locked;
        public int stamina;
        public float time;
        public bool[] items = new bool[GameLevel.ItemsPerLevel];

        /// <summary>
        /// Returns the amount of Items that have been collected.
        /// </summary>
        public int CollectedItems()
        {
            return items.Where((star) => star).Count();
        }
    }
}
