using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.PLC.FX
{
	public class FxAddressManager
	{
		#region 单实例
		private static FxAddressManager _Instance;

		public static FxAddressManager Instance
		{
			get
			{
				if(_Instance == null)
					_Instance = new FxAddressManager();
				return FxAddressManager._Instance;
			}
		}
		#endregion

		private Dictionary<FxAddressLayoutType, FxAddressLayoutInfo> _LayoutList;
		private Dictionary<string, FxAddressType> _FxAddressTypeDictionary;

		public const FxAddressLayoutType DefaultLayoutType = FxAddressLayoutType.AddressLayoutByte;			// 最常用地址布局

		#region 属性代码块。用于根据布局类型/地址类型返回初始偏移量

		public FxAddressType this[string tagChar]
		{
			get
			{
				if(string.IsNullOrEmpty(tagChar))
					return FxAddressType.Undefine;
				else {
					if(tagChar.Length > 1)
						tagChar = tagChar.Substring(0, 1);

					if(_FxAddressTypeDictionary.ContainsKey(tagChar)) {
						return _FxAddressTypeDictionary[tagChar];
					} else
						return FxAddressType.Undefine;
				}
			}
		}

		public FxAddressLayoutInfo this[FxAddressLayoutType layoutType]
		{
			get
			{
				if(_LayoutList.ContainsKey(layoutType))
					return (_LayoutList[layoutType]);
				return (null);
			}
		}

		public uint this[FxAddressType addrType]
		{
			get
			{
				if(_LayoutList.ContainsKey(DefaultLayoutType)) {
					return (_LayoutList[DefaultLayoutType][addrType]);
				}
				return (0);
			}
		}

		public uint this[FxAddressLayoutType layoutType, FxAddressType addrType]
		{
			get
			{
				if(_LayoutList.ContainsKey(layoutType)) {
					return (_LayoutList[layoutType][addrType]);
				}
				return (0);
			}
		}
		#endregion

		private FxAddressManager ()
		{
			LoadDefault();
		}

		private void LoadDefault ()
		{
			_LayoutList = new Dictionary<FxAddressLayoutType, FxAddressLayoutInfo>();
			_LayoutList.Add(FxAddressLayoutType.AddressLayoutBin, new FxAddressLayoutInfo(GetLayoutInfo_BinLayout()));
			_LayoutList.Add(FxAddressLayoutType.AddressLayoutByte, new FxAddressLayoutInfo(GetLayoutInfo_ByteLayout()));
			_LayoutList.Add(FxAddressLayoutType.AddressLayoutInt16, new FxAddressLayoutInfo(GetLayoutInfo_Int16Layout()));
			_LayoutList.Add(FxAddressLayoutType.AddressLayoutInt32, new FxAddressLayoutInfo(GetLayoutInfo_Int32Layout()));

			_FxAddressTypeDictionary = new Dictionary<string, FxAddressType>();
			foreach(string s in Enum.GetNames(typeof(FxAddressType))) {
				_FxAddressTypeDictionary.Add(s, (FxAddressType)Enum.Parse(typeof(FxAddressType), s));
			}

		}

		#region 构建支持的各个布局信息块并返回

		/// <summary>
		/// FX系列地址描述符(BIN)
		/// 地址算法:
		///		D:	address	= address*2 + 1000h
		///		X:	address = address + 0x80
		///		Y:　	address = address + 0xA0;
		///		M:　	address = address + 0x100;　　
		///		S:　	address = address ;
		///		T:　	address = address + 0xC0;
		///		C:　	address = address + 0x1C0;
		/// 设备强制中的地址公式:
		///				Address	= Address/8+100h
		/// </summary>
		private Dictionary<FxAddressType, uint> GetLayoutInfo_BinLayout ()
		{
			Dictionary<FxAddressType, uint> lst = new Dictionary<FxAddressType, uint>();
			lst.Add(FxAddressType.X, 0x0080);
			lst.Add(FxAddressType.Y, 0x00A0);
			lst.Add(FxAddressType.M, 0x0100);
			lst.Add(FxAddressType.S, 0x0000);
			lst.Add(FxAddressType.T, 0x00C0);
			lst.Add(FxAddressType.C, 0x01C0);
			lst.Add(FxAddressType.D, 0x1000);
			return (lst);
		}

		/// <summary>
		/// FX系列地址描述符
		/// 地址算法:
		///		X:		address = address + 0x0400
		///		Y:　	address = address + 0x0500;
		///		M:　	address = address + 0x0800;　　
		///		S:　	address = address ;
		///		T:　	address = address + 0x0600;
		///		C:　	address = address + 0x0E00;
		///		D:		address	= address*2 + 1000h	(short类型)
		/// 设备强制中的地址公式:
		///				Address	= Address/8+100h
		/// </summary>
		private Dictionary<FxAddressType, uint> GetLayoutInfo_ByteLayout ()
		{
			Dictionary<FxAddressType, uint> lst = new Dictionary<FxAddressType, uint>();
			lst.Add(FxAddressType.X, 0x0400);
			lst.Add(FxAddressType.Y, 0x0500);
			lst.Add(FxAddressType.M, 0x0800);
			lst.Add(FxAddressType.S, 0);
			lst.Add(FxAddressType.T, 0x0600);
			lst.Add(FxAddressType.C, 0x0E00);
			lst.Add(FxAddressType.D, 0x1000);
			return (lst);
		}

		private Dictionary<FxAddressType, uint> GetLayoutInfo_Int16Layout ()
		{
			Dictionary<FxAddressType, uint> lst = new Dictionary<FxAddressType, uint>();
			lst.Add(FxAddressType.X, 0x0400);
			lst.Add(FxAddressType.Y, 0x0500);
			lst.Add(FxAddressType.M, 0x0800);
			lst.Add(FxAddressType.S, 0);
			lst.Add(FxAddressType.T, 0x0600);
			lst.Add(FxAddressType.C, 0x0E00);
			lst.Add(FxAddressType.D, 0x1000);
			return (lst);
		}

		private Dictionary<FxAddressType, uint> GetLayoutInfo_Int32Layout ()
		{
			Dictionary<FxAddressType, uint> lst = new Dictionary<FxAddressType, uint>();
			lst.Add(FxAddressType.X, 0x0400);
			lst.Add(FxAddressType.Y, 0x0500);
			lst.Add(FxAddressType.M, 0x0800);
			lst.Add(FxAddressType.S, 0);
			lst.Add(FxAddressType.T, 0x0600);
			lst.Add(FxAddressType.C, 0x0E00);
			lst.Add(FxAddressType.D, 0x1000);
			return (lst);
		}

		#endregion
	}
}
