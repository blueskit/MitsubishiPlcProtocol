using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace InControls.SerialDevice
{

	/// <summary>
	/// 串行口管理类
	/// 
	/// 管理全局性端口的类,用于串口的打开、关闭、IO(输入/输出)的管理
	/// 每个串口设备一旦打开，则保持全局唯一，以保持共享特性
	/// </summary>
	public sealed class SerialPortManager
	{
		private static SerialPortManager _Instance;

		public static SerialPortManager Instance
		{
			get
			{
				if (_Instance == null) {
					lock (typeof(SerialPortManager)) {
						if (_Instance == null) {
							_Instance = new SerialPortManager();
						}
					}
				}
				return SerialPortManager._Instance;
			}
		}

		private List<System.IO.Ports.SerialPort> _PortList;
		private int _PortCount = 0;

		private SerialPortManager()
		{
			string[] names;
			names = System.IO.Ports.SerialPort.GetPortNames();

			_PortCount = 2;				// 默认至少2个
			foreach (string s in names) {
				int ct = Convert.ToInt16(s.Substring(3));
				if (ct > _PortCount) _PortCount = ct;
			}

			_PortList = new List<SerialPort>(Math.Max(8, _PortCount + 1));			// 暂时支持最少 8 个端口
			for (int i = 0; i < _PortList.Capacity; i++) {
				_PortList.Add(null);
			}
		}

		public int ComPortCount()
		{
			return (_PortCount);
		}

		/// <summary>
		/// 得到指定序号的端口的名称
		/// </summary>
		/// <param name="PortNo">串口号，从1..Max</param>
		/// <returns>端口名称</returns>
		public string GetPortName(int portNo)
		{
			string[] names;
			names = System.IO.Ports.SerialPort.GetPortNames();

			foreach (string s in names) {
				if (int.Parse(s.Substring(3, 1)) == portNo) {
					return (s);
				}
			}
			return (null);
		}

		/// <summary>
		/// 根据串口名称，得到其对应的序号
		/// </summary>
		/// <param name="PortNo">端口名称，例如“COM1”、“COM2”</param>
		/// <returns>串口序号，例如“1”、“2”，如名称不存在返回0</returns>
		public int GetPortNo(string portName)
		{
			string[] names;
			names = System.IO.Ports.SerialPort.GetPortNames();

			foreach (string s in names) {
				if (s == portName) {
					return (int.Parse(s.Substring(3, 1)));
				}
			}
			return (0);
		}

		/// <summary>
		/// 得到指定端口号码的实例
		/// 该实例可用于后续的操作。可能需要打开关联的端口
		/// </summary>
		/// <param name="PortNo">串行端口号，范围 1..Max </param>
		/// <returns></returns>
		public System.IO.Ports.SerialPort GetComPortInstance(int portNo)
		{
			return (GetComPortInstance("COM" + portNo.ToString()));
		}

		/// <summary>
		/// 得到指定端口名称的实例
		/// 该实例可用于后续的IO操作。可能需要打开关联的端口
		/// </summary>
		/// <param name="PortNo">串行端口名，例如 “COM1” ，范围 1..Max </param>
		/// <returns></returns>
		public System.IO.Ports.SerialPort GetComPortInstance(string portName)
		{
			int nPortNo;

			nPortNo = GetPortNo(portName);
			if (nPortNo <= 0) return (null);

			System.IO.Ports.SerialPort sp = _PortList[nPortNo];

			if (sp == null) {							// 一般不可能出现找不到设备的情况
				sp = new SerialPort(portName);
				sp.BaudRate = 9600;						// 默认速率、数据位、停止位、校验位等参数
				sp.DataBits = 8;
				sp.StopBits = StopBits.One;
				sp.Parity = Parity.None;
				sp.Handshake = Handshake.RequestToSendXOnXOff;
				_PortList[nPortNo] = sp;
				return (sp);
			}
			return (sp);
		}


	}
}
