using UnityEngine;
using UnityEngine.UI;

namespace CMythos
{


    [RequireComponent(typeof(RawImage))]
    public class PlayMatPileRenderer : MonoBehaviour
    {
        public bool IsDiscardPile { get; set; } = false;


        [SerializeField]
        private CardType pileType;

        public CardType PileType
        {
            get => pileType;
            set
            {
                pileType = value;
            }
        }

        private RawImage rawImage;
        private bool initialized = false;
        private void Awake() {
            Init();
        }
        private void Init()
        {
            initialized = true;
            rawImage = GetComponent<RawImage>();
            rawImage.rectTransform.sizeDelta = new Vector2(Card.CARD_WIDTH, Card.CARD_HEIGHT);

        }

        public void Refresh(PlayMat playMat)
        {
            
            if (!initialized)
                Init();

            rawImage.texture = GetPileTopCard(playMat);
            

        }
        private PlayMatPile GetPile(PlayMat playMat)
        {

            if (IsDiscardPile)
                return playMat?.GetDiscardPile(pileType);
            else
                return playMat?.GetPile(pileType);
        }
        private Texture2D GetPileTopCard(PlayMat playMat)
        {
            
            PlayMatPile pile = GetPile(playMat);
            if (pile == null)
                return null;
            else
                return pile.GetTopCard()?.graphic;
        }


    }
}