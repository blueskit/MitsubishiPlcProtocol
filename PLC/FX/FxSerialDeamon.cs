using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using InControls.Common;
using InControls.SerialDevice;
using Vila.Common;
using Vila.Extensions;

namespace InControls.PLC.FX
{
	/// <summary>
	/// 串口数据收发守护者
	/// 用于封装对多个下位机的最多15个串口的串行化读写操作
	/// 主要用途：
	///     1、重发机制的实现
	///     2、报文整合
	/// </summary>
	public sealed class FxSerialDeamon : ControllerBaseImpl, IControllerAction, IDisposable
	{
		private SerialPortSync _SerialPort;
		private FxRingBuffer _RingBuffer;

		private const int MAX_RETRY_READ_COUNT = 50;

		public FxSerialDeamon()
			: this(ControllerTypeConst.ctPLC_Fx, 0, string.Empty, string.Empty, 0, 128)
		{
		}

		/// <summary>
		/// 构建FxSerialDeamon类实例
		/// </summary>
		/// <param name="controllerType">目前仅支持ctPLC_Fx</param>
		/// <param name="controllerId"></param>
		/// <param name="controllerName">控制器名称----程序内部一般不需要</param>
		/// <param name="controllerAddress">表示为：串口号、IP地址</param>
		/// <param name="controllerAddressPort">控制器地址的端口号（表示为：TCP/UDP的Port、485的设备ID）</param>
		/// <param name="controllerChannelCount">通道数，例如128，256 ... ...</param>
		public FxSerialDeamon(ControllerTypeConst controllerType, int controllerId, string controllerName, string controllerAddress, short controllerAddressPort, short controllerChannelCount)
			: base(controllerType, controllerId, controllerName, controllerAddress, controllerAddressPort, controllerChannelCount)
		{
			Debug.Assert(controllerType == ControllerTypeConst.ctPLC_Fx, "请确认！这里仅仅支持 ControllerTypeConst.ctPLC_Fx .");

			_SerialPort = null;
			_RingBuffer = new FxRingBuffer();
		}

		~FxSerialDeamon()
		{
			Dispose(false);
		}

		/// <summary>
		/// 启动串口通讯
		/// 
		///		PLC 的 D8120 设置为 0x0897，则表示 19200,7,1,E
		///		PLC 的 D8120 设置为 0x0C8E，则表示 9600,7,1,E
		/// </summary>
		/// <returns>如果成功返回 true,否则返回 false</returns>
		public bool Start(int portNo)
		{
			return Start(portNo, "115200,E,7,1");
			//return Start(portNo, "9600,E,7,1");
		}

		public bool Start(int portNo, string serialParamString)
		{
			if (_SerialPort != null) {
				_SerialPort.Dispose();
			}

			_SerialPort = new SerialPortSync();
			_SerialPort.OpenPort(portNo, serialParamString);

			Debug.Print("\n FxSerialDeamon 打开 COM{0}: {1},\t{2}\n", portNo, serialParamString, _SerialPort.IsOpen ? "成功" : "失败");

			return (_SerialPort.IsOpen);
		}

		public bool Start(SerialPortSync serialPortSyncObject)
		{
			if (_SerialPort != null) {
				_SerialPort.Dispose();
			}

			_SerialPort = serialPortSyncObject;

			Debug.Print("\n 打开 COM{0}: {1} \n", _SerialPort.PortNo, _SerialPort.IsOpen ? "成功" : "失败");

			return (_SerialPort.IsOpen);
		}


		public FxCommandResponse Send(short channelNo, string data)
		{
			if (string.IsNullOrEmpty(data))
				return (null);

			byte[] buff = ASCIIEncoding.ASCII.GetBytes(data);

			return (Send(channelNo, buff, buff.Length));
		}

		public FxCommandResponse Send(short channelNo, string data, ICellDataType responseDataType)
		{
			if (string.IsNullOrEmpty(data))
				return (null);

			byte[] buff = ASCIIEncoding.ASCII.GetBytes(data);

			return (Send(channelNo, buff, buff.Length, responseDataType));
		}

		public FxCommandResponse Send(short channelNo, byte[] dataBuff, int dataSize)
		{
			return (Send(channelNo, dataBuff, dataSize, UInt8DataType.Default));
		}

		public FxCommandResponse Send(short channelNo, byte[] dataBuff, int dataSize, ICellDataType responseDataType)
		{
			byte[] resultBuff = null;
			FxCommandResponse result = new FxCommandResponse(ResultCodeConst.rcNotSettting, null, responseDataType);

			if (!_SerialPort.IsOpen)
				return (result);

			if (_SerialPort.BytesToWrite > 0) {
				//MyApp._LogWriter.WriteLine("准备写串口前，发现写缓冲中尚存{0}字节没有发出！", _SerialPort.BytesToWrite);
			}

			int reReadTimes = 0;                                            // 重读次数

			_SerialPort.Write(dataBuff);

			System.Threading.Thread.Sleep(1);

			_RingBuffer.Clear();

			// 读取数据，直到得到完整报文或超时
			reReadTimes = 0;
			while (reReadTimes < MAX_RETRY_READ_COUNT) {                    // 连续超时指定次数，则不再等待
				byte[] data = new byte[256];
				int len = _SerialPort.Read(data, 0, data.Length);

				if (len > 0) {
					reReadTimes = 0;
					_RingBuffer.Append(data, 0, len);

					resultBuff = _RingBuffer.PickPackage();

					if (resultBuff != null) {
						break;
					}

				} else {
					reReadTimes++;
					System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(1));
				}
			}

			// 处理收到的数据
			if (resultBuff == null) {
				result.ResultCode = ResultCodeConst.rcFailt;
				Debug.Print("发送命令超时候仍没有收到FX PLC的合法响应.");
			} else if (resultBuff.Length == 1) {
				result.ResultCode = (ResultCodeConst)resultBuff[0];
			} else {
				result.ResultCode = ResultCodeConst.rcSuccess;

				List<int> valuelist;
				FxCommandHelper.ParseSmart(resultBuff, 0, responseDataType, out valuelist);

				result.SetResponseValue(valuelist);
				result.SetRawData(resultBuff);

			}

			return (result);
		}

		#region IControllerAction 成员

		public override bool Start()
		{
			if (string.IsNullOrEmpty(base.Param))
				return Start(base.ControllerAddress.ToDbInt());
			else
				return Start(base.ControllerAddress.ToDbInt(), base.Param);
		}

		public override bool Stop()
		{
			return base.Stop();
		}

		/// <summary>
		/// 将给定节点的“输出值”写入外部设备或PLC
		/// </summary>
		/// <param name="sourceList">节点列表</param>
		/// <param name="timeout">最大超时值</param>
		/// <returns>返回成功写的点数</returns>
		public override int WritePoints(List<AcquirePoint> outputList, TimeSpan timeout)
		{
			int ct = 0;
			string cmd;
			FxCommandResponse res;

			foreach (AcquirePoint ap in outputList) {
				if (ap.ControllerObject.ControllerId != base.ControllerId)
					continue;
				if (ap.AV.Output.IsNull)
					continue;

				if (ap.AV.Output.B)
					cmd = FxCommandHelper.Make(FxCommandConst.FxCmdForceOn, new FxAddress(ap.ChannelNoAlias, FxAddressLayoutType.AddressLayoutByte));
				else
					cmd = FxCommandHelper.Make(FxCommandConst.FxCmdForceOff, new FxAddress(ap.ChannelNoAlias, FxAddressLayoutType.AddressLayoutByte));

				res = Send(0, cmd);

#if DEBUG
				//Logger.Instance.WriteLine("FxPLC输出: {0} ", res.ToString());
#endif

				ct++;
			}

			return ct;
		}

		/// <summary>
		/// 读取设备所有IO点数据，并返回这些点的数据值
		/// </summary>
		public override List<AcquireRawValue> ReadAllPoints(List<AcquirePoint> sourceAPList, TimeSpan timeout)
		{
			if (_SerialPort == null || !_SerialPort.IsOpen)
				return null;

			List<AcquireRawValue> vlist = new List<AcquireRawValue>();

			if (sourceAPList != null) {
				sourceAPList.ForEach(o => {
					if (o.ControllerObject.ControllerId == this.ControllerId)
						vlist.Add(new AcquireRawValue(o.Id, o.ControllerObject, o.ChannelNoAlias));
				});
			} else {
				//for(int result = 0; result < 127; result++) {
				//    vlist.Add(new AcquireRawValue(0, 0, string.Format("X{0}", result)));
				//    vlist.Add(new AcquireRawValue(0, 0, string.Format("Y{0}", result)));
				//}
			}

			string cmd;
			FxCommandResponse res;

			// 读取 X0..X177,一次性读取所有X
			cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, new FxAddress("X0", ControllerTypeConst.ctPLC_Fx), 16);
			res = Send(0, cmd);
			//Debug.Print("X0--X177 = {0}", res.ToString());

			// 根据通道号别名，例如"X001","Y177","Mxxx"等内容更新
			if (res.ResultCode == ResultCodeConst.rcSuccess) {
				foreach (AcquireRawValue o in vlist) {
					if (o.PLCAddr != null && o.PLCAddr.AddressType == FxAddressType.X) {
						int byteIndex = (int)(o.PLCAddr.TagOffset / 8);
						int byteOff = (int)(o.PLCAddr.TagOffset % 8);

						if (byteIndex >= 0 && byteIndex < res.ResponseValue.Count) {
							o.Value = new ValueStruct(((res.ResponseValue[byteIndex] >> byteOff) & 0x01) == 0x01);

							//Debug.Print("X{0} <== {1}", Convert.ToString(o.PLCAddr.TagOffset, 8), o.Value);
						} else {
							//Debug.Print("X{0} <== {1}", Convert.ToString(o.PLCAddr.TagOffset, 8), o.Value);
						}
					}
				}
			} else {
				Debug.Print("{0}\t{1} 读取失败(X001-X177)", DateTime.Now, this.ToString());
			}

			// 读取 Y0..Y177,一次性读取所有Y
			cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, new FxAddress("Y0", ControllerTypeConst.ctPLC_Fx), 16);
			res = Send(0, cmd);
			//Debug.Print("Y0--Y177 = {0}", res.ToString());
			// 根据通道号别名，例如"Y177"等内容更新
			if (res.ResultCode == ResultCodeConst.rcSuccess) {
				foreach (AcquireRawValue o in vlist) {
					if (o.PLCAddr != null && o.PLCAddr.AddressType == FxAddressType.Y) {
						int byteIndex = (int)(o.PLCAddr.TagOffset / 8);
						int byteOff = (int)(o.PLCAddr.TagOffset % 8);

						if (byteIndex >= 0 && byteIndex < res.ResponseValue.Count) {
							o.Value = new ValueStruct(((res.ResponseValue[byteIndex] >> byteOff) & 0x01) == 0x01);
						} else {
							//Debug.Print("Y{0} <== {1}", Convert.ToString(o.PLCAddr.TagOffset, 8), o.Value);
						}
					}
				}
			} else {
				Debug.Print("{0}\t{1} 读取失败(Y001-Y177)", DateTime.Now, this.ToString());
			}

			// 一次性读取 M0..M499 
			cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, new FxAddress("M0", ControllerTypeConst.ctPLC_Fx), 64);
			res = Send(0, cmd);

			//Debug.Print("M0--M499 = {0}", res.ToString());
			// 根据通道号别名，例如"M19"等内容更新
			if (res.ResultCode == ResultCodeConst.rcSuccess) {
				foreach (AcquireRawValue o in vlist) {
					if (o.PLCAddr != null && o.PLCAddr.AddressType == FxAddressType.M && o.PLCAddr.TagOffset < 1000) {
						int byteIndex = (int)(o.PLCAddr.TagOffset / 8);
						int byteOff = (int)(o.PLCAddr.TagOffset % 8);

						if (byteIndex >= 0 && byteIndex < res.ResponseValue.Count) {
							o.Value = new ValueStruct(((res.ResponseValue[byteIndex] >> byteOff) & 0x01) == 0x01);
						} else {
							//Debug.Print("M{0} <== {1}", Convert.ToString(o.PLCAddr.TagOffset, 8), o.Value);
						}
					}
				}
			} else {
				Debug.Print("{0}\t{1} 读取失败(M001-M499)", DateTime.Now, this.ToString());
			}

			#region 一次性读取 M1000-1999 ---- 目前不读取这个范围的 M 点
			if (false) {
				cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, new FxAddress("M1000", ControllerTypeConst.ctPLC_Fx), 32);
				res = Send(0, cmd);
				//Debug.Print("M1000--M1127 = {0}", res.ToString());
				// 根据通道号别名，例如"M1099"等内容更新
				if (res.ResultCode == ResultCodeConst.rcSuccess) {
					foreach (AcquireRawValue o in vlist) {
						if (o.PLCAddr != null && o.PLCAddr.AddressType == FxAddressType.M && o.PLCAddr.TagOffset >= 1000) {
							int byteIndex = (int)((o.PLCAddr.TagOffset - 1000) / 8);
							int byteOff = (int)((o.PLCAddr.TagOffset - 1000) % 8);

							if (byteIndex >= 0 && byteIndex < res.ResponseValue.Count) {
								o.Value = new ValueStruct(((res.ResponseValue[byteIndex] >> byteOff) & 0x01) == 0x01);
							} else {
								//Debug.Print("M{0} <== {1}", o.PLCAddr.TagOffset, o.Value);
							}
						}
					}
				}
			}
			#endregion

			// 读取其他内容：C/D/T
			foreach (AcquireRawValue o in vlist) {
				if (o.PLCAddr != null) {
					switch (o.PLCAddr.AddressType) {
					case FxAddressType.C:
						cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, o.PLCAddr, 2);
						break;
					case FxAddressType.D:
						cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, o.PLCAddr, 2);
						break;
					case FxAddressType.T:
						cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, o.PLCAddr, 2);
						break;
					case FxAddressType.S:
						cmd = FxCommandHelper.Make(FxCommandConst.FxCmdRead, o.PLCAddr, 2);
						break;
					default:
						continue;
					}

					res = Send(0, cmd);
					if (res.ResultCode == ResultCodeConst.rcSuccess) {
						o.Value = new ValueStruct((int)res.ResponseValue[0]);
						//Debug.Print("{0} <== {1}", o.PLCAddr.ToString(), o.Value);
					} else {
						//Debug.Print("{0} <== {1}. Error", o.PLCAddr.ToString(), o.Value);
					}

				}
			}

			return vlist;
		}

		public override string ToString()
		{
			return string.Format("FxSerialDeamon(COM{0}:,{1}),{2},{3},{4},{5}",
									_SerialPort == null ? base.ControllerAddress.ToDbInt() : _SerialPort.PortNo,
									base.Param,
									base.ControllerId, base.ControllerName, base.ControllerType,
									base.Enabled ? "Enabled" : "Disable");
		}

		#endregion

		private void Dispose(bool disposing)
		{
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
