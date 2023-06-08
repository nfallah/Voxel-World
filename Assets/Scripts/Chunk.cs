using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private const int chunkSize = 8;
    private enum Side { LEFT, RIGHT, DOWN, UP, BACK, FORWARD }

    [SerializeField] Material textureAtlas;

    private readonly int[] triangles = new int[] { 0, 1, 3, 3, 1, 2 };

    private void Start()
    {
        GenerateChunk();
    }

    private void GenerateChunk()
    {
        for (int z = 0; z < chunkSize; z++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    GameObject block = new GameObject("Cube");

                    block.transform.parent = transform;
                    block.transform.localPosition = new Vector3(x, y, z);
                    GenerateCube(block);
                }
            }
        }

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshFilter[] childMeshes = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstance = new CombineInstance[childMeshes.Length];
        int childIndex = 0;

        foreach (MeshFilter childMeshFilter in childMeshes)
        {
            combineInstance[childIndex].mesh = childMeshFilter.sharedMesh;
            combineInstance[childIndex].transform = childMeshFilter.transform.localToWorldMatrix * transform.worldToLocalMatrix;
            childIndex++;
        }

        mesh.CombineMeshes(combineInstance);
        meshFilter.sharedMesh = mesh;
        meshRenderer.material = textureAtlas;

        foreach (Transform child in transform) // Can this be combined with the above loop?
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateCube(GameObject block)
    {
        GenerateQuad(Side.LEFT, block);
        GenerateQuad(Side.RIGHT, block);
        GenerateQuad(Side.DOWN, block);
        GenerateQuad(Side.UP, block);
        GenerateQuad(Side.BACK, block);
        GenerateQuad(Side.FORWARD, block);

        MeshFilter meshFilter = block.AddComponent<MeshFilter>();
        // MeshRenderer meshRenderer = block.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshFilter[] childMeshes = block.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstance = new CombineInstance[childMeshes.Length];
        int childIndex = 0;

        foreach (MeshFilter childMeshFilter in childMeshes)
        {
            combineInstance[childIndex].mesh = childMeshFilter.sharedMesh;
            combineInstance[childIndex].transform = childMeshFilter.transform.localToWorldMatrix * block.transform.worldToLocalMatrix;
            childIndex++;
        }

        mesh.CombineMeshes(combineInstance);
        meshFilter.sharedMesh = mesh;
        // meshRenderer.material = textureAtlas;

        foreach (Transform child in block.transform) // Can this be combined with the above loop?
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateQuad(Side side, GameObject block)
    {
        GameObject quad = new GameObject(side.ToString());

        quad.transform.parent = block.transform;
        quad.transform.localPosition = Vector3.zero;

        MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        mesh.vertices = GetVertices(side);
        mesh.triangles = triangles;
        mesh.uv = TextureAtlas.GetUVCoordinates(new Vector2Int(1, 0));
        meshFilter.sharedMesh = mesh;
    }
    
    private Vector3[] GetVertices(Side side)
    {
        switch (side)
        {
            case Side.LEFT:
                return new Vector3[]
                {
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f)
                };
            case Side.RIGHT:
                return new Vector3[]
                {
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f)
                };
            case Side.DOWN:
                return new Vector3[]
                {
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f)
                };
            case Side.UP:
                return new Vector3[]
                {
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f)
                };
            case Side.BACK:
                return new Vector3[]
                {
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f)
                };
            case Side.FORWARD:
                return new Vector3[]
                {
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f)
                };
            default:
                throw new Exception("Failed to return vertices");
        }
    }
}