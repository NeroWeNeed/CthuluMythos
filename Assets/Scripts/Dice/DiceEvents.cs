using UnityEngine.Events;
using System;
namespace CMythos.Events
{
    [Serializable]
    public class DiceStablizedEvent : UnityEvent<Dice, string> { }
    [Serializable]
    public class DiceDestablizedEvent : UnityEvent<Dice> { }
    [Serializable]
    public class DiceRollingEvent : UnityEvent<Dice> { }
}