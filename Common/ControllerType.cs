using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 目前本系统支持的所有控制器类型
	/// </summary>
	[Flags]
	public enum ControllerTypeConst : int
	{
		ctUnDefine = 0,

		ctSerialPort = 1,					// 标准串口，一般不直接使用
		ctNetTCP = 2,						// TCP
		ctNetUDP = 3,						// UDP

		ctOPC = 10,							// 标准OPC

		ctPLC = 0x03E8,						// 十进制:1000,标准PLC
		ctPLC_Fx = 0x03F2,					// 十进制:1010
		ctPLC_Qn = 0x03FC,					// 十进制:1020,
		ctPLC_QnMC = 0x03FD,				// 十进制:1021（采用MC通讯协议的Qn系列PLC）

		ctCB920 = 0x0834,					// 十进制:2100,
		ctPT650 = 0x083E,					// 十进制:2110,

		ctSWP201B = 0x0899,					// 十进制:2201,

		ctACDrive = 0x08FC,					// 十进制:2300,变频器
		ctACDriveYaskawa = 0x08FD,			// 十进制:2301,安川变频器

		// 以下是附加的判断，需要采用按位“或”操作获得

		ctInitiativeController = 0x10000,								// 是否“主动控制器”
		ctSlowController = 0x20000,										// 是否“慢速控制器”

		ctCB920InitiativeController = ctCB920 | ctInitiativeController,	// 十进制:67636 ，“主动CB920控制器”
		ctPT650InitiativeController = ctPT650 | ctInitiativeController,	// 十进制:67646 ，“主动PT650控制器”

		ctSWP201BSlowController = ctSWP201B | ctSlowController,			// 十进制:133273,  "慢速SWP201"
	}

	public static class ControllerTypeExtension
	{

		/// <summary>
		/// 判断并返回控制器是否属于 “主动控制器”
		/// </summary>
		public static bool IsInitiativeController (this ControllerTypeConst ct)
		{
			return ((ct & ControllerTypeConst.ctInitiativeController) == ControllerTypeConst.ctInitiativeController);
		}

		/// <summary>
		/// 判断并返回控制器是否属于 “慢速控制器”
		/// </summary>
		public static bool IsSlowController (this ControllerTypeConst ct)
		{
			return ((ct & ControllerTypeConst.ctSlowController) == ControllerTypeConst.ctSlowController);
		}

		/// <summary>
		/// 判断并返回控制器是否属于 “PLC控制器”
		/// </summary>
		public static bool IsPLCController (this ControllerTypeConst ct)
		{
			return ((int)ct >= (int)ControllerTypeConst.ctPLC && (int)ct <= ((int)ControllerTypeConst.ctPLC + 0xff));
		}

		/// <summary>
		/// 判断并返回控制器是否属于 “FX 系列 PLC控制器”
		/// </summary>
		public static bool IsFxPLCController (this ControllerTypeConst ct)
		{
			return (ct == ControllerTypeConst.ctPLC_Fx);
		}

		/// <summary>
		/// 得到并返回给定的PLC控制器类型的地址表示法中 X/Y 点的进制数
		/// 例如：
		///		1，针对 Fx 系列 X007、Y117 等中的 007、117 均为 8进制，则返回8
		///		2，针对 Qn 系列,X101、Y00F 等中的 101、00F 均为 16进制，则返回16进制
		///		3、其它非PLC控制器类型，返回 0
		/// </summary>
		public static int ToBaseNumber (this ControllerTypeConst ct)
		{
			if(ct == ControllerTypeConst.ctPLC_Fx)
				return 8;
			else if(ct == ControllerTypeConst.ctPLC_Qn || ct == ControllerTypeConst.ctPLC_QnMC)
				return 16;
			else
				return 0;
		}


	}


}
