using UnityEngine;
using System;
using System.Linq;

namespace CMythos
{
    [RequireComponent(typeof(Canvas))]
    public class PlayMatRenderer : MonoBehaviour
    {


        [SerializeField]
        private PlayMat playMat;
        [SerializeField]
        private float pileSpacing = 5.0f;

        [SerializeField]
        private float pileHorizontalMargin = 10.0f;

        [SerializeField]
        private float pileVerticalMargin = 10.0f;

        [SerializeField]
        private Texture2D texture;

        private void Start()
        {
            Init(playMat);

        }
        public void Init(PlayMat playMat)
        {
            this.playMat = playMat;
            PlayMatPileRenderer[] renderers = GetComponentsInChildren<PlayMatPileRenderer>();
            GameObject obj;
            GameObject obj2;
            PlayMatPileRenderer pileRenderer, discardPileRenderer;
            float lastX = 0.0f;
            var index = 0;
            foreach (var item in Enum.GetValues(typeof(CardType)).Cast<CardType>())
            {
                if (renderers.All(x => x.Pile != item))
                {
                    obj = new GameObject($"{item} Pile Renderer", typeof(PlayMatPileRenderer));
                    obj2 = new GameObject($"{item} Discard Pile Renderer", typeof(PlayMatPileRenderer));
                    pileRenderer = obj.GetComponent<PlayMatPileRenderer>();
                    discardPileRenderer = obj2.GetComponent<PlayMatPileRenderer>();
                    pileRenderer.Pile = item;
                    pileRenderer.PlayMat = playMat;
                    discardPileRenderer.Pile = item;
                    discardPileRenderer.PlayMat = playMat;
                    discardPileRenderer.IsDiscardPile = true;
                    obj.transform.SetParent(transform, false);
                    obj2.transform.SetParent(transform, false);

                    if (index == 0)
                    {
                        obj.transform.position = new Vector3(pileHorizontalMargin, 0, 0);
                        obj2.transform.position = new Vector3(pileHorizontalMargin, Card.CARD_HEIGHT + 20, 0);
                        lastX += pileHorizontalMargin;

                    }
                    else
                    {
                        obj.transform.position = new Vector3(Card.CARD_WIDTH + pileSpacing + lastX, 0, 0);
                        obj2.transform.position = new Vector3(Card.CARD_WIDTH + pileSpacing + lastX, Card.CARD_HEIGHT + 20, 0);
                        lastX += Card.CARD_WIDTH + pileSpacing;
                    }


                }
                index++;

            }
        }

        public void Refresh()
        {
            foreach (var item in GetComponentsInChildren<PlayMatPileRenderer>())
            {
                item.Refresh();
            }
        }
    }
}

