using UnityEngine;
namespace CMythos
{
    public class Initializable : MonoBehaviour
    {
        public delegate void initiator();

        public initiator Initiator { get; set; }

        public void Init()
        {
            if (Initiator != null)
                Initiator.Invoke();
        }
    }
}