using System;
using System.Linq;

namespace InControls.IO.PLC.Mitsubishi
{
	public static class McSwapExtersions
	{
		/// <summary>
		/// 颠倒 UInt16 字节顺序
		/// </summary>
		public static UInt16 SwapByteOrder(this UInt16 value)
		{
			byte[] buff = BitConverter.GetBytes(value);
			buff.Reverse();
			return BitConverter.ToUInt16(buff, 0);
		}

		/// <summary>
		/// 颠倒 Int16 字节顺序
		/// </summary>
		public static Int16 SwapByteOrder(this Int16 value)
		{
			byte[] buff = BitConverter.GetBytes(value);
			buff.Reverse();
			return BitConverter.ToInt16(buff, 0);
		}

		/// <summary>
		/// 颠倒 UInt32 字节顺序
		/// </summary>
		public static UInt32 SwapByteOrder(this UInt32 value)
		{
			byte[] buff = BitConverter.GetBytes(value);
			buff.Reverse();
			return BitConverter.ToUInt32(buff, 0);
		}

		/// <summary>
		/// 颠倒 Int32 字节顺序
		/// </summary>
		public static Int32 SwapByteOrder(this Int32 value)
		{
			byte[] buff = BitConverter.GetBytes(value);
			buff.Reverse();
			return BitConverter.ToInt32(buff, 0);
		}

	}
}
