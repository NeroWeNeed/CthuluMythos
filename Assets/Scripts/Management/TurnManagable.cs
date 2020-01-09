using UnityEngine;
using CMythos.Events;
using UnityEngine.Events;
namespace CMythos
{
    public class TurnManagable : MonoBehaviour
    {

        [SerializeField]
        private UnityEvent initEvent;

        public UnityEvent InitEvent
        {
            get => initEvent;
        }

        [SerializeField]
        private UnityEvent turnStartedEvent;

        public UnityEvent TurnStartedEvent
        {
            get => turnStartedEvent;
        }

        [SerializeField]
        private UnityEvent turnEndedEvent;

        public UnityEvent TurnEndedEvent
        {
            get => turnEndedEvent;
        }

        public TurnManagable() : base()
        {
            if (turnStartedEvent == null)
                turnStartedEvent = new UnityEvent();
            if (turnEndedEvent == null)
                turnEndedEvent = new UnityEvent();
            if (initEvent == null)
                initEvent = new UnityEvent();
        }
        public void EndTurn()
        {

            GetComponentInParent<TurnManager>().EndTurn(this);
        }
        public TurnManager GetTurnManager()
        {
            return GetComponentInParent<TurnManager>();
        }


    }

}