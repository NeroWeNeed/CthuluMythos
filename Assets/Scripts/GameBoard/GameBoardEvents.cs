using UnityEngine;
using UnityEngine.Events;

namespace CMythos.Events
{
    [System.Serializable]
    public class TileLandEvent : UnityEvent<GameBoardEntity, Vector3Int> { }
    [System.Serializable]
    public class TileLeaveEvent : UnityEvent<GameBoardEntity, Vector3Int> { }
    [System.Serializable]
    public class TilePassEvent : UnityEvent<GameBoardEntity, Vector3Int> { }
    [System.Serializable]
    public class TileStartTurnEvent : UnityEvent<GameBoardEntity, Vector3Int> { }
    [System.Serializable]
    public class TileEndTurnEvent : UnityEvent<GameBoardEntity, Vector3Int> { }

    [System.Serializable]

    public class DirectionSelectEvent : UnityEvent<GameBoardEntityDirection> { }

}