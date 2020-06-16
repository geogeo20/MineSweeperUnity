using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Component
{
    private static T instance;

    public static T Instace
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
