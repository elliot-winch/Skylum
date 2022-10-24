using System.Linq;
using UnityEngine;

namespace MarchingCubes
{
    [RequireComponent(typeof(MeshCollider))]
    public class FieldCollider : MonoBehaviour
    {
        [SerializeField]
        private FieldBuilder m_FieldBuilder;
        [SerializeField]
        private ComputeShader _builderCompute = null;
        [Range(0, 21778)]
        [SerializeField]
        private int m_TriangleBudget = 2048;
        [SerializeField]
        private float m_isoValue = 0;

        private MeshBuilderStandard _builder;

        private MeshCollider m_MeshCollider;

        void OnDestroy()
        {
            if (_builder != null)
            {
                _builder.Dispose();
            }
        }

        private void Start()
        {
            if (m_MeshCollider == null)
            {
                m_MeshCollider = GetComponent<MeshCollider>();
            }

            m_FieldBuilder.GeneratePoints();
            m_FieldBuilder.Dimensions.Subscribe(CreateBuilder);
            m_FieldBuilder.Field.Subscribe(UpdateMesh);
        }

        private void UpdateMesh()
        {
            _builder.BuildIsosurface(m_FieldBuilder.VoxelBuffer, m_isoValue, m_FieldBuilder.GridScale);

            Mesh mesh = _builder.Mesh;

            if(mesh.vertices.Distinct().Count() < 3)
            {
                mesh = null;
            }

            m_MeshCollider.sharedMesh = mesh;
        }

        private void CreateBuilder(Vector3Int dimensions)
        {
            if (_builder != null)
            {
                _builder.Dispose();
            }

            _builder = new MeshBuilderStandard(dimensions, m_TriangleBudget, _builderCompute);
        }
    }
}
