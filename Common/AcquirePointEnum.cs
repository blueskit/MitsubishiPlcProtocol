using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.Common
{

	/// <summary>
	/// 所有IO点/软单元枚举值
	/// </summary>
	public enum AcquirePointEnum : int
	{
		/// <summary>
		/// 复位PLC所有
		/// </summary>
		ResetAll = 30240,

		/// <summary>
		/// 拌缸电流
		/// </summary>
		MixCurrent = 1001,

		/// <summary>
		/// 拌缸温度
		/// </summary>
		MixTemperature = 1002,


		/// <summary>
		/// 拌缸
		/// </summary>
		MixFan_Y = 20100,

		/// <summary>
		/// 振动筛
		/// </summary>
		ShakeSieve_Y = 20070,

		/// <summary>
		/// 集料输送
		/// </summary>
		ColdBelt_Y = 20000,

		/// <summary>
		/// 上料输送
		/// </summary>
		BurntBelt_Y = 20010,

		/// <summary>
		/// 石料提升机
		/// </summary>
		StoneLifter_Y = 20060,

		/// <summary>
		/// 烘筒
		/// </summary>
		Burnt_Y = 20020,

		/// <summary>
		/// 烘筒启动
		/// </summary>
		Burnt_M = 30002,

		/// <summary>
		/// 燃烧器运行
		/// </summary>
		BurntFire_X = 10040,

		/// <summary>
		/// 烘筒电机
		/// </summary>
		BurntMotor_Y = 20040,

		/// <summary>
		/// 烘筒风扇
		/// </summary>
		BurntFan_Y = 20050,

		/// <summary>
		/// 旋风排灰阀
		/// </summary>
		WindBarrel_Y = 20310,

		/// <summary>
		/// 尾气阀
		/// </summary>
		TailGasBarrel_Y = 21160,

		/// <summary>
		/// 本体绞龙1
		/// </summary>
		DustTD1_Y = 20250,

		/// <summary>
		/// 本体绞龙2
		/// </summary>
		DustTD2_Y = 20260,

		/// <summary>
		/// 回收粉进秤绞龙
		/// </summary>
		RecoveryPowderWeightTD_Y = 20710,

		/// <summary>
		/// 回收粉进秤绞龙
		/// </summary>
		RecoveryPowderWeightTD_X = 10300,

		/// <summary>
		/// 矿粉进秤开关
		/// </summary>
		SlagValveSwith_M = 30221,

		/// <summary>
		/// 矿粉进秤绞龙
		/// </summary>
		SlagWeightTD_Y = 20660,

		/// <summary>
		/// 矿粉进秤绞龙
		/// </summary>
		SlagWeightTD_M = 30222,

		/// <summary>
		/// 快速绞龙
		/// </summary>
		SpeedUpTD_X = 11412,

		/// <summary>
		/// 手动模式
		/// </summary>
		ManualMode_M = 30018,

		/// <summary>
		/// 自动模式
		/// </summary>
		AutoMode_M = 30019,

		/// <summary>
		/// 电机手动
		/// </summary>
		MotorManualMode_M = 30028,

		/// <summary>
		/// 电机自动模式
		/// </summary>
		MotorAutoMode_M = 30029,

		/// <summary>
		/// 壁振1
		/// </summary>
		Shake1_Y = 20460,

		/// <summary>
		/// 壁振2
		/// </summary>
		Shake2_Y = 20470,

		/// <summary>
		/// 壁振1
		/// </summary>
		Shake1_M = 30206,

		/// <summary>
		/// 壁振2
		/// </summary>
		Shake2_M = 30207,

		/// <summary>
		/// 布袋工作时间
		/// </summary>
		BagWorkTime = 40200,

		/// <summary>
		/// 布袋等待时间
		/// </summary>
		BagWaitTime = 40210,

		/// <summary>
		/// 石料计量斗开
		/// </summary>
		StoneValveSwitchOn = 20640,

		/// <summary>
		/// 回收粉进秤开关
		/// </summary>
		RecoveryPowderSwitch_M = 30224,

		/// <summary>
		/// 矿粉罐低
		/// </summary>
		SlagBarrelLow_M = 11420,

		/// <summary>
		/// 矿粉罐高
		/// </summary>
		SlagBarrelHigh_M = 11430,

		/// <summary>
		/// 矿粉过渡罐低
		/// </summary>
		SlagTransitBarrelLow_M = 11431,

		/// <summary>
		/// 矿粉过渡罐罐高
		/// </summary>
		SlagTransitBarrelHigh_M = 11432,


		/// <summary>
		/// 回收粉(废粉)罐低
		/// </summary>
		RecoveryPowderBarrelLow_M = 11440,

		/// <summary>
		/// 回收粉(废粉)罐高
		/// </summary>
		RecoveryPowderBarrelHigh_M = 11450,

		/// <summary>
		/// 废粉过渡罐低
		/// </summary>
		RecoveryPowderTransitBarrelLow_M = 11400,

		/// <summary>
		/// 废粉过渡罐罐高
		/// </summary>
		RecoveryPowderTransitBarrelHigh_M = 11410,



		/// <summary>
		/// 石料计量斗开
		/// </summary>
		StoneDropDoor_M = 30220,

		/// <summary>
		/// 矿粉计量斗开
		/// </summary>
		SlagDropDoor_M = 30223,

		/// <summary>
		/// 拌缸门开
		/// </summary>
		MixerDropValve = 30228,

		/// <summary>
		/// 拌缸门开(行程开关指示)
		/// </summary>
		MixerDoorOpening_X = 10110,

		#region 操作M点

		/// <summary>
		/// 集料输送启动
		/// </summary>
		User_StoneBeltStart = 30000,

		/// <summary>
		/// 集料输送停止
		/// </summary>
		User_StoneBeltStop = 30050,

		/// <summary>
		/// 上料输送启动
		/// </summary>
		User_BurntBeltStart = 30001,

		/// <summary>
		/// 上料输送停止
		/// </summary>
		User_BurntBeltStop = 30051,

		/// <summary>
		/// 烘筒启动
		/// </summary>
		User_BurntStart = 30002,

		/// <summary>
		/// 烘筒停止
		/// </summary>
		User_BurntStop = 30052,

		/// <summary>
		/// 石料提升机启动
		/// </summary>
		User_StoneUpStart = 30006,

		/// <summary>
		/// 石料提升机停止
		/// </summary>
		User_StoneUpStop = 30056,

		/// <summary>
		/// 振动筛启动
		/// </summary>
		User_ShakeSieveStart = 30007,

		/// <summary>
		/// 振动筛停止
		/// </summary>
		User_ShakeSieveStop = 30057,

		/// <summary>
		/// 拌缸启动
		/// </summary>
		User_MixFanStart = 30010,

		/// <summary>
		/// 拌缸停止
		/// </summary>
		User_MixFanStop = 30060,

		/// <summary>
		/// 矿粉提升机启动
		/// </summary>
		User_SlagUpStart = 30012,

		/// <summary>
		/// 矿粉提升机停止
		/// </summary>
		User_SlagUpStop = 30062,

		/// <summary>
		/// 回收粉提升机启动
		/// </summary>
		User_RecoveryPowderUpStart = 30013,

		/// <summary>
		/// 回收粉提升机停止
		/// </summary>
		User_RecoveryPowderUpStop = 30063,

		/// <summary>
		/// 空压机
		/// </summary>
		User_AirCompressor = 31016,

		#endregion

		#region 指示M点

		/// <summary>
		/// 集料输送运行指示
		/// </summary>
		OR_StoneBelt = 31000,

		/// <summary>
		/// 上料输送运行指示
		/// </summary>
		OR_BurntBelt = 31001,

		/// <summary>
		/// 烘筒运行指示
		/// </summary>
		OR_Burnt = 31002,

		/// <summary>
		/// 石料提升机运行指示
		/// </summary>
		OR_StoneUp = 31006,

		/// <summary>
		/// 振动筛运行指示
		/// </summary>
		OR_ShakeSieve = 31007,

		/// <summary>
		/// 拌缸运行指示
		/// </summary>
		OR_MixFan = 31010,

		/// <summary>
		/// 矿粉提升指示
		/// </summary>
		OR_SlagUp = 31012,

		/// <summary>
		/// 回收粉提升指示
		/// </summary>
		OR_RecoveryPowderUp = 31013,

		/// <summary>
		/// 沥青泵运行指示
		/// </summary>
		OR_AsphaltPump = 31014,

		/// <summary>
		/// 喷射泵运行指示
		/// </summary>
		OR_JetPump = 31015,

		/// <summary>
		/// 空压机指示
		/// </summary>
		OR_Compressor = 31016,

		/// <summary>
		/// 回收粉提升机送粉绞龙
		/// </summary>
		OR_RecoveryPowderUpTD = 31017,

		/// <summary>
		/// 回收粉回用绞龙指示
		/// </summary>
		OR_RecoveryPowderReuseTD = 31038,

		/// <summary>
		/// 矿粉送粉绞龙指示
		/// </summary>
		OR_SlagTD = 31020,

		/// <summary>
		/// 本体绞龙1指示
		/// </summary>
		OR_DustTD1 = 31021,

		/// <summary>
		/// 本体绞龙2指示
		/// </summary>
		OR_DustTD2 = 31023,

		/// <summary>
		/// 排灰阀1指示
		/// </summary>
		OR_ExcludeAsh1 = 31022,

		/// <summary>
		/// 排灰阀2指示
		/// </summary>
		OR_ExcludeAsh2 = 31024,

		/// <summary>
		/// 回收粉绞龙1指示
		/// </summary>
		OR_RecoveryPowderTD1 = 31025,

		/// <summary>
		/// 回收粉绞龙2指示
		/// </summary>
		OR_RecoveryPowderTD2 = 31026,

		/// <summary>
		/// 旋风排灰阀指示
		/// </summary>
		OR_TornadoExcludeAsh = 31031,

		/// <summary>
		/// 卸油泵指示
		/// </summary>
		OR_UnloadOilPump = 31032,

		/// <summary>
		/// 回收粉外排绞龙指示
		/// </summary>
		OR_RecoveryPowderOutTD = 31033,

		/// <summary>
		/// 引风机指示
		/// </summary>
		OR_DraughtFan = 31037,

		/// <summary>
		/// 小车上限位
		/// </summary>
		OR_DollyUpLimit = 31108,

		/// <summary>
		/// 小车下限位
		/// </summary>
		OR_DollyDownLimit = 31109,

		/// <summary>
		/// 速度1 
		/// </summary>
		OR_Speed1 = 31118,

		/// <summary>
		/// 速度3
		/// </summary>
		OR_Speed3 = 31129,

		/// <summary>
		/// 1号仓限位
		/// </summary>
		OR_WarehouseLimit = 31119,

		/// <summary>
		/// 废料仓限位
		/// </summary>
		OR_WastehouseLimit = 31128,

		/// <summary>
		/// 1号仓减速
		/// </summary>
		OR_WarehouseDeceleration1 = 31139,

		/// <summary>
		/// 2号仓减速
		/// </summary>
		OR_WarehouseDeceleration2 = 31138,
		#endregion

		#region 料仓位指示

		MaterialLow1_M = 11240,
		MaterialLow2_M = 11250,
		MaterialLow3_M = 11260,
		MaterialLow4_M = 11270,
		MaterialLow5_M = 11300,
		MaterialLow6_M = 11310,

		MaterialHigh1_M = 11320,
		MaterialHigh2_M = 11330,
		MaterialHigh3_M = 11340,
		MaterialHigh4_M = 11350,
		MaterialHigh5_M = 11360,
		MaterialHigh6_M = 11370,
		#endregion

		#region 石料计量门

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse01_M = 30208,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse02_M = 30209,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse03_M = 30210,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse04_M = 30211,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse05_M = 30212,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse06_M = 30213,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse01_Y = 20500,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse02_Y = 20510,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse03_Y = 20520,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse04_Y = 20530,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse05_Y = 20540,

		/// <summary>
		/// 石料粗计量
		/// </summary>
		Coarse06_Y = 20550,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro01_M = 30214,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro02_M = 30215,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro03_M = 30216,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro04_M = 30217,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro05_M = 30218,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro06_M = 30219,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro01_Y = 20560,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro02_Y = 20570,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro03_Y = 20600,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro04_Y = 20610,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro05_Y = 20620,

		/// <summary>
		/// 石料微计量
		/// </summary>
		Micro06_Y = 20630,

		#endregion

		#region 秤


		/// <summary>
		/// 石料秤
		/// </summary>
		Weight_Stone = 3100,

		/// <summary>
		/// 沥青秤
		/// </summary>
		Weight_Asphalt = 3200,

		/// <summary>
		/// 矿粉秤
		/// </summary>
		Weight_Slag = 3300,

		#endregion

		#region 冷料斗

		/// <summary>
		/// 1#冷料斗速度
		/// </summary>
		ColdSpeed1 = 2011,

		/// <summary>
		/// 1#冷料斗给料空
		/// </summary>
		ColdFeedEmpty1_M = 30144,

		/// <summary>
		/// 1#冷料斗运行
		/// </summary>
		ColdFeed1_Y = 20400,

		/// <summary>
		/// 1#冷料斗运行
		/// </summary>
		ColdFeed1_M = 30200,

		/// <summary>
		/// 2#冷料斗速度
		/// </summary>
		ColdSpeed2 = 2021,

		/// <summary>
		/// 2#冷料斗给料空
		/// </summary>
		ColdFeedEmpty2_M = 30145,

		/// <summary>
		/// 2#冷料斗运行
		/// </summary>
		ColdFeed2_Y = 20410,

		/// <summary>
		/// 2#冷料斗运行
		/// </summary>
		ColdFeed2_M = 30201,

		/// <summary>
		/// 3#冷料斗速度
		/// </summary>
		ColdSpeed3 = 2031,

		/// <summary>
		/// 3#冷料斗给料空
		/// </summary>
		ColdFeedEmpty3_M = 30146,

		/// <summary>
		/// 3#冷料斗运行
		/// </summary>
		ColdFeed3_Y = 20420,

		/// <summary>
		/// 3#冷料斗运行
		/// </summary>
		ColdFeed3_M = 30202,

		/// <summary>
		/// 4#冷料斗速度
		/// </summary>
		ColdSpeed4 = 2041,

		/// <summary>
		/// 4#冷料斗给料空
		/// </summary>
		ColdFeedEmpty4_M = 30147,

		/// <summary>
		/// 4#冷料斗运行
		/// </summary>
		ColdFeed4_Y = 20430,

		/// <summary>
		/// 4#冷料斗运行
		/// </summary>
		ColdFeed4_M = 30203,

		/// <summary>
		/// 5#冷料斗速度
		/// </summary>
		ColdSpeed5 = 2051,

		/// <summary>
		/// 5#冷料斗给料空
		/// </summary>
		ColdFeedEmpty5_M = 30148,

		/// <summary>
		/// 5#冷料斗运行
		/// </summary>
		ColdFeed5_Y = 20440,

		/// <summary>
		/// 5#冷料斗运行
		/// </summary>
		ColdFeed5_M = 30204,

		/// <summary>
		/// 6#冷料斗速度
		/// </summary>
		ColdSpeed6 = 2061,

		/// <summary>
		/// 6#冷料斗给料空
		/// </summary>
		ColdFeedEmpty6_M = 30149,

		/// <summary>
		/// 6#冷料斗运行
		/// </summary>
		ColdFeed6_Y = 20450,

		/// <summary>
		/// 6#冷料斗运行
		/// </summary>
		ColdFeed6_M = 30205,
		#endregion

		#region 除尘器布袋

		/// <summary>
		/// 1#布袋
		/// </summary>
		DustBag01_Y = 21000,

		/// <summary>
		/// 2#布袋
		/// </summary>
		DustBag02_Y = 21010,

		/// <summary>
		/// 3#布袋
		/// </summary>
		DustBag03_Y = 21020,

		/// <summary>
		/// 4#布袋
		/// </summary>
		DustBag04_Y = 21030,

		/// <summary>
		/// 5#布袋
		/// </summary>
		DustBag05_Y = 21040,

		/// <summary>
		/// 6#布袋
		/// </summary>
		DustBag06_Y = 21050,

		/// <summary>
		/// 7#布袋
		/// </summary>
		DustBag07_Y = 21060,

		/// <summary>
		/// 8#布袋
		/// </summary>
		DustBag08_Y = 21070,

		/// <summary>
		/// 9#布袋
		/// </summary>
		DustBag09_Y = 21100,

		/// <summary>
		/// 10#布袋
		/// </summary>
		DustBag10_Y = 21110,

		/// <summary>
		/// 11#布袋
		/// </summary>
		DustBag11_Y = 21120,

		/// <summary>
		/// 12#布袋
		/// </summary>
		DustBag12_Y = 21130,

		/// <summary>
		/// 13#布袋
		/// </summary>
		DustBag13_Y = 21140,

		/// <summary>
		/// 14#布袋
		/// </summary>
		DustBag14_Y = 21150,
		#endregion

		#region 沥青

		/// <summary>
		/// 沥青泵
		/// </summary>
		AsphaltPump_Y = 20140,

		/// <summary>
		/// 喷射泵
		/// </summary>
		JetPump_Y = 20150,

		/// <summary>
		/// 沥青进口阀门
		/// </summary>
		AsphaltFeedIn_Y = 20720,

		/// <summary>
		/// 沥青进油阀
		/// </summary>
		AsphaltFeedIn_M = 30226,

		/// <summary>
		/// 沥青卸油阀
		/// </summary>
		AsphaltFeedOut_Y = 20730,

		/// <summary>
		/// 沥青卸油阀
		/// </summary>
		AsphaltFeedOut_M = 30227,
		#endregion
	}
}
