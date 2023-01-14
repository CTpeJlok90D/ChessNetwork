using UnityEngine;

public static class LocalPlayer
{
    public static string Nickname 
    {
        get
        {
            return PlayerPrefs.GetString("NICKNAME");
        }
        set
        {
            PlayerPrefs.SetString("NICKNAME", value.ToString());
        }
    }

    
}
