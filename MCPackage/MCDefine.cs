namespace InControls.IO.PLC.MCPackage
{
	/// <summary>
	/// 数据命令
	/// </summary>
	public enum McCommandConst
	{
		UnDefine = 0,
		Read = 0x0403,                          // <随机>读
		BatchRead = 0x0401,                     // 成批读
		Write = 0x1402,                         // <随机>写
		BatchWrite = 0x1401,                    // 成批写

		MultiBlockBatchRead = 0x0406,           // 多块成批读（本系统多块成批读均仅仅支持字单元模式）
		MultiBlockBatchWrite = 0x1406,          // 多块成批写（本系统多块成批写同时支持字单元、位单元模式）
	}

	/// <summary>
	/// 异常结束时的错误代码
	/// </summary>
	public enum McCommandResponseConst
	{
		UNKNOWN = -1,
		Success = 0,
		Error = 0xC051,
	}

}
