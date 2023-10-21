using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyCoroutine 
{
	public static IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup < (start + time))
		{
			yield return null;
		}

	}
	public static IEnumerator After(this IEnumerator coroutine, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		yield return coroutine;
	}
}
