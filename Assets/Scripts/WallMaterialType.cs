using UnityEngine;

public class WallMaterialType : MaterialTypeObject
{
    public override Material GetMaterial()
    {
        // Return the specific material for wall objects
        return Resources.Load<Material>("Materials/Wall");
    }
}
