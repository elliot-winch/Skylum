using UnityEngine;

namespace MarchingCubes 
{
    [RequireComponent(typeof(MeshFilter))]
    public class FieldMesh : MonoBehaviour
    {
        private enum MeshBuilderMode
        {
            Standard,
            Advanced
        }

        [SerializeField]
        private MeshBuilderMode m_Mode;

        [SerializeField]
        private FieldBuilder m_FieldBuilder;
        [SerializeField] 
        private ComputeShader _builderCompute = null;
        [Range(0, 21778)]
        [SerializeField]
        private int m_TriangleBudget = 2048;
        [SerializeField]
        private float m_isoValue = 0;

        private MeshBuilder _builder;

        private MeshFilter m_MeshFilter;

        void OnDestroy()
        {
            if(_builder != null)
            {
                _builder.Dispose();
            }
        }

        private void Start()
        {
            if (m_MeshFilter == null)
            {
                m_MeshFilter = GetComponent<MeshFilter>();
            }

            m_FieldBuilder.GeneratePoints(); //TODO: consider this. It's here to prevent 0 length buffers but does so imperfectly
            m_FieldBuilder.Dimensions.Subscribe(CreateBuilder);
            m_FieldBuilder.Field.Subscribe(UpdateMesh);

        }

        public void UpdateMesh()
        {
            // Isosurface reconstruction
            _builder.BuildIsosurface(m_FieldBuilder.VoxelBuffer, m_isoValue, m_FieldBuilder.GridScale);

            m_MeshFilter.sharedMesh = _builder.Mesh;
        }

        private void CreateBuilder(Vector3Int dimensions)
        {
            if (_builder != null)
            {
                _builder.Dispose();
            }

            switch (m_Mode)
            {
                case MeshBuilderMode.Standard:
                    _builder = new MeshBuilderStandard(dimensions, m_TriangleBudget, _builderCompute);
                    break;
                case MeshBuilderMode.Advanced:
                    _builder = new MeshBuilderAdvanced(dimensions, m_TriangleBudget, _builderCompute);
                    break;
            }
        }
    }
} // namespace MarchingCubes
