using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InControls.Common;

namespace InControls.PLC.FX
{

	/// <summary>
	/// FX/Qn系列地址描述符
	/// 构造时需提供PLC控制器类型
	/// </summary>
	public class FxAddress
	{
		private FxAddressLayoutType _AddressLayoutType;
		private FxAddressType _AddressType;
		private string _TagName;			// 符号名(支持X/Y/S/T/C/M/D),包含地址类型和偏移量
		private int _TagOffset;				// 符号名的偏移。X/Y/S/T/C/M/D/.. 等之后的地址码
		private uint _UniformAddr;			// 统一地址！内部地址索引（所有类型统一编址）
		private int _BaseNumberOfXY = 8;	// 针对不同PLC控制器，X/Y符号中的地址偏移可能采用 8进制、16进制。默认 8 进制

		#region 属性代码块
		public FxAddressLayoutType AddressSpaceType
		{
			get { return _AddressLayoutType; }
		}

		public FxAddressType AddressType
		{
			get { return _AddressType; }
		}
		public string TagName
		{
			get { return _TagName; }
		}

		public int TagOffset
		{
			get { return _TagOffset; }
		}

		public uint UniformAddr
		{
			get { return _UniformAddr; }
		}
		#endregion

		public FxAddress ()
		{
			_TagName = string.Empty;
			_TagOffset = 0;
			_UniformAddr = 0;
			_AddressLayoutType = FxAddressManager.DefaultLayoutType;
			_AddressType = FxAddressType.D;
			_BaseNumberOfXY = 8;			// 默认支持 Fx 系列
		}

		/// <summary>
		/// 按给定符号名、控制器类型构建地址对象（推荐使用）
		/// </summary>
		public FxAddress (string tagName, ControllerTypeConst controllerType)
		{
			_BaseNumberOfXY = controllerType.ToBaseNumber();

			_TagName = tagName;
			_AddressType = FxAddressManager.Instance[tagName];

			// 按实际地址更新 地址布局类型
			switch(_AddressType) {
			case FxAddressType.X:
			case FxAddressType.Y:
			case FxAddressType.M:
				_AddressLayoutType = FxAddressLayoutType.AddressLayoutBin;
				break;
			case FxAddressType.T:
				_AddressLayoutType = FxAddressLayoutType.AddressLayoutInt32;
				break;
			default:
				_AddressLayoutType = FxAddressLayoutType.AddressLayoutBin;
				break;
			}

			_TagOffset = GetTagOffset(tagName, _BaseNumberOfXY);

			if(controllerType.IsFxPLCController())
				_UniformAddr = GetUniformAddr(tagName, out _AddressType, _AddressLayoutType);

		}

		/// <summary>
		/// 按默认的 FX 系列构造地址
		/// </summary>
		[Obsolete("停止使用本构造函数。请同时指定PLC控制器类型")]
		public FxAddress (string tagName)
			: this(tagName, ControllerTypeConst.ctPLC_Fx)
		{
		}

		public FxAddress (int uniformAddr)
		{
		}

		/// <summary>
		/// 根据地址符号与地址偏移构建地址
		/// </summary>
		/// <param name="tag">地址符号，一个字符。X/Y/M/C/T/S/D/...</param>
		/// <param name="offset">地址偏移量</param>
		[Obsolete("请勿使用本构造函数。应明确指定控制器类型")]
		public FxAddress (string tag, int offset)
			: this(FxAddressManager.Instance[tag], offset)
		{
		}

		[Obsolete("请勿使用本构造函数。应明确指定控制器类型")]
		public FxAddress (FxAddressType addressType, int offset)
			: this(ControllerTypeConst.ctPLC_Fx, addressType, offset)
		{
		}

		/// <summary>
		/// 根据地址类型与地址偏移构建地址
		/// </summary>
		public FxAddress (ControllerTypeConst controllerType, FxAddressType addressType, int offset)
		{
			_BaseNumberOfXY = controllerType.ToBaseNumber();
			_TagOffset = offset;
			_AddressType = addressType;
			_AddressLayoutType = FxAddressManager.DefaultLayoutType;
			_TagName = ToFormatedTagName();
			_UniformAddr = GetUniformAddr(_TagName, out _AddressType, _AddressLayoutType);
		}

		public FxAddress (string tagName, FxAddressLayoutType addrLayoutType)
		{
			_TagName = tagName;
			_AddressLayoutType = addrLayoutType;
			_TagOffset = GetTagOffset(tagName, _BaseNumberOfXY);
			_UniformAddr = GetUniformAddr(tagName, out _AddressType, addrLayoutType);
		}

		#region 静态的功能支持函数集

		[Obsolete("这个函数仅用于 FX 系列PLC,请明确指定 X/Y 的进制")]
		public static int GetTagOffset (string tagName)
		{
			return GetTagOffset(tagName, 8);
		}

		public static int GetTagOffset (string tagName, int baseNum)
		{
			if(string.IsNullOrEmpty(tagName))
				return 0;

			int off = 0;
			switch(tagName[0]) {
			case 'X':
			case 'x':
			case 'Y':
			case 'y':
				if(baseNum == 8)
					off = (int)FxConvert.OctToValue(tagName.Substring(1));			// 8进制
				else
					off = (int)FxConvert.HexToDec(tagName.Substring(1));			// 16进制
				break;
			default:
				off = (int)FxConvert.DecToValue(tagName.Substring(1));				// 十进制
				break;
			}
			return off;
		}

		/// <summary>
		/// 根据地址符号名称得到内部地址索引
		/// </summary>
		/// <param name="tagName">以一个字符X/Y/M/C/T/S/D/加上10进制的序号构成</param>
		/// <returns>返回的地址序号，如果为0一般表示地址错误或不存在</returns>
		public static uint GetUniformAddr (string tagName, out FxAddressType addrType)
		{
			return (GetUniformAddr(tagName, out addrType, FxAddressManager.DefaultLayoutType));
		}

		/// <summary>
		/// 根据地址符号名称得到内部地址索引
		/// 这是函数仅由 FX 系列PLC使用
		/// </summary>
		/// <param name="tagName">以一个字符X/Y/M/C/T/S/D/加上10进制的序号构成</param>
		/// <returns>返回的地址序号，如果为0一般表示地址错误或不存在</returns>
		public static uint GetUniformAddr (string tagName, out FxAddressType addrType, FxAddressLayoutType addrLayoutType)
		{
			if(string.IsNullOrWhiteSpace(tagName)) {
				addrType = FxAddressType.Undefine;
				return (0);
			}

			char tagType = tagName[0];

			uint pos = (uint)GetTagOffset(tagName, 8);
			if(addrLayoutType == FxAddressLayoutType.AddressLayoutBin) {
				pos = (uint)(pos / 8);
			}

			uint off = 0;

			switch(tagType) {
			case 'X':
			case 'x':
				addrType = FxAddressType.X;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			case 'Y':
			case 'y':
				addrType = FxAddressType.Y;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			case 'M':
			case 'm':
				addrType = FxAddressType.M;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			case 'S':
			case 's':
				addrType = FxAddressType.S;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			case 'T':
			case 't':
				addrType = FxAddressType.T;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			case 'C':
			case 'c':
				addrType = FxAddressType.C;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			case 'D':
			case 'd':
				addrType = FxAddressType.D;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				pos *= 2;
				break;
			case 'K':
			case 'k':
				addrType = FxAddressType.K;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				break;
			default:								// 默认视作“D"类型
				addrType = FxAddressType.D;
				off = FxAddressManager.Instance[addrLayoutType, addrType];
				pos = 0;
				break;
			}

			return (off + pos);
		}


		public static string GetTagNameByUniformAddr (int uniformAddr)
		{
			string addr = string.Empty;


			return (addr);
		}

		#endregion


		/// <summary>
		/// 是否是合法的地址？
		/// 只要“格式”符合要求即可，不对地址范围做限制
		/// </summary>
		public bool IsValidAddress ()
		{
			return _AddressType != FxAddressType.Undefine;
		}

		/// <summary>
		/// 得到格式化后的tagName。
		/// 主要将不规则的表达式，格式化为标准写法。例如:
		///		偏移部分为8进制：	X01 --> X001,Y17 -> Y017			
		///		偏移部分为10进制：	M1 -> M001, D8--> D008				
		/// </summary>
		public string ToFormatedTagName ()
		{
			if(_AddressType == FxAddressType.X || _AddressType == FxAddressType.Y) {

				string s1 = string.Empty;
				switch(_BaseNumberOfXY) {
				case 8:
					s1 = Convert.ToString(_TagOffset, _BaseNumberOfXY).ToUpper();
					if(s1.Length < 3) {
						s1 = "000".Substring(0, 3 - s1.Length) + s1;
					}
					break;
				case 10:
					s1 = string.Format("{0:000}", _TagOffset);
					break;
				case 16:
					s1 = string.Format("{0:X3}", _TagOffset);
					break;
				}

				return _AddressType.ToString() + s1;

			} else if(_AddressType != FxAddressType.Undefine)
				return _AddressType.ToString() + (_TagOffset > 999 ? _TagOffset.ToString("0000") : _TagOffset.ToString("000"));
			else
				return _TagName;
		}

		/// <summary>
		/// 返回标准格式的地址字符串,用于MOD-BUS协议控制命令中的“地址”部分
		/// 注意：地址需要交换高低字节
		/// </summary>
		public string ToAddressHexString ()
		{
			uint addr = _UniformAddr;
			if(_AddressLayoutType != FxAddressLayoutType.AddressLayoutBin) {
				addr = ((_UniformAddr >> 8) & 0xff) | ((_UniformAddr & 0xff) << 8);
			}
			return string.Format("{0:X4}", addr);
		}

		public override bool Equals (object obj)
		{
			return ((FxAddress)obj)._AddressType == this._AddressType
					&& ((FxAddress)obj)._TagOffset == this._TagOffset;
		}

		public override int GetHashCode ()
		{
			return _TagName.GetHashCode();
		}

		public override string ToString ()
		{
			return ToFormatedTagName();
		}

	}
}
