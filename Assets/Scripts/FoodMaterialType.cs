using UnityEngine;

public class FoodMaterialType : MaterialTypeObject
{
    public override Material GetMaterial()
    {
        // Return the specific material for food objects
        return Resources.Load<Material>("Materials/Food");
    }
}
