using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.PLC.FX
{
	public enum FxCommandConst : byte			// 命令字
	{
		FxCmdRead = 0x30,						// 元件读取，X,Y,M,S,T,C,D 
		FxCmdWrite = 0x31,						// 元件写入，X,Y,M,S,T,C,D 
		FxCmdForceOn = 0x37,					// 强迫ON，X,Y,M,S,T,C 
		FxCmdForceOff = 0x38,					// 强迫OFF，X,Y,M,S,T,C
	}

	public enum FxResponseConst : byte			// 命令字
	{
		FxACK = FxControlCode._ACK,				//接受正确
		FxNAK = FxControlCode._NAK,				//接受错误
	}

	/// <summary>
	/// 控制代码
	/// </summary>
	public class FxControlCode
	{
		public const byte _NUL = 0x00;			//  NULL 
		public const byte _STX = 0x02;			//  主机报文开始 
		public const byte _ETX = 0x03;			//  主机报文结束
		public const byte _EOT = 0x04;			//  End of Transmission
		public const byte _ENQ = 0x05;			//  （从机）来自从机的请求信号 
		public const byte _ACK = 0x06;			//  （从机）PLC正确响应 
		public const byte _NAK = 0x15;			//  （从机）PLC错误响应 

		public const byte _DLE = 0x10;			//  Body Link Escape

		public const byte _LF = 0x0A;			//  
		public const byte _CR = 0x0D;			//  


		public const byte _CLEAR = 0x0C;		//  
	}


	public enum FxAddressLayoutType : ushort
	{
		AddressLayoutBin = 1,
		AddressLayoutByte = 2,
		AddressLayoutInt16 = 3,
		AddressLayoutInt32 = 4,
	}

	public enum FxAddressType : short
	{
		X = 1,
		Y = 2,
		M = 3,
		S = 4,
		T = 5,
		C = 6,
		D = 7,

		/// <summary>
		/// 常数
		/// </summary>
		K = 8,

		Undefine = 0,								// 为定义的错误地址类型
	}


}
