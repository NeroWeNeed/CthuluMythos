using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CMythos
{

    [ExecuteInEditMode]
    public class GameBoardSetup : MonoBehaviour
    {

        private static int DiceCollisionLayer = -1;
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private DiceShooter diceShooter;

        private float distance = 0.04f;


        [SerializeField]
        private PlayMat playMat;

        [SerializeField]
        private DiceCollisionBox diceCollisionBox;

        [SerializeField]
        private GameBoardGround gameBoardGround;


        [SerializeField]
        private GameBoardTileRenderer gameBoardTileRenderer;

        // Start is called before the first frame update

        void Start()
        {
            if (diceCollisionBox == null)
            {
                DiceCollisionBox t = GetComponentInChildren<DiceCollisionBox>();
                if (t == null)
                {
                    GameObject obj = new GameObject("Dice Collision Box", typeof(DiceCollisionBox));
                    obj.transform.parent = transform;
                    diceCollisionBox = obj.GetComponent<DiceCollisionBox>();
                }
                else
                {
                    diceCollisionBox = t;
                }
            }
            if (gameBoardGround == null)
            {
                GameBoardGround g = GetComponentInChildren<GameBoardGround>();
                if (g == null)
                {

                    GameObject obj = new GameObject("Game Board Ground", typeof(GameBoardGround));
                    obj.transform.parent = transform;
                    gameBoardGround = obj.GetComponent<GameBoardGround>();
                }
                else
                {
                    gameBoardGround = g;
                }
            }
            if (gameBoardTileRenderer == null)
            {
                GameBoardTileRenderer g = GetComponentInChildren<GameBoardTileRenderer>();
                if (g == null)
                {

                    GameObject obj = new GameObject("Game Board Tile Renderer", typeof(GameBoardTileRenderer));
                    obj.transform.parent = transform;
                    gameBoardTileRenderer = obj.GetComponent<GameBoardTileRenderer>();
                }
                else
                {
                    gameBoardTileRenderer = g;
                }
            }

            Vector3 farLeft = camera.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, camera.farClipPlane * distance));
            Vector3 farCenterBase = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.0f, camera.farClipPlane * distance));
            Vector3 farCenter = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.farClipPlane * distance));
            Vector3 farRight = camera.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, camera.farClipPlane * distance));
            Vector3 farTop = camera.ViewportToWorldPoint(new Vector3(0.5f, 1.0f, camera.farClipPlane * distance));

            transform.position = new Vector3(farLeft.x, farCenterBase.y, farCenterBase.z);


            diceCollisionBox.Width = farRight.x - farLeft.x;
            diceCollisionBox.Height = farTop.y - farCenterBase.y;
            diceCollisionBox.Length = farCenter.z - camera.transform.position.z;
            diceCollisionBox.UpdateMesh();
            Debug.Log(farCenter);
            diceCollisionBox.transform.position = farCenterBase + new Vector3(-diceCollisionBox.Width / 2, 0, 0);
            diceShooter.transform.position = new Vector3(diceCollisionBox.transform.position.x + (diceCollisionBox.Width / 2), diceCollisionBox.transform.position.y + diceCollisionBox.Height, diceCollisionBox.transform.position.z);
            diceShooter.transform.LookAt( new Vector3(diceCollisionBox.transform.position.x + (diceCollisionBox.Width / 2), diceCollisionBox.transform.position.y + (diceCollisionBox.Height/2), diceCollisionBox.transform.position.z+(diceCollisionBox.Length/2)), Vector3.up);
            

            gameBoardGround.UpdateMesh(diceCollisionBox.Width, diceCollisionBox.Length);
            gameBoardGround.transform.position = farCenterBase + new Vector3(-diceCollisionBox.Width / 2, 0, 0);
            gameBoardTileRenderer.transform.position = farCenterBase + new Vector3(-diceCollisionBox.Width * 2, 0, 0);
        }




    }
}