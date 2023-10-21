using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DriverRemoteAccess
{
    public int DriverID = 0;
    public static Dictionary<int, Driver> RemoteDriverDict = new Dictionary<int, Driver>();

    public void UpdateRemoteDriver(Driver myDriver)
    {
        if (DriverID != 0)
        {
            if (RemoteDriverDict.ContainsKey(DriverID) == false) RemoteDriverDict.Add(DriverID, myDriver);
        }
    }
}
