using System.Collections.Generic;

namespace ThePotentialJump.ProgressSystem
{
    // [System.Serializable]
    public class GameProgressData
    {
        public GameProgressData()
        {
            CompletedLevels = new HashSet<string>();
        }
        public HashSet<string> CompletedLevels { get; set; } 
        public string LastCompletedLevel { get; set; }
        public int Score { get; set; }
    }

}