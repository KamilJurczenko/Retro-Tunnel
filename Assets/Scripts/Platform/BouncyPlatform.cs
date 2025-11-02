using System.Collections;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    [SerializeField] PlayerController pc;
    [SerializeField] AudioSource bounceSoundFX;

    private MaterialPropertyBlock materialPropertyBlock;
    private Renderer rend;

    private float initialAlpha;

    public float velocity = 10f;
    private void Start()
    {
        rend = GetComponent<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        initialAlpha = rend.material.GetFloat("_Alpha");
    }

    private IEnumerator ColorFade()
    {
        rend.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetFloat("_Color", 1);
        /*while ()
        {

            yield return null;
        }*/
        float t = 0;
        while (t <= 1)
        {
            t += 0.1f;
            float l = Mathf.Lerp(1, initialAlpha, t);
            materialPropertyBlock.SetFloat("_Alpha", l);
            rend.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Check for normal Collsion Vector
            //ContactPoint contactPoint = collision.contacts[0];
            Vector3 normal = collision.contacts[0].normal;
            if(Mathf.Abs(normal.z) > 0.2f)
            {
                Debug.Log("Normal Vector hit: " + normal);
                pc.OnGameOver();
            }
            else
            {
                StartCoroutine(ColorFade());
                if(GameManager.playSoundFX)
                    bounceSoundFX.Play();
                if (Mathf.Abs(normal.y) > 0.9f) 
                    pc.SetPlayerYVelocity(velocity);
                else if(Mathf.Sign(normal.x) == 1) 
                    pc.SetPlayerXVelocity(-velocity);
                else if(Mathf.Sign(normal.x) == -1)
                    pc.SetPlayerXVelocity(velocity);
            }

        }
    }
}
