using UnityEngine;

public class OnGameStart : MonoBehaviour
{
    [SerializeField] PhysicMaterial floorPhysicsMat;
    // Start is called before the first frame update
    void Awake()
    {
        floorPhysicsMat.bounciness = 0.85f;
    }
}
