using UnityEngine;
namespace CMythos
{

    public class DirectionSelector : MonoBehaviour
    {
        [SerializeField]
        private GameBoardEntityRelativeDirection direction;

        public GameBoardEntityRelativeDirection Direction { get => direction; }

    }
}