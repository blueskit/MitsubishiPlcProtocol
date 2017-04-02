using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.PLC.FX
{
	public class FxCommandArgs : IDisposable
	{
		/// <summary>
		/// 命令字
		/// </summary>
		public short Cmd { get; private set; }

		/// <summary>
		/// 待发送(已经构建完成)或接收的完整的数据报文
		/// </summary>
		public byte[] Data { get; private set; }

		/// <summary>
		/// 目标通道
		/// 针对PLC-FX，这是表示PLC的站号。
		/// 没有多个PLC协作时，默认0即可
		/// </summary>
		public short ChannelNo { get; private set; }


		public FxCommandResponse Result { get; set; }


		public FxCommandArgs(short cmd, byte[] data)
			: this(cmd, data, 0)
		{
		}

		public FxCommandArgs(short cmd, byte[] data, short channelNo)
		{
			Cmd = cmd;
			Data = data;
			ChannelNo = channelNo;
		}


		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
