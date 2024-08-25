using UnityEngine;

//Singleton Save Object Reference When Move Scene
namespace Alphabet.Pattern.Singleton
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Kalau instance kosong, maka script akan mencari object lain
                    // yang memiliki tipe atau class yang sama dengan target

                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        // Jika masih kosong, maka script akan membuat gameObject baru
                        // dengan class yang sama dengan target
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            //Singleton method
            if (_instance == null) 
            {
                //First run, set the instance
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            } 
            else if (_instance != this)
            {
                //Instance is not the same as the one we have, destroy old one, and reset to newest one
                Destroy(_instance.gameObject);
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}