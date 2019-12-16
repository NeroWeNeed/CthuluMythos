using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CMythos.Events
{
    [Serializable]
    public class RoundCycleStartEvent : UnityEvent { }
    
    [Serializable]
    public class RoundCycleEndEvent : UnityEvent { }

    [Serializable]
    public class RoundEvent : UnityEvent<List<TurnManagable>> { }

    [Serializable]
    public class RoundStartEvent : RoundEvent { }

    [Serializable]
    public class RoundEndEvent : RoundEvent { }

    [Serializable]
    public class TurnEvent : UnityEvent<TurnManagable> { }

    [Serializable]
    public class TurnStartEvent : TurnEvent { }

    [Serializable]
    public class TurnEndEvent : TurnEvent { }
}