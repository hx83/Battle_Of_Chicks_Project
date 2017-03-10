using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 安卓的 SDK 回调
/// </summary>
public class SDKAndroidCallBack : MonoBehaviour {

	public void Setup()
	{
		// 1|31.055932|121.267398|021|上海市|1|1 
		// GPS 数据格式
		Debug.Log("外部记忆装置连接着 Android 上线, <color=#0000ff>type blue</color>");
	}
	/// <summary>
	/// GPS字符串========> 回调方法名字|Tips|维度|经度|国际名字|省名字|城市名字|城市代号
	/// 二维码字符串=========> 
	/// </summary>
	/// <param name="strFromAndroid">String from android.</param>
	private void CallBack(string strFromAndroid)
	{
        /*
		Debug.Log("strFromAndroid ==========================> " + strFromAndroid);
		//将来自安卓的字符串通过 ‘|’ 截断
		string [] _data = strFromAndroid.Split('|');

		switch( _data[0] )
		{
		case "QuitGameEvent":
			Debug.Log ("退出游戏消息");
				#if UNITY_ANDROID
				//                    UFE.UIRoot.gameObject.SetActive(false);
				//                    SDKForAndroid _android = m_SdkApi as SDKForAndroid;
				//                    AlertUI.Show(AlertUI.emAlertUIType.QuitGameDialog, "退出游戏", "您确定退出游戏吗？", _android.OnClickCloseEvent, _android.OnClickLeftBtnEvent, _android.OnClickRightBtnEvent, _android.OnClickAddGroup, "退出游戏", "返回游戏");
				#elif UNITY_IPHONE
				#endif

			break;
		case "BarcodeResult":
			//PopManager.ShowSimpleItem ("二维码扫描返回:" + strFromAndroid, POP_TYPE.normal, 10);
			QRCodeScan.Ins.Save (strFromAndroid);
			break;
		case "SetGPS":
		    //PopManager.ShowSimpleItem ("获取GPS成功返回:"+strFromAndroid, POP_TYPE.normal, 10);
			GPSInfo.Ins.Save (strFromAndroid);
			break;
		case "GetGPSError":
			//PopManager.ShowSimpleItem ("GPS数据异常",POP_TYPE.normal, 10);
			break;
		case "RefreshGPS":
			//PopManager.ShowSimpleItem ("GPS 刷新:"+strFromAndroid, POP_TYPE.normal, 10);
			break;
		case "SetPoiSearch":
			break;
		}
      * */
	}
}
