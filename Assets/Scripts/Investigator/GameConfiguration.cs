using System.Collections.Generic;

namespace CMythos {
    public static class GameConfiguration {
        public static Queue<Investigator> investigators = new Queue<Investigator>();

        public static int difficulty;

        public static Dictionary<string,float> standings = new Dictionary<string, float>();



    }
}