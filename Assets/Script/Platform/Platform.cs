using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //to change color
    [SerializeField] private MeshRenderer[] MeshRenderers;
    
    [SerializeField] private int maxDurability = 1;
    public int MaxDurability { set { maxDurability = value; } }
    [SerializeField] private int Durability;
    public int durability
    {
        get { return Durability; }
        set { Durability = value; }
    }

    private Vector2Int coordinates;
    public Vector2Int Coordinates
    {
        get { return coordinates; }
        set { coordinates = value; }
    }

    //When the platform break
    [SerializeField] private PlatformBreakEvent PlatformBreakEvent;
    [SerializeField] private float BreakForce;
    
    //Reduce size of debris after platform shattered
    [SerializeField] private float WaitBeforeStartDisapear;
    [SerializeField] private float DisapearDuration;
    private Vector3 DebrisBaseScale;

    [SerializeField] private ParticleSystem FireParticle;

    private void Start()
    {
        MeshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    /// <summary>
    /// Reduce the durability of the platform by the given value.
    /// </summary>
    /// <param name="qte">Damage applied to the platform.</param>
    public void ReduceDurability(int qte)
    {
        Durability -= qte;

        if (Durability == 0)
        {
            BreakPlatform();
        }

        UpdateColor();
    }

    public void UpdateColor()
    {
        float alpha = 1f - (float)Durability / maxDurability;
        foreach (MeshRenderer Renderer in MeshRenderers)
        {
            Renderer.material.color = Color.Lerp(Color.white, Color.black, alpha);
        }
    }

    /// <summary>
    /// Reduce the durability of the platform by 1.
    /// </summary>
    public void ReduceDurability()
    {
        ReduceDurability(1);
    }

    /// <summary>
    /// Activate RigidBody off all platform pieces and apply some outward force.
    /// </summary>
    private void BreakPlatform()
    {
        PlatformBreakEvent.Raise(this);
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            Vector3 Force = (rb.transform.position - transform.position).normalized * BreakForce;
            rb.AddForce(Force);
        }

        StartCoroutine(DebrisReduceSizeDisappearCoroutine());
    }
    
    /// <summary>
    /// Wait for a while then gradually reduce the size of the debris until nothing's left.
    /// </summary>
    private  IEnumerator DebrisReduceSizeDisappearCoroutine()
    {
        DebrisBaseScale = transform.GetChild(0).localScale;
        yield return new WaitForSeconds(WaitBeforeStartDisapear);

        List<ParticleSystem> PS = new List<ParticleSystem>();
        
        foreach (Transform tr in transform)
        {
            PS.Add(Instantiate(FireParticle, tr.position, Quaternion.identity, ArenaManager.Instance.platformsParent.transform));
        }
        
        for (int i = 0; i < DisapearDuration * 60; i++)
        {
            foreach (Transform tr in transform)
            {
                float alpha = 1 / (DisapearDuration * 60) * i;
                tr.localScale = Vector3.Lerp(DebrisBaseScale, Vector3.zero, alpha);
            }
            
            yield return new WaitForSeconds(1 / 60f);
        }

        foreach (ParticleSystem ps in PS)
        {
            ps.Stop();
        }
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }

    public PlatformData GetPlatformData()
    {
        PlatformData pData = new PlatformData(this);
        return pData;
    }

    public void SetPlatformData(PlatformData pData)
    {
        Durability = pData.durability;
        coordinates = pData.position;
    }
}
