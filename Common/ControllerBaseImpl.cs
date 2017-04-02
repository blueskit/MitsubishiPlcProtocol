using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InControls.Common
{
	public class ControllerBaseImpl : IControllerAction, IControllerBase
	{
		private int _ControllerId;                              // 控制器唯一ID
		private string _ControllerName;                         // 控制器名称----程序内部一般不需要
		private ControllerTypeConst _ControllerType;
		private string _ControllerAddress;                          // 控制器地址号（表示为：串口号、IP地址）
		private short _ControllerAddressPort;                   // 控制器地址的端口号（表示为：TCP/UDP的Port、485的设备ID）
		private short _ControllerChannelCount;                  // 控制器通道数。默认1

		private string _Param;                                  // 打开控制器的参数,一般空白表示取默认值。例如“9600,n,8,1”
		private bool _Enabled;                                  // 是否启用本控制器？

		private AcquirePointConversion _APConversion;           // 转换参数

		#region IControllerBase 成员

		public ControllerTypeConst ControllerType
		{
			get
			{
				return _ControllerType;
			}
		}

		public int ControllerId
		{
			get
			{
				return _ControllerId;
			}
			set
			{
				_ControllerId = value;
			}
		}

		public string ControllerName
		{
			get { return _ControllerName; }
			set { _ControllerName = value; }
		}

		public string ControllerAddress
		{
			get
			{
				return _ControllerAddress;
			}
			set
			{
				_ControllerAddress = value;
			}
		}

		public short ControllerAddressPort
		{
			get { return _ControllerAddressPort; }
			set { _ControllerAddressPort = value; }
		}

		public int ControllerChannelCount
		{
			get { return _ControllerChannelCount; }
		}

		public string Param
		{
			get { return _Param; }
			set { _Param = value; }
		}

		public bool Enabled
		{
			get { return _Enabled; }
			set { _Enabled = value; }
		}

		public AcquirePointConversion APConversion
		{
			get { return _APConversion; }
			set { _APConversion = value; }      // 设置新的转换类
		}

		#endregion

		public ControllerBaseImpl(ControllerTypeConst controllerType)
			: this(controllerType, 0, string.Empty, string.Empty, 0, 1)
		{
		}

		public ControllerBaseImpl(ControllerTypeConst controllerType, int controllerId, string controllerName)
			: this(controllerType, controllerId, controllerName, string.Empty, 0, 1)
		{
		}

		public ControllerBaseImpl(ControllerTypeConst controllerType, int controllerId, string controllerName, string controllerAddress, short controllerAddressPort, short controllerChannelCount)
		{
			_ControllerType = controllerType;
			_ControllerId = controllerId;
			_ControllerName = controllerName;
			_ControllerAddress = controllerAddress;
			_ControllerAddressPort = controllerAddressPort;
			_ControllerChannelCount = controllerChannelCount;
			_APConversion = AcquirePointConversion.GetDefaultNullConversion();
		}

		public ControllerBaseImpl(ControllerTypeConst controllerType, int controllerId, string controllerName, string controllerAddress, short controllerAddressPort, short controllerChannelCount, string param, bool enabled)
		{
			_ControllerType = controllerType;
			_ControllerId = controllerId;
			_ControllerName = controllerName;
			_ControllerAddress = controllerAddress;
			_ControllerAddressPort = controllerAddressPort;
			_ControllerChannelCount = controllerChannelCount;
			_Param = param;
			_Enabled = enabled;
			_APConversion = AcquirePointConversion.GetDefaultNullConversion();
		}

		#region IControllerAction 成员

		public virtual bool Start()
		{
			throw new NotImplementedException();
		}

		public virtual bool Stop()
		{
			throw new NotImplementedException();
		}

		public virtual List<AcquireRawValue> ReadAllPoints(List<AcquirePoint> sourceAPList, TimeSpan timeout)
		{
			Debug.Print("控制器 #{0}/{1},(类型 {2}, 地址 {3},通道数 {4} ) 不支持或没有启动。所以 ReadAllPoints() 无法执行！ ", _ControllerId, _ControllerName, _ControllerType, _ControllerAddress, _ControllerChannelCount);
			return null;
		}

		public virtual List<AcquireRawValue> ReadAllPointsDirect(List<AcquirePoint> sourceAPList, TimeSpan timeout)
		{
			Debug.Print("控制器 #{0}/{1},(类型 {2}, 地址 {3},通道数 {4} ) 不支持或没有启动。所以 ReadAllPointsDirect() 无法执行！ ", _ControllerId, _ControllerName, _ControllerType, _ControllerAddress, _ControllerChannelCount);
			return null;
		}

		public virtual int WritePoints(List<AcquirePoint> sourceList, TimeSpan timeout)
		{
			throw new NotImplementedException();
		}

		#endregion

		public override string ToString()
		{
			return string.Format("Controller={0},{1},(COM{2}: {3}), {4}", _ControllerId, _ControllerName,
				_ControllerAddress, string.IsNullOrEmpty(_Param) ? "UDP" : _Param,
				_Enabled ? "Enabled" : "Disabled");
		}

	}
}
