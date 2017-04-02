using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.PLC.FX
{
	/// <summary>
	/// 布局定义类
	/// 
	/// 用于提供特定布局下各个地址段(X/Y/M/D/C/T/...)在统一编址下的起始偏移量
	/// </summary>
	public class FxAddressLayoutInfo : IDisposable
	{
		private Dictionary<FxAddressType, uint> _OffsetCollection;

		public FxAddressLayoutInfo()
			: this(new Dictionary<FxAddressType, uint>())
		{
		}

		public FxAddressLayoutInfo(Dictionary<FxAddressType, uint> offsetCollecton)
		{
			_OffsetCollection = offsetCollecton;
		}

		public uint this[FxAddressType addrType]
		{
			get
			{
				if (_OffsetCollection.ContainsKey(addrType))
					return (_OffsetCollection[addrType]);
				return (0);
			}
		}

		private void Dispose(bool disposing)
		{
			if (_OffsetCollection != null) {
				_OffsetCollection.Clear();
				_OffsetCollection = null;
			}
		}

		#region IDisposable 成员

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
