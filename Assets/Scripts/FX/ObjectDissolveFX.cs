using System.Collections;
using UnityEngine;

public class ObjectDissolveFX : MonoBehaviour
{
    [SerializeField] bool dissolveOnDistance = true;

    public static float startDissolveDist = 90;
    private bool dissolved = false;

    private Renderer rend;
    private MaterialPropertyBlock materialPropertyBlock;

    private float xExtents;
    private float yExtents;
    private float randExtent;

    private float l;
    private float lMax = 1;
    private float lStart = 0;
    private Vector3 cachedPosition = Vector3.zero;
    private int dissolveModes = 2;
    DissolveProperties po;

    private bool isDissolved = false;
    // Start is called before the first frame update
    void Start()
    {
        po = GetComponent<DissolveProperties>();
        cachedPosition = transform.position;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        xExtents = rend.bounds.extents.x;
        yExtents = rend.bounds.extents.y;
        materialPropertyBlock = new MaterialPropertyBlock();
        if(dissolveOnDistance)
            InitialState();
    }

    // Update is called once per frame
    void Update()
    {
        if (dissolveOnDistance && (CameraController.lerpedToGameStart || GameManager.gameIsRestarting))
        {
            float distance = (transform.position.z - PlayerController.playerPosition.z);
            if (distance < startDissolveDist && Mathf.Sign(distance) == 1 && !dissolved)
            {
                //Debug.Log("Dissolving Obstacle: " + gameObject.name);
                StartCoroutine(StartDissolve());
                dissolved = true;
            }

            if (dissolved && cachedPosition != transform.position)
            {
                InitialState();
                cachedPosition = transform.position;
            }
        }
    }
    private int RandNumber()
    {
        return Random.Range(0, dissolveModes);
    }
    private void InitialState()
    {
        dissolved = false;
        l = lStart;
        rend.GetPropertyBlock(materialPropertyBlock);
        int rand;
        if (po != null)
        {
            rand = po.rand;
        }
        else
            rand = RandNumber();
        randExtent = rand == 0 ? xExtents : yExtents;
        if (randExtent == xExtents)
            materialPropertyBlock.SetVector("_dissolveDirection", new Vector3(1, 0, 0));
        else
            materialPropertyBlock.SetVector("_dissolveDirection", new Vector3(0, 1, 0));
        materialPropertyBlock.SetFloat("_minMaxClip", randExtent);
        materialPropertyBlock.SetFloat("_AlphaClipVal", l);
        materialPropertyBlock.SetFloat("_dissolveThickness", 0.3f);
        rend.SetPropertyBlock(materialPropertyBlock);
    }

    public IEnumerator StartDissolve(float init = -1)
    {
        rend.enabled = true;
        rend.GetPropertyBlock(materialPropertyBlock);
        if (init == 0)
        {
            materialPropertyBlock.SetFloat("_dissolveThickness", 0.08f);
            l = init;
        }
        while (l < lMax)
        {
            l += Time.deltaTime * 2;
            materialPropertyBlock.SetFloat("_AlphaClipVal", l);
            rend.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
        l = lMax;
        materialPropertyBlock.SetFloat("_AlphaClipVal", l);
        materialPropertyBlock.SetFloat("_dissolveThickness", 0);
        rend.SetPropertyBlock(materialPropertyBlock);
        isDissolved = true;
    }
}
