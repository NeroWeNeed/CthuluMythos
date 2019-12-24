using UnityEngine;
using UnityEngine.Events;
using System;
namespace CMythos.Events
{

    [Serializable]
    public class PlayerUIEvent : UnityEvent<GameBoardPlayer> { }

    [Serializable]
    public class PlayerChangedEvent : PlayerUIEvent { }

    [Serializable]
    public class PlayerUIActionEvent : PlayerUIEvent { }
}