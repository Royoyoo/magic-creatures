using UnityEngine;

public abstract class UiWidget : MonoBehaviour
{
    public abstract void Initialize();

    public virtual void Deinitialize() { }
}
