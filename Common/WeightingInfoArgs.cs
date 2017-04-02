using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Vila.Communication.Common;
using Vila.Communication.Data;

namespace InControls.Common
{

	/// <summary>
	/// 各称重信息将集中到此结构中
	/// 并在IOSERVER和客户端之间共享,数据一般来源于 IOSERVER 中的 WeightingWorkData
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
	public unsafe struct WeightingInfoArgs : IVikiSerialzer
	{
		#region 数据成员(所有数据成员必须是 值类型！！！)

		/// <summary>
		/// 1#热料的实际称重Kg（热料的当前实际称重重量（计算得到），单位：Kg）
		/// </summary>
		public double H01;
		public double H02;
		public double H03;
		public double H04;
		public double H05;
		public double H06;

		/// <summary>
		/// 所有热料的已称重总重量（Kg）
		/// </summary>
		public double H_TOTAL;

		#endregion


		#region 外部内存复制函数声明
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(ref WeightingInfoArgs dest, IntPtr src, int size_t);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(IntPtr dest, ref WeightingInfoArgs src, int size_t);

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
