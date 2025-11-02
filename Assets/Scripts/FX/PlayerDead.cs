using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.ShapeModule shape;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        shape = ps.shape;
    }

    public void AnimatePlayerDeath()
    {
        Vector3 vec = transform.parent.GetComponent<Transform>().rotation.eulerAngles;
        //shape.rotation = Vector3.RotateTowards(shape.rotation, cp.point, Time.deltaTime,0.0f);
        shape.rotation = vec * -1;
        ps.Play();

    }
    public void StopParticles()
    {
        ps.Clear();
    }
}
