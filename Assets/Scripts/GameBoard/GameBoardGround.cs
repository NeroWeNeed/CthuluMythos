using UnityEngine;

namespace CMythos
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GameBoardGround : MonoBehaviour
    {
        private Mesh mesh;

        [SerializeField]
        private Vector2 size;
        public float Width
        {
            get => size.x;
        }
        public float Length
        {
            get => size.y;
        }
        private void Start()
        {
            if (size == null)
                size = new Vector2(200.0f, 100.0f);
            if (mesh == null)
                mesh = new Mesh();
            UpdateMesh();
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }
        private void Awake()
        {
            if (size == null)
                size = new Vector2(200.0f, 100.0f);
            if (mesh == null)
                mesh = new Mesh();
            UpdateMesh();
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }
        public void UpdateMesh(float width = -1.0f, float length = -1.0f)
        {
            if (width < 0)
                width = Width;
            else
                size.x = width;

            if (length < 0)
                length = Length;
            else
                size.y = length;
            if (mesh != null)
                UpdateMesh(mesh, width, length);
        }
        private void UpdateMesh(Mesh mesh, float width, float length)
        {
            mesh.vertices = new Vector3[] {
                new Vector3(0,0,0),
                new Vector3(width,0,0),
                new Vector3(0,0,length),
                new Vector3(width,0,length)
            };
            mesh.triangles = new int[6]
            {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
            };
            mesh.normals = new Vector3[] {
                Vector3.up,
                Vector3.up,
                Vector3.up,
                Vector3.up
            };
            mesh.uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

        }
    }
}
