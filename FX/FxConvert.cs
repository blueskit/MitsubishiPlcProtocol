using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.IO.PLC.FX
{
	public static class FxConvert
	{
		/// <summary>
		/// 16进制字符转换为对应的10进制值
		/// </summary>
		/// <param name="c"> 0-9,A-F </param>
		/// <returns>返回对应的 0- 15 </returns>
		public static int HexToDec (char c)
		{
			int v = (int)c;
			if(v >= (int)'0' && v <= (int)'9') {
				return (v - (int)'0');
			} else if(v >= (int)'A' && v <= (int)'F') {
				return (v - (int)'A' + 10);
			} else if(v >= (int)'a' && v <= (int)'f') {
				return (v - (int)'a' + 10);
			}
			return 0;
		}

		/// <summary>
		/// 16进制Byte转换为对应的10进制值
		/// </summary>
		/// <param name="b"> '0'-'9','A'-'F' </param>
		/// <returns>返回对应的 0- 15 </returns>
		public static int HexToDec (byte b)
		{
			if(b >= '0' && b <= '9')
				return b - (byte)'0';
			else if(b >= 'A' && b <= 'F')
				return b - (byte)'A' + 10;
			else if(b >= 'a' && b <= 'f')
				return b - (byte)'a' + 10;
			else
				return (0);
		}

		/// <summary>
		/// 16进制字符串转换为对应的10进制值
		/// </summary>
		/// <param name="s">16进制字符串</param>
		public static int HexToDec (string s)
		{
			int result;
			int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out result);
			return result;
		}


		public static int HexToDec (byte[] data, int fromIndex, int bytes)
		{
			int result = 0;
			for(int i = fromIndex; i < Math.Min(data.Length, fromIndex + bytes); i++) {
				result = result * 16 + HexToDec(data[i]);
			}
			return (result);
		}

		/// <summary>
		/// 16进制串转换为对应的2进制描述格式
		/// 例如输入 "1A"，则返回“00011010”
		/// </summary>
		/// <param name="s">待转换的16进制串</param>
		/// <returns>返回的二进制描述串</returns>
		public static string HexToBin (string hexString)
		{
			string result = string.Empty;

			for(int i = 0; i < hexString.Length; i++) {
				char c = hexString[i];
				int b = HexToDec(c);
				string s = Convert.ToString(b, 2);
				if(s.Length != 4)
					s = new string('0', 4 - s.Length) + s;
				result += s;
			}
			return (result);
		}


		/// <summary>
		/// 数值转换为16进制字符串
		/// </summary>
		/// <param name="v">数值</param>
		/// <returns>对应的16进制串</returns>
		public static string DecToHex (uint v)
		{
			StringBuilder sb = new StringBuilder(8);
			sb.AppendFormat("{0:X}", v);
			if(sb.Length % 2 == 1)
				return ("0" + sb.ToString());
			else
				return sb.ToString();
		}

		/// <summary>
		/// 数值转换为16进制字符串
		/// </summary>
		/// <param name="v">数值</param>
		/// <param name="width">返回的字符串的字符数</param>
		/// <returns>对应的16进制串</returns>
		public static string DecToHex (uint v, int width)
		{
			string s = string.Format("{0:X}", v);
			if(width > s.Length) {
				string s0 = new string('0', width - s.Length);
				s = s0 + s;
			}
			return (s);
		}

		/// <summary>
		/// 10进制字符串转换为值
		/// </summary>
		public static uint DecToValue (string s)
		{
			uint v = 0;
			for(int i = 0; i < s.Length; i++) {
				v = v * 10 + (uint)((char)s[i] - (char)'0');
			}
			return v;
		}

		/// <summary>
		/// 8进制字符串转换为值
		/// </summary>
		public static uint OctToValue (string s)
		{
			uint v = 0;
			for(int i = 0; i < s.Length; i++) {
				v = v * 8 + (uint)((char)s[i] - (char)'0');
			}
			return v;
		}

		/// <summary>
		/// 将给定字符串转换为16进制串
		/// 例如将 “1234”转换为“31323334”
		/// </summary>
		public static string ToHexString (string source)
		{
			StringBuilder sb = new StringBuilder(source.Length * 2);
			for(int i = 0; i < source.Length; i++) {
				char c = source[i];
				sb.Append(DecToHex(c));
			}
			return (sb.ToString());
		}


	}
}
