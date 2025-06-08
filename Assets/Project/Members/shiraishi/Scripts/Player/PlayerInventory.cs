using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> collectedItems = new List<string>();

    [SerializeField]
    public GameObject ActivateObject;
    public bool activeKey = false;
    private bool alreadyActive = false;

    private void Update()
    {
        if (!alreadyActive)
        {

            if (activeKey)
            {
                if (collectedItems.Contains("key"))
                {
                    var direct = ActivateObject.GetComponents<IActivatable>();
                    foreach (var a in direct) a.Activate();

                    var children = ActivateObject.GetComponentsInChildren<IActivatable>(true);
                    foreach (var a in children)
                    {
                        if (System.Array.IndexOf(direct, a) == -1)
                            a.Activate();
                    }

                    alreadyActive = true;
                }
            }
        }


    }
}
