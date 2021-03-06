﻿using UnityEngine;
using System;
using System.Collections;
using KBEngine;

/*
	可以理解为插件的入口模块
*/
	
public class KBEMain : MonoBehaviour 
{	
	// 在unity3d界面中可见选项
	public DEBUGLEVEL debugLevel = DEBUGLEVEL.DEBUG;
    static public bool isStartEngine = false;

	void Awake() 
	 {
		DontDestroyOnLoad(transform.gameObject);
	 }
 
	// Use this for initialization
	void Start () 
	{
		MonoBehaviour.print("clientapp::start()");
        KBEngine.Event.registerIn("onResourceInitFinish", this, "onResourceInitFinish");
	}

    public void onResourceInitFinish()
    {
        initKBEngine();
    }
	
	public virtual void initKBEngine()
	{
        isStartEngine = true;
		Dbg.debugLevel = debugLevel;

        KBEngine.Event.registerIn("_closeNetwork", this, "_closeNetwork");

        LuaFramework.Util.CallMethod("KBEngineLua", "InitEngine");
	}

    public void _closeNetwork(NetworkInterface networkInterface)
    {
        networkInterface.close();
    }	

	void OnDestroy()
	{
		MonoBehaviour.print("clientapp::OnDestroy(): begin");

        Dbg.WARNING_MSG("KBEngine::destroy()");
        KBEngine.Event.deregisterIn(this);
        LuaFramework.Util.CallMethod("KBEngineLua", "Destroy");

		MonoBehaviour.print("clientapp::OnDestroy(): end");
	}
	
	void FixedUpdate () 
	{
        // 处理外层抛入的事件
        KBEngine.Event.processInEvents();
        KBEngine.Event.processOutEvents();
	}

}
