using UnityEngine;
using System.Collections;

/// <summary>
/// 初始化优先级很高，建议放在最开始启动的同时
/// </summary>
public class SDKCenter : SingletonBase<SDKCenter> {

	GameObject SDKGameObject;
	public void Setup(string GoName = "sdkObj")
	{
		SDKGameObject = new GameObject (GoName);
		GameObject.DontDestroyOnLoad (SDKGameObject);
		//#if UNITY_ANDROID
		SDKAndroidCallBack ANDROIDSDK = SDKGameObject.AddComponent<SDKAndroidCallBack> ();
		ANDROIDSDK.Setup ();
		//#endif



	}
}
