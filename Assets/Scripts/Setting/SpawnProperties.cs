using UnityEngine;

public class SpawnProperties : MonoBehaviour
{
    [SerializeField] public float xRandomness; // xRandomness from origin x point
    [SerializeField] public float zOffset; //Offset from previous z point
    [SerializeField] public bool spawnOnce;
}
