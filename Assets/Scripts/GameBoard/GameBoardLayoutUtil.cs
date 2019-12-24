using UnityEngine;

namespace CMythos
{
    [ExecuteInEditMode]
    public class GameBoardLayoutUtil : MonoBehaviour
    {
        [SerializeField]
        private GameBoardManager gameBoardManager;


        private DiceShooter diceShooter;

        [SerializeField]
        private Camera camera;
        [SerializeField]
        private PlayMatRenderer playMatRenderer;

        private float distance = 0.01f;

        private float renderDistance = 0.4f;

        private GameBoardGround gameBoardGround;

        private DiceCollisionBox diceCollisionBox;

        private GameBoardTileRenderer gameBoardTileRenderer;

        private void OnGUI()
        {
            if (camera != null && gameBoardManager != null)
            {
                gameBoardGround = gameBoardManager.GetComponentInChildren<GameBoardGround>();
                diceCollisionBox = gameBoardManager.GetComponentInChildren<DiceCollisionBox>();
                diceShooter = gameBoardManager.GetComponentInChildren<DiceShooter>();
                gameBoardTileRenderer = gameBoardManager.GetComponentInChildren<GameBoardTileRenderer>();

                Vector3 topLeft = camera.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, camera.farClipPlane * distance));
                Vector3 topMid = camera.ViewportToWorldPoint(new Vector3(0.5f, 1.0f, camera.farClipPlane * distance));
                Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, camera.farClipPlane * distance));

                Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, camera.farClipPlane * distance));
                Vector3 bottomMid = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.0f, camera.farClipPlane * distance));
                Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, camera.farClipPlane * distance));

                Vector3 centerLeft = camera.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, camera.farClipPlane * distance));
                Vector3 centerMid = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.farClipPlane * distance));
                Vector3 centerRight = camera.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, camera.farClipPlane * distance));

                Vector3 cameraPosition = camera.transform.position;
                Vector3 cameraDirection = camera.transform.forward;
                LayoutDiceCollisionBox(diceCollisionBox, centerRight, centerLeft, topMid, bottomMid, centerMid, cameraPosition);
                LayoutGameBoardGround(gameBoardGround, diceCollisionBox, bottomMid);
                LayoutGameBoardTileRender(gameBoardTileRenderer, gameBoardGround, camera);
                LayoutDiceShooter(diceShooter, diceCollisionBox);

                LayoutPlayMatPileRenderer(playMatRenderer, gameBoardGround);
            }
        }
        private void LayoutDiceCollisionBox(DiceCollisionBox diceCollisionBox, Vector3 centerRight, Vector3 centerLeft, Vector3 topMid, Vector3 bottomMid, Vector3 center, Vector3 cameraPosition)
        {
            diceCollisionBox.Width = centerRight.x - centerLeft.x;
            diceCollisionBox.Height = topMid.y - bottomMid.y;
            diceCollisionBox.Length = center.z - cameraPosition.z;
            diceCollisionBox.UpdateMesh();
            diceCollisionBox.transform.position = bottomMid + new Vector3(-diceCollisionBox.Width / 2, 0, 0);
        }
        private void LayoutGameBoardGround(GameBoardGround gameBoardGround, DiceCollisionBox diceCollisionBox, Vector3 bottomMid)
        {
            gameBoardGround.UpdateMesh(diceCollisionBox.Width, diceCollisionBox.Length);
            gameBoardGround.transform.position = bottomMid + new Vector3(-diceCollisionBox.Width / 2, 0, 0);
        }
        private void LayoutGameBoardTileRender(GameBoardTileRenderer gameBoardTileRenderer, GameBoardGround gameBoardGround, Camera camera)
        {
            Vector3 cameraPosition = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.farClipPlane * (distance / renderDistance)));
            gameBoardTileRenderer.transform.position = new Vector3(cameraPosition.x, gameBoardGround.transform.position.y, cameraPosition.z);

        }
        private void LayoutDiceShooter(DiceShooter diceShooter, DiceCollisionBox diceCollisionBox)
        {
            diceShooter.transform.position = new Vector3(diceCollisionBox.transform.position.x + (diceCollisionBox.Width / 2), diceCollisionBox.transform.position.y + diceCollisionBox.Height, diceCollisionBox.transform.position.z);
            diceShooter.transform.LookAt(new Vector3(diceCollisionBox.transform.position.x + (diceCollisionBox.Width / 2), diceCollisionBox.transform.position.y + (diceCollisionBox.Height / 2), diceCollisionBox.transform.position.z + (diceCollisionBox.Length / 2)), Vector3.up);
        }
        private void LayoutPlayMatPileRenderer(PlayMatRenderer playMatRenderer, GameBoardGround gameBoardGround)
        {
            playMatRenderer.transform.position = new Vector3(gameBoardGround.transform.position.x+(gameBoardGround.Width/2),gameBoardGround.transform.position.y+0.01f,gameBoardGround.transform.position.z+(gameBoardGround.Length/2));
            //playMatRenderer.transform.position = gameBoardGround.transform.position;
            playMatRenderer.transform.rotation = Quaternion.Euler(90.0f, 0, 0);
            playMatRenderer.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
        }

    }
}
