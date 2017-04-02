using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 称重阶段枚举
	/// </summary>
	[Flags]
	public enum WeightingStepEnum : uint
	{
		UnDefined = 0x0000,								// 未定义。软件启动后进入此状态
		Waitting = 0x0001,								// 停工/等待来料
		Weighting = 0x0002,                             // 准备称量
		Opening = 0x0004,                               // 料仓门打开
		Opening_Half = 0x0008,                          // 料仓门半开(有对应软单元)
		Opening_Full = 0x0010,                          // 料仓门全开(有对应软单元)
		Closing_Half = 0x0020,                          // 料仓门半关，相当于半开(有对应软单元)
		Closing_Full = 0x0040,                          // 料仓门全关(有对应软单元)
		Closed = 0x0080,                                // 料仓门关闭完成
		WeightingFinished = 0x0100,						// 称重完成，等待放料到搅拌室
		Droping = 0x0200,							    // 放料中
		DropFinished = 0x0400,							// 放料完成（可以开始下一轮的称重）

		Mixing = 0x0800,                                // 拌缸专用工作阶段，正在搅拌阶段
		Mixed = 0x1000,                                 // 拌缸专用工作阶段，搅拌完成阶段

		DelayMask = 0x8000,                             // 延时位掩码，可与其它状态组合
		DelayOpening = DelayMask | Weighting,           // 延时打开，表示某个Bin需要延迟一段时间（2S）再真正打开
	}
}
