using UnityEngine;
using System.Collections.Generic;

public class bl_Passenger : bl_PhotonHelper
{
    [Separator("Vehicle Seats")]
    public List<Seat> Seats = new List<Seat>();

    private bool LocalOnTrigger = false;
    public GameObject Player { get; set; }
    private bl_PlayerCar PlayerCar;
    private int SeatTriggerID = -1;
    private bl_VehicleManager VehicleManager;
    private bl_VehicleUI VehicleUI;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        VehicleManager = GetComponent<bl_VehicleManager>();
        VehicleUI = FindObjectOfType<bl_VehicleUI>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Player != null)
        {
            InputControll();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void InputControll()
    {
        if (Input.GetKeyDown(VehicleManager.EnterKey))
        {
            if (PlayerCar.isPassenger)
            {
                ExitSeat();
            }
            else if(LocalOnTrigger && !Seats[SeatTriggerID].isOcuped)
            {
                EnterInSeat();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void EnterInSeat()
    {
        Seat s = Seats[SeatTriggerID];
        bl_EventHandler.OnLocalPlayerVehicle(true, VehicleType.Car);
        PlayerCar.OnEnterInSeat(this, s);
        s.CameraView.SetActive(true);
        Player.transform.parent = VehicleManager.PlayerHolder;
        Player.transform.localPosition = s.Position;
        Player.transform.localEulerAngles = s.Rotation;
        s.isOcuped = true;
        VehicleUI.SetEnterUI(false);
        LocalPlayerView.RPC("RpcPassengerEvent", PhotonTargets.OthersBuffered, true, SeatTriggerID, photonView.viewID);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ExitSeat(bool byDeath = false)
    {
        if (SeatTriggerID < 0)
            return;

        Seat s = Seats[SeatTriggerID];
        Player.transform.parent = null;
        Player.transform.position = s.ExitPoint.position;
        Player.transform.rotation = s.ExitPoint.rotation;
        s.isOcuped = false;
        s.CameraView.SetActive(false);
        PlayerCar.OnExitFromSeat(byDeath);
        bl_EventHandler.OnLocalPlayerVehicle(false, VehicleType.Car);
        LocalPlayerView.RPC("RpcPassengerEvent", PhotonTargets.OthersBuffered, false, SeatTriggerID, photonView.viewID);
    }

    public void TriggerEnter(int id)
    {
        if (Player == null)
            return;
        if (PlayerCar.isPassenger)
            return;
        if (isSeatOcuped(id))
            return;

        SeatTriggerID = id;
        LocalOnTrigger = true;
        VehicleUI.SetEnterUI(true, VehicleManager.EnterKey);
    }

    public void TriggerExit()
    {
        SeatTriggerID = -1;
        LocalOnTrigger = false;
        VehicleUI.SetEnterUI(false);
    }

    public bool isSeatOcuped(int id) { return Seats[id].isOcuped; }

    void OnEnable()
    {
        bl_EventHandler.LocalPlayerSpawnEvent += OnLocalPlayerSpawn;
    }
    void OnDisable()
    {
        bl_EventHandler.LocalPlayerSpawnEvent -= OnLocalPlayerSpawn;
    }
    public PhotonView LocalPlayerView { get; set; }
    void OnLocalPlayerSpawn(GameObject player)
    {
        Player = player;
        LocalPlayerView = Player.GetComponent<PhotonView>();
        PlayerCar = Player.GetComponent<bl_PlayerCar>();
    }

    void OnDrawGizmosSelected()
    {
        if (playerHolder == null)
            return;

        Gizmos.matrix = playerHolder.localToWorldMatrix;
        for (int i = 0; i < Seats.Count; i++)
        {
            Color c = Color.blue;
            Gizmos.color = c;
            Gizmos.DrawWireSphere(Seats[i].Position, 0.5f);
            c.a = 0.4f;
            Gizmos.color = c;
            Gizmos.DrawSphere(Seats[i].Position, 0.5f);
           
        }
        Gizmos.matrix = Matrix4x4.identity;
    }

    public Transform playerHolder
    {
        get
        {
            if(VehicleManager == null) { VehicleManager = GetComponent<bl_VehicleManager>(); }
            return VehicleManager.PlayerHolder;
        }
    }

    [System.Serializable]
    public class Seat
    {
        public string Name;
        public Vector3 Position;
        public Vector3 Rotation;
        public GameObject CameraView;
        public Transform ExitPoint;

        public bool isOcuped = false;

        private Transform _heatLook;
        public Transform HeatLook
        {
            get
            {
                if(_heatLook == null)
                {
                    _heatLook = CameraView.transform.GetChild(0);
                }
                return _heatLook;
            }
        }
    }
}