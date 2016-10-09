using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using InControls.IO.Common;
using InControls.IO.PLC;

namespace InControls.IO.PLC.FX
{
	public sealed class FxCommandHelper
	{
		/// <summary>
		/// 解析PLC响应数据（到集合中）
		/// 默认数据类型UInt16DataType
		/// </summary>
		/// <param name="pureData"></param>
		/// <param name="value">解析后的返回值</param>
		public static void ParseSmart(byte[] data, int startIndex, out List<int> value)
		{
			ParseSmart<UInt16DataType>(data, startIndex, out value);
		}

		/// <summary>
		/// 解析PLC响应数据（到集合中）
		/// </summary>
		/// <param name="pureData"></param>
		/// <param name="startIndex"></param>
		/// <param name="cellDataType">支持数据类型 UInt16DataType.Default,UInt32DataType.Default</param>
		/// <param name="value">解析后的返回值</param>
		public static void ParseSmart(byte[] data, int startIndex, ICellDataType cellDataType, out List<int> value)
		{
			if (cellDataType.DataItemSize == 1) {
				ParseSmart<UInt8DataType>(data, startIndex, out value);
			} else if (cellDataType.DataItemSize == 4) {
				ParseSmart<UInt32DataType>(data, startIndex, out value);
			} else {
				ParseSmart<UInt16DataType>(data, startIndex, out value);
			}
		}

		/// <summary>
		/// 解析PLC响应数据（到集合中）
		/// </summary>
		/// <typeparam name="T">支持数据类型 UInt16DataType,UInt32DataType</typeparam>
		/// <param name="pureData"></param>
		/// <param name="startIndex"></param>
		/// <param name="value">解析后的返回值</param>
		public static void ParseSmart<T>(byte[] data, int startIndex, out List<int> value) where T : ICellDataType, new()
		{
			T t = new T();

			value = new List<int>();

			if (data.Length < 7) return;						// 长度至少大于7
			if (data[startIndex] != FxControlCode._STX) return;

			// 每次处理一个 4B 或 8B,对应着 ushort，uint

			for (int i = startIndex + 1; i < (data.Length - startIndex - t.DataItemHexStringSize); i += t.DataItemHexStringSize) {
				int v = FxConvert.HexToDec(data, i, t.DataItemHexStringSize);
				value.Add(v);
			}
		}

		/// <summary>
		/// 构建PLC FX命令
		/// 支持各种“读数据”或“查询”类命令，默认数据类型UInt16DataType
		/// </summary>
		/// <param name="response">命令字</param>
		/// <param name="addr">起始地址</param>
		public static string Make(FxCommandConst cmd, FxAddress addr)
		{
			int length = 0;

			if (cmd == FxCommandConst.FxCmdForceOff | cmd == FxCommandConst.FxCmdForceOn) {
				length = 0;
			} else {
				switch (addr.AddressSpaceType) {
				case FxAddressLayoutType.AddressLayoutBin:
					length = 0;
					break;
				case FxAddressLayoutType.AddressLayoutByte:
					length = 1;
					break;
				case FxAddressLayoutType.AddressLayoutInt16:
					length = 2;
					break;
				case FxAddressLayoutType.AddressLayoutInt32:
					length = 4;
					break;
				}
			}
			return (Make(cmd, addr, length));
		}

		/// <summary>
		/// 构建PLC FX命令
		/// 支持各种“读数据”或“查询”类命令，支持多种数据类型，例如 UInt16DataType,UIntDataType
		/// </summary>
		/// <param name="response">命令字</param>
		/// <param name="addr">起始地址</param>
		/// <param name="length">数据长度，以字节为单位。例如：如果是16bits则为2、32bits则为4</param>
		/// <returns>返回构造完成的命令串</returns>
		public static string Make(FxCommandConst cmd, FxAddress addr, int length)
		{
			StringBuilder sb = new StringBuilder(64);
			sb.Append((char)FxControlCode._STX);
			sb.Append((char)cmd);
			sb.Append(addr.ToAddressHexString());
			if (length > 0) sb.Append(FxConvert.DecToHex((uint)length, 2));
			sb.Append((char)FxControlCode._ETX);
			sb.Append(FxConvert.DecToHex(GetCheckSum(sb.ToString(), 1), 2));

			return (sb.ToString());
		}

		/// <summary>
		/// 构建PLC FX命令
		/// 支持各种“写数据”命令，支持多种数据类型，例如 UInt16DataType,UInt32DataType
		/// </summary>
		/// <param name="response">命令字</param>
		/// <param name="addr">起始地址</param>
		/// <param name="pureData">数据部分</param>
		/// <returns>返回构造完成的命令串</returns>
		public static string Make<T>(FxCommandConst cmd, FxAddress addr, List<uint> data) where T : ICellDataType, new()
		{
			T t = new T();
			return (Make(cmd, addr, data.Count * t.DataItemHexStringSize, ConvertToString<T>(data)));
		}

		/// <summary>
		/// 构建PLC FX命令
		/// 这是针对 ushort 的特殊命令，也可使用 Make<UShortDataType>(...) 函数达到同样目的
		/// </summary>
		/// <param name="response">命令字</param>
		/// <param name="addr">起始地址</param>
		/// <param name="pureData">数据部分</param>
		/// <returns>返回构造完成的命令串</returns>
		public static string Make(FxCommandConst cmd, FxAddress addr, List<ushort> data)
		{
			return (Make(cmd, addr, data.Count * sizeof(ushort) * 2, ConvertToString(data)));
		}

		public static string Make(FxCommandConst cmd, FxAddress addr, int length, byte[] data)
		{
			Debug.Assert(length == data.Length, "length(长度)必须与给定的数据实际一致");

			return (Make(cmd, addr, length, ASCIIEncoding.ASCII.GetString(data)));
		}

		public static string Make(FxCommandConst cmd, FxAddress addr, int length, string data)
		{
			Debug.Assert(length == data.Length, "length(长度)必须与给定的数据实际一致");

			StringBuilder sb = new StringBuilder(64 + data.Length);
			sb.Append((char)FxControlCode._STX);
			sb.Append((char)cmd);
			sb.Append(addr.ToAddressHexString());
			sb.Append(FxConvert.DecToHex((uint)(length / 2), 2));
			sb.Append(data);
			sb.Append((char)FxControlCode._ETX);
			sb.Append(FxConvert.DecToHex(GetCheckSum(sb.ToString(), 1), 2));

			return (sb.ToString());
		}

		/// <summary>
		/// 将给定数据序列转换为命令串格式
		/// 目前支持 16位、32位 两种格式,例如 UInt16DataType,UInt32DataType
		/// </summary>
		/// <param name="pureData">数据序列</param>
		public static string ConvertToString<T>(List<uint> data) where T : ICellDataType, new()
		{
			T t = new T();
			StringBuilder sb = new StringBuilder(data.Count * t.DataItemHexStringSize);

			for (int i = 0; i < data.Count; i++) {
				sb.Append(FxConvert.DecToHex(data[i], t.DataItemHexStringSize));
			}
			return (sb.ToString());
		}

		/// <summary>
		/// 将给定数据序列转换为命令串格式
		/// 这是针对 ushort 的特殊命令版本，也可使用 ConvertToString<UShortDataType>(...) 函数达到同样目的
		/// </summary>
		/// <param name="pureData"></param>
		/// <returns></returns>
		public static string ConvertToString(List<ushort> data)
		{
			StringBuilder sb = new StringBuilder(data.Count * sizeof(ushort) * 2);

			for (int i = 0; i < data.Count; i++) {
				sb.Append(FxConvert.DecToHex(data[i], sizeof(ushort) * 2));
			}
			return (sb.ToString());
		}

		#region 计算CRC检查和
		/// <summary>
		/// 计算并返回CRC检查和
		/// </summary>
		public static byte GetCheckSum(string data)
		{
			byte chk = 0;
			for (int i = 0; i < data.Length; i++) {
				char c = data[i];
				chk = (byte)unchecked(chk + Convert.ToByte(c));
			}
			return (chk);
		}

		public static byte GetCheckSum(string data, int fromIndex)
		{
			byte chk = 0;
			for (int i = fromIndex; i < data.Length; i++) {
				char c = data[i];
				chk = (byte)unchecked(chk + Convert.ToByte(c));
			}
			return (chk);
		}

		/// <summary>
		/// 计算并返回CRC检查和
		/// </summary>
		public static ushort GetCheckSum(byte[] data)
		{
			ushort chk = 0;
			for (int i = 0; i < data.Length; i++) {
				byte c = data[i];
				chk = (ushort)unchecked(chk + Convert.ToByte(c));
			}
			return (chk);
		}
		#endregion
	}
}
