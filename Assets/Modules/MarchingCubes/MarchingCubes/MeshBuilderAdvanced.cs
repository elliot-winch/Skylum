using UnityEngine;
using UnityEngine.Rendering;

namespace MarchingCubes {

    //
    // Isosurface mesh builder with the marching cubes algorithm
    //
    sealed class MeshBuilderAdvanced : MeshBuilder, System.IDisposable
    {
        #region Public members

        public override Mesh Mesh => _mesh;

        public MeshBuilderAdvanced(Vector3Int size, int budget, ComputeShader compute) : base(size, budget, compute)
        {
            Initialize();
        }

        public override void Dispose()
          => ReleaseAll();

        public override void BuildIsosurface(ComputeBuffer voxels, float target, float scale)
          => RunCompute(voxels, target, scale);

        #endregion

        #region Private members

        void Initialize()
        {
            AllocateBuffers();
            AllocateMesh(3 * m_NumTriangles);
        }

        void ReleaseAll()
        {
            ReleaseBuffers();
            ReleaseMesh();
        }

        void RunCompute(ComputeBuffer voxels, float target, float scale)
        {
            _counterBuffer.SetCounterValue(0);

            // Isosurface reconstruction
            m_MarchingCubes.SetInts("Dims", m_Size);
            m_MarchingCubes.SetInt("MaxTriangle", m_NumTriangles);
            m_MarchingCubes.SetFloat("Scale", scale);
            m_MarchingCubes.SetFloat("Isovalue", target);
            m_MarchingCubes.SetBuffer(0, "TriangleTable", _triangleTable);
            m_MarchingCubes.SetBuffer(0, "Voxels", voxels);
            m_MarchingCubes.SetBuffer(0, "VertexBuffer", _vertexBuffer);
            m_MarchingCubes.SetBuffer(0, "IndexBuffer", _indexBuffer);
            m_MarchingCubes.SetBuffer(0, "Counter", _counterBuffer);
            m_MarchingCubes.DispatchThreads(0, m_Size);

            // Clear unused area of the buffers.
            m_MarchingCubes.SetBuffer(1, "VertexBuffer", _vertexBuffer);
            m_MarchingCubes.SetBuffer(1, "IndexBuffer", _indexBuffer);
            m_MarchingCubes.SetBuffer(1, "Counter", _counterBuffer);
            m_MarchingCubes.DispatchThreads(1, 1, 1, 1);

            _mesh.RecalculateBounds();
        }

        #endregion

        #region Compute buffer objects

        ComputeBuffer _triangleTable;
        ComputeBuffer _counterBuffer;

        void AllocateBuffers()
        {
            // Marching cubes triangle table
            _triangleTable = new ComputeBuffer(256, sizeof(ulong));
            _triangleTable.SetData(PrecalculatedData.TriangleTable);

            // Buffer for triangle counting
            _counterBuffer = new ComputeBuffer(1, 4, ComputeBufferType.Counter);
        }

        void ReleaseBuffers()
        {
            _triangleTable.Dispose();
            _counterBuffer.Dispose();
        }

        #endregion

        #region Mesh objects

        Mesh _mesh;
        GraphicsBuffer _vertexBuffer;
        GraphicsBuffer _indexBuffer;

        void AllocateMesh(int vertexCount)
        {
            _mesh = new Mesh();

            // We want GraphicsBuffer access as Raw (ByteAddress) buffers.
            _mesh.indexBufferTarget |= GraphicsBuffer.Target.Raw;
            _mesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;

            // Vertex position: float32 x 3
            var vp = new VertexAttributeDescriptor
              (VertexAttribute.Position, VertexAttributeFormat.Float32, 3);

            // Vertex normal: float32 x 3
            var vn = new VertexAttributeDescriptor
              (VertexAttribute.Normal, VertexAttributeFormat.Float32, 3);

            // Vertex/index buffer formats
            _mesh.SetVertexBufferParams(vertexCount, vp, vn);
            _mesh.SetIndexBufferParams(vertexCount, IndexFormat.UInt32);

            // Submesh initialization
            _mesh.SetSubMesh(0, new SubMeshDescriptor(0, vertexCount),
                             MeshUpdateFlags.DontRecalculateBounds);

            // GraphicsBuffer references
            _vertexBuffer = _mesh.GetVertexBuffer(0);
            _indexBuffer = _mesh.GetIndexBuffer();
        }

        void ReleaseMesh()
        {
            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
            Object.Destroy(_mesh);
        }

        #endregion
    }
} // namespace MarchingCubes
