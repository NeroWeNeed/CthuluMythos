using UnityEngine;

namespace CMythos
{
    public delegate void playerViewUIRefresher(GameBoardPlayer player);
    
    public class PlayerViewUIRefreshable : MonoBehaviour {
        public playerViewUIRefresher Refresher { get; set; }
    }
    

}