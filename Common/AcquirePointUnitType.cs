using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	[Flags]
	public enum AcquirePointUnitTypeConst : short
	{
		aputNothing = 0,								// 未定义
		aputDI = 0x01,
		aputDO = 0x02,
		aputDIDO = (aputDI | aputDO),
		aputAI = 0x04,
		aputAO = 0x08,
		aputAIAO = (aputAI | aputAO),

		aputPulseFlag = 0x100,							// “脉冲”标志，如果 DI/DO 为脉冲式，则添加此标志位
		aputPulsedDI = aputPulseFlag | aputDI,			// 脉冲型DI
		aputPulsedDO = aputPulseFlag | aputDO,			// 脉冲型DO
		aputPulsedDIDO = aputPulseFlag | aputDIDO,		// 脉冲型DIDO

	}

	public enum AcquirePointDataTypeConst : short
	{
		apdtInvalid = 0,				// 非法，未设置
		apdtSwitch,						// 开关
		apdtInteger,					// 整数
		apdtFloat,						// 浮点数
		apdtDateTime,					// 日期/时间
		apdtString,						// 字符串
		apdtArray,						// 数组
	}


	public static class AcquirePointUnitTypeExtention
	{
		/// <summary>
		/// 是否 DI/DO/DIDO ?
		/// </summary>
		public static bool IsDiOrDo(this AcquirePointUnitTypeConst aput)
		{
			return aput.HasFlag(AcquirePointUnitTypeConst.aputDI) || aput.HasFlag(AcquirePointUnitTypeConst.aputDO);
		}

		/// <summary>
		/// 是否 DI/DO/DIDO ?
		/// </summary>
		public static bool IsAiOrAo(this AcquirePointUnitTypeConst aput)
		{
			return aput.HasFlag(AcquirePointUnitTypeConst.aputAI) || aput.HasFlag(AcquirePointUnitTypeConst.aputAO);
		}

		/// <summary>
		/// 是否有“脉冲”标记？
		/// 如果是脉冲式DIDO则拥有此 Pulse 标记
		/// </summary>
		public static bool HasPulseFlag(this AcquirePointUnitTypeConst aput)
		{
			return ((aput & AcquirePointUnitTypeConst.aputPulseFlag) == AcquirePointUnitTypeConst.aputPulseFlag);
		}

	}


}
