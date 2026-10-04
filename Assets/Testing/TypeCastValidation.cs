using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;

public class TypeCastValidation : EmptyClass
{
    
    [SerializeField] protected EmptyClass targetInstance;
    
    private void Start()
    {
        DataToken token = new DataToken(targetInstance);
        
        EmptyClass thing = token.CastReference<EmptyClass>();

        Debug.Log($"thing == null: {thing == null}");
        Debug.Log($"thing: {thing}");

        if (thing)
        {
            Debug.Log($"Disabling: {thing.name}");
            thing.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"Thing was not found.");
        }
    }
}
