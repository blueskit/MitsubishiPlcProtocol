using System;

using Vila.Communication;
using Vila.Communication.Common;
using Vila.Communication.Data;

namespace InControls.Common
{

	public enum INC_APP_TYPE : byte
	{
		AsphaltMixAppType = VikiAppTypeConst.VK_APP_ASPHALTMIX_TYPE,				// 沥青系统
		BetonMixAppType = VikiAppTypeConst.VK_APP_BETONMIX_TYPE,					// 混凝土系统
	}

	/// <summary>
	/// 沥青系统 应用命令枚举值
	/// </summary>
	public enum INC_ASPHALTMIX_APP_CMD : byte
	{
		aaCmdRealData = 0x10,														// IOSERVER 发给客户端的实时数据
		aaCmdRealDataChanged = 0x11,												// 仅针对发生变化的实时数据，IOSERVER 发给客户端

		aaCmdOutputToAcquirePoint = 0x21,											// 客户端发往采集点（设置或查询采集点的命令）
		aaCmdOutputToAcquirePointResponse = 0x80 | aaCmdOutputToAcquirePoint,		// IOSERVER 对 aaCmdOutputToAcquirePoint 的响应

		aaCmdOutputToController = 0x22,												// 客户端发往控制器（设置或查询控制器的命令）
		aaCmdOutputToControllerResponse = 0x80 | aaCmdOutputToController,			// IOSERVER 对 aaCmdOutputToController 的响应

		aaCmdChangeCraftwork = 0x25,												// 客户端改用工艺（包括冷料、热料的工艺编码）
		aaCmdChangeCraftworkReponse = 0x80 | aaCmdChangeCraftwork,					// IOSERVER 对 aaCmdChangeCraftwork 的响应

		aaCmdUserDefine = 0x60,														// 用户自定义命令起始，到 0x7F 结束
		aaCmdColdStart = 0x61,                                                      // 启动冷料斗命令
		aaCmdChangeColdSpeed = 0x62,                                                // 冷料斗输出速度改变


		// 附加的用户定义命令
		aaCmdUser_ResetTower = aaCmdUserDefine + 4,									// 塔楼复位
		aaCmdUser_ResetAll = aaCmdUserDefine + 5,									// 全部复位
		aaCmdUser_ChangeMixerCurrent = aaCmdUserDefine + 6,							// 改变拌缸电流 测试命令

		aaCmdUser_Alarm = aaCmdUserDefine + 7,										// 报警
		aaCmdUser_Debug = aaCmdUserDefine + 8,										// 调试用命令

		aaCmdUser_NotifyAcquireValue = aaCmdUserDefine + 11,						// 通知（软单元）需要改变
		aaCmdUser_NotifyIOServerStatus = aaCmdUserDefine + 12,						// 通知（IO服务器状态）已经改变

	}

	/// <summary>
	/// 混凝土系统 应用命令枚举值
	/// </summary>
	public enum INC_BETONMIX_APP_CMD : byte
	{
		baCmdRealData = 0x10,														// IOSERVER 发给客户端的实时数据
		baCmdRealDataChanged = 0x11,												// 仅针对发生变化的实时数据，IOSERVER 发给客户端

		baCmdOutputToAcquirePoint = 0x21,											// 客户端发往采集点（设置或查询采集点的命令）
		baCmdOutputToAcquirePointResponse = 0x80 + baCmdOutputToAcquirePoint,		// IOSERVER 对 baCmdOutputToAcquirePoint 的响应

		baCmdOutputToController = 0x22,												// 客户端发往控制器（设置或查询控制器的命令）
		baCmdOutputToControllerResponse = 0x80 + baCmdOutputToController,			// IOSERVER 对 baCmdOutputToController 的响应

		baCmdChangeCraftwork = 0x25,												// 客户端改用工艺（包括冷料、热料的工艺编码）
		baCmdChangeCraftworkReponse = 0x80 + baCmdChangeCraftwork,					// IOSERVER 对 baCmdChangeCraftwork 的响应

		baCmdUserDefine = 0x60,														// 用户自定义命令起始，到 0x7F 结束

		// 附加的用户定义命令
		baCmdUser_ResetTower = baCmdUserDefine + 4,									// 塔楼复位
		baCmdUser_ResetAll = baCmdUserDefine + 5,									// 全部复位
		baCmdUser_ChangeMixerCurrent = baCmdUserDefine + 6,							// 改变拌缸电流 测试命令

		baCmdUser_Debug = baCmdUserDefine + 8,										// 调试用命令

		baCmdUser_NotifyAcquireValue = baCmdUserDefine + 11,						// 通知（软单元）需要改变
		baCmdUser_NotifyIOServerStatus = baCmdUserDefine + 12,						// 通知（IO服务器状态）已经改变

	}

	public class EnumForMixSystem
	{
	}
}
