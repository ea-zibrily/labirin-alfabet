using UnityEngine;

namespace Alphabet.DesignPattern.Singleton
{
    public class MonoDDOL<T> : MonoBehaviour where T: MonoBehaviour
    {
        public static T Instance;

        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        
        public virtual void InitComponent() { }
    }
}