using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 采集点换算器
	/// </summary>
	public class AcquirePointConversion
	{
		private static AcquirePointConversion _DefaultNullConversion = new AcquirePointConversion();	// 内部默认哑设备

		public double InputLo;					// 采集值的范围 低端 -- 从外部传感器获取到的电压(0-5V),电流（5-20ma),数字量（0-1023）等数据
		public double InputHi;					// 采集值的范围 高端

		public double RangeLo;					// 量程范围的 低端 -- 显示给人类看的数值
		public double RangeHi;					// 量程范围的 高端

		public AcquirePointConversion()
		{
			InputLo = 0;
			InputHi = 0;
			RangeLo = 0;
			RangeHi = 0;
		}

		/// <summary>
		/// 根据给定的测试值(value)
		/// 依据内部配置转换并输出
		/// </summary>
		public double ConvertFrom(double value)
		{
			if (this == _DefaultNullConversion) return value;

			Debug.Assert(InputHi != 0, "InputHi 不允许为0，请检查代码。");
			double d;
			d = ((value - InputLo) / InputHi) * (RangeHi - RangeLo) + RangeLo;
			return d;
		}

		/// <summary>
		/// 针对不需要转换的设备，可使用这个“哑设备”引用
		/// </summary>
		public static AcquirePointConversion GetDefaultNullConversion()
		{
			return _DefaultNullConversion;
		}

		/// <summary>
		/// 标准电压型传感器的默认转换
		/// </summary>
		/// <param name="rangeLo">量程低端</param>
		/// <param name="rangeHi">量程高端</param>
		public static AcquirePointConversion GetFromVoltageType(double rangeLo, double rangeHi)
		{
			AcquirePointConversion o = new AcquirePointConversion();
			o.InputLo = 0;
			o.InputHi = 5;
			o.RangeLo = rangeLo;
			o.RangeHi = rangeHi;
			return (o);
		}

		/// <summary>
		/// 标准电流型传感器的默认转换
		/// </summary>
		/// <param name="rangeLo">量程低端</param>
		/// <param name="rangeHi">量程高端</param>
		public static AcquirePointConversion GetFromCurrentType(double rangeLo, double rangeHi)
		{
			AcquirePointConversion o = new AcquirePointConversion();
			o.InputLo = 5;
			o.InputHi = 20;
			o.RangeLo = rangeLo;
			o.RangeHi = rangeHi;
			return (o);
		}

	}
}
