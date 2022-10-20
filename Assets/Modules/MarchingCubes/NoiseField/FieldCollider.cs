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
            Initialise();
            m_FieldBuilder.GeneratePoints();
            m_FieldBuilder.Field.Subscribe(UpdateMesh);
        }

        private void UpdateMesh()
        {
            _builder.BuildIsosurface(m_FieldBuilder.VoxelBuffer, m_isoValue, m_FieldBuilder.GridScale);

            m_MeshCollider.sharedMesh = _builder.Mesh;
        }

        private void Initialise()
        {
            if (m_MeshCollider == null)
            {
                m_MeshCollider = GetComponent<MeshCollider>();
            }

            if (_builder == null)
            {
                _builder = new MeshBuilderStandard(m_FieldBuilder.Dimensions, m_TriangleBudget, _builderCompute);
            }
        }
    }
} // namespace MarchingCubes
