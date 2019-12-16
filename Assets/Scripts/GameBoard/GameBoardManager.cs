using UnityEngine;
using System.Collections.Generic;
using System;

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
        private int cardsPerDeck = 40;

        


        [SerializeField]
        private GameBoard gameBoard;

        [SerializeField]
        private Dice[] movementDice;

        public Dice[] MovementDice
        {
            get => movementDice;
        }

        public GameBoard GameBoard
        {
            get => gameBoard;
        }
        private List<GameBoardEntity> entities;
        public List<GameBoardEntity> Entities
        {
            get => entities;
        }


        private Dictionary<GameBoardEntity, GameBoardEntityInfo> entityInfo;

        public Dictionary<GameBoardEntity, GameBoardEntityInfo> EntityInfo
        {
            get => entityInfo;
        }
        private bool initialized = false;
        [SerializeField]
        private PlayerViewUI playerViewUI;
        private TurnManager turnManager;
        private void Start()
        {
            entities = new List<GameBoardEntity>(GetComponentsInChildren<GameBoardEntity>());
            entityInfo = new Dictionary<GameBoardEntity, GameBoardEntityInfo>();


        }
        public void Init()
        {
            if (!initialized)
            {
                initialized = true;
                UpdateEntityInfo();
                turnManager = GetComponentInChildren<TurnManager>();
                turnManager.PlayerViewUI = playerViewUI;
                turnManager.Begin();
                playerViewUI.DiceShooter = GetComponentInChildren<DiceShooter>();
                playerViewUI.GameBoardManager = this;
            }
        }

        private void UpdateEntityInfo()
        {
            entityInfo.Clear();
            entities.ForEach(x =>
            {
                entityInfo[x] = x.InitialInfo(gameBoard);
            });
        }
        private void Update()
        {

            if (Input.GetKey(KeyCode.P))
            {
                Init();
            }
        }

        public Vector3Int GetCoordinates(GameBoardEntity entity)
        {
            return entityInfo[entity].coordinates;
        }

        public void MoveForward(GameBoardEntity entity)
        {
            MoveForward(entity, entityInfo[entity].direction);
        }

        public void MoveForward(GameBoardEntity entity, GameBoardEntityDirection preferredDirection)
        {
            GameBoardEntityInfo info = entityInfo[entity];
            if (IsAmbiguousDirection(info))
            {
                if (CanMove(info.coordinates, preferredDirection))
                {
                    info.coordinates = info.coordinates.NextTile(preferredDirection);
                    info.direction = preferredDirection;
                }
            }
            else
            {
                GameBoardEntityDirection newDir = PathOutDirection(info.coordinates, info.direction) ?? throw new ArgumentOutOfRangeException();
                if (CanMove(info.coordinates, newDir))
                {
                    info.coordinates = info.coordinates.NextTile(newDir);
                    info.direction = newDir;
                }


            }




        }
        public int TryMove(GameBoardEntity entity, int spaces)
        {
            while (spaces > 0)
            {
                if (IsAmbiguousDirection(entity))
                {
                    Debug.Log("AMBIGUOUS");
                    return spaces;
                }
                else
                {
                    MoveForward(entity);
                }
                spaces -= 1;
                Debug.Log("SPACES LEFT " + spaces);
                
            }
            
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


        public bool IsAmbiguousDirection(GameBoardEntity entity)
        {
            return IsAmbiguousDirection(entityInfo[entity]);
        }
        private bool IsAmbiguousDirection(GameBoardEntityInfo entityInfo)
        {
            return IsAmbiguousDirection(entityInfo.coordinates, entityInfo.direction);
        }
        private bool IsAmbiguousDirection(Vector3Int coordinates, GameBoardEntityDirection forward)
        {
            Debug.Log($"Path Type: {gameBoard.GetTile(coordinates).PathType}");
            Debug.Log($"Forward: {forward}");
            byte b = (byte)((byte)gameBoard.GetTile(coordinates).PathType & (byte)(((int)forward.Opposite()) ^ 15));
            Debug.Log($"Result: {b}");
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