﻿using UnityEngine;
using System;
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public static class bl_CoopUtils
{

    public static void LoadLevel(string scene)
    {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
 SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }

    public static void LoadLevel(int scene)
    {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
 SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string CoopColorStr(string text)
    {

        return "<color=#5F9DF5>" + text + "</color>";
    }

    /// <summary>
    /// 
    /// </summary>
    public static string GetGuid
    {
        get
        {
            string s = Guid.NewGuid().ToString();
            return s;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static bl_LobbyUI GetLobbyUI
    {
        get
        {
            bl_LobbyUI l = GameObject.FindWithTag("GameController").GetComponent<bl_LobbyUI>();

            return l;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static bl_PreScene GetPreScene
    {
        get
        {
            bl_PreScene ps = GameObject.FindWithTag("GameController").GetComponent<bl_PreScene>();
            return ps;
        }
    }
    /// <summary>
    /// Get ClampAngle
    /// </summary>
    /// <param name="ang"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float ClampAngle(float ang, float min, float max)
    {
        if (ang < -360f)
        {
            ang += 360f;
        }
        if (ang > 360f)
        {
            ang -= 360f;
        }
        return UnityEngine.Mathf.Clamp(ang, min, max);
    }
    /// <summary>
    /// obtain only the first two values
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static string GetDoubleChar(float f)
    {
        return f.ToString("00");
    }
    /// <summary>
    /// obtain only the first three values
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static string GetThreefoldChar(float f)
    {
        return f.ToString("000");
    }

    public static string GetTimeFormat(float m, float s)
    {
        return string.Format("{0:00}:{1:00}", m, s);
    }
    /// <summary>
    /// Helper for Cursor locked in Unity 5
    /// </summary>
    /// <param name="mLock">cursor state</param>
    public static void LockCursor(bool mLock)
    {
#if UNITY_5
        if (mLock == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
#else
        Screen.lockCursor = mLock;
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool GetCursorState
    {
        get
        {
#if UNITY_5
            if (Cursor.visible && Cursor.lockState != CursorLockMode.Locked)
            {
                return false;
            }
            else
            {
                return true;
            }
#else
            return Screen.lockCursor;
#endif
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float HorizontalAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static CloudRegionCode IntToRegionCode(int id)
    {
        switch (id)
        {
            case 0:
                return CloudRegionCode.asia;
            case 1:
                return CloudRegionCode.au;
            case 2:
                return CloudRegionCode.eu;
            case 3:
                return CloudRegionCode.jp;
            case 4:
                return CloudRegionCode.us;
            case 5:
                return CloudRegionCode.cae;
            case 6:
                return CloudRegionCode.sa;
            case 7:
                return CloudRegionCode.usw;
            case 8:
                return CloudRegionCode.@in;
            case 9:
                return CloudRegionCode.kr;
        }

        return CloudRegionCode.none;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static int RegionCodeToInt(CloudRegionCode code)
    {
        switch (code)
        {
            case CloudRegionCode.asia:
                return 0;
            case CloudRegionCode.au:
                return 1;
            case CloudRegionCode.eu:
                return 2;
            case CloudRegionCode.jp:
                return 3;
            case CloudRegionCode.us:
                return 4;
            case CloudRegionCode.cae:
                return 5;
            case CloudRegionCode.sa:
                return 6;
            case CloudRegionCode.usw:
                return 7;
            case CloudRegionCode.@in:
                return 8;
            case CloudRegionCode.kr:
                return 9;
        }
        return 5; // equal a none
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static string RegionToString(CloudRegionCode code)
    {
        switch (code)
        {
            case CloudRegionCode.asia:
                return "ASIA";
            case CloudRegionCode.au:
                return "AUSTRALIA";
            case CloudRegionCode.eu:
                return "EUROPE";
            case CloudRegionCode.jp:
                return "JAPAN";
            case CloudRegionCode.us:
                return "USA";
            case CloudRegionCode.sa:
                return "SOUTH AMERICA";
            case CloudRegionCode.cae:
                return "CANADA";
            case CloudRegionCode.usw:
                return "USA WEST";
            case CloudRegionCode.@in:
                return "INDIA";
            case CloudRegionCode.kr:
                return "SOUTH KOREA";
        }
        return "NONE"; // equal a none
    }
}