using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using CMythos.Events;
using UnityEngine.SceneManagement;

namespace CMythos
{
    public class TurnManager : MonoBehaviour
    {
        //Events
        [SerializeField]
        private TurnStartEvent turnStartEvent;

        public TurnStartEvent TurnStartEvent
        {
            get => turnStartEvent;
        }

        [SerializeField]
        private TurnEndEvent turnEndEvent;

        public TurnEndEvent TurnEndEvent
        {
            get => turnEndEvent;
        }
        [SerializeField]
        private RoundStartEvent roundStartEvent;

        public RoundStartEvent RoundStartEvent
        {
            get => roundStartEvent;
        }
        [SerializeField]
        private RoundEndEvent roundEndEvent;

        public RoundEndEvent RoundEndEvent
        {
            get => roundEndEvent;
        }

        [SerializeField]
        private RoundCycleStartEvent roundCycleStartEvent;

        public RoundCycleStartEvent RoundCycleStartEvent
        {
            get => roundCycleStartEvent;
        }
        [SerializeField]
        private RoundCycleEndEvent roundCycleEndEvent;

        public RoundCycleEndEvent RoundCycleEndEvent
        {
            get => roundCycleEndEvent;
        }

        public bool Active { get; private set; } = false;

        private List<TurnManagable> turnEntities;

        private List<TurnManagable> turnOrder;
        private int turnOrderIndex = 0;

        private TurnManagable currentTurnManagable;
        [SerializeField]
        private PlayerViewUI playerViewUI;

        public PlayerViewUI PlayerViewUI
        {
            get => playerViewUI;
            set => playerViewUI = value;
        }

        public int CurrentTurn { get; private set; }
        public int MaxTurns = 10;

        private void Awake()
        {
            turnEntities = new List<TurnManagable>(GetComponentsInChildren<TurnManagable>());
            if (turnStartEvent == null)
                turnStartEvent = new TurnStartEvent();
            if (TurnEndEvent == null)
                turnEndEvent = new TurnEndEvent();
            if (roundStartEvent == null)
                roundStartEvent = new RoundStartEvent();
            if (RoundEndEvent == null)
                roundEndEvent = new RoundEndEvent();
            if (roundCycleStartEvent == null)
                roundCycleStartEvent = new RoundCycleStartEvent();
            if (roundCycleEndEvent == null)
                roundCycleEndEvent = new RoundCycleEndEvent();
        }
        public void Init(int playerCount)
        {
            if (!Active)
            {
                Active = true;
                turnEntities.AddRange(GetComponentsInChildren<TurnManagable>());


                InitRound();
                roundCycleStartEvent.Invoke();
                RoundStartEvent.Invoke(turnOrder);
                currentTurnManagable = turnOrder[turnOrderIndex++];
                NextTurn(currentTurnManagable);
            }
        }
        public void End()
        {
            if (Active)
            {
                Active = false;
                RoundCycleEndEvent.Invoke();


            }
        }

        private void InitRound()
        {
            turnOrderIndex = 0;
            if (turnOrder == null)
                turnOrder = new List<TurnManagable>(turnEntities);
            else if (!turnOrder.ContainsAll(turnEntities))
            {
                turnOrder.Clear();
                turnOrder.AddRange(turnEntities);
            }

            turnOrder.Shuffle();
        }
        public void EndTurn(TurnManagable turnManagable)
        {
            if (turnManagable == currentTurnManagable && turnManagable != null && Active)
            {
                TurnEndEvent.Invoke(currentTurnManagable);
                Debug.Log(turnOrderIndex);
                if (turnOrderIndex < turnOrder.Count)
                {
                    currentTurnManagable = turnOrder[turnOrderIndex++];
                    NextTurn(currentTurnManagable);
                }
                else
                {
                    
                    RoundEndEvent.Invoke(turnOrder);
                    CurrentTurn++;
                    if (CurrentTurn > MaxTurns)
                    {
                        End();
                        SceneManager.LoadScene(2);
                    }
                    else
                    {
                        InitRound();
                        RoundStartEvent.Invoke(turnOrder);
                        currentTurnManagable = turnOrder[turnOrderIndex++];
                        NextTurn(currentTurnManagable);
                    }


                }

            }
        }
        private void NextTurn(TurnManagable managable)
        {

            TurnStartEvent.Invoke(managable);
            managable.TurnStartedEvent.Invoke();


        }
    }

}

