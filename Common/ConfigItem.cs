using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	public class ConfigItem : EventArgs
	{
		public ConfigItemEnum Key { get; set; }
		public string KeyName { get; set; }
		public double Value { get; set; }
		public string Comment { get; set; }

		public ConfigItem()
			: this(ConfigItemEnum.Dolly_DecelerationPulse, null, 0d, null)
		{
		}

		public ConfigItem(ConfigItemEnum key, string keyname)
			: this(key, keyname, 0d, null)
		{
		}

		public ConfigItem(ConfigItemEnum key, string keyname, double value, string comment)
		{
			Key = key;
			KeyName = keyname;
			Value = value;
			Comment = comment;
		}
	}

	/// <summary>
	/// 配置项枚举编码
	/// </summary>
	public enum ConfigItemEnum : int
	{
		UnDefined = 0,

		#region 主界面配置
		UI_ScanInterval = -1000,
		#endregion

		#region 参数设置属性
		/// <summary>
		/// 除尘风门全部开启时间
		/// </summary>
		System_DustOpenTime = 1,
		/// <summary>
		/// 除尘风门设为自动时间
		/// </summary>
		System_DustAutoTime,
		/// <summary>
		/// 除尘风门开度设定
		/// </summary>
		System_DustOpenAngle,
		/// <summary>
		/// 拌缸料满电流
		/// </summary>
		System_MixFullCurrent,
		/// <summary>
		/// 拌缸开门时间
		/// </summary>
		System_MixOpenTime,
		/// <summary>
		/// 振动筛延迟停止时间
		/// </summary>
		System_ShakeStopTime,
		/// <summary>
		/// 骨料秤延迟进料时间
		/// </summary>
		System_BoneFeedInTime,
		/// <summary>
		/// 石粉秤延迟进料时间
		/// </summary>
		System_StoneFeedInTime,
		/// <summary>
		/// 沥青秤延迟进料时间
		/// </summary>
		System_AsphaltFeedInTime,
		/// <summary>
		/// 燃烧器风门开/关时间
		/// </summary>
		System_BurntSwitchTime,
		/// <summary>
		/// 燃烧器自动控制时间
		/// </summary>
		System_BurntAutoTime,
		/// <summary>
		/// 骨料秤上限
		/// </summary>
		System_BoneMax,
		/// <summary>
		/// 石粉秤上限
		/// </summary>
		System_StoneMax,
		/// <summary>
		/// 沥青秤上限
		/// </summary>
		System_AsphaltMax,
		/// <summary>
		/// SMA秤上限
		/// </summary>
		System_SMAMax,
		/// <summary>
		/// 滚桶润滑油阀开时间
		/// </summary>
		System_BarrelOpen,
		/// <summary>
		/// 滚桶润滑油阀关时间
		/// </summary>
		System_BarrelClose,
		/// <summary>
		/// 最后一批拌合时间
		/// </summary>
		System_LastMixTime,
		/// <summary>
		/// 报警器开时间
		/// </summary>
		System_AnnunciatorOpen,
		/// <summary>
		/// 石粉秤就绪延迟
		/// </summary>
		System_StoneReadyTime,
		/// <summary>
		/// SMA秤放料延迟
		/// </summary>
		System_SMAFeedOutTime,
		/// <summary>
		/// 沥青秤放料延迟
		/// </summary>
		System_AsphaltFeedOutTime,
		/// <summary>
		/// 石粉秤放料延迟
		/// </summary>
		System_StoneFeedOutTime,
		/// <summary>
		/// 小车启动时间
		/// </summary>
		System_DollyStartTime,
		/// <summary>
		/// 小车放料延迟
		/// </summary>
		System_DollyFeedOutTime,
		/// <summary>
		/// 沥青进料增加量
		/// </summary>
		System_AsphaltFeedAdd,
		/// <summary>
		/// 门关落差时间
		/// </summary>
		System_FinishCloseTime,
		#endregion

		#region 校正参数属性
		/// <summary>
		/// 1#冷料斗速度
		/// </summary>
		Revise_ColdSpeed1,
		/// <summary>
		/// 2#冷料斗速度
		/// </summary>
		Revise_ColdSpeed2,
		/// <summary>
		/// 3#冷料斗速度
		/// </summary>
		Revise_ColdSpeed3,
		/// <summary>
		/// 4#冷料斗速度
		/// </summary>
		Revise_ColdSpeed4,
		/// <summary>
		/// 5#冷料斗速度
		/// </summary>
		Revise_ColdSpeed5,
		/// <summary>
		/// 6#冷料斗速度
		/// </summary>
		Revise_ColdSpeed6,
		/// <summary>
		/// 拌缸温度调零
		/// </summary>
		Revise_MixTempZero,
		/// <summary>
		/// 拌缸温度校正
		/// </summary>
		Revise_MixTempRevise,
		/// <summary>
		/// 沥青温度调零
		/// </summary>
		Revise_AsphaltTempZero,
		/// <summary>
		/// 沥青温度校正
		/// </summary>
		Revise_AsphaltTempRevise,
		/// <summary>
		/// 成品仓温度调零
		/// </summary>
		Revise_FinishTempZero,
		/// <summary>
		/// 成品仓温度校正
		/// </summary>
		Revise_FinishTempRevise,
		/// <summary>
		/// 沙仓温度调零
		/// </summary>
		Revise_SandTempZero,
		/// <summary>
		/// 沙仓温度校正
		/// </summary>
		Revise_SandTempRevise,
		/// <summary>
		/// 尾气温度调零
		/// </summary>
		Revise_ExhaustGasTempZero,
		/// <summary>
		/// 尾气温度校正
		/// </summary>
		Revise_ExhaustGasTempRevise,
		/// <summary>
		/// 滚桶负压调零
		/// </summary>
		Revise_BarrelZero,
		/// <summary>
		/// 滚桶负压校正
		/// </summary>
		Revise_BarrelRevise,
		/// <summary>
		/// 骨料秤调零
		/// </summary>
		Revise_BoneZero,
		/// <summary>
		/// 骨料称校正
		/// </summary>
		Revise_BoneRevise,
		/// <summary>
		/// 沥青秤调零
		/// </summary>
		Revise_AsphaltZero,
		/// <summary>
		/// 沥青秤校正
		/// </summary>
		Revise_AsphaltRevise,
		/// <summary>
		/// 石粉秤调零
		/// </summary>
		Revise_StoneZero,
		/// <summary>
		/// 石粉秤校正
		/// </summary>
		Revise_StoneRevise,
		/// <summary>
		/// SMA秤调零
		/// </summary>
		Revise_SMAZero,
		/// <summary>
		/// SMA秤校正
		/// </summary>
		Revise_SMARevise,
		/// <summary>
		/// 1#热料仓料位调零
		/// </summary>
		Revise_HotBitZero1,
		/// <summary>
		/// 1#热料仓料位校正
		/// </summary>
		Revise_HotBitRevise1,
		/// <summary>
		/// 2#热料仓料位调零
		/// </summary>
		Revise_HotBitZero2,
		/// <summary>
		/// 2#热料仓料位校正
		/// </summary>
		Revise_HotBitRevise2,
		/// <summary>
		/// 3#热料仓料位调零
		/// </summary>
		Revise_HotBitZero3,
		/// <summary>
		/// 3#热料仓料位校正
		/// </summary>
		Revise_HotBitRevise3,
		/// <summary>
		/// 4#热料仓料位调零
		/// </summary>
		Revise_HotBitZero4,
		/// <summary>
		/// 4#热料仓料位校正
		/// </summary>
		Revise_HotBitRevise4,
		/// <summary>
		/// 5#热料仓料位调零
		/// </summary>
		Revise_HotBitZero5,
		/// <summary>
		/// 5#热料仓料位校正
		/// </summary>
		Revise_HotBitRevise5,
		/// <summary>
		/// 6#热料仓料位调零
		/// </summary>
		Revise_HotBitZero6,
		/// <summary>
		/// 6#热料仓料位校正
		/// </summary>
		Revise_HotBitRevise6,
		/// <summary>
		/// 滚桶电流校正
		/// </summary>
		Revise_BarrelCurrent,
		/// <summary>
		/// 引风机电流校正
		/// </summary>
		Revise_WindCurrent,
		/// <summary>
		/// 鼓风机电流校正
		/// </summary>
		Revise_FanCurrent,
		/// <summary>
		/// 拌缸电流校正
		/// </summary>
		Revise_MixCurrent,
		#endregion

		#region 限制参数属性

		/// <summary>
		/// 热料配方调用批量限值
		/// </summary>
		Limit_HotLimit,
		/// <summary>
		/// 冷料供给速度限值
		/// </summary>
		Limit_ColdFeedLimit,
		/// <summary>
		/// 1#冷料斗产量限值
		/// </summary>
		Limit_ColdYield1,
		/// <summary>
		/// 2#冷料斗产量限值
		/// </summary>
		Limit_ColdYield2,
		/// <summary>
		/// 3#冷料斗产量限值
		/// </summary>
		Limit_ColdYield3,
		/// <summary>
		/// 4#冷料斗产量限值
		/// </summary>
		Limit_ColdYield4,
		/// <summary>
		/// 5#冷料斗产量限值
		/// </summary>
		Limit_ColdYield5,
		/// <summary>
		/// 6#冷料斗产量限值
		/// </summary>
		Limit_ColdYield6,

		#endregion

		#region 小城参数属性

		/// <summary>
		/// 1#仓脉冲
		/// </summary>
		Dolly_StoreHousePulse1,
		/// <summary>
		/// 2#仓脉冲
		/// </summary>
		Dolly_StoreHousePulse2,
		/// <summary>
		/// 废料仓脉冲
		/// </summary>
		Dolly_ScrapPulse,
		/// <summary>
		/// 小车停止脉冲
		/// </summary>
		Dolly_StopPulse,
		/// <summary>
		/// 减速脉冲
		/// </summary>
		Dolly_DecelerationPulse,
		/// <summary>
		/// 超行程脉冲
		/// </summary>
		Dolly_ExtraPulse,
		/// <summary>
		/// 快上脉冲
		/// </summary>
		Dolly_FastUpPulse,
		/// <summary>
		/// 快下脉冲
		/// </summary>
		Dolly_FastDownPulse,
		/// <summary>
		/// 慢上脉冲
		/// </summary>
		Dolly_SlowUpPulse,
		/// <summary>
		/// 慢下脉冲
		/// </summary>
		Dolly_SlowDownPulse,

		#endregion

		#region 慢速放料属性

		/// <summary>
		/// 粗料
		/// </summary>
		Before_Cold01,
		/// <summary>
		/// 大料
		/// </summary>
		Before_Cold02,
		/// <summary>
		/// 中料
		/// </summary>
		Before_Cold03,
		/// <summary>
		/// 小料
		/// </summary>
		Before_Cold04,
		/// <summary>
		/// 细料
		/// </summary>
		Before_Cold05,
		/// <summary>
		/// 直料
		/// </summary>
		Before_Cold06,
		/// <summary>
		/// 沥青
		/// </summary>
		Before_Asphalt,

		#endregion

		#region 落差参数属性

		/// <summary>
		/// 粗料落差
		/// </summary>
		Fall_Hot01,
		/// <summary>
		/// 大料落差
		/// </summary>
		Fall_Hot02,
		/// <summary>
		/// 中料落差
		/// </summary>
		Fall_Hot03,
		/// <summary>
		/// 小料落差
		/// </summary>
		Fall_Hot04,
		/// <summary>
		/// 细料落差
		/// </summary>
		Fall_Hot05,
		/// <summary>
		/// 直料落差
		/// </summary>
		Fall_Hot06,
		/// <summary>
		/// 回收粉落差
		/// </summary>
		Fall_Hot07,
		/// <summary>
		/// 添加粉落差
		/// </summary>
		Fall_Hot08,
		/// <summary>
		/// 沥青落差
		/// </summary>
		Fall_Hot09,
		/// <summary>
		/// SMA落差
		/// </summary>
		Fall_Hot10,

		#endregion


		#region 余量设定

		/// <summary>
		/// 矿粉计量斗余量
		/// </summary>
		Remain_SlagWeighingHopper,

		/// <summary>
		/// 石料计量斗余量
		/// </summary>
		Remain_StoneWeighingHopper,

		#endregion
	}
}

