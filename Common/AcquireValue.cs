using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Vila.Common;

using Vila.Communication.Common;
using Vila.Communication.Data;

namespace InControls.Common
{
	/// <summary>
	/// IO点/软单元/采集点的值
	/// 这是换算后的采集值，在这里保存多个状态(版本)的值，以便于系统的管理
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
	public unsafe struct AcquireValue : IVikiSerialzer
	{
		public enum OUTPUT_MODE : short
		{
			omAuto = 0,
			omManual = 1,                                           // 如果是手工操作界面按钮、或输出，则为此模式
		}

		#region 数据成员-下述成员将需序列化、反序列化
		private int _Id;											// 采集点全局唯一索引号(冗余信息).

		public ValueStruct CurrValue;								// 当前值
		public ValueStruct PreviousValue;							// 前一次的采集值
		public ValueStruct Output;									// 输出值,如果没有输出，则默认为 NO_OUTPUT
		public ValueStruct DefaultValue;							// 默认值（大多数为 vtNull）,用于脉冲式DIDO、常开或常闭开关的自动复位维护

		private float _ZeroOffset;									// 零点偏移
		private float _CalibrateValue;								// 校准值
		private float _Gain;										// 增益

		private OUTPUT_MODE _OutputMode;                            // 输出模式

		private bool _Changed;										// 最后一次Update（或Output）时，数值是否发生了变化？
		private DateTime _LastChangedTime;							// 最后一次CurrValue发生了改变的时刻（如果不改变，则不予改变这个值）
		private DateTime _UpdateTime;								// 最后一次更新 PreviousValue/Output 的时刻
		#endregion

		#region 属性代码块
		public int Id
		{
			get { return _Id; }
		}

		public OUTPUT_MODE OutputMode
		{
			get { return _OutputMode; }
		}

		public float ZeroOffset
		{
			get { return _ZeroOffset; }
		}

		public float CalibrateValue
		{
			get { return _CalibrateValue; }
		}

		public float Gain
		{
			get { return _Gain; }
		}

		/// <summary>
		/// 最后一次更新 PreviousValue 的时刻
		/// </summary>
		public DateTime UpdateTime
		{
			get { return _UpdateTime; }
		}

		/// <summary>
		/// 最后一次发生了改变的时刻（如果不改变，则不予改变这个值）
		/// </summary>
		public DateTime LastChangedTime
		{
			get { return _LastChangedTime; }
		}

		public bool Changed
		{
			get { return _Changed; }
		}
		#endregion

		#region 外部内存复制函数声明
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(ref AcquireValue dest, IntPtr src, int size_t);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(IntPtr dest, ref AcquireValue src, int size_t);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(IntPtr dest, IntPtr src, int size_t);
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">IO点/软单元/采集点全局唯一索引号</param>
		public AcquireValue(int id)
		{
			_Id = id;
			CurrValue = new ValueStruct();
			PreviousValue = new ValueStruct();
			Output = new ValueStruct();
			DefaultValue = ValueStruct.NullDefault;
			_OutputMode = OUTPUT_MODE.omAuto;
			_ZeroOffset = 0;
			_Gain = 0;
			_CalibrateValue = 0;
			_UpdateTime = DateTime.MinValue;
			_LastChangedTime = DateTime.MinValue;
			_Changed = false;
		}

		/// <summary>
		/// 将“当前值”备份到“前一次值”
		/// </summary>
		public void Backup()
		{
			PreviousValue = CurrValue;
			Output = new ValueStruct(ValueTypeEnum.vtNull);
			_Changed = false;
		}

		/// <summary>
		/// 设置内参数
		/// </summary>
		public void SetParam(float zeroOffset, float calibrateValue, float gain)
		{
			_ZeroOffset = zeroOffset;
			_CalibrateValue = calibrateValue;
			_Gain = gain;
		}

		/// <summary>
		/// 将采集到的数值更新到内部
		/// 注意：
		///		1、如果值相等，允许重复设置。此时仅仅影响 _UpdateTime
		///		2、如果已更新、且数值发生改变，则不再允许设置新值
		///		3、除非Backup()之后，方才允许设置新值
		/// </summary>
		public void SetNewValue(ValueStruct value)
		{
			if (!_Changed) {
				SetNewValueForce(value);
			}
		}

		/// <summary>
		/// 将采集到的数值更新到内部
		/// 注意：
		///		1、如果值相等，允许重复设置。此时仅仅影响 _UpdateTime
		///		2、如果已更新、且数值发生改变，则不再允许设置新值
		///		3、除非Backup()之后，方才允许设置新值
		/// </summary>
		public void SetNewValue(double value)
		{
			if (!_Changed) {
				SetNewValueForce(value);
			}
		}

		/// <summary>
		/// 强制将采集到的数值更新到内部
		/// 说明：
		///		一般不直接使用强制更新，除非有充分的理由 ~_*
		/// </summary>
		public void SetNewValueForce(ValueStruct value)
		{
			_UpdateTime = DateTime.Now;

			CurrValue = value;

			if (CurrValue != PreviousValue) {
				_Changed = true;
				_LastChangedTime = _UpdateTime;
			}

			// 如果是“脉冲式开关量”，则将初次获取值视作“默认值”
			if (DefaultValue.IsNull && value.ValueTypeConst == ValueTypeEnum.vtBool) {
				DefaultValue = value;
			}
		}

		/// <summary>
		/// 强制将采集到的数值更新到内部
		/// 说明：
		///		一般不直接使用强制更新，除非有充分的理由 ~_*
		/// </summary>
		public void SetNewValueForce(double value)
		{
			_UpdateTime = DateTime.Now;

			CurrValue = new ValueStruct(value);

			if (CurrValue != PreviousValue) {
				_Changed = true;
				_LastChangedTime = _UpdateTime;
			}
		}

		public override string ToString()
		{
			return string.Format("Value={0},Output={1},UpdateTime={2},LastChangedTime={3}", CurrValue.ToString(), Output.ToString(), _UpdateTime.ToString(), _LastChangedTime.ToString());
		}


		#region 输出控制函数
		/// <summary>
		/// 是否有输出？
		/// </summary>
		public bool HasOutput()
		{
			return !Output.IsNull;
		}

		/// <summary>
		/// 将输出值复位
		/// 准备进入逻辑判断、运算环节前复位输出
		/// </summary>
		public void ResetOutput()
		{
			Output = ValueStruct.NullDefault;
			_OutputMode = OUTPUT_MODE.omAuto;
			_UpdateTime = DateTime.Now;
		}

		/// <summary>
		/// 设置新的输出值，支持 Double(模拟量)/Int(数字量)/Bool(开关量)
		/// 如果是开关量，则可取 True(1)、False(0) 值
		/// </summary>
		public void SetNewOutput(ValueStruct value)
		{
			SetNewOutput(value, OUTPUT_MODE.omAuto);
		}

		/// <summary>
		/// 设置新的输出值，支持 Double(模拟量)/Int(数字量)/Bool(开关量)
		/// 如果是开关量，则可取 True(1)、False(0) 值
		/// </summary>
		public void SetNewOutput(ValueStruct value, OUTPUT_MODE outMode)
		{
			Output = value;
			_OutputMode = outMode;
			_UpdateTime = DateTime.Now;
			_Changed = !_OutputMode.Equals(CurrValue);
		}


		public void SetNewOutput(double value)
		{
			SetNewOutput(value, OUTPUT_MODE.omAuto);
		}

		/// <summary>
		/// 设置模拟量“输出值”
		/// 如果是开关量，则可取 1、0 值
		/// </summary>
		public void SetNewOutput(double value, OUTPUT_MODE outMode)
		{
			Output = new ValueStruct(value);
			_OutputMode = outMode;
			_UpdateTime = DateTime.Now;
			_Changed = !_OutputMode.Equals(CurrValue);
		}

		/// <summary>
		/// 设置开关量“输出值”
		/// 如果是开关量1=True，0=False
		/// </summary>
		public void SetNewOutput(bool value)
		{
			SetNewOutput(value, OUTPUT_MODE.omAuto);
		}

		/// <summary>
		/// 设置开关量“输出值”
		/// 如果是开关量1=True，0=False
		/// </summary>
		public void SetNewOutput(bool value, OUTPUT_MODE outMode)
		{
			Output = new ValueStruct(value);
			_OutputMode = outMode;
			_UpdateTime = DateTime.Now;
			_Changed = !_OutputMode.Equals(CurrValue);
		}

		/// <summary>
		/// 设置开关量“输出值”根据当前值翻转
		/// 如果当前值是 Null、False,则输出为 True，否则输出 False
		/// </summary>
		public void SetNewOutputRollingOver(OUTPUT_MODE outMode)
		{
			if (CurrValue.IsNull || CurrValue.B == false)
				Output = new ValueStruct(true);
			else
				Output = new ValueStruct(false);

			_OutputMode = outMode;
			_UpdateTime = DateTime.Now;
			_Changed = true;
		}

		#endregion

		/// <summary>
		/// 返回两个变量是否相等
		/// </summary>
		public override bool Equals(object obj)
		{
			AcquireValue v = (AcquireValue)obj;
			return _Id == v.Id
					&& CurrValue.Equals(v.CurrValue)
					&& PreviousValue.Equals(v.PreviousValue)
					&& Output.Equals(v.Output);
		}

		public override int GetHashCode()
		{
			return _Id.GetHashCode();
		}

		#region IVikiSerialzer 成员(这是标准的实现),另加若干辅助函数

		public byte[] ToBytesArray()
		{
			byte[] buff = new byte[Marshal.SizeOf(this)];
			fixed (byte* pDest = &buff[0]) {
				CopyMemory((IntPtr)pDest, ref this, buff.Length);
			}
			return (buff);
		}


		/// <summary>
		/// 将当前结构复制到目标字节数组（指定的起始偏移处）
		/// </summary>
		public void ConvertTo(out byte[] destBuff, int destPos)
		{
			int sizeThis = Marshal.SizeOf(this);
			destBuff = new byte[sizeThis + destPos];

			fixed (byte* pDest = &destBuff[destPos]) {
				CopyMemory((IntPtr)pDest, ref this, sizeThis);
			}
		}

		/// <summary>
		/// 从给定的字节数组构建当前结构实例
		/// </summary>
		public void ConvertFrom(byte[] sourceBuff, int fromPos)
		{
			if (sourceBuff.Length > fromPos) {
				fixed (void* pSource = &sourceBuff[fromPos]) {
					CopyMemory(ref this, (IntPtr)pSource, Math.Min(Marshal.SizeOf(this), sourceBuff.Length - fromPos));
				}
			}
		}
		#endregion
	}
}
