using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{


    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;

        [SerializeField]
        private GameBoardManager gameBoardManager;

        private void Start()
        {

            startButton.onClick.AddListener(() =>
            {
                gameBoardManager.PlayMatRenderer.gameObject.SetActive(true);
                gameBoardManager.PlayerViewUI.gameObject.SetActive(true);
                gameObject.SetActive(false);
                gameBoardManager.StartGame(4);



            });
        }
    }
}