using CMythos.Events;
using UnityEngine;

namespace CMythos {
public class GameBoardEffect : MonoBehaviour
    {

        [SerializeField]
        private TileLandEvent tileLandEvent;

        public TileLandEvent TileLandEvent
        {
            get => tileLandEvent;
        }

        [SerializeField]
        private TilePassEvent tilePassEvent;

        public TilePassEvent TilePassEvent
        {
            get => tilePassEvent;
        }

        [SerializeField]
        private TileLeaveEvent tileLeaveEvent;

        public TileLeaveEvent TileLeaveEvent
        {
            get => tileLeaveEvent;
        }
    }
}