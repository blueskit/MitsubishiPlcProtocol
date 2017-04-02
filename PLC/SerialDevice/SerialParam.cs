using System;
using System.IO.Ports;
using Vila.Extensions;


namespace InControls.SerialDevice
{
	public class SerialParam
	{
		public int BaudRate { get; set; }
		public int DataBits { get; set; }
		public StopBits StopBits { get; set; }
		public Parity Parity { get; set; }


		/// <summary>
		/// 初始化默认的串口参数
		/// </summary>
		public SerialParam()
		{
			SetDefaultParam();
		}

		public SerialParam(string openParamString)
		{
			ParseSerialParam(openParamString);
		}

		public void ParseSerialParam(string openParamString)
		{
			SetDefaultParam();

			Func<string, Parity> func = (s) =>
			{
				switch (s) {
				case "N":
				case "n":
					return Parity.None;
				case "E":
				case "e":
					return Parity.Even;
				case "O":
				case "o":
					return Parity.Odd;
				case "M":
				case "m":
					return Parity.Mark;
				}
				return Parity.None;
			};

			string[] ss = openParamString.Split(',');
			if (ss.Length >= 4) {
				this.BaudRate = ss[0].ToDbInt();
				this.Parity = func(ss[1]);
				this.DataBits = ss[2].ToDbInt();

				float stops = ss[3].ToDbFloat();
				if (stops == 0F)
					this.StopBits = StopBits.None;
				else if (stops == 1F)
					this.StopBits = StopBits.One;
				else if (stops == 1.5F)
					this.StopBits = StopBits.OnePointFive;
				else if (stops == 2F)
					this.StopBits = StopBits.Two;
				else
					this.StopBits = StopBits.None;
			}
		}

		public void SetDefaultParam()
		{
			this.BaudRate = 9600;
			this.DataBits = 8;
			this.StopBits = StopBits.One;
			this.Parity = Parity.None;
		}


		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}",
					this.BaudRate,
					this.Parity,
					this.DataBits,
					this.StopBits);

		}
	}
}
