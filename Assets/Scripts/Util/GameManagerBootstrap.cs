namespace CMythos
{
    using UnityEngine;

    public class GameManagerBootstrap : MonoBehaviour
    {
        [SerializeField]
        private GameBoardManager gameBoardManager;
        private void Start()
        {
            
            gameBoardManager.StartGame(GameConfiguration.investigators.ToArray());
        }
    }
}