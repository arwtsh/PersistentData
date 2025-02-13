using UnityEngine;

namespace PersistentData
{
    public abstract class PersistentDataBase
    {
        // Get a copy of the persistent data without knowing the members of the data.
        internal PersistentDataBase Clone()
        {
            return (PersistentDataBase)this.MemberwiseClone();
        }
    }
}
