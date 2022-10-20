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
            Initialise();
            m_FieldBuilder.GeneratePoints();
            m_FieldBuilder.Field.Subscribe(UpdateMesh);
        }

        public void UpdateMesh()
        {
            // Isosurface reconstruction
            _builder.BuildIsosurface(m_FieldBuilder.VoxelBuffer, m_isoValue, m_FieldBuilder.GridScale);

            m_MeshFilter.sharedMesh = _builder.Mesh;
        }

        private void Initialise()
        {
            if(m_MeshFilter == null)
            {
                m_MeshFilter = GetComponent<MeshFilter>();
            }

            if(_builder == null)
            {
                switch (m_Mode)
                {
                    case MeshBuilderMode.Standard:
                        _builder = new MeshBuilderStandard(m_FieldBuilder.Dimensions, m_TriangleBudget, _builderCompute);
                        break;
                    case MeshBuilderMode.Advanced:
                        _builder = new MeshBuilderAdvanced(m_FieldBuilder.Dimensions, m_TriangleBudget, _builderCompute);
                        break;
                }
            }
        }
    }
} // namespace MarchingCubes
