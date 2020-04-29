

using System;
using UnityEngine.Events;

namespace CMythos.Events
{
    [Serializable]
    public class CardEvent : UnityEvent<GameBoardPlayer, Card> { }
    [Serializable]
    public class CardPlayEvent : CardEvent { }

    [Serializable]
    public class CardDrawEvent : CardEvent { }

    [Serializable]
    public class CardDiscardEvent : CardEvent { }

    [Serializable]
    public class CardActiveEvent : CardEvent { }

    [Serializable]
    public class CardScoreEvent : CardEvent { }

}