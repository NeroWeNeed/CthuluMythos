using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CMythos.Events;

namespace CMythos
{

    [CreateAssetMenu(fileName = "GameBoard", menuName = "Cthulhu Mythos/GameBoard", order = 0)]
    public class GameBoard : ScriptableObject, IEnumerable<GameBoardTile>
    {
        [SerializeField]
        public Vector3Int size;
        public Vector3Int Size
        {
            get => size;
        }

        [SerializeField]
        public GameBoardTile[] tiles;

        [SerializeField]
        public GameObject defaultRender;

        public int Width
        {
            get => size.x;
        }
        public int Length
        {
            get => size.z;
        }
        public int Height
        {
            get => size.y;
        }





        // Start is called before the first frame update
        private void OnEnable()
        {
            if (size == null)
                size = new Vector3Int(0, 0, 0);
            if (tiles == null)
            {
                tiles = new GameBoardTile[size.x * size.y * size.z];
                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i] = new GameBoardTile();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public GameBoardTile GetTile(Vector3Int coordinates)
        {
            return tiles[coordinates.y * (Width * Length) + coordinates.z * (Width) + coordinates.x];
        }
        public Vector3Int GetCoordinates(GameBoardTile tile)
        {
            int index = Array.IndexOf(tiles, tile);
            if (index >= 0)
            {
                return new Vector3Int(index % Width, index / (Width * Length), (index % (Width * Length)) / Width);
            }
            else
                return new Vector3Int(-1, -1, -1);
        }


        public IEnumerator<GameBoardTile> GetEnumerator()
        {
            
            return tiles.GetEnumerator() as IEnumerator<GameBoardTile>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    [Serializable]
    public class GameBoardTile
    {


        public GameBoardPathType PathType;

        public GameBoardEffect Effect;
        public GameObject Render;
        public void Clear()
        {
            PathType = GameBoardPathType.BLOCKED;
            Effect = null;
            Render = null;
        }

    }
    public enum GameBoardPathType
    {
        BLOCKED = 0,
        WEST = 1,
        EAST = 2,
        EASTWEST = 3,
        SOUTH = 4,
        SOUTHWEST = 5,
        SOUTHEAST = 6,
        SOUTHEASTWEST = 7,
        NORTH = 8,
        NORTHWEST = 9,
        NORTHEAST = 10,
        NORTHEASTWEST = 11,
        NORTHSOUTH = 12,
        NORTHSOUTHWEST = 13,
        NORTHSOUTHEAST = 14,
        ALL = 15
    }



}