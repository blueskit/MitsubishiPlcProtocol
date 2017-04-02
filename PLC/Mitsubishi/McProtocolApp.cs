using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.PLC.Mitsubishi
{
	/// <summary>
	/// ASCII-3E格式
	/// </summary>
	abstract public class McProtocolApp : IMitsubishiPlc
	{
		// ====================================================================================
		public McFrame CommandFrame { get; set; }   // 使用フレーム
		public string HostName { get; set; }        // ホスト名またはIPアドレス
		public int PortNumber { get; set; }         // ポート番号

		// ====================================================================================
		// コンストラクタ
		protected McProtocolApp(string iHostName, int iPortNumber)
		{
			CommandFrame = McFrame.MC3E_ASCII;
			HostName = iHostName;
			PortNumber = iPortNumber;
		}

		// ====================================================================================
		// 後処理
		public void Dispose()
		{
			Close();
		}

		// ====================================================================================
		public int Open()
		{
			DoConnect();
			Command = new McCommand(CommandFrame);
			return 0;
		}
		// ====================================================================================
		public int Close()
		{
			DoDisconnect();
			return 0;
		}

		// ====================================================================================
		public int SetBitDevice(string iDeviceName, int iSize, byte[] onOffBits)
		{
			PlcDeviceType type;
			int addr;
			GetDeviceCode(iDeviceName, out type, out addr);
			return SetBitDevice(type, addr, iSize, onOffBits);
		}

		// ====================================================================================
		// 位单元的连续写入
		public int SetBitDevice(PlcDeviceType iType, int iAddress, int iSize, byte[] onOffBits)
		{
			StringBuilder data = new StringBuilder();
			data.AppendFormat("{0}", iType.ToAsciiName());
			data.AppendFormat("{0:000000}", iAddress);
			data.AppendFormat("{0:0000}", iSize);

			for (int i = 0; i < onOffBits.Length; i++) {
				data.AppendFormat("{0}", onOffBits[i] == 1 ? '1' : '0');
			}

			byte[] sdCommand = Command.SetCommand(0x1401, 0x0001, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);
			return rtCode;
		}

		// ====================================================================================
		// 位单元的随机写入
		public int SetBitDevice(PlcDeviceType iType, Dictionary<int, byte> iAddressAndOnOffMap)
		{
			StringBuilder data = new StringBuilder();
			data.AppendFormat("{0:00}", iAddressAndOnOffMap.Count);
			foreach (var kv in iAddressAndOnOffMap) {
				data.AppendFormat(iType.ToAsciiName());
				data.AppendFormat("{0:000000}", kv.Key);        // 地址
				data.AppendFormat("{0:X2}", kv.Value);			// On/Off
			}

			byte[] sdCommand = Command.SetCommand(0x1402, 0x0001, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);
			return rtCode;
		}

		// ====================================================================================
		public int GetBitDevice(string iDeviceName, int iSize, byte[] outOnOffBits)
		{
			PlcDeviceType type;
			int addr;
			GetDeviceCode(iDeviceName, out type, out addr);
			return GetBitDevice(type, addr, iSize, outOnOffBits);
		}

		// ====================================================================================
		public int GetBitDevice(PlcDeviceType iType, int iAddress, int iSize, byte[] outOnOffBits)
		{
			var data = new StringBuilder(iType.ToAsciiName());
			data.AppendFormat("{0:X6}", iAddress);
			data.AppendFormat("{0:X4}", iSize);
			byte[] sdCommand = Command.SetCommand(0x0401, 0x0001, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);
			byte[] rtData = Command.Response;
			for (int i = 0; i < iSize; ++i) {
				if (i % 2 == 0) {
					outOnOffBits[i] = (byte)((rtCode == 0) ? ((rtData[i / 2] >> 4) & 0x01) : 0);
				} else {
					outOnOffBits[i] = (byte)((rtCode == 0) ? (rtData[i / 2] & 0x01) : 0);
				}
			}
			return rtCode;
		}

		// ====================================================================================
		public int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData)
		{
			PlcDeviceType type;
			int addr;
			GetDeviceCode(iDeviceName, out type, out addr);
			return WriteDeviceBlock(type, addr, iSize, iData);
		}

		// ====================================================================================
		public int WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] iData)
		{
			var data = new StringBuilder(iType.ToAsciiName());
			data.AppendFormat("{0:X6}", iAddress);
			data.AppendFormat("{0:X4}", iSize);
			foreach (int t in iData) {
				data.AppendFormat("{0:X4}", t);
			}
			byte[] sdCommand = Command.SetCommand(0x1401, 0x0000, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);
			return rtCode;
		}

		// ====================================================================================
		public int ReadDeviceBlock(string iDeviceName, int iSize, int[] oData)
		{
			PlcDeviceType type;
			int addr;
			GetDeviceCode(iDeviceName, out type, out addr);
			return ReadDeviceBlock(type, addr, iSize, oData);
		}

		// ====================================================================================
		public int ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] oData)
		{
			var data = new StringBuilder(iType.ToAsciiName());
			data.AppendFormat("{0:X6}", iAddress);
			data.AppendFormat("{0:X4}", iSize);
			byte[] sdCommand = Command.SetCommand(0x0401, 0x0000, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);

			byte[] rtData = Command.Response;
			for (int i = 0; i < iSize; ++i) {
				oData[i] = (rtCode == 0) ? BitConverter.ToInt16(rtData, i * 2) : 0;
			}
			return rtCode;
		}
		// ====================================================================================
		public int SetDevice(string iDeviceName, int iData)
		{
			PlcDeviceType type;
			int addr;
			GetDeviceCode(iDeviceName, out type, out addr);
			return SetDevice(type, addr, iData);
		}
		// ====================================================================================
		public int SetDevice(PlcDeviceType iType, int iAddress, int iData)
		{
			var data = new StringBuilder(iType.ToAsciiName());
			data.AppendFormat("{0:X6}", iAddress);
			data.AppendFormat("{0:X4}", 1);
			data.AppendFormat("{0:X4}", iData);
			byte[] sdCommand = Command.SetCommand(0x1401, 0x0000, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);
			return rtCode;
		}
		// ====================================================================================
		public int GetDevice(string iDeviceName, out int oData)
		{
			PlcDeviceType type;
			int addr;
			GetDeviceCode(iDeviceName, out type, out addr);
			return GetDevice(type, addr, out oData);
		}
		// ====================================================================================
		public int GetDevice(PlcDeviceType iType, int iAddress, out int oData)
		{
			int addr = iAddress;
			var data = new StringBuilder(iType.ToAsciiName());
			data.AppendFormat("{0:X6}", addr);
			data.AppendFormat("{0:X4}", 1);
			byte[] sdCommand = Command.SetCommand(0x0401, 0x0000, data.ToString());
			byte[] rtResponse = TryExecution(sdCommand);
			int rtCode = Command.SetResponse(rtResponse, CommandFrame);
			if (0 < rtCode) {
				oData = 0;
			} else {
				byte[] rtData = Command.Response;
				oData = BitConverter.ToInt16(rtData, 0);
			}
			return rtCode;
		}

		// ====================================================================================
		//public int GetCpuType(out string oCpuName, out int oCpuType)
		//{
		//    int rtCode = Command.Execute(0x0101, 0x0000, new byte[0]);
		//    oCpuName = "dummy";
		//    oCpuType = 0;
		//    return rtCode;
		//}
		// ====================================================================================
		public static PlcDeviceType GetDeviceType(string s)
		{
			return (s == "M") ? PlcDeviceType.M :
				   (s == "SM") ? PlcDeviceType.SM :
				   (s == "L") ? PlcDeviceType.L :
				   (s == "F") ? PlcDeviceType.F :
				   (s == "V") ? PlcDeviceType.V :
				   (s == "S") ? PlcDeviceType.S :
				   (s == "X") ? PlcDeviceType.X :
				   (s == "Y") ? PlcDeviceType.Y :
				   (s == "B") ? PlcDeviceType.B :
				   (s == "SB") ? PlcDeviceType.SB :
				   (s == "DX") ? PlcDeviceType.DX :
				   (s == "DY") ? PlcDeviceType.DY :
				   (s == "D") ? PlcDeviceType.D :
				   (s == "SD") ? PlcDeviceType.SD :
				   (s == "R") ? PlcDeviceType.R :
				   (s == "ZR") ? PlcDeviceType.ZR :
				   (s == "W") ? PlcDeviceType.W :
				   (s == "SW") ? PlcDeviceType.SW :
				   (s == "TC") ? PlcDeviceType.TC :
				   (s == "TS") ? PlcDeviceType.TS :
				   (s == "TN") ? PlcDeviceType.TN :
				   (s == "CC") ? PlcDeviceType.CC :
				   (s == "CS") ? PlcDeviceType.CS :
				   (s == "CN") ? PlcDeviceType.CN :
				   (s == "SC") ? PlcDeviceType.SC :
				   (s == "SS") ? PlcDeviceType.SS :
				   (s == "SN") ? PlcDeviceType.SN :
				   (s == "Z") ? PlcDeviceType.Z :
				   (s == "TT") ? PlcDeviceType.TT :
				   (s == "TM") ? PlcDeviceType.TM :
				   (s == "CT") ? PlcDeviceType.CT :
				   (s == "CM") ? PlcDeviceType.CM :
				   (s == "A") ? PlcDeviceType.A :
								 PlcDeviceType.Max;
		}

		// ====================================================================================
		public static bool IsBitDevice(PlcDeviceType type)
		{
			return !((type == PlcDeviceType.D)
				  || (type == PlcDeviceType.SD)
				  || (type == PlcDeviceType.Z)
				  || (type == PlcDeviceType.ZR)
				  || (type == PlcDeviceType.R)
				  || (type == PlcDeviceType.W));
		}

		// ====================================================================================
		public static bool IsHexDevice(PlcDeviceType type)
		{
			return (type == PlcDeviceType.X)
				|| (type == PlcDeviceType.Y)
				|| (type == PlcDeviceType.B)
				|| (type == PlcDeviceType.W);
		}

		// ====================================================================================
		public static void GetDeviceCode(string iDeviceName, out PlcDeviceType oType, out int oAddress)
		{
			string s = iDeviceName.ToUpper();
			string strAddress;

			// 1文字取り出す
			string strType = s.Substring(0, 1);
			switch (strType) {
			case "A":
			case "B":
			case "D":
			case "F":
			case "L":
			case "M":
			case "R":
			case "V":
			case "W":
			case "X":
			case "Y":
				// 2文字目以降は数値のはずなので変換する
				strAddress = s.Substring(1);
				break;
			case "Z":
				// もう1文字取り出す
				strType = s.Substring(0, 2);
				// ファイルレジスタの場合     : 2
				// インデックスレジスタの場合 : 1
				strAddress = s.Substring(strType.Equals("ZR") ? 2 : 1);
				break;
			case "C":
				// もう1文字取り出す
				strType = s.Substring(0, 2);
				switch (strType) {
				case "CC":
				case "CM":
				case "CN":
				case "CS":
				case "CT":
					strAddress = s.Substring(2);
					break;
				default:
					throw new Exception("Invalid format.");
				}
				break;
			case "S":
				// もう1文字取り出す
				strType = s.Substring(0, 2);
				switch (strType) {
				case "SD":
				case "SM":
					strAddress = s.Substring(2);
					break;
				default:
					throw new Exception("Invalid format.");
				}
				break;
			case "T":
				// もう1文字取り出す
				strType = s.Substring(0, 2);
				switch (strType) {
				case "TC":
				case "TM":
				case "TN":
				case "TS":
				case "TT":
					strAddress = s.Substring(2);
					break;
				default:
					throw new Exception("Invalid format.");
				}
				break;
			default:
				throw new Exception("Invalid format.");
			}

			oType = GetDeviceType(strType);
			oAddress = IsHexDevice(oType) ? Convert.ToInt32(strAddress, BlockSize) :
											Convert.ToInt32(strAddress);
		}

		// &&&&& protected &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
		abstract protected void DoConnect();
		abstract protected void DoDisconnect();
		abstract protected byte[] Execute(byte[] iCommand);
		// &&&&& private &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
		private const int BlockSize = 0x0010;
		private McCommand Command { get; set; }
		// ================================================================================
		private byte[] TryExecution(byte[] iCommand)
		{
			byte[] rtResponse;
			int tCount = 10;
			do {
				rtResponse = Execute(iCommand);
				--tCount;
				if (tCount < 0) {
					throw new Exception("PLC超时无响应.");
				}
			} while (Command.IsIncorrectResponse(rtResponse));
			return rtResponse;
		}

	}
}
