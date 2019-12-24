using CMythos.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{
    [RequireComponent(typeof(Button))]
    public class PlayerViewUIAction : MonoBehaviour
    {
        [SerializeField]
        private PlayerUIActionEvent playerUIActionEvent;

        public PlayerUIActionEvent PlayerUIActionEvent
        {
            get => playerUIActionEvent;
        }
        [SerializeField]
        private bool shouldDisableAfterUse = true;

        [SerializeField]
        private DiceShooter diceShooter;

        [SerializeField]
        private GameBoardManager gameBoardManager;

        private PlayerViewUI playerViewUI;
        private bool available = true;
        private void Start()
        {
            bool diceShooterSet = diceShooter != null;
            bool gameBoardManagerSet = gameBoardManager != null;
            playerViewUI = GetComponentInParent<PlayerViewUI>();
            if (!diceShooterSet || !gameBoardManagerSet)
            {

                if (!diceShooterSet)
                {
                    diceShooter = playerViewUI.DiceShooter;
                }
                if (!gameBoardManagerSet)
                    gameBoardManager = playerViewUI.GameBoardManager;
            }
            if (playerUIActionEvent == null)
                playerUIActionEvent = new PlayerUIActionEvent();
            GetComponent<Button>().onClick.AddListener(PlayerUIAction);

        }
        private void PlayerUIAction()
        {
            if (available)
            {
                if (shouldDisableAfterUse)
                    available = false;
                playerUIActionEvent.Invoke(playerViewUI.CurrentPlayer);
            }
        }
        public void Refresh()
        {
            available = true;
        }




    }
}