using System;
using System.Collections;
using Umeng;
using UnityEngine;

public class MainEntry : MonoBehaviour
{
    private static MainEntry _instance;
    private static System.Action _nextFrameCall;
    private static System.Action _updateFrameList;
    private static System.Action _lateUpdateList;
    public static MainEntry Instance
    {
        get
        {
            return _instance;
        }
    }

    public static void RunInUpdate(Action function)
    {
        if(function != null)
            _updateFrameList += function;
    }

    public static void CancelRunInUpdate(Action function)
    {
        if (_updateFrameList != null)
        {
            _updateFrameList -= function;
        }
    }

    public static void RunInLateUpdate(Action function)
    {
        if(function != null)
            _lateUpdateList += function;
    }

    public static void CancelRunInLateUpdate(Action function)
    {
        if (_lateUpdateList != null)
        {
            _lateUpdateList -= function;
        }
    }

    public static void RunInNextFrame(System.Action function)
    {
        if (function != null)
            _nextFrameCall += function;
    }

    public static void CancelInNextFrame(System.Action function)
    {
        if (_nextFrameCall != null)
            _nextFrameCall -= function;
    }

    void Awake()
    {
        _instance = this;
        
    }
    /// <summary>
    /// 游戏初始化
    /// </summary>
    void Start()
    {
        InitPersistantData();
        UIUtil.Setup();
		SDKCenter.Ins.Setup ();
        AudioSourceManager.Instance.Setup(this.gameObject);
        /***
        SceneLoadingPanel.Instance.Show();
        SceneLoadingPanel.Instance.SetPercent(0.25f);
        SceneLoadingPanel.Instance.SetTitle("正在加载资源...");
         * */
        //
        //??StartCoroutine(CheckNotice());

#if UNITY_STANDALONE_WIN
        if (Global.IsUseAB)
            Preload();
        else
            InitGame();
#else
        Preload();
#endif
    }


    void Update()
    {
        CheckEscape();

        if (_nextFrameCall != null)
        {
            //_nextFrameCall.Invoke();
            Delegate[] list = _nextFrameCall.GetInvocationList();
            if (list != null)
            {
                int count = list.Length;
                for(int i = 0; i< count; i++)
                {
                    System.Action function = (System.Action)list[i];
                    _nextFrameCall -= function;
                    function.Invoke();
                }
                //_nextFrameCall = null;
            }
        }
        if (_updateFrameList != null)
        {
            _updateFrameList.Invoke();
        }

        //StageManager.Update();
    }

    void FixedUpdate()
    {
        //StageManager.FixedUpdate();
    }

    void LateUpdate()
    {
        if (_lateUpdateList != null)
        {
            _lateUpdateList.Invoke();
        }
    }

    void OnDrawGizmos()
    {
        //APEngine.DebugDraw();
    }

    //----------------------------- 退出游戏


    private void CheckEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ModuleManager.Show(ModuleType.GAME_EXIT_PANEL, true);
        }
    }
    //
    //
    //
    public string GetDeviceInfo()
    {
        string dev;
        //if (PlayerPrefs.HasKey(LoginManager.LOCAL_DEVICE_NO))
        //{
        //    dev = PlayerPrefs.GetString(LoginManager.LOCAL_DEVICE_NO);
        //    return dev;
        //}
        dev = GA.GetDeviceInfo();
        if (dev == null)
        {
            dev = SystemInfo.deviceUniqueIdentifier;
        }
#if UNITY_IPHONE
		//dev =  _GetIDFA();
        if(dev.Length>=36)
		    dev = dev.Substring(0, 36);

        //处理ios 6.0 出现的idfa获取为0的问题
        if(dev == "00000000-0000-0000-0000-000000000000")
        {
            dev = SystemInfo.deviceUniqueIdentifier;
        }
#endif
        //PlayerPrefs.SetString(LoginManager.LOCAL_DEVICE_NO, dev);
        return dev;
    }
    //
    //
    //
    #region 登陆模块
    /********************************
    /// <summary>
    /// 登陆
    /// </summary>
    private void Login()
    {
        SceneLoadingPanel.Instance.Show();
        SceneLoadingPanel.Instance.SetPercent(1);
        SceneLoadingPanel.Instance.SetTitle("正在登录游戏");
        //

        
        LoginManager.InitAccountCache();

        //debug测试，固定账号
        if (RoomDebugMgr.Instance != null&&RoomDebugMgr.Instance.IsUseDebugAcc)
        {
            LoginManager.DeviceNo = RoomDebugMgr.Instance.DebugAcc;
        }
        else
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(LoginManager.LOCAL_DEVICE_NO)))
            {
                LoginManager.DeviceNo = GetDeviceInfo();
                PlayerPrefs.SetString("DeviceInfo", LoginManager.DeviceNo);
            }
            else
            {
                LoginManager.DeviceNo = PlayerPrefs.GetString(LoginManager.LOCAL_DEVICE_NO);
            }

        }

     

        //ThreadManager.Start();
        TeamHeartBeatListener.Setup();

        LoginManager.AddEventListener(LoginEvent.LOGIN_SUCCESS_ForUI, OnLoginSuccess);
        LoginManager.Login(LoginManager.Account, LoginManager.Password);
    }

    private void OnLoginSuccess(BaseEvent evt)
    {
        LoginManager.RemoveEventListener(LoginEvent.LOGIN_SUCCESS_ForUI, OnLoginSuccess);
        Debug.Log("Login Success,Show UI Stage");
        SceneLoadingPanel.Instance.Hide();
        //StageManager.Show(EnumStageType.GUIDE);
        //return;

        if (PlayerPrefs.HasKey(LoginPanel.ALREADY_PLAYED))
        {
            StageManager.Show(EnumStageType.UI);
        }
        else
        {
            StageManager.Show(EnumStageType.GUIDE);
        }
    }
     */
#endregion

    #region 检查公告
    /// <summary>
    /// 检查是否有公告
    /// </summary>
/****
    private IEnumerator CheckNotice()
    {
        WWW www = new WWW(Initiator.instance.cdnUrl + "notice/" + VersionManager.localVersion + ".txt");
        yield return www;

        if (www.error == null)
        {
            NoticeManager.Setup(www.data);
        }

    }
 * */
    #endregion

    private void Preload()
    {
        /**SceneLoadingPanel.Instance.SetPercent(0.5f);*/
        ResourceManager.LoadABFileInfo(LoadNecessaryRes);
    }

    private void LoadNecessaryRes()
    {
        ResourceManager.Preload(InitGame);
    }

    private void InitGame()
    {
        /*
        SceneLoadingPanel.Instance.SetTitle("正在初始化资源...");
        SceneLoadingPanel.Instance.SetPercent(0.75f);
        TimeManager.Setup();
        StageManager.Setup();
        ModuleManager.Setup();
        ConfigManager.AddEventListener(BaseEvent.COMPLETE, InitGameOver);
        ConfigManager.Setup();
        
        gameObject.AddComponent<ShareTool>();
         * */
    }

    private void InitGameOver(BaseEvent evt)
    {
        /*
        ConfigManager.RemoveEventListener(BaseEvent.COMPLETE, InitGameOver);
        Login();
         * */
    }

    private void InitPersistantData()
    {
        /**
        if(PlayerPrefs.HasKey(PersistentUtil.OPERATE_HAND_KEY))
        {
            int operateHand = PlayerPrefs.GetInt(PersistentUtil.OPERATE_HAND_KEY);
            Global.isLeftHand = (operateHand == 1);
        }
        if (PlayerPrefs.HasKey(PersistentUtil.JOYSTICK_FOLLOW_KEY))
        {
            int joystickFollow = PlayerPrefs.GetInt(PersistentUtil.JOYSTICK_FOLLOW_KEY);
            Global.isJoystickFollow = (joystickFollow == 1);
        }
        if (PlayerPrefs.HasKey(PersistentUtil.MICROPHONE_KEY))
        {
            int microphone = PlayerPrefs.GetInt(PersistentUtil.MICROPHONE_KEY);
            Global.IsShowMicphone = (microphone == 1);
        }
         * */
    }

    public void OnApplicationQuit()
    {
        /*
        ThreadManager.Stop();
        SocketManager.Close();
        TeamSocketManager.Close();
         * */
    }

}
