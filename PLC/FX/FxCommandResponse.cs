using System;
using System.Collections.Generic;
using System.Text;
using InControls.Common;

namespace InControls.PLC.FX
{
	public class FxCommandResponse
	{
		private ICellDataType _ResponseDataType;		// 返回的数据类型,默认 UShortDataType
		private List<int> _ResponseValue;

		public ResultCodeConst ResultCode { get; set; }

		#region 属性代码块
		public ICellDataType ResponseDataType
		{
			get { return _ResponseDataType; }
			set { _ResponseDataType = value; }
		}
		public List<int> ResponseValue
		{
			get { return _ResponseValue; }
		}
		#endregion

		/// <summary>
		/// 接收到的完整的数据报文
		/// </summary>
		public byte[] RawData { get; private set; }



		public FxCommandResponse(ResultCodeConst resultCode)
			: this(resultCode, null)
		{
		}

		public FxCommandResponse(ResultCodeConst resultCode, byte[] rawData)
			: this(resultCode, rawData, new UInt16DataType())
		{
		}

		public FxCommandResponse(ResultCodeConst resultCode, byte[] rawData, ICellDataType responseDataType)
		{
			ResultCode = resultCode;
			RawData = rawData;
			_ResponseDataType = responseDataType;
		}

		public void SetRawData(byte[] rawData)
		{
			RawData = rawData;
		}

		public void SetResponseValue(List<int> value)
		{
			_ResponseValue = value;
		}


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("ResultCode={0}", ResultCode);
			if (_ResponseValue != null) {
				sb.AppendFormat(",Values=", ResultCode);
				foreach (var v in _ResponseValue)
					sb.AppendFormat("[{0:X},{1}] ", v, Convert.ToString(v, 2));
			} else {
				sb.AppendFormat(",Values=Nothing");
			}
			return sb.ToString();
		}

	}
}
