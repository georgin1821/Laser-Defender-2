using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomYieldDriverFinished : CustomYieldInstruction
{
    private Driver myDriver;

    public CustomYieldDriverFinished(Driver _myDriver)
    {
        myDriver = _myDriver;
    }



    public override bool keepWaiting
    {
        get { return myDriver.Running; }
    }
}


public class CustomYieldDriverListFinished: CustomYieldInstruction
{
    private List<Driver> myDriverList;

    public CustomYieldDriverListFinished(List<Driver> _myDriverList)
    {
        myDriverList = _myDriverList;
    }

    public override bool keepWaiting
    {
        get
        {
            bool result = false;
            foreach (var myDriver in myDriverList)
            {
                if (myDriver.Running == true) result = true;
            }

            return result;
        }
    }
}
