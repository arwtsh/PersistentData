using UnityEngine;

namespace PersistentData
{
    /// <summary>
    /// Determines how PersistentData is saved and loaded
    /// </summary>
    public abstract class SaveStyle
    {
        public abstract Awaitable Save<T>(T data) where T : PersistentDataBase;
        public abstract Awaitable<T> Load<T>() where T : PersistentDataBase;
    }
}
