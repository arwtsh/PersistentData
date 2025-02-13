using UnityEngine;

/// <summary>
/// A simple SaveStyle that serializes the Data using Unity's JSON API and then saves it to a file.
/// It will always save to the location specified at creation. This is not a good SaveStyle for multiple save slots.
/// Does not have any control over versioning the data or repairing corrupted saves.
/// </summary>
public class FileSaver : SaveStyle
{
    private string filePath;
    private bool isBusy = false; //Prevents async calls while it is reading and writing.

    public FileSaver(string fileName, string fileExtension)
    {
        filePath = Application.persistentDataPath + "/" + fileName + "." + fileExtension;

        Assert.IsTrue(IsFilePathValid(filePath), "The FileSaver did not have a valid file path.");
    }

    /// <summary>
    /// A simple file saving system
    /// </summary>
    public override async Awaitable Save<T>(T data)
    {
        if (isBusy)
        {
            Debug.LogWarning("FileSaver tried to save data of type " + typeof(T) + " while it was busy. Canceling the saving progress.");
            return;
        }
        else
            isBusy = true;

        try
        {
            string json = JsonUtility.ToJson(data, true); //Set json to be readable (line breaks & spaces included)
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            Debug.LogError("Unable to save data " + typeof(T) + " to file.");
        }

        isBusy = false;
        return;
    }

    /// <summary>
    /// A simple file loading system.
    /// Does not attempt any file corruption repair, will just return null.
    /// </summary>
    public override async Awaitable<T> Load<T>()
    {
        if (isBusy)
        {
            Debug.LogWarning("FileSaver tried to load data of type " + typeof(T) + " while it was busy. Canceling the loading progress.");
            return null;
        }
        else
            isBusy = true;

        T data = null;

        try
        {
            string readData = await File.ReadAllTextAsync(filePath);
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

    // Used the filePath in an API and checks for exceptions.
    private static bool IsFilePathValid(string filePath)
    {
        try
        {
            File.GetCreationTime(filePath);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
