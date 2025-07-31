using JCSUnity;

public class FT_Instance : JCS_InstanceNew<FT_Instance>
{
    /* Variables */

    public string data = "";

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        CheckInstance(this, true);
    }

    protected override void TransferData(FT_Instance _old, FT_Instance _new)
    {
        _new.data = _old.data;
    }
}
