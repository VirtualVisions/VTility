using Monocle;
using UdonSharp;
using VRC.SDK3.Data;

public class MyCustomTypeUser : UdonSharpBehaviour
{

    public MyCustomType<float> thing => (MyCustomType<float>)_thing;
    private DataList _thing;


    private void Start()
    {
        _thing = MyCustomType<float>.Create();
        thing.Set(0);
    }

    public override void Interact()
    {
        AddValue();
    }

    public void AddValue()
    {
        thing.Set(thing.Get() + 1);
        thing.Log();
    }
    
}
