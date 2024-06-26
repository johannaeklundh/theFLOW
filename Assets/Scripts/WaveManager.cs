using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Material waveMaterial;
    public ComputeShader waveCompute;
    public RenderTexture NState, Nm1State, Np1State; // Nstate = current state of wave, Nm1State = Previous state, Np1State = next state
    public Vector2Int resolution;
    public RenderTexture obstaclesTex;
    public float size = 1;
    private bool render;
    public Transform[] objects;
    private Vector4[] array = new Vector4[15];
    public float dispersion; //How much the waves are disappering, can be changed in Unitys inspector during runtime to see the changes

    // Start is called before the first frame update
    void Start()
    {
        // All textures are initialized and set
        InitializeTexture(ref NState);
        InitializeTexture(ref Nm1State);
        InitializeTexture(ref Np1State);
        waveMaterial.SetTexture("_rippleTex", NState);
        obstaclesTex.enableRandomWrite = true;
        waveMaterial.mainTexture = NState;
    }

    void InitializeTexture(ref RenderTexture tex)
    {
        tex = new RenderTexture(resolution.x, resolution.y, 1, UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SNorm)
        {
            enableRandomWrite = true
        };
        tex.Create();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        render = !render;
        if(!render) return;
        Graphics.CopyTexture(NState, Nm1State);
        Graphics.CopyTexture(Np1State, NState);

        waveCompute.SetTexture(0, "NState", NState);
        waveCompute.SetTexture(0, "Nm1State", Nm1State);
        waveCompute.SetTexture(0, "Np1State", Np1State);

        // Array with objects that create ripples. update if more objects are being added.
        for (int i = 0; i < objects.Length; i++)
        {
            if(i > 15) break;
            Vector2 pos = Remap(objects[i].position.x, objects[i].position.z);
            array[i] = new Vector4(pos.x, pos.y, size);
        }
        waveCompute.SetVectorArray("rippleObjs", array);
        waveCompute.SetFloat("arrayLength", array.Length);

        waveCompute.SetInts("resolution", resolution.x, resolution.y);
        waveCompute.SetFloat("dispersion", dispersion);
        waveCompute.SetTexture(0, "obstaclesTex", obstaclesTex);
        waveCompute.Dispatch(0, resolution.x / 8, resolution.y / 8, 1);
    }
    Vector2 Remap(float posX, float posZ) // Places objects in the world
    {
        float x2 = transform.position.x - transform.localScale.x * 5;
        float x1 = transform.position.x + transform.localScale.x * 5;
        float y2 = transform.position.z - transform.localScale.z * 5;
        float y1 = transform.position.z + transform.localScale.z * 5;

        float x = (posX - x1) / (x2 - x1) * resolution.x;
        float y = (posZ - y1) / (y2 - y1) * resolution.y;
        Vector2 dis = new(x, y);
        return dis;
    }
}
