using UnityEngine;
namespace Hiyazcool 
{
    namespace Unity
    {
        public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
        {
            [SerializeField]
            protected bool isPersistent = true;
            [SerializeField]
            private bool debugLogCreation = false;
            public static T instance { get; protected set; }
            protected virtual void Awake()
            {
                if (debugLogCreation)
                    Debug.Log("Creating");
                if (instance != null && instance != this)
                {
                    Destroy(this);
                    throw new System.Exception("An instance of this singleton already exists.");
                }
                else
                {
                    instance = (T)this;
                    if (isPersistent)
                        DontDestroyOnLoad((T)this);

                }
            }
        }
    }
}