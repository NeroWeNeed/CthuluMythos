using UnityEngine;
using System.Linq;

namespace CMythos
{
    [RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
    public class DiceCollisionBox : MonoBehaviour
    {
        private static int DiceCollisionLayer = -1;
        [SerializeField]
        float width;

        public float Width
        {
            get => width;
            set => width = value;
        }
        [SerializeField]
        float length;
        public float Length
        {
            get => length;
            set => length = value;
        }
        [SerializeField]
        float height;
        public float Height
        {
            get => height;
            set => height = value;
        }
        private Mesh mesh = null;
        private void Start()
        {
            if (DiceCollisionLayer == -1)
                DiceCollisionLayer = LayerMask.NameToLayer("DiceCollision");
            if (mesh == null)
            {
                mesh = new Mesh();
                UpdateMesh(mesh);
            }
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }
        public void UpdateMesh()
        {
            if (mesh == null)
                mesh = new Mesh();
            UpdateMesh(mesh);
            GetComponent<MeshCollider>().sharedMesh = mesh;
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }
        private void UpdateMesh(Mesh mesh)
        {


            mesh.vertices = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(width,0,0),
            new Vector3(width,height,0),
            new Vector3(0,height,0),
            new Vector3(0,height,length),
            new Vector3(width,height,length),
            new Vector3(width,0,length),
            new Vector3(0,0,length)
        };

            mesh.triangles = (new int[] {
        0, 2, 1, //face front
	    0, 3, 2,
        2, 3, 4, //face top
	    2, 4, 5,
        1, 2, 5, //face right
	    1, 5, 6,
        0, 7, 4, //face left
	    0, 4, 3,
        5, 4, 7, //face back
	    5, 7, 6,
        0, 6, 7, //face bottom
	    0, 1, 6
        }).Reverse().ToArray();

            mesh.RecalculateNormals();



        }
    }
}