using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.IO.PLC.Mitsubishi
{
	/// <summary>
	/// 对PlcDeviceType的扩展：
	///     提供ASCII方式下的命令与二进制下的对照表
	/// </summary>
	public static class PlcDeviceTypeExtersions
	{
		private static Dictionary<PlcDeviceType, string> typeMapping = new Dictionary<PlcDeviceType, string>() {
			{PlcDeviceType.M,"M*"},
			{PlcDeviceType.X,"X*"},
			{PlcDeviceType.Y,"Y*"},
			{PlcDeviceType.D,"D*"},
			{PlcDeviceType.R,"R*"},
			{PlcDeviceType.TN,"TN"},
		};

		public static string ToAsciiName(this PlcDeviceType deviceType)
		{
			return typeMapping[deviceType];
		}

		public static byte[] ToAsciiNameBytes(this PlcDeviceType deviceType)
		{
			return ASCIIEncoding.ASCII.GetBytes(typeMapping[deviceType]);
		}

		public static PlcDeviceType ToPlcDeviceType(this string deviceTypeName)
		{
			foreach (var kv in typeMapping) {
				if (kv.Value.Equals(deviceTypeName, StringComparison.OrdinalIgnoreCase))
					return kv.Key;
			}
			return PlcDeviceType.M;
		}

	}
}
