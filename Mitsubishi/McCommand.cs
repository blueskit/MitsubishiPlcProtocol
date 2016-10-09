using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace InControls.IO.PLC.Mitsubishi
{
	// ####################################################################################
	// 通信に使用するコマンドを表現するインナークラス
	public class McCommand
	{
		private McFrame FrameType { get; set; }         // フレーム種別
		private uint SerialNumber { get; set; }         // 
		private byte NetwrokNumber { get; set; }        // 网络编号
		private byte PcNumber { get; set; }             // PC编号/PLC编号
		private uint IoNumber { get; set; }             // 请求目标模块IO编号
		private byte ChannelNumber { get; set; }        // 请求目标模块站编号
		private uint CpuTimer { get; set; }             // CPU监视定时器
		private int ResultCode { get; set; }            // 返回代码（如果没有返回，则为0xcccc）
		public byte[] Response { get; private set; }    // 応答データ

		public McCommand(McFrame iFrame)
		{
			FrameType = iFrame;
			SerialNumber = 0x0001;
			NetwrokNumber = 0x00;
			PcNumber = 0xff;
			IoNumber = 0x03FF;
			ChannelNumber = 0x00;
			CpuTimer = 0x0010;
		}

		public byte[] SetCommand(uint iMainCommand, uint iSubCommand, byte[] iData)
		{
			var dataLength = (uint)(iData.Length + 6);
			var ret = new List<byte>(iData.Length + 20);
			uint frame = (FrameType.HasFlag(McFrame.MC3E)) ? 0x0050u :
						 (FrameType.HasFlag(McFrame.MC4E)) ? 0x0054u : 0x0000u;
			ret.Add((byte)frame);
			ret.Add((byte)(frame >> 8));
			if (FrameType.HasFlag(McFrame.MC4E)) {
				ret.Add((byte)(SerialNumber >> 8));
				ret.Add((byte)SerialNumber);
				ret.Add(0x00);
				ret.Add(0x00);
			}
			ret.Add((byte)NetwrokNumber);
			ret.Add((byte)PcNumber);

			ret.Add((byte)(IoNumber >> 8));
			ret.Add((byte)IoNumber);

			ret.Add((byte)ChannelNumber);

			ret.Add((byte)(dataLength >> 8));
			ret.Add((byte)dataLength);

			ret.Add((byte)(CpuTimer >> 8));
			ret.Add((byte)CpuTimer);

			ret.Add((byte)(iMainCommand >> 8));
			ret.Add((byte)iMainCommand);

			ret.Add((byte)(iSubCommand >> 8));
			ret.Add((byte)iSubCommand);
			ret.AddRange(iData);
			return ret.ToArray();
		}

		/// <summary>
		/// 按ASCII格式的3E格式构造
		/// 注意：需要颠倒int的顺序
		/// </summary>
		public byte[] SetCommand(uint iMainCommand, uint iSubCommand, string data)
		{
			var dataLength = (uint)(data.Length + 12);

			StringBuilder ret = new StringBuilder();
			ret.Append("5000");     // 头部
			ret.AppendFormat("{0:X2}", NetwrokNumber);
			ret.AppendFormat("{0:X2}", PcNumber);
			ret.AppendFormat("{0:X4}", IoNumber);
			ret.AppendFormat("{0:X2}", ChannelNumber);
			ret.AppendFormat("{0:X4}", dataLength);
			ret.AppendFormat("{0:X4}", CpuTimer);
			ret.AppendFormat("{0:X4}", iMainCommand);
			ret.AppendFormat("{0:X4}", iSubCommand);
			ret.Append(data);

			return ASCIIEncoding.ASCII.GetBytes(ret.ToString());
		}

		public int SetResponse(byte[] responseBytes, McFrame mcFrame)
		{
			if (mcFrame.HasFlag(McFrame.ASCII_FLAG))
				return SetResponse(ASCIIEncoding.ASCII.GetString(responseBytes));

			ResultCode = 0xcccc;

			int min = (FrameType.HasFlag(McFrame.MC3E)) ? 11 : 15;
			if (min <= responseBytes.Length) {
				var btCount = new[] { responseBytes[min - 3], responseBytes[min - 4] };
				var btCode = new[] { responseBytes[min - 1], responseBytes[min - 2] };
				int rsCount = BitConverter.ToUInt16(btCount, 0);
				if (FrameType.HasFlag(McFrame.ASCII_FLAG)) rsCount = rsCount / 2;

				ResultCode = BitConverter.ToUInt16(btCode, 0);
				Response = new byte[rsCount - 2];
				Buffer.BlockCopy(responseBytes, min, Response, 0, Response.Length);
			}
			return ResultCode;
		}

		/// <summary>
		/// 按ASCII格式的3E格式构造
		/// 注意：需要颠倒int的顺序
		/// </summary>
		public int SetResponse(string responseText)
		{
			Debug.WriteLine(responseText);

			byte[] buffer = new byte[responseText.Length / 2];
			for (int i = 0; i < buffer.Length; i++) {
				byte b;
				byte.TryParse(responseText.Substring(i * 2, 2), NumberStyles.HexNumber, null, out b);
				buffer[i] = b;
			}
			return SetResponse(buffer, McFrame.MC3E);
		}

		public bool IsIncorrectResponse(byte[] iResponse)
		{
			if (iResponse.Length == 0)
				return false;

			var min = (FrameType.HasFlag(McFrame.MC3E)) ? 11 : 15;
			var btCount = new[] { iResponse[min - 3], iResponse[min - 4] };
			var btCode = new[] { iResponse[min - 1], iResponse[min - 2] };
			var rsCount = BitConverter.ToUInt16(btCount, 0) - 2;
			var rsCode = BitConverter.ToUInt16(btCode, 0);
			return (rsCode == 0 && rsCount != (iResponse.Length - min));
		}
	}
}
