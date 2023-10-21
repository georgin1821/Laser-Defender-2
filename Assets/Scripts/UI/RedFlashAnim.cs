using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedFlashAnim : MonoBehaviour
{
    [SerializeField] Image redImage;
    [SerializeField] Animator anim;
    public void Flash()
    {
        anim.SetTrigger("RedFlash");
    }

}
