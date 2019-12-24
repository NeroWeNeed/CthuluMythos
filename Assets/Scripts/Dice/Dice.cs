using UnityEngine;
using System.Collections.Generic;
using System;
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

        public bool IsStable()
        {

            return rigidbody.IsSleeping();
        }
        public string GetValue()
        {
            if (IsStable())
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


        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (DiceCollisionLayer == -1)
                DiceCollisionLayer = LayerMask.NameToLayer("DiceCollision");
            Debug.Log($"Created at {transform.position}");
        }
        public void OnAfterDeserialize()
        {
            values = new Dictionary<Vector3, string>();
            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                values.Add(_keys[i], _values[i]);
        }
        private void FixedUpdate()
        {

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

            if (diceBoxCollider.GetComponent<BoxCollider>().bounds.Contains(transform.position))
            {
                Debug.Log("Inside");
                gameObject.layer = DiceCollisionLayer;
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