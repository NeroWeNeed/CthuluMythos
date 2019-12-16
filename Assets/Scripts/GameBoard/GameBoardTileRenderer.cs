using UnityEngine;

namespace CMythos
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GameBoardTileRenderer : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private GameBoardTile tile;



        public GameBoardTile Tile
        {
            get => tile;
            set
            {
                tile = value;
                CreateRender();
            }
        }

        private void Start()
        {
            if (offset == null)
            {
                offset = new Vector3(0f, 100f, 0f);
            }
        }
        private void CreateRender()
        {
            while (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0));
            }
            Instantiate(tile.Render, offset, Quaternion.identity, transform);
        }
    }
}