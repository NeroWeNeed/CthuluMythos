using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace CMythos
{
    public class GameBoardManager : MonoBehaviour
    {
        private static Vector3Int invalidTile = new Vector3Int(-1, -1, -1);
        public static Vector3Int INVALID_TILE
        {
            get => invalidTile;
        }

        [SerializeField]
        private UnityEvent ambiguousDirectionEvent;

        public UnityEvent AmbiguousDirectionEvent { get => ambiguousDirectionEvent; set => ambiguousDirectionEvent = value; }
        [SerializeField]
        private UnityEvent ambiguousDirectionSolvedEvent;

        public UnityEvent AmbiguousDirectionSolvedEvent { get => ambiguousDirectionSolvedEvent; set => ambiguousDirectionSolvedEvent = value; }
        [SerializeField]
        private UnityEvent ambiguousDirectionUpdateEvent;

        public UnityEvent AmbiguousDirectionUpdateEvent { get => ambiguousDirectionUpdateEvent; set => ambiguousDirectionUpdateEvent = value; }

        [SerializeField]
        private int cardsPerDeck = 40;
        public int CardsPerDeck
        {
            get => cardsPerDeck;
        }



        [SerializeField]
        private GameBoard gameBoard;
        public GameBoard GameBoard
        {
            get => gameBoard;
        }

        [SerializeField]
        private DiceShooter diceShooter;
        public DiceShooter DiceShooter
        {
            get => diceShooter;
        }


        [SerializeField]
        private Dice[] movementDice;

        public Dice[] MovementDice
        {
            get => movementDice;
        }


        private List<GameBoardEntity> entities;
        public List<GameBoardEntity> Entities
        {
            get => entities;
        }

        public Dictionary<GameBoardEntity, GameBoardEntityInfo> EntityInfo { get; private set; }
        private bool initialized = false;
        [SerializeField]
        private PlayerViewUI playerViewUI;
        public PlayerViewUI PlayerViewUI
        {
            get => playerViewUI;
        }
        [SerializeField]
        private PlayMatRenderer playMatRenderer;

        public PlayMatRenderer PlayMatRenderer
        {
            get => playMatRenderer;
        }


        [SerializeField]
        private TurnManager turnManager;

        public TurnManager TurnManager
        {
            get => turnManager;
        }
        private void Awake()
        {
            entities = new List<GameBoardEntity>(GetComponentsInChildren<GameBoardEntity>());
            EntityInfo = new Dictionary<GameBoardEntity, GameBoardEntityInfo>();
            if (turnManager == null)
                turnManager = GetComponentInChildren<TurnManager>();
            if (playerViewUI == null)
                playerViewUI = GetComponentInChildren<PlayerViewUI>();
            if (playMatRenderer == null)
                playMatRenderer = playerViewUI.GetComponentInChildren<PlayMatRenderer>();
            if (diceShooter == null)
                diceShooter = GetComponentInChildren<DiceShooter>();
        }

        public void StartGame(int playerCount, GameBoard gameBoard = null)
        {

            if (!initialized)
            {
                initialized = true;
                if (gameBoard != null)
                    this.gameBoard = gameBoard;
                GameObject obj;
                PlayMatRenderer.Init();

                for (int i = 0; i < playerCount; i++)
                {
                    obj = new GameObject("Player " + (i + 1), typeof(GameBoardPlayer));
                    obj.transform.SetParent(turnManager.transform);
                    entities.Add(obj.GetComponent<GameBoardEntity>());

                }

                UpdateEntityInfo();
                turnManager.PlayerViewUI = playerViewUI;
                playerViewUI.DiceShooter = diceShooter;
                playerViewUI.GameBoardManager = this;
                TurnManager.TurnStartEvent.AddListener(x =>
                {
                    playerViewUI.SetCurrentPlayer(x);
                });
                /*                 foreach (var item in GetComponentsInChildren<Initializable>())
                                {
                                    item.Init();
                                } */
                playerViewUI.Init();
                turnManager.Init(playerCount);

            }

        }



        private void UpdateEntityInfo()
        {
            EntityInfo.Clear();
            entities.ForEach(x =>
            {
                EntityInfo[x] = x.InitialInfo(gameBoard);
            });
        }



        public Vector3Int GetCoordinates(GameBoardEntity entity)
        {
            return EntityInfo[entity].coordinates;
        }
        public GameBoardTile GetTile(GameBoardEntity entity)
        {
            Vector3Int coords = GetCoordinates(entity);
            if (coords != INVALID_TILE)
                return GameBoard.GetTile(coords);
            else
                return null;
        }
        public GameBoardTile GetTile(Vector3Int coords)
        {

            if (coords != INVALID_TILE)
                return GameBoard.GetTile(coords);
            else
                return null;
        }

        public void MoveForward(GameBoardEntity entity)
        {
            MoveForward(entity, EntityInfo[entity].direction);
        }

        public void MoveForward(GameBoardEntity entity, GameBoardEntityDirection preferredDirection)
        {
            GameBoardEntityInfo info = EntityInfo[entity];
            if (IsAmbiguousDirection(info))
            {
                if (CanMove(info.coordinates, preferredDirection))
                {
                    GetTile(info.coordinates).Effect?.TileLeaveEvent?.Invoke(entity, info.coordinates);
                    info.coordinates = info.coordinates.NextTile(preferredDirection);
                    info.direction = preferredDirection;
                    GetTile(info.coordinates).Effect?.TilePassEvent?.Invoke(entity, info.coordinates);
                }
            }
            else
            {
                GameBoardEntityDirection newDir = PathOutDirection(info.coordinates, info.direction) ?? throw new ArgumentOutOfRangeException();
                if (CanMove(info.coordinates, newDir))
                {
                    GetTile(info.coordinates).Effect?.TileLeaveEvent?.Invoke(entity, info.coordinates);
                    info.coordinates = info.coordinates.NextTile(newDir);
                    info.direction = newDir;
                    GetTile(info.coordinates).Effect?.TilePassEvent?.Invoke(entity, info.coordinates);
                }


            }




        }
        public int TryMove(GameBoardEntity entity, int spaces)
        {
            while (spaces > 0)
            {
                if (IsAmbiguousDirection(entity))
                {

                    return spaces;
                }
                else
                {
                    MoveForward(entity);
                }
                spaces -= 1;

            }
            GetTile(entity).Effect?.TileLandEvent?.Invoke(entity, EntityInfo[entity].coordinates);
            return 0;
        }
        public int TryMove(GameBoardEntity entity, int spaces, GameBoardEntityDirection preferredDirection)
        {
            bool followedPreferredDirection = false;
            while (spaces > 0)
            {
                if (IsAmbiguousDirection(entity))
                {
                    if (!followedPreferredDirection)
                    {
                        MoveForward(entity, preferredDirection);
                        followedPreferredDirection = true;
                    }
                    else
                        return spaces;
                }
                else
                {
                    MoveForward(entity);
                }
                spaces -= 1;

            }
            GetTile(entity).Effect?.TileLandEvent?.Invoke(entity, EntityInfo[entity].coordinates);
            return 0;
        }

        public bool CanMove(Vector3Int coordinates, GameBoardEntityDirection direction)
        {
            if (coordinates == INVALID_TILE)
                return false;
            Vector3Int nextCoordinates = coordinates.NextTile(direction);
            if (nextCoordinates == coordinates)
                return false;
            byte curTile = (byte)gameBoard.GetTile(coordinates).PathType;
            byte nextTile = (byte)gameBoard.GetTile(nextCoordinates).PathType;
            byte dir = (byte)direction;
            byte invDir = (byte)direction.Opposite();
            return ((curTile & dir) == dir) && ((nextTile & invDir) == invDir);

        }
        public GameBoardEntityDirection? PathOutDirection(Vector3Int coordinates, GameBoardEntityDirection forward)
        {
            if (coordinates == INVALID_TILE || IsAmbiguousDirection(coordinates, forward))
                return null;
            byte invDir = (byte)((int)forward.Opposite() ^ 15);
            byte curDir;
            byte tile = (byte)gameBoard.GetTile(coordinates).PathType;
            tile = (byte)(tile & invDir);
            foreach (GameBoardEntityDirection item in Enum.GetValues(typeof(GameBoardEntityDirection)))
            {
                curDir = (byte)(tile & ((byte)item));
                if (curDir != 0)
                    return item;
            }
            return (GameBoardEntityDirection)(invDir ^ 15);

        }
        public GameBoardEntityDirection? PathOutDirection(Vector3Int coordinates)
        {
            if (coordinates == INVALID_TILE)
                return null;

            byte tile = (byte)gameBoard.GetTile(coordinates).PathType;
            byte curDir;
            foreach (GameBoardEntityDirection item in Enum.GetValues(typeof(GameBoardEntityDirection)))
            {
                curDir = (byte)(tile & ((byte)item));
                if (curDir != 0)
                    return item;
            }
            return null;

        }
        public GameBoardEntityDirection GetDirection(GameBoardEntity entity, GameBoardEntityRelativeDirection direction)
        {
            GameBoardEntityInfo info = EntityInfo[entity];
            switch (direction)
            {
                case GameBoardEntityRelativeDirection.FORWARD:
                    return info.direction;
                case GameBoardEntityRelativeDirection.BACKWARD:
                    return info.direction.Opposite();
                case GameBoardEntityRelativeDirection.LEFT:
                    return info.direction.Left();
                case GameBoardEntityRelativeDirection.RIGHT:
                    return info.direction.Right();
            }
            throw new UnityException("Invalid Direction");
        }


        public bool IsAmbiguousDirection(GameBoardEntity entity)
        {
            return IsAmbiguousDirection(EntityInfo[entity]);
        }
        private bool IsAmbiguousDirection(GameBoardEntityInfo entityInfo)
        {
            return IsAmbiguousDirection(entityInfo.coordinates, entityInfo.direction);
        }
        private bool IsAmbiguousDirection(Vector3Int coordinates, GameBoardEntityDirection forward)
        {

            byte b = (byte)((byte)gameBoard.GetTile(coordinates).PathType & (byte)(((int)forward.Opposite()) ^ 15));

            return (b & (b - 1)) != 0;
        }

        public GameBoardEntityInfo GetInformation(GameBoardEntity entity)
        {
            if (EntityInfo.ContainsKey(entity))
            {
                return EntityInfo[entity];
            }
            else
                return null;
        }


    }
}