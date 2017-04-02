using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Vila.Communication.Common;
using Vila.Communication.Data;

namespace InControls.Common
{
	/// <summary>
	/// IOSERVER 的综合状态
	/// 将在IOSERVER、客户端之间传输
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
	public unsafe struct IOServerStatus : IVikiSerialzer
	{
		#region 数据成员

		public WeightingStepEnum StoneStep;						// 石料/热料 阶段
		public int StoneHoldTime;								// 石料当前阶段持续秒数

		public WeightingStepEnum AsphaltStep;					// 沥青 阶段
		public int AsphaltHoldTime;								// 沥青当前阶段持续秒数

		public WeightingStepEnum SlagStep;						// 矿粉 阶段
		public int SlagHoldTime;								// 矿粉当前阶段持续秒数

		public WeightingStepEnum MixerStep;						// 拌缸 阶段
		public int MixerHoldTime;								// 拌缸当前阶段持续秒数

		public CraftworkItemInfo Craftwork;						// 当前工艺

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public double[] Weighting;                              // 各个秤的称量重量，0下标的元素为占位符

		public int InnerRuntimeRound;				            // InnerRuntimeEngine 内部的扫描次数

		#endregion

		#region 外部内存复制函数声明
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(ref IOServerStatus dest, IntPtr src, int size_t);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(IntPtr dest, ref IOServerStatus src, int size_t);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(IntPtr dest, IntPtr src, int size_t);
		#endregion

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
