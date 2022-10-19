using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace MarchingCubes {

    //
    // Isosurface mesh builder with the marching cubes algorithm
    //
    sealed class MeshBuilderStandard : MeshBuilder, IDisposable
    {
        public override Mesh Mesh => _mesh;

        public MeshBuilderStandard(Vector3Int size, int budget, ComputeShader compute) : base(size, budget, compute)
        {
            AllocateBuffers();
        }

        public override void Dispose()
          => ReleaseAll();

        public override void BuildIsosurface(ComputeBuffer voxels, float target, float scale)
          => RunCompute(voxels, target, scale);


        #region Private members


        ComputeBuffer m_VertexBuffer;
        ComputeBuffer m_NormalBuffer;

        ComputeBuffer _triangleTable;
        ComputeBuffer _counterBuffer;

        Mesh _mesh;

        void ReleaseAll()
        {
            ReleaseBuffers();
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
            m_MarchingCubes.SetBuffer(0, "VertexBuffer", m_VertexBuffer);
            m_MarchingCubes.SetBuffer(0, "NormalBuffer", m_NormalBuffer);
            m_MarchingCubes.SetBuffer(0, "Counter", _counterBuffer);
            m_MarchingCubes.DispatchThreads(0, m_Size);

            // Clear unused area of the buffers.
            m_MarchingCubes.SetBuffer(1, "VertexBuffer", m_VertexBuffer);
            m_MarchingCubes.SetBuffer(1, "NormalBuffer", m_NormalBuffer);
            m_MarchingCubes.SetBuffer(1, "Counter", _counterBuffer);
            m_MarchingCubes.DispatchThreads(1, 1, 1, 1);

            BuildMesh(scale);
        }

        private void BuildMesh(float scale)
        {
            if(_mesh == null)
            {
                _mesh = new Mesh
                {
                    indexFormat = IndexFormat.UInt32
                };
            }

            _mesh.Clear();

            Vector3[] verts = new Vector3[m_NumTriangles * 3];
            m_VertexBuffer.GetData(verts);

            Vector3[] normals = new Vector3[m_NumTriangles * 3];
            m_NormalBuffer.GetData(normals);

            _mesh.vertices = verts;
            _mesh.normals = normals;
            _mesh.triangles = Enumerable.Range(0, m_NumTriangles * 3).ToArray();

            //_mesh.RecalculateNormals();

            _mesh.RecalculateBounds();
        }

        #endregion

        #region Compute buffer objects

        void AllocateBuffers()
        {
            // Marching cubes triangle table
            _triangleTable = new ComputeBuffer(256, sizeof(ulong));
            _triangleTable.SetData(PrecalculatedData.TriangleTable);

            m_VertexBuffer = new ComputeBuffer(m_NumTriangles * 3, sizeof(float) * 3);
            m_NormalBuffer = new ComputeBuffer(m_NumTriangles * 3, sizeof(float) * 3);

            // Buffer for triangle counting
            _counterBuffer = new ComputeBuffer(1, 4, ComputeBufferType.Counter);
        }

        void ReleaseBuffers()
        {
            _triangleTable.Dispose();
            _counterBuffer.Dispose();

            m_VertexBuffer.Dispose();
            m_NormalBuffer.Dispose();
        }
        #endregion
    }
} // namespace MarchingCubes
