using UnityEngine;

namespace PersistentData
{
    public class SavableData<T> where T : PersistentDataBase, new()
    {
        private SaveStyle saver;

        private T data;

        public SavableData(SaveStyle saveStyle)
        {
            saver = saveStyle;

            data = new T();
        }

        // Implicit conversion to the PersistentData (automatically calls Get())
        public static implicit operator T(SavableData<T> savableData)
        {
            return savableData.Get();
        }

        /// <summary>
        /// Get the PersistentData this SavableData wraps.
        /// </summary>
        public T Get()
        {
            return data;
        }

        /// <summary>
        /// Save the data syncronously using the provided Saver
        /// </summary>
        public void Save()
        {
            SaveAsync().GetAwaiter().GetResult(); //Force function to run synchronously
        }

        /// <summary>
        /// Save the data asyncronously using the provided Saver
        /// </summary>
        public async Awaitable SaveAsync()
        {
            await saver.Save<T>(data);
            Debug.Log("Successfully saved data " + typeof(T) + " with " + saver.GetType());
        }

        /// <summary>
        /// Load the data syncronously using the provided Saver
        /// </summary>
        public void Load()
        {
            LoadAsync().GetAwaiter().GetResult(); //Force function to run synchronously
        }

        /// <summary>
        /// Load the data asyncronously using the provided Saver
        /// </summary>
        public async Awaitable LoadAsync()
        {
            T loadedData = await saver.Load<T>();
            if (loadedData != null)
            {
                data = loadedData;
                Debug.Log("Successfully loaded data " + typeof(T) + " with " + saver.GetType());
            }
            else
            {
                Debug.LogError("Was unable to load data for " + typeof(T));
            }
        }

        /// <summary>
        /// Reset the data to it's default values
        /// </summary>
        public void Reset()
        {
            data = new T();
        }

        /// <summary>
        /// Set the data to match external data
        /// This can be used along with GetTemp() to temporarilly make changes to the data, and then apply them later.
        /// </summary>
        public void Set(T tempData)
        {
            data = tempData;
        }

        /// <summary>
        /// Get a copy of the data
        /// This can be used along with Set() to temporarilly make changes to the data, and then apply them later.
        /// </summary>
        public T GetTemp()
        {
            return (T)data.Clone();
        }
    }
}
