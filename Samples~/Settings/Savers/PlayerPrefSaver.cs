using UnityEngine;

/// <summary>
/// A simple SaveStyle that serializes the Data using Unity's JSON API and then saves it to a single string PlayerPref.
/// Does not have any control over versioning the data or repairing corrupted saves.
/// </summary>
public class PlayerPrefSaver : SaveStyle
{
    private string key;
    private bool isBusy = false; //Prevents async calls while it is reading and writing.

    public PlayerPrefSaver(string playerPrefKey)
    {
        Assert.IsFalse(string.IsNullOrEmpty(playerPrefKey), "Invalid PlayerPref key assigned to PlayerPrefsSaver.");

        key = playerPrefKey;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public override async Awaitable Save<T>(T data)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        if (isBusy)
        {
            Debug.LogWarning("PlayerPrefsSaver tried to save data of type " + typeof(T) + " while it was busy. Canceling the saving progress.");
            return;
        }
        else
            isBusy = true;

        try
        {
            string json = JsonUtility.ToJson(data);
            //PlayerPrefs.SetString(key, json);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            Debug.LogError("Unable to save data " + typeof(T) + " to file.");
        }

        isBusy = false;
        return;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public override async Awaitable<T> Load<T>()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        if (isBusy)
        {
            Debug.LogWarning("PlayerPrefsSaver tried to load data of type " + typeof(T) + " while it was busy. Canceling the loading progress.");
            return null;
        }
        else
            isBusy = true;

        T data = null;

        try
        {
            string readData = PlayerPrefs.GetString(key);
            data = JsonUtility.FromJson<T>(readData);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            Debug.LogError("Unable to load data " + typeof(T) + ".");
        }

        isBusy = false;
        return data;
    }
}
