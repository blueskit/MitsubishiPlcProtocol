namespace InControls.PLC.Mitsubishi
{
	public static class McTest
	{
		private static IMitsubishiPlc _plc = null;


		public static void Test()
		{
			// _plc = new McProtocolUdp("192.0.1.254", 8195);
			//_plc = new McProtocolTcp("192.0.1.254", 8195);
			_plc = new McProtocolTcp("tunnel.qydev.com", 52155);

			_plc.Open();

			_plc.Excute("D0,32");
			_plc.Excute("M850,32");
			_plc.Excute("D001");
			_plc.Excute("D126,2");
			_plc.Excute("D126..130=0");
			_plc.Excute("D10=135");
		}

	}
}
