using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InControls.PLC.FX;

namespace InControls.PLC.MCPackage
{
	/// <summary>
	/// 主要用于PLC反馈数据的解析处理
	/// </summary>
	public class ResponseMessage
	{
		public McCommandResponseConst ResultCode { get; set; }

		private MCComponentGroup _componentGroup;

		public MCComponentGroup ComponentGroup
		{
			get { return _componentGroup; }
		}

		public ResponseMessage()
		{
			ResultCode = McCommandResponseConst.UNKNOWN;
			_componentGroup = new MCComponentGroup();
		}

		/// <summary>
		/// 获取地址类型对应的ASCII名。用于MC通讯
		/// </summary>
		public static string GetComponentTagName(FxAddressType componentType)
		{
			switch (componentType) {
			case FxAddressType.X:
				return "X*";
			case FxAddressType.Y:
				return "Y*";
			case FxAddressType.M:
				return "M*";
			case FxAddressType.C:
				return "CS";                        // 视作“触点计数器”
			case FxAddressType.D:
				return "D*";
			}
			return "D*";
		}

		/// <summary>
		/// 获取地址类型对应的 Bin 值。用于MC通讯
		/// 参见《MC通讯协议参考手册》3-60页
		/// </summary>
		public static byte GetComponentTagValue(FxAddressType componentType)
		{
			switch (componentType) {
			case FxAddressType.X:
				return 0x9C;
			case FxAddressType.Y:
				return 0x9D;
			case FxAddressType.M:
				return 0x90;
			case FxAddressType.C:
				return 0xC4;            // 视作“触点计数器”
			case FxAddressType.D:
				return 0xA8;
			}
			return 0xA8;                // 默认作为 FxAddressType.D
		}

	}
}
