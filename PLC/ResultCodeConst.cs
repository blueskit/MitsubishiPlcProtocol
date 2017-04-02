using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	public enum ResultCodeConst : byte
	{
		rcNotSettting = 0,						// 未设定
		rcSuccess = 0x06,						// 成功
		rcFailt = 0x15,							// 失败
		rcTimeout = 0x55,						// 超时(也是失败)
	}
}
