using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using InControls.PLC.FX;
using Vila.Common;

namespace InControls.Common
{
	/// <summary>
	/// 采集点的原始值(从控制器读出的未经换算的值)
	/// </summary>
	public class AcquireRawValue
	{
		#region 数据成员
		private int _Id;											// 软单元（采集点）全局唯一索引号(冗余信息).有时候不准确
		private int _ControllerId;									// 控制器编号，包括 PLC/CP920/SWP20/...
		private int _ControllerChannelNo;							// 控制器内的通道号。如果是PLC, 必须使用 _PLCAddr
		private FxAddress _PLCAddr;									// PLC地址。如果是PLC,则使用这个 FxAddress 表达地址
		public ValueStruct Value;									// 值
		#endregion

		#region 属性代码块

		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		public int ControllerId
		{
			get { return _ControllerId; }
			set { _ControllerId = value; }
		}

		public int ControllerChannelNo
		{
			get { return _ControllerChannelNo; }
			set { _ControllerChannelNo = value; }
		}

		public FxAddress PLCAddr
		{
			get { return _PLCAddr; }
			set { _PLCAddr = value; }
		}

		/// <summary>
		/// 返回是否没有输入值？
		/// </summary>
		public bool IsNoInput
		{
			get { return Value.IsNull; }
		}

		#endregion

		public AcquireRawValue (int id, int controllerId, int controllerChannelNo, double value)
		{
			_Id = id;
			_ControllerId = controllerId;
			_ControllerChannelNo = controllerChannelNo;
			_PLCAddr = null;
			Value = new ValueStruct(value);
		}

		public AcquireRawValue (int id, IControllerBase controller, string controllerChannelAlias)
			: this(id, controller, controllerChannelAlias, ValueStruct.NullDefault)
		{
		}

		public AcquireRawValue (int id, IControllerBase controller, string controllerChannelAlias, double value)
		{
			_Id = id;
			_ControllerId = controller.ControllerId;
			_ControllerChannelNo = 0;

			if(controller.ControllerType.IsPLCController())
				_PLCAddr = new FxAddress(controllerChannelAlias, controller.ControllerType);
			else
				_PLCAddr = null;
			Value = new ValueStruct(value);
		}

		public AcquireRawValue (int id, IControllerBase controller, string controllerChannelAlias, ValueStruct value)
		{
			_Id = id;
			_ControllerId = controller.ControllerId;
			_ControllerChannelNo = 0;

			if(controller.ControllerType.IsPLCController())
				_PLCAddr = new FxAddress(controllerChannelAlias, controller.ControllerType);
			else
				_PLCAddr = null;

			Value = value;
		}

		/// <summary>
		/// 返回便于阅读的概要性信息
		/// </summary>
		/// <returns></returns>
		public override string ToString ()
		{
			return string.Format("{0}(Controller:{1}),{2},Value={3}", _Id, _ControllerId,
									_PLCAddr == null ? _ControllerChannelNo.ToString() : _PLCAddr.ToString(),
									Value.ToString());
		}
	}
}
