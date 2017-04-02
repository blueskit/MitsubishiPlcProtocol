using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 所有的控制器均需要从这个类中派生
	/// 主要通过这里的属性来达成全局管理的目的
	/// </summary>
	public interface IControllerBase : IControllerAction
	{
		int ControllerId { get; set; }                              // 控制器全局唯一性ID
		string ControllerName { get; set; }                         // 控制器名称----程序内部一般不需要
		int ControllerChannelCount { get; }                         // 控制器的通道数，单通道默认为1
		string ControllerAddress { get; set; }                      // 控制器地址号（表示为：串口号、IP地址）
		short ControllerAddressPort { get; set; }                   // 控制器地址的端口号（表示为：TCP/UDP的Port、485的设备ID）
		ControllerTypeConst ControllerType { get; }                 // 控制器类型：这决定了其功能及其实现的类
		string Param { get; set; }                                  // 打开控制器的参数。例如“9600,n,8,1”
		bool Enabled { get; set; }                                  // 是否启用本控制器？

		AcquirePointConversion APConversion { get; set; }           // 转换器(绝大多数采集器支持1种转换，如果大于1种，则需要按采集点类型选择)
	}
}
