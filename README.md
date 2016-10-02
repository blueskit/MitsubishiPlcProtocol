# MitsubishiPlcProtocol
三菱PLC(Mitsubishi)通讯协议的C#实现，支持FX、Q系列的ASCII-3E、BIN-3E、FX串口格式。
感谢 https://github.com/SatohNorio, 这里的MC协议的代码在 https://github.com/SatohNorio/McProtocol 的基础上改进得来。


## 下面是读写Q系列的示例代码（ASCII-3E或BIN-3E格式）
	public static class McTest
	{
		private static IMitsubishiPlc _plc = null;


		public static void Test()
		{
			_plc = new McProtocolUdp("192.0.1.254", 8195);

			_plc.Open();

			_plc.Excute("D0,32");
			_plc.Excute("M850,32");
			_plc.Excute("D001");
			_plc.Excute("D126,2");
			_plc.Excute("D126..130=0");
			_plc.Excute("D10=135");
		}

	}
