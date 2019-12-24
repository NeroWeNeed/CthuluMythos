using UnityEngine;

namespace CMythos
{
    public class GameBoardEntity : MonoBehaviour
    {
        public delegate GameBoardEntityInfo EntityInfoInitializer(GameBoard gameBoard);


        public EntityInfoInitializer InfoInitializer { get; set; }


        private void Start()
        {
            if (InfoInitializer == null)
                InfoInitializer = DefaultInitialInfo;
        }
        public GameBoardEntityInfo InitialInfo(GameBoard gameBoard)
        {
            return InfoInitializer(gameBoard);
        }
        private GameBoardEntityInfo DefaultInitialInfo(GameBoard gameBoard)
        {

            foreach (var item in gameBoard.tiles)
            {

                if (item != null && item.PathType != GameBoardPathType.BLOCKED)
                {
                    return new GameBoardEntityInfo
                    {
                        coordinates = gameBoard.GetCoordinates(item)
                    };

                };

            }
            return new GameBoardEntityInfo
            {
                coordinates = new Vector3Int(-1, -1, -1)
            };
        }
    }
    public class GameBoardEntityInfo
    {
        public Vector3Int coordinates;
        public GameBoardEntityDirection direction;
    }
    public enum GameBoardEntityDirection
    {
        NORTH = 8, SOUTH = 4, EAST = 2, WEST = 1
    }
    public static class GameBoardEntityDirectionUtil
    {
        public static GameBoardEntityDirection Opposite(this GameBoardEntityDirection dir)
        {
            switch (dir)
            {
                case GameBoardEntityDirection.NORTH:
                    return GameBoardEntityDirection.SOUTH;
                case GameBoardEntityDirection.SOUTH:
                    return GameBoardEntityDirection.NORTH;
                case GameBoardEntityDirection.EAST:
                    return GameBoardEntityDirection.WEST;
                case GameBoardEntityDirection.WEST:
                    return GameBoardEntityDirection.EAST;
                default:
                    return GameBoardEntityDirection.SOUTH;
            }

        }
        public static GameBoardEntityDirection Left(this GameBoardEntityDirection dir)
        {
            switch (dir)
            {
                case GameBoardEntityDirection.NORTH:
                    return GameBoardEntityDirection.WEST;
                case GameBoardEntityDirection.SOUTH:
                    return GameBoardEntityDirection.EAST;
                case GameBoardEntityDirection.EAST:
                    return GameBoardEntityDirection.NORTH;
                case GameBoardEntityDirection.WEST:
                    return GameBoardEntityDirection.SOUTH;
                default:
                    return GameBoardEntityDirection.SOUTH;
            }

        }
                public static GameBoardEntityDirection Right(this GameBoardEntityDirection dir)
        {
            switch (dir)
            {
                case GameBoardEntityDirection.NORTH:
                    return GameBoardEntityDirection.EAST;
                case GameBoardEntityDirection.SOUTH:
                    return GameBoardEntityDirection.WEST;
                case GameBoardEntityDirection.EAST:
                    return GameBoardEntityDirection.SOUTH;
                case GameBoardEntityDirection.WEST:
                    return GameBoardEntityDirection.NORTH;
                default:
                    return GameBoardEntityDirection.SOUTH;
            }

        }

        public static Vector3Int NextTile(this Vector3Int coordinates, GameBoardEntityDirection dir)
        {
            switch (dir)
            {
                case GameBoardEntityDirection.NORTH:
                    return new Vector3Int(coordinates.x, coordinates.y, coordinates.z + 1);
                case GameBoardEntityDirection.SOUTH:
                    return new Vector3Int(coordinates.x, coordinates.y, coordinates.z - 1);
                case GameBoardEntityDirection.EAST:
                    return new Vector3Int(coordinates.x + 1, coordinates.y, coordinates.z);
                case GameBoardEntityDirection.WEST:
                    return new Vector3Int(coordinates.x - 1, coordinates.y, coordinates.z);
                default:
                    return coordinates;
            }
        }
    }
}