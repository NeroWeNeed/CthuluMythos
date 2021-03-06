using UnityEngine;

namespace CMythos
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Initializable))]
    public class GameBoardTileRenderer : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private GameBoardManager gameBoardManager;

        [SerializeField]
        private GameBoardTile tile;

        private bool initiated = false;

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
            if (gameBoardManager == null)
            {
                gameBoardManager = GetComponentInParent<GameBoardManager>();
            }

            gameBoardManager.TurnManager.TurnStartEvent.AddListener(UpdateRender);
            

        }

        public void UpdateRender(TurnManagable turnManagable)
        {
            Debug.Log("Updating!");
            GameBoardPlayer player = turnManagable.GetComponent<GameBoardPlayer>();
            if (player != null)
            {
                GameBoardTile tile = gameBoardManager.GetTile(player.GetComponent<GameBoardEntity>());
                if (tile != null)
                {
                    this.tile = tile;
                    CreateRender();
                }
            }
        }
        private void CreateRender()
        {
            for (int i=0;i<transform.childCount;i++) {
                
                Destroy(transform.GetChild(i).gameObject);
            }

            if (tile.Render != null)
                Instantiate(tile.Render, transform.position + offset, Quaternion.identity, transform);
        }
    }
}