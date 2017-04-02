using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Vila.Communication.Common;
using Vila.Communication.Data;

namespace InControls.Common
{
	/// <summary>
	/// 工艺及其参数信息
	/// 无论冷、热料，均共享统一结构
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
	public unsafe struct CraftworkItemInfo : IVikiSerialzer
	{
		private const int MAX_RATIO_NO = 10;

		#region 数据成员
		private int _CraftworkID;					// 工艺编码/配方编码

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		private string _CraftworkName;				// 配方名称
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		private string _UserName;				    // 用户名称

		private TimeSpan _MixTime;					// 混合时间

		/// <summary>
		/// 配方的数量/比例(下标为0的元素为占位符，不使用)
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_RATIO_NO + 1)]
		private double[] _Ratio;

		private double _Total;                      // 需要生产的总量

		private double _Once;                       // 单次搅拌的量

		private int _TotalMixCount;                 // 搅拌总次数

		#endregion

		#region 属性实现代码
		public int CraftworkID
		{
			get { return _CraftworkID; }
			set { _CraftworkID = value; }
		}

		public string CraftworkName
		{
			get { return _CraftworkName; }
			set { _CraftworkName = value; }
		}

		public string UserName
		{
			get { return _UserName; }
			set { _UserName = value; }
		}

		/// <summary>
		/// 搅拌混合时间（一般为20-30秒）
		/// </summary>
		public TimeSpan MixTime
		{
			get { return _MixTime; }
			set { _MixTime = value; }
		}

		/// <summary>
		/// 配方的数量/比例
		/// </summary>
		/// <param name="ratioIndex">配方中材料的序号，从1..Max</param>
		/// <returns></returns>
		public double this[int ratioIndex]
		{
			get
			{
				Debug.Assert(ratioIndex > 0, "配方中材料的序号,必须从1开始编码");
				return _Ratio[ratioIndex];
			}
			set
			{
				Debug.Assert(ratioIndex > 0, "配方中材料的序号,必须从1开始编码");
				_Ratio[ratioIndex] = value;
			}
		}

		public double this[CraftworkMaterialNoEnum ratioIndex]
		{
			get { return _Ratio[(int)ratioIndex]; }
			set { _Ratio[(int)ratioIndex] = value; }
		}

		/// <summary>
		/// 放回配方(比例)数组
		/// </summary>
		public List<double> Ratio
		{
			get { return new List<double>(_Ratio); }
		}

		/// <summary>
		/// 返回配方重量数组
		/// </summary>
		public List<double> Weight
		{
			get { return new List<double>(_Ratio); }
		}

		/// <summary>
		/// 总生产量
		/// </summary>
		public double Total
		{
			get { return _Total; }
			set { _Total = value; }
		}

		/// <summary>
		/// 一次搅拌的量
		/// </summary>
		public double OnceMix
		{
			get { return _Once; }
			private set { _Once = value; }
		}

		/// <summary>
		/// 搅拌次数
		/// </summary>
		public int TotalMixCount
		{
			get { return _TotalMixCount; }
			set { _TotalMixCount = value; }
		}

		public double M01Ratio { get { if (_Ratio == null) return 0; return _Ratio[1]; } }
		public double M02Ratio { get { if (_Ratio == null) return 0; return _Ratio[2]; } }
		public double M03Ratio { get { if (_Ratio == null) return 0; return _Ratio[3]; } }
		public double M04Ratio { get { if (_Ratio == null) return 0; return _Ratio[4]; } }
		public double M05Ratio { get { if (_Ratio == null) return 0; return _Ratio[5]; } }
		public double M06Ratio { get { if (_Ratio == null) return 0; return _Ratio[6]; } }
		public double M07Ratio { get { if (_Ratio == null) return 0; return _Ratio[7]; } }
		public double M08Ratio { get { if (_Ratio == null) return 0; return _Ratio[8]; } }
		public double M09Ratio { get { if (_Ratio == null) return 0; return _Ratio[9]; } }		// 油石比。已经改为 百分比
		public double M10Ratio { get { if (_Ratio == null) return 0; return _Ratio[10]; } }


		public double M01Wight { get { return ToCraftworkWeightArray()[1]; } }
		public double M02Wight { get { return ToCraftworkWeightArray()[2]; } }
		public double M03Wight { get { return ToCraftworkWeightArray()[3]; } }
		public double M04Wight { get { return ToCraftworkWeightArray()[4]; } }
		public double M05Wight { get { return ToCraftworkWeightArray()[5]; } }
		public double M06Wight { get { return ToCraftworkWeightArray()[6]; } }
		public double M07Wight { get { return ToCraftworkWeightArray()[7]; } }
		public double M08Wight { get { return ToCraftworkWeightArray()[8]; } }
		public double M09Wight { get { return ToCraftworkWeightArray()[9]; } }
		public double M10Wight { get { return ToCraftworkWeightArray()[10]; } }
		#endregion

		#region 外部内存复制函数声明
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(ref CraftworkItemInfo dest, IntPtr src, int size_t);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static unsafe extern void CopyMemory(IntPtr dest, ref CraftworkItemInfo src, int size_t);
		#endregion

		public CraftworkItemInfo(int craftworkID, string craftworkName)
			: this(craftworkID, craftworkName, "", TimeSpan.FromSeconds(0D), 0, null)
		{
		}

		public CraftworkItemInfo(int craftworkID, string craftworkName, string userName, TimeSpan mixTime, double onceWeight, List<double> ratio)
		{
			_CraftworkID = craftworkID;
			_CraftworkName = craftworkName;
			_MixTime = mixTime;
			_UserName = userName;
			_Total = 0D;
			_Once = onceWeight;
			_TotalMixCount = 0;
			_Ratio = new double[MAX_RATIO_NO + 1];
			if (ratio != null) Update(onceWeight, ratio);
		}

		/// <summary>
		/// 更新配方比例
		/// </summary>
		/// <param name="ratio">新的配方/或更改后的配方</param>
		[Obsolete("这个函数应废弃，务必调用 Update(double onceWight, double[] ratio) 函数！ ")]
		public void Update(List<double> ratio)
		{
			if (ratio != null) {
				for (int i = 0; i < Math.Min(ratio.Count, MAX_RATIO_NO); i++) {
					_Ratio[i] = ratio[i];
				}
			}
		}


		/// <summary>
		/// 更新配方比例
		/// </summary>
		/// <param name="onceWight">单次搅拌的重量（Kg）</param>
		/// <param name="ratio">新的配方/或更改后的配方。注意：0下标元素为占位符</param>
		public void Update(double onceWight, List<double> ratio)
		{
			Debug.Assert(ratio[0] == 0, "可能使用了错误的 0 下标");

			_Once = onceWight;
			if (ratio != null) {
				if (_Ratio == null) _Ratio = new double[MAX_RATIO_NO + 1];
				for (int i = 0; i < Math.Min(ratio.Count, MAX_RATIO_NO); i++) {
					_Ratio[i] = ratio[i];
				}
			}
		}

		public void Update(int craftworkID, string craftworkName, int totalMixCount, TimeSpan mixTime, double onceWight, List<double> ratio)
		{
			_CraftworkID = craftworkID;
			_CraftworkName = craftworkName;
			_MixTime = mixTime;
			_TotalMixCount = totalMixCount;

			Update(onceWight, ratio);
		}

		public void Update(ref CraftworkItemInfo info)
		{
			Debug.Assert(info.Ratio[0] == 0, "可能使用了错误的 0 下标");

			_CraftworkID = info.CraftworkID;
			_CraftworkName = info.CraftworkName;

			_UserName = info.UserName;

			_MixTime = info.MixTime;
			_Once = info.OnceMix;

			_Total = info._Total;
			_TotalMixCount = info._TotalMixCount;

			if (_Ratio == null) {
				_Ratio = new double[info.Ratio.Count];
			}

			for (int i = 0; i < info.Ratio.Count; i++) {
				_Ratio[i] = info.Ratio[i];
			}

		}

		/// <summary>
		/// 得到配方的重量
		/// </summary>
		public List<double> ToCraftworkWeightArray()
		{
			double[] lst = new double[11];

			if (_Ratio != null) {
				for (int i = 0; i < _Ratio.Length; i++) {

					if (i <= 8) {
						lst[i] = _Ratio[i] * _Once;
					} else {
						lst[i] = _Ratio[i] * _Once;		// 千分比 从数据库读出时已经改为百分比
					}
				}
			}

			return new List<double>(lst);
		}


		/// <summary>
		/// 转换为比例方式
		/// </summary>
		public void ConvertToScaleMode()
		{
			double sum = 0D;
			int ct = 0;

			for (int i = 1; i <= Math.Min(8, _Ratio.Length); i++) {
				sum += _Ratio[i];
				ct = i;
			}

			if (sum > 0) {
				for (int i = 1; i <= ct; i++) {
					_Ratio[i] = _Ratio[i] / sum;
				}
			}
		}



		#region IVikiSerialzer 成员

		public byte[] ToBytesArray()
		{
			byte[] buff = new byte[Marshal.SizeOf(this)];
			fixed (byte* pDest = &buff[0]) {
				CopyMemory((IntPtr)pDest, ref this, buff.Length);
			}
			return (buff);
		}


		/// <summary>
		/// 将当前结构复制到目标字节数组（指定的起始偏移处）
		/// </summary>
		public void ConvertTo(out byte[] destBuff, int destPos)
		{
			int sizeThis = Marshal.SizeOf(this);
			destBuff = new byte[sizeThis + destPos];

			fixed (byte* pDest = &destBuff[destPos]) {
				CopyMemory((IntPtr)pDest, ref this, sizeThis);
			}
		}

		/// <summary>
		/// 从给定的字节数组构建当前结构实例
		/// </summary>
		public void ConvertFrom(byte[] sourceBuff, int fromPos)
		{
			if (sourceBuff.Length > fromPos) {
				fixed (void* pSource = &sourceBuff[fromPos]) {
					CopyMemory(ref this, (IntPtr)pSource, Math.Min(Marshal.SizeOf(this), sourceBuff.Length - fromPos));
				}
			}
		}
		#endregion
	}
}
