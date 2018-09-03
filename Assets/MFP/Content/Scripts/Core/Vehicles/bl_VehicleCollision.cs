using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bl_VehicleCollision : bl_PhotonHelper
{

    public float CollisionMultiplie = 1;
    public float ImpactMultiplier = 1;
    public bool DamagePlayersByCollision = false;
    [SerializeField] private AudioClip CollisionAudio;

    private List<Collider> LocalColliders = new List<Collider>();
    private bl_VehicleManager Vehicle;
    private float LastTime;

    private void Awake()
    {
        LocalColliders.AddRange(transform.GetComponentsInChildren<Collider>());
        Vehicle = GetComponent<bl_VehicleManager>();
        LastTime = Time.time;
    }

    public void DoDamage(float d, int view)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LocalColliders.Contains(collision.collider) || collision.transform.tag == "NonDamage")
            return;

        if (DamagePlayersByCollision)
        {
            if (collision.transform.tag == bl_PlayerPhoton.PlayerTag || collision.transform.tag == bl_PlayerPhoton.RemoteTag)
            {
                if (photonView.isMine)
                {
                    float d = collision.relativeVelocity.magnitude * CollisionMultiplie;
                    collision.transform.GetComponent<bl_PlayerDamage>().DoDamage((int)d);
                    return;
                }
            }
        }

        float impact = collision.relativeVelocity.magnitude;
        bool infront = (Vector3.Dot(transform.forward,collision.contacts[0].normal) < 0);
        if (impact > 22 && Time.time > LastTime && infront)
        {
            float v = (impact / 30);
            AudioSource.PlayClipAtPoint(CollisionAudio, transform.position, v);
            Vehicle.OnCollision(impact * ImpactMultiplier);
            LastTime = Time.time + 0.5f;
        }
    }

    public bool isLocalOwner { get { return Vehicle.isPlayerIn; } }
}