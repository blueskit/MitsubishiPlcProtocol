using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.PLC.FX
{
	public unsafe class FxRingBuffer
	{
		/// <summary>
		/// 实际有效数据长度
		/// </summary>
		private int _DataSize;

		public int DataSize
		{
			get { return _DataSize; }
			private set { _DataSize = value; }
		}

		public byte[] Buffer { get; private set; }

		public FxRingBuffer()
			: this(1024)
		{
		}

		public FxRingBuffer(int capacity)
		{
			Buffer = new byte[capacity];
			_DataSize = 0;
		}

		public void Clear()
		{
			_DataSize = 0;
		}

		public void Append(byte[] sourceData)
		{
			Append(sourceData, 0, sourceData.Length);
		}

		public void Append(byte[] sourceData, int index, int length)
		{
			if ((_DataSize + length) <= Buffer.Length) {
				Array.Copy(sourceData, index, Buffer, _DataSize, length);
				_DataSize += length;
			} else if (length <= Buffer.Length) {						// 源数据太多，缓冲区的剩余空间不足：直接覆盖以前数据算啦
				Array.Copy(sourceData, index, Buffer, 0, length);
				_DataSize = length;
			} else {													// 缓冲区太小，连源数据都无法容纳：此时出错
				System.Diagnostics.Debug.Assert(false, "缓冲区太小，连源数据都无法容纳：此时出错");
				System.Diagnostics.Debugger.Break();
			}
		}

		/// <summary>
		/// 是否存在一个完整报文
		/// </summary>
		public bool IsExistWholePackage()	// 
		{
			return (PickPackage() != null);
		}

		/// <summary>
		/// 是否存在一个完整报文
		/// </summary>
		public bool IsExistWholePackage(int cellCount)
		{
			return (false);
		}

		public byte[] PickPackage()									// 如果存在一个完整报文，则返回这个报文
		{
			int posSTX = -1;
			int posETX = -1;

			// 首先如果PLC直接返回 失败/成功，也需要立刻返回
			for (int i = 0; i < _DataSize; i++) {
				if (Buffer[i] == FxControlCode._ACK) {
					posSTX = i;
					break;
				} else if (Buffer[i] == FxControlCode._NAK) {
					posSTX = i;
					break;
				}
			}

			if (posSTX >= 0) {
				return new byte[1] { Buffer[posSTX] };
			}

			// 随后，寻找常规报文头部
			for (int i = 0; i < _DataSize; i++) {
				if (Buffer[i] == FxControlCode._STX) {
					posSTX = i;
					break;
				}
			}

			if (posSTX < 0) return (null);							// 没有头部，直接返回 null

			// 寻找报文结束标记
			for (int i = posSTX; i < _DataSize; i++) {
				if (Buffer[i] == FxControlCode._ETX) {
					posETX = i;
					break;
				}
			}

			if (posETX > posSTX && _DataSize > posETX) {			// 此时发现一个完整的报文
				byte[] resultBuff = new byte[posETX - posSTX + 2];

				Array.Copy(Buffer, posSTX, resultBuff, 0, resultBuff.Length);

				return (resultBuff);
			}

			return (null);
		}


	}
}
