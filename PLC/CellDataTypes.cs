using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InControls.PLC
{

	/// <summary>
	/// PLC数据类型声明及支持
	/// 一般PLC支持16位、32位无符号类型
	/// </summary>
	public interface ICellDataType
	{
		int DataItemSize { get; }						// 数据元素占用字节数（2,4,8...)
		int DataItemHexStringSize { get; }				// 数据元素的16进制串表示形式的长度（4,8,16...)
	}

	public interface ICellDataType<T> : ICellDataType
	{
	}

	public class UInt8DataType : ICellDataType<ushort>
	{
		private static UInt8DataType _Default;
		public static UInt8DataType Default
		{
			get
			{
				if (_Default == null) _Default = new UInt8DataType();
				return (_Default);
			}
		}

		public int DataItemSize { get { return (sizeof(byte)); } }
		public int DataItemHexStringSize { get { return (sizeof(byte)) * 2; } }
	}

	public class UInt16DataType : ICellDataType<ushort>
	{
		private static UInt16DataType _Default;
		public static UInt16DataType Default
		{
			get
			{
				if (_Default == null) _Default = new UInt16DataType();
				return (_Default);
			}
		}

		public int DataItemSize { get { return (sizeof(ushort)); } }
		public int DataItemHexStringSize { get { return (sizeof(ushort)) * 2; } }
	}

	public class UInt32DataType : ICellDataType<uint>
	{
		private static UInt32DataType _Default;
		public static UInt32DataType Default
		{
			get
			{
				if (_Default == null) _Default = new UInt32DataType();
				return (_Default);
			}
		}

		public int DataItemSize { get { return (sizeof(uint)); } }
		public int DataItemHexStringSize { get { return (sizeof(uint)) * 2; } }
	}

}
