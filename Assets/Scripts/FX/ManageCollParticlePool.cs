using System.Collections.Generic;
using UnityEngine;

public class ManageCollParticlePool : MonoBehaviour
{
    [SerializeField] private ParticleSystem collisionFXPrefab;
    [SerializeField] private ParticleSystem collisionStayFXPrefab;

    public ParticleSystem stayFxTransform;

    public Queue<ParticleSystem> qPool;
    private int poolAmount = 3;

    // Start is called before the first frame update
    void Start()
    {
        qPool = new Queue<ParticleSystem>();

        if (qPool.Count == 0)
        {
            for (int i = 0; i < poolAmount; i++)
            {
                var instantiatedObj = Instantiate(collisionFXPrefab);
                //instantiatedObj.gameObject.SetActive(false);
                qPool.Enqueue(instantiatedObj);
                //poolingList[i] = Instantiate(collisionFXPrefab).transform;
            }
            stayFxTransform = Instantiate(collisionStayFXPrefab);
        }
    }
}
