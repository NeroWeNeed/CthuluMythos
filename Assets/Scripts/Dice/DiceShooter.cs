using UnityEngine;
using UnityEditor;
using CMythos.Events;
using System.Collections.Generic;
using System.Linq;
namespace CMythos
{
    [RequireComponent(typeof(BoxCollider))]
    public class DiceShooter : MonoBehaviour
    {
        public delegate void DiceValueCallBack(Dictionary<Dice, string> values);
        [SerializeField]
        private DiceStablizedEvent diceStablized;
        public DiceStablizedEvent DiceStabilized
        {
            get => diceStablized;
        }
        [SerializeField]
        private DiceDestablizedEvent diceDestablized;
        public DiceDestablizedEvent DiceDestabilized
        {
            get => diceDestablized;
        }
        [SerializeField]
        private DiceRollingEvent diceRolling;
        public DiceRollingEvent DiceRolling
        {
            get => diceRolling;
        }
        [SerializeField]
        public Vector2 spawnFuzziness = new Vector2(100, 100);

        [SerializeField]
        public float spawnRadius = 10.0f;

        [SerializeField]
        private float force = 100.0f;


        [SerializeField]
        public float forceFuzziness = 0.2f;


        private List<ShootInfo> shootInfos;

        private Vector3 offset;
        public Vector3 Offset
        {
            get => offset;
        }
        private void UpdateShootInfos(Dice dice, string value)
        {
            Debug.Log("Updating... " + value);
            ShootInfo shootInfo;
            for (int i = 0; i < shootInfos.Count; i++)
            {
                shootInfo = shootInfos[i];
                if (shootInfo.ContainsDice(dice))
                {
                    shootInfo.Update(dice, value);
                    if (shootInfo.IsCompleted())
                    {
                        shootInfos.RemoveAt(i);
                        shootInfo.callBack.Invoke(shootInfo.values);
                    }
                }
            }
        }
        public void Shoot(float force, bool create, params Dice[] dice)
        {
            Dice newDice;

            Rigidbody body;
            foreach (var d in dice)
            {
                newDice = SpawnDice(d, create);
                body = newDice.GetComponent<Rigidbody>();
                body.AddTorque(transform.up + (new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f))));
                Vector3 force2 = transform.forward * (force * Random.Range(1.0f - forceFuzziness, 1.0f + forceFuzziness));
                force2.Normalize();
                body.AddForce(force2);


            }
        }
        private Dice SpawnDice(Dice basis, bool create)
        {

            Dice newDice;
            if (create)
            {
                Vector3 vect = transform.position + (transform.forward * 2);
                Debug.Log(vect);
                Debug.Log(offset);

                newDice = Instantiate(basis, vect,

                Quaternion.FromToRotation(-vect, new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f))), transform);
                newDice.DiceBoxCollider = boxCollider;
            }
            else
                newDice = basis;
            return newDice;
        }
        public void Shoot(DiceValueCallBack callBack, params Dice[] dice)
        {
            Shoot(force, callBack, dice);
        }
        public void Shoot(float force, DiceValueCallBack callBack, params Dice[] dice)
        {
            Dice[] newDice = new Dice[dice.Length];
            for (int i = 0; i < newDice.Length; i++)
            {
                newDice[i] = SpawnDice(dice[i], true);
            }
            ShootInfo shootInfo = new ShootInfo
            {
                dice = newDice,
                callBack = callBack
            };
            shootInfos.Add(shootInfo);
            Shoot(force, false, newDice);
        }


        private BoxCollider boxCollider;
        private void Start()
        {
            if (diceRolling == null)
                diceRolling = new DiceRollingEvent();
            if (diceStablized == null)
                diceStablized = new DiceStablizedEvent();
            if (diceDestablized == null)
                diceDestablized = new DiceDestablizedEvent();
            shootInfos = new List<ShootInfo>();
            diceStablized.AddListener(UpdateShootInfos);
            boxCollider = GetComponent<BoxCollider>();
            offset = boxCollider.size / 2;

        }

        public class ShootInfo
        {
            public Dice[] dice;

            public DiceValueCallBack callBack;
            public Dictionary<Dice, string> values = new Dictionary<Dice, string>();

            public bool IsCompleted()
            {
                return dice.All(x => values.ContainsKey(x));
            }
            public bool ContainsDice(Dice dice)
            {
                return this.dice.Contains(dice);
            }
            public void Update(Dice dice, string value)
            {
                values[dice] = value;
            }
        }

    }
}