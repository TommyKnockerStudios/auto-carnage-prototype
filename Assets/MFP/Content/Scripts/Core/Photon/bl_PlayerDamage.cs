using UnityEngine;
using System.Collections;

public class bl_PlayerDamage : MonoBehaviour
{

    [Range(1, 100)]public int Health = 100;
    [Range(1, 15)] public int RespawnTime = 7;

    [Separator("References")]
    [SerializeField] private AudioClip PainClip;

    private bool isDeath = false;
    private bl_PlayerUI PlayerUI;
    private bl_PlayerAnimator PlayerAnim;
    private bl_GameController GameController;
    private bl_PlayerPhoton PlayerPhoton;
    private bl_PlayerCar PlayerCar;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        PlayerUI = FindObjectOfType<bl_PlayerUI>();
        PlayerAnim = transform.GetComponent<bl_PlayerSync>().m_PlayerAnimation;
        GameController = FindObjectOfType<bl_GameController>();
        PlayerPhoton = GetComponent<bl_PlayerPhoton>();
        PlayerCar = GetComponent<bl_PlayerCar>();
        PlayerUI.SetHealth(Health);
        PlayerUI.DeathUI.SetActive(false);
    }

    /// <summary>
    /// Call this to hurt this player
    /// </summary>
    public void DoDamage(int damage)
    {
        if (isDeath)
            return;

        View.RPC("RpcDoDamage", PhotonTargets.AllBuffered, damage);
    }

    [PunRPC]
    void RpcDoDamage(int damage, PhotonMessageInfo info)
    {
        if (isDeath)
            return;

        Health -= damage;
        Health = Mathf.Clamp(Health, 0, 100); //check each time that player get damage that health is never > 100
        if (View.isMine)
        {
            PlayerUI.SetHealth(Health);
        }

        if (Health > 0)
        {
            OnDamage();
        }
        else
        {
            OnDeath();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && View.isMine)
        {
            DoDamage(100);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDamage()
    {
        if (View.isMine)
        {
            PlayerUI.OnDamage();
            AudioSource.PlayClipAtPoint(PainClip, transform.position, 1);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDeath()
    {
        if (isDeath)
            return;

        isDeath = true;
        GetComponent<CharacterController>().enabled = false;
        if (View.isMine)
        {
            bl_EventHandler.OnLocalPlayerDeath();
            bl_EventHandler.OnSmallImpactEvent();
            if (PlayerCar.isPlayerInsideVehicle)
            {
                if (PlayerCar.isInVehicle)
                {
                    PlayerCar.Vehicle.OnExit(true);
                }
                else
                {
                    PlayerCar.Passenger.ExitSeat(true);
                }
            }
            //Send a new log information
            string logText = View.owner.NickName + " Die";
            bl_LogInfo inf = new bl_LogInfo(logText, Color.yellow);
            bl_EventHandler.OnLogMsnEvent(inf);
            StartCoroutine(Respawn());
        }
        else
        {
            PlayerPhoton.RemoteObject.SetActive(true);
            PlayerPhoton.RemoteObject.transform.parent = null;
            PlayerAnim.OnRemoteDeath(RespawnTime);
           // Destroy(gameObject);
        }
    }

    IEnumerator Respawn()
    {
        PlayerPhoton.RemoteObject.SetActive(true);
        PlayerPhoton.RemoteObject.transform.parent = null;
        PlayerAnim.OnLocalDeath();
        PlayerUI.DeathUI.SetActive(true);
        Vector3 back = transform.position - (transform.forward * 3);
        float t = RespawnTime;
        while(t > 0)
        {
            t -= Time.deltaTime;
            PlayerUI.UpdateRespawnText(t);
            transform.position = Vector3.Lerp(transform.position, back, Time.deltaTime * 4);
            yield return new WaitForEndOfFrame();
        }
        Destroy(PlayerPhoton.RemoteObject);
        GameController.SpawnPlayer();
    }

    private PhotonView _view = null;
    private PhotonView View
    {
        get
        {
            if (_view == null)
            {
                _view = PhotonView.Get(this);
            }
            return _view;
        }
    }

}