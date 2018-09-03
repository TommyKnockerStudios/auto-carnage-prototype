using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bl_VehicleUI : MonoBehaviour
{

    [SerializeField] private GameObject EnterVehicleUI;
    [SerializeField] private Text EnterVehicleText;
    public Text SpeedometerText;

    public void SetEnterUI(bool Active, KeyCode key = KeyCode.None)
    {
        if (EnterVehicleText != null)
        {
            string t = string.Format("PRESS <color=#4D90F0>'{0}'</color> TO ENTER", key.ToString().ToUpper());
            EnterVehicleText.text = t;
        }
        EnterVehicleUI.SetActive(Active);
    }

    public void UpdateSpeedometer(float v, VehicleType vehicleType, SpeedType dt = SpeedType.KPH)
    {
        if (vehicleType == VehicleType.Car)
        {
            SpeedometerText.text = v.ToString("000") + "\n<size=16>" + dt.ToString() + "/h</size>";
        }
        else if (vehicleType == VehicleType.Jet)
        {
            SpeedometerText.text = v.ToString("000") + "\n<size=16> KMT/h</size>";
        }
    }

    public void OnEnter(VehicleType vt)
    {
        if (vt == VehicleType.Car)
        {
            SpeedometerText.gameObject.SetActive(true);
        }
        else if (vt == VehicleType.Jet)
        {

        }
    }

    public void OnExit(VehicleType vt)
    {
        if (vt == VehicleType.Car)
        {
            SpeedometerText.gameObject.SetActive(false);
        }
        else if (vt == VehicleType.Jet)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        bl_EventHandler.LocalPlayerDeathEvent += OnLocalDeath;
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        bl_EventHandler.LocalPlayerDeathEvent -= OnLocalDeath;
    }

    void OnLocalDeath()
    {
        EnterVehicleUI.SetActive(false);
    }
}