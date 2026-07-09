using UdonSharp;
using VirtualVisions.VTility;

public class ChildDestroyer : UdonSharpBehaviour
{
    private void Start()
    {
        gameObject.DestroyChildren();
    }
}
