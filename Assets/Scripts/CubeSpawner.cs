using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject prefabCubeHighlight;
    public GameObject prefabCube;
    public GameObject prefabBase;
    public CinemachineFreeLook cinemachineVirtualCamera;

    private GameObject cubeCurrent;
    private static readonly List<Vector3> allDirection = new()
    {
        Vector3.up,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
    private Vector3 v3Connection;
    private Vector3 v3CurrentNormal;
    private GameObject cubeSpawn;
    private bool spawning = true;
    private IEnumerator<Vector3> itFaces;
    private readonly float offset = 1f;
    private GameState gameState;
    private Vector3 v3Start = new(0, 0.5f, 0);
    private readonly List<GameObject> cubes = new(); 
    private bool isSpawningInvoked = false;

    void Start()
    {
        // first connection is connected in down direction
        v3Connection = Vector3.down;
        itFaces = CreateFaceIterator(v3Connection).GetEnumerator();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessKeySpace();
        }
        
        // updating spawn cube based on previous one
        if (cubeCurrent != null && cubeSpawn != null)
        {
            // Assuming both cubes are of equal size
            float cubeSize = cubeCurrent.transform.localScale.y;

            // Calculate the world-space direction of the connection face normal taking Cube A's rotation into account
            Vector3 worldSpaceNormal = cubeCurrent.transform.rotation * v3CurrentNormal;

            // Calculate the position for Cube B
            // The position of Cube B is determined by the position of Cube A plus the offset calculated from the connection face normal in world space
            Vector3 offset = worldSpaceNormal * cubeSize;
            
            // Align Cube B's rotation with Cube A's rotation
            cubeSpawn.transform.SetPositionAndRotation(cubeCurrent.transform.position + offset, cubeCurrent.transform.rotation);
        }
    }

    public void Init(GameState gameState)
    {
        this.gameState = gameState;

        // remove previous instances if possible
        RestartLevel();

        // create first two cubes
        GameObject firstCube = SpawnCube(prefabBase, v3Start, Quaternion.identity);
        cubeCurrent = SpawnCube(prefabCube, v3Start + Vector3.up, Quaternion.identity);

        FixedJoint fixedJoint = cubeCurrent.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = firstCube.GetComponent<Rigidbody>();

        CenterCamera();
        StartSpawningCube();
    }

    private void SpawningNextBlock() {
        if (!spawning) {
            return;
        }

        itFaces.MoveNext();
        v3CurrentNormal = itFaces.Current;

        Vector3 spawnPosition = cubeCurrent.transform.position + v3CurrentNormal * offset;

        if (cubeSpawn != null) {
            Destroy(cubeSpawn);
        }

        cubeSpawn = SpawnCube(prefabCubeHighlight, spawnPosition, Quaternion.identity);
    }

    private void ProcessKeySpace()
    {
        spawning = false;

        Vector3 spawnPosition = cubeCurrent.transform.position + v3CurrentNormal * offset;
        GameObject cubeNew = SpawnCube(prefabCube, spawnPosition, Quaternion.identity);

        FixedJoint fixedJoint = cubeNew.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = cubeCurrent.GetComponent<Rigidbody>();

        Rigidbody rigidBodyCube = cubeNew.GetComponent<Rigidbody>();
        rigidBodyCube.useGravity = true;
        rigidBodyCube.mass = gameState.massCube;

        cubeCurrent = cubeNew;

        v3Connection = -v3CurrentNormal;
        itFaces = CreateFaceIterator(v3Connection).GetEnumerator();

        CenterCamera();

        spawning = true;
    }
    
    private GameObject SpawnCube(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject newObject = Instantiate(prefab, position, rotation) as GameObject;
        cubes.Add(newObject);
        return newObject;
    }

    private void RestartLevel(){
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
    }

    private void CenterCamera()
    {
        cinemachineVirtualCamera.Follow = cubeCurrent.transform;
        cinemachineVirtualCamera.LookAt = cubeCurrent.transform;
    }

    private void StartSpawningCube()
    {
        if (!isSpawningInvoked) {
            InvokeRepeating("SpawningNextBlock", 0, gameState.speedSpawn);
            isSpawningInvoked = true;
        }
    }

    private IEnumerable<Vector3> CreateFaceIterator(Vector3 connection)
    {
        List<Vector3> dir = allDirection.Where(v => v != connection).ToList();

        if (this.gameState.randomized)
        {
            dir = dir.OrderBy(a => new System.Random().Next()).ToList();
        }

        int index = 0;
        int count = dir.Count;
        while (true)
        {
            yield return dir[index % count];
            index++;
        }
    }
}
