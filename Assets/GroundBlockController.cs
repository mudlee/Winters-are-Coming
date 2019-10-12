using UnityEngine;

public class GroundBlockController : MonoBehaviour
{
    [SerializeField] VariantType _variant;

    public VariantType variant
    {
        get { return _variant; }
        set { _variant = value; }
    }
}