using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace InControls.SerialDevice
{
	/// <summary>
	/// 对 .Net 的 SerialPort 进行简单封装，形成同步收发的专用版本
	/// 如果需要在系统内部使用串口，必须使用这个类（SerialPortSync）
	/// </summary>
	public class SerialPortSync : IDisposable
	{
		private System.IO.Ports.SerialPort _PortObj;					// 用于收发数据的串口
		private int _PortNo;											// 端口号，例如“COM1:”用 1 表示

		private int _BaudRate;
		private int _DataBits;
		private StopBits _StopBites;
		private Parity _Parity;

		private TimeSpan _MaxTimeout;									// 最大超时间隔（读/写），默认1秒

		#region 属性代码块
		public int PortNo
		{
			get { return _PortNo; }
			set { _PortNo = value; }
		}

		public int DataBits
		{
			get { return _DataBits; }
		}

		public int BytesToRead
		{
			get { return (_PortObj == null ? 0 : _PortObj.BytesToRead); }
		}

		public int BytesToWrite
		{
			get { return (_PortObj == null ? 0 : _PortObj.BytesToWrite); }
		}

		public string NewLine
		{
			get { return (_PortObj == null ? String.Empty : _PortObj.NewLine); }
			set { if (_PortObj != null)_PortObj.NewLine = value; }
		}

		public bool IsOpen
		{
			get
			{
				return (_PortObj == null ? false : _PortObj.IsOpen);
			}
		}

		public TimeSpan MaxTimeout
		{
			get { return _MaxTimeout; }
			set { _MaxTimeout = value; }
		}

		#endregion

		public SerialPortSync()
		{
			_PortNo = 0;
			_PortObj = null;
			_BaudRate = 9600;
			_DataBits = 8;
			_StopBites = StopBits.One;
			_Parity = Parity.None;

			_MaxTimeout = TimeSpan.FromSeconds(1);
		}

		public SerialPortSync(System.IO.Ports.SerialPort existedPort)
		{
			_PortNo = Convert.ToInt16(existedPort.PortName.Substring(3));
			_PortObj = existedPort;
			_BaudRate = existedPort.BaudRate;
			_DataBits = existedPort.DataBits;
			_StopBites = existedPort.StopBits;
			_Parity = existedPort.Parity;

			_PortObj.ReadTimeout = 100;
			_MaxTimeout = TimeSpan.FromSeconds(1);
		}

		/// <summary>
		/// 默认采用预设波特率打开 COM1.
		/// </summary>
		public bool OpenPort()
		{
			try {
				OpenPort(_PortNo, 9600);
			} catch (Exception e) {
				Trace.WriteLine(e.ToString());
			}
			return (IsOpen);
		}

		public bool OpenPort(int portNo, int baudRate)
		{
			return OpenPort(portNo, baudRate, 8, _StopBites, _Parity, Handshake.None);
		}

		/// <summary>
		/// 使用指定参数打开串口
		/// </summary>
		/// <param name="portNo"></param>
		/// <param name="openParamString">打开串口的参数，形如“9600,n,8,1”</param>
		/// <returns></returns>
		public bool OpenPort(int portNo, string openParamString)
		{
			SerialParam sp = new SerialParam(openParamString);
			return (OpenPort(portNo, sp.BaudRate, sp.DataBits, sp.StopBits, sp.Parity, Handshake.None));
		}

		public bool OpenPort(int portNo, int baudRate, int dataBits, StopBits stopBites, Parity parity, Handshake handshake)
		{

			_PortNo = portNo;

			if (_PortNo == 0) {
				Debug.Assert(false, "必须首先设定串口号，否则无法打开！");
				return (false);										// 没有设定端口号
			}

			if (_PortObj == null) {
				_PortObj = SerialPortManager.Instance.GetComPortInstance(_PortNo);
			}
			if (_PortObj == null) {
				Debug.Print("串口 COM{0}: 不存在.请检查配置或程序.", portNo);
				return (false);
			} else {

				try {
					if (!_PortObj.IsOpen) {
						_PortObj.BaudRate = baudRate;							// 默认速率、数据位、停止位、校验位等参数
						_PortObj.DataBits = dataBits;
						_PortObj.StopBits = stopBites;
						_PortObj.Parity = parity;
						_PortObj.Handshake = handshake;
						_PortObj.DtrEnable = true;
						_PortObj.ReceivedBytesThreshold = 256;
						//m_PortObj.RtsEnable = false;
						//m_PortObj.DiscardNull = false;
						//m_PortObj.ReadBufferSize = 20480;
						//m_PortObj.WriteBufferSize = 10240;
						//m_PortObj.NewLine = string.Empty;
						_PortObj.ReadTimeout = 10000;
						_PortObj.WriteTimeout = Timeout.Infinite;
						_PortObj.Open();

						_PortObj.ErrorReceived += new SerialErrorReceivedEventHandler(this.OnSerialErrorReceivedEvent);

					} else {
						Debug.Print("尝试打开串口 {0}: 时，发现其早已打开，可能是系统中的多个设备使用了相同的串口号！请仔细检查。如果系统使用共享的串行设备，则不再此限。", _PortObj.PortName);
					}
				} catch (UnauthorizedAccessException e) {						// 串口不存在或已经被其他程序打开
					Debug.Print("串口 {0}: 不存在或已经被其他程序打开", _PortObj.PortName, e.ToString());
					_PortObj.Dispose();
					_PortObj = null;
				} catch (Exception e) {
					Debug.Print("串口 {0}: 打开失败", _PortObj.PortName, e.ToString());
				}
			}
			return (true);
		}

		public void ClosePort()
		{
			if (_PortObj != null) {
				if (_PortObj.IsOpen) {
					_PortObj.Close();
				}
				_PortObj.Dispose();
				_PortObj = null;
			}
		}

		public void Write(string s)
		{
			_PortObj.Write(s);
		}

		public void Write(byte[] buff)
		{
			Write(buff, 0, buff.Length);
		}

		public void Write(byte[] buff, int offset, int count)
		{
			_PortObj.Write(buff, offset, count);
		}

		public string ReadExisting()
		{
			return (_PortObj.ReadExisting());
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			int bytesToRead = _PortObj.BytesToRead;

			try {
				if (bytesToRead > 0) {
					return (_PortObj.Read(buffer, offset, Math.Min(buffer.Length - offset, count)));
				}
			} catch (IOException e) {
				int errno = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				if (errno != 997) {
					Debug.Print("{0}\t同步读发现异常！ COM{1}, Read(buffer size={2},offset={3},count={4}),BytesToRead={5},Win32Error={6} \r\n\t {7}",
								DateTime.Now,
								_PortNo, buffer.Length, offset, count, bytesToRead, errno, e.ToString());
				}

			}
			return (0);
		}

		/// <summary>
		/// 同步读数据。直到因为：缓冲区满、超时
		/// </summary>
		/// <param name="buffer">返回的缓冲区</param>
		/// <param name="offset">存放到缓冲区的起始偏移量</param>
		/// <param name="count">最大字节数</param>
		/// <param name="timeout">超时</param>
		/// <returns></returns>
		public int ReadSync(byte[] buffer, int offset, int count, TimeSpan timeout)
		{
			return ReadSync(buffer, offset, count, timeout, null);
		}

		/// <summary>
		/// 同步读数据。直到因为：缓冲区满、超时、出现任一结束标记
		/// 注意：收到结束标记时，不能假定或确保它恰好处于缓冲区的尾部
		/// </summary>
		/// <param name="buffer">返回的缓冲区</param>
		/// <param name="offset">存放到缓冲区的起始偏移量</param>
		/// <param name="count">最大字节数</param>
		/// <param name="timeout">超时</param>
		/// <param name="endFlags">结束标记组，一旦接收缓冲区中发现任一结束标记,则不再继续读取</param>
		/// <returns></returns>
		public int ReadSync(byte[] buffer, int offset, int count, TimeSpan timeout, List<byte[]> endFlags)
		{
			DateTime start = DateTime.Now;
			int len = offset;

			while (DateTime.Now.Subtract(start).TotalMilliseconds < timeout.TotalMilliseconds) {                  // 连续超时指定次数，则不再等待

				if (_PortObj.BytesToRead == 0) System.Threading.Thread.Sleep(1);
				len = len + _PortObj.Read(buffer, len, count - len);

				// 查询接收到的缓冲区中是否出现特征字
				if (endFlags != null) {
					foreach (byte[] bytsMark in endFlags) {
						for (int i = 0; i < (len - bytsMark.Length); i++) {
							bool found = true;

							for (int k = 0; k < bytsMark.Length; k++) {
								if (buffer[i + k] != bytsMark[k]) {
									found = false;
									break;
								}
							}

							if (found) goto LBL_EXIT;
						}
					}
				}
			}

		LBL_EXIT:
			return (len);
		}

		public void DiscardInBuffer()
		{
			_PortObj.DiscardInBuffer();
		}

		public void DiscardOutBuffer()
		{
			_PortObj.DiscardOutBuffer();
		}

		private void OnSerialErrorReceivedEvent(Object sender, SerialErrorReceivedEventArgs e)
		{
			//Debug.Assert( false, e.ToString( ) );
		}

		#region IDisposable 成员
		public void Dispose()
		{
			if (_PortObj != null) {
				ClosePort();
			}
			_PortObj = null;
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
