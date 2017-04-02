using System;
using System.Linq;
using System.Text;
using InControls.PLC.MCPackage;

namespace InControls.PLC.Mitsubishi
{
	public static class McCommandExtersions
	{
		/// <summary>
		/// 执行命令，并返回执行的结果
		/// 读写PLC单元的字符串型命令格式如下：
		///		excute("D126");				读单一元件值
		///		excute("D126,2");			读连续多个元件值
		///		excute("M100,8");			读连续多个元件值
		///		excute("D10=135");			为单一元件赋值
		///		excute("M401=1");			为单一元件赋值
		///		excute("D126,6=0");			为连续元件赋值
		///		excute("D126..130=0");		为连续元件赋值
		/// </summary>
		/// <param name="plc"></param>
		/// <param name="cmdText">读写PLC单元的字符串型命令</param>
		internal static ResponseMessage Excute(this IMitsubishiPlc plc, string cmdText)
		{
			bool isMulti = cmdText.Contains("..") || cmdText.Contains(',');
			bool isWrite = cmdText.Contains("=");

			if (isWrite) {
				if (isMulti) return writeMulti(plc, cmdText);
				else return writeSingle(plc, cmdText);
			} else {
				if (isMulti) return readMulti(plc, cmdText);
				else return readSingle(plc, cmdText);
			}
		}


		#region 具体读写函数
		/// <summary>
		/// 读取单个单元
		/// 例如“D10”、“M100”、“X201”等格式
		/// </summary>
		private static ResponseMessage readSingle(this IMitsubishiPlc plc, string cmdText)
		{
			ResponseMessage resp = new ResponseMessage();
			StringBuilder result = new StringBuilder();

			PlcDeviceType type;
			int addr;
			McProtocolApp.GetDeviceCode(cmdText.ToUpper(), out type, out addr);

			int n;
			int rtCode;
			if (McProtocolApp.IsBitDevice(type)) {
				var data = new byte[1];
				rtCode = plc.GetBitDevice(cmdText, data.Length, data);
				n = data[0];
			} else {
				rtCode = plc.GetDevice(cmdText.ToUpper(), out n);
			}
			result.AppendLine(cmdText.ToUpper() + "=" + n.ToString());
			if (0 < rtCode) {
				result.AppendLine("ERROR:0x" + rtCode.ToString("X4"));
			}

			return resp;
		}

		/// <summary>
		/// 读取多个单元
		/// 例如“D10,2”、“M100,20”、“X201,8”等格式
		/// </summary>
		private static ResponseMessage readMulti(this IMitsubishiPlc plc, string cmdText)
		{
			ResponseMessage resp = new ResponseMessage();
			StringBuilder result = new StringBuilder();

			string[] s = cmdText.Split(new string[] { ",", ".." }, StringSplitOptions.None);
			bool hasDots = cmdText.Contains("..");

			if (s.Length == 2) {
				PlcDeviceType type;
				int addr;
				McProtocolApp.GetDeviceCode(s[0], out type, out addr);

				var n = int.Parse(s[1]);
				var val = new byte[hasDots ? n - addr + 1 : n];
				var ival = new int[Math.Max(16, val.Length)];
				int rtCode = McProtocolApp.IsBitDevice(type) ? plc.GetBitDevice(s[0], val.Length, val) :
															   plc.ReadDeviceBlock(s[0], val.Length, ival);
				if (0 < rtCode) {
					result.AppendLine("ERROR:0x" + rtCode.ToString("X4"));
				} else {
					for (int i = 0; i < val.Length; ++i) {
						result.AppendLine(type.ToString() + (addr + i).ToString() + "=" + val[i].ToString());
					}
				}
			}
			return resp;
		}

		/// <summary>
		/// 为单一单元赋值
		///		"D10=0"
		///		"M301=1"
		/// </summary>
		private static ResponseMessage writeSingle(this IMitsubishiPlc plc, string cmdText)
		{
			ResponseMessage resp = new ResponseMessage();
			StringBuilder result = new StringBuilder();

			string[] s = cmdText.Split('=');

			PlcDeviceType type;
			int addr;
			McProtocolApp.GetDeviceCode(s[0], out type, out addr);

			int val = int.Parse(s[1]);
			int rtCode;
			if (McProtocolApp.IsBitDevice(type)) {
				var data = new byte[1];
				data[0] = (byte)val;
				rtCode = plc.SetBitDevice(s[0], data.Length, data);
			} else {
				rtCode = plc.SetDevice(s[0], val);
			}
			result.AppendLine(cmdText.ToUpper());
			if (0 < rtCode) {
				result.AppendLine("ERROR:0x" + rtCode.ToString("X4"));
			}

			return resp;
		}

		private static ResponseMessage writeMulti(this IMitsubishiPlc plc, string cmdText)
		{
			ResponseMessage resp = new ResponseMessage();
			StringBuilder result = new StringBuilder();

			bool hasDots = cmdText.Contains("..");
			string[] s = cmdText.Split(new string[] { ",", "..", "=" }, StringSplitOptions.None);

			int m;
			int n = int.Parse(s[1]);

			PlcDeviceType type;
			McProtocolApp.GetDeviceCode(s[0], out type, out m);

			byte[] data;

			if (hasDots) data = new byte[n - m + 1];
			else data = new byte[n];

			var v = byte.Parse(s[2]);
			for (int i = 0; i < data.Length; ++i) {
				data[i] = v;
			}
			var iData = new int[16];

			int rtCode = McProtocolApp.IsBitDevice(type) ? plc.SetBitDevice(s[0], 1, data) :
														   plc.WriteDeviceBlock(s[0], data.Length, iData);
			result.AppendLine(cmdText.ToUpper());
			if (0 < rtCode) {
				result.AppendLine("ERROR:0x" + rtCode.ToString("X4"));
			}


			return resp;
		}
		#endregion
	}
}
