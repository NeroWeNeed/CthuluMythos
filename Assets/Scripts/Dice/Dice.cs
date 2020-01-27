using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
namespace CMythos
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(Rigidbody))]
    public class Dice : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static int DiceCollisionLayer = -1;
        [SerializeField]
        private List<Vector3> _keys = new List<Vector3>();
        [SerializeField]
        private List<string> _values = new List<string>();

        public Dictionary<Vector3, string> values = new Dictionary<Vector3, string>();
        [SerializeField]
        public Vector3 matchDirection = Vector3.up;

        [SerializeField]
        private BoxCollider diceBoxCollider;

        public BoxCollider DiceBoxCollider
        {
            get => diceBoxCollider;
            set => diceBoxCollider = value;
        }
        private float timeout = 50;



        public Vector3 MatchDirection
        {
            get => matchDirection;
        }
        private Rigidbody rigidbody;
        private bool stabilized = false;
        private bool forceStabilized = false;

        public bool IsStable()
        {

            return rigidbody.IsSleeping() || forceStabilized;
        }
        public string GetValue(bool force = false)
        {
            if (IsStable() || force)
            {

                foreach (var kvp in values)
                {
                    if (Vector3.Distance(transform.rotation * kvp.Key, matchDirection) < 0.001f)
                        return kvp.Value;
                }
                return null;

            }
            else
                return null;
        }
        public string ApproximateValue()
        {

            return values.Select(kvp => Tuple.Create(kvp, Vector3.Distance(transform.rotation * kvp.Key, matchDirection))).Aggregate((acc, tuple) => acc.Item2 < tuple.Item2 ? acc : tuple).Item1.Value;


        }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        public void OnAfterDeserialize()
        {
            values = new Dictionary<Vector3, string>();
            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                values.Add(_keys[i], _values[i]);
        }

        int maxTime = 10;
        int count = 0;
        private void FixedUpdate()
        {
            if (count >= maxTime / Time.fixedDeltaTime)
            {
                forceStabilized = true;
                foreach (var d in GetComponentsInParent<DiceShooter>())
                {

                    d.DiceStabilized.Invoke(this, GetValue(true));
                }
                stabilized = true;
                return;
            }
            else
            {
                count++;
            }
            if (stabilized && !IsStable())
            {
                foreach (var d in GetComponentsInParent<DiceShooter>())
                {
                    d.DiceDestabilized.Invoke(this);
                }
                stabilized = false;
            }
            else if (!stabilized && IsStable())
            {
                foreach (var d in GetComponentsInParent<DiceShooter>())
                {
                    d.DiceStabilized.Invoke(this, GetValue());
                }
                stabilized = true;
            }
            else if (!stabilized)
            {
                foreach (var d in GetComponentsInParent<DiceShooter>())
                {
                    d.DiceRolling.Invoke(this);
                }
            }



        }
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (var face in values)
            {
                _keys.Add(face.Key);
                _values.Add(face.Value);
            }
        }
    }
}