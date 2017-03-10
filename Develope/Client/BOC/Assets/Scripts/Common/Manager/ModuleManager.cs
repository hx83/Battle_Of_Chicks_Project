using System.Collections.Generic;
using System;
public class ModuleManager
{
    private static Dictionary<string, AppModuleProxy> _moduleDictionary = new Dictionary<string, AppModuleProxy>();
    private static Dictionary<string, Type> _launchClassDic;
    private static List<AppModuleProxy> _showingList;
    public static void Setup()
    {
        _moduleDictionary = new Dictionary<string, AppModuleProxy>();
        _launchClassDic = new Dictionary<string, Type>();
        /*
        _launchClassDic.Add(ModuleType.LOGIN_PANEL, typeof(LoginPanel));
        _launchClassDic.Add(ModuleType.NOTICE_PANEL, typeof(NoticePanel));
        _launchClassDic.Add(ModuleType.SHARE_PANEL, typeof(ShareGamePanel));

        _launchClassDic.Add(ModuleType.ACCOUNTSETTING_PANEL, typeof(AccountSettingPanel));
        _launchClassDic.Add(ModuleType.ACCOUNTLOGIN_PANEL, typeof(AccountLoginPanel));

		_launchClassDic.Add(ModuleType.FRIEND_ENTRY, typeof(FriendUIEntry));
		_launchClassDic.Add(ModuleType.FRIEND_RELATION, typeof(FriendUIRelation));
		_launchClassDic.Add(ModuleType.FRIEND_FINDPLAYER, typeof(FriendUIFindPlayer));
		_launchClassDic.Add(ModuleType.FRIEND_RADAR, typeof(FriendUIRadar));
		_launchClassDic.Add(ModuleType.FRIEND_YAOYIYAO, typeof(FriendUIYaoYiYao));
		_launchClassDic.Add(ModuleType.GAMETRACK, typeof(UIGameTrack));
		_launchClassDic.Add (ModuleType.FRIEND_QRCODE,typeof(FriendQRCode));

        _launchClassDic.Add(ModuleType.SIGN_PANELl, typeof(SignPanel));
        _launchClassDic.Add(ModuleType.SHARESCREENSHOT_PANEL, typeof(ShareScreenshotPanel));

        _launchClassDic.Add(ModuleType.HELPER, typeof(UIHelper));

        // Shop
        _launchClassDic.Add(ModuleType.SHOPUI, typeof(ShopUI));
        _launchClassDic.Add(ModuleType.GLOBALRANKUI, typeof(GlobalRankUI));
        _launchClassDic.Add(ModuleType.WEEKLYAWARDUI, typeof(WeeklyAwardUI));

        _launchClassDic.Add(ModuleType.GAMESETTING_PANEL, typeof(GameSettingPanel));

        _launchClassDic.Add(ModuleType.REWARD_PANEL, typeof(RewardPanel));

        _launchClassDic.Add(ModuleType.RANKSTAR_PANEL, typeof(RankStarPanel));

        _launchClassDic.Add(ModuleType.TOP_REMIND_PANEL, typeof(TopRemindPanel));

        _launchClassDic.Add(ModuleType.SURVEY_PANEL, typeof(SurveyPanel));

        _launchClassDic.Add(ModuleType.Team_Panel, typeof(TeamPanel));

        _launchClassDic.Add(ModuleType.TeamInvite_Panel, typeof(TeamInvitePanel));

        _launchClassDic.Add(ModuleType.GAME_EXIT_PANEL, typeof(GameExitPanel));

        _launchClassDic.Add(ModuleType.BATTLE_RESULT_PANEL, typeof(BattleResultPanel));

        _launchClassDic.Add(ModuleType.TEAM_BATTLE_RESULT_PANEL, typeof(TeamBattleResultPanel));
        */
        _showingList = new List<AppModuleProxy>();
    }

    public static void HideAll()
    {
        foreach(AppModuleProxy module in _moduleDictionary.Values)
        {
            module.Hide();
        }
        _showingList.Clear();
    }

    public static void DisposeAll()
    {
        foreach (AppModuleProxy module in _moduleDictionary.Values)
        {
            module.Destroy();
        }
        _moduleDictionary.Clear();
        _showingList.Clear();
    }

    public static void Show(string type, object data = null)
    {
        AppModuleProxy appModule;
        if (_moduleDictionary.ContainsKey(type))
        {
            appModule = _moduleDictionary[type];
        }
        else
        {
            appModule = new AppModuleProxy(type, _launchClassDic[type]);        
            _moduleDictionary.Add(type, appModule);
        }
        appModule.SetSiblingIndex(_moduleDictionary.Count - 1);
        appModule.Show(data);
        //if(type != ModuleType.LOGIN_PANEL)
        //{
        //    Hide(ModuleType.LOGIN_PANEL);
        //}
        _showingList.Add(appModule);
        DispathcEvent(ModuleEvent.SHOW, type);
    }

    public static void Hide(string type)
    {
        AppModuleProxy appModule = _moduleDictionary[type];
        appModule.Hide();
        if (_showingList.Contains(appModule))
            _showingList.Remove(appModule);
        //if (_showingList.Count == 0)
        //    Show(ModuleType.LOGIN_PANEL);

        DispathcEvent(ModuleEvent.HIDE, type);
    }

    //----------------------------------------
    //event Listener
    //----------------------------------------

    private static EventDispatcher _eventDispatcher = new EventDispatcher();

    public static bool HasEventListener(string type)
    {
        return _eventDispatcher.HasEventListener(type);
    }

    public static void AddEventListener(string type, Action<BaseEvent> listener)
    {
        _eventDispatcher.AddEventListener(type, listener);
    }

    public static void RemoveEventListener(string type, Action<BaseEvent> listener)
    {
        _eventDispatcher.RemoveEventListener(type, listener);
    }

    public static void DispathcEvent(string type, object eventObj = null)
    {
        _eventDispatcher.DispatchEvent(type, eventObj);
    }
}
