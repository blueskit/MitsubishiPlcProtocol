using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 每个控制器，至少应该实现下述接口中的方法
	/// </summary>
	public interface IControllerAction
	{
		bool Start ();
		bool Stop ();

		/// <summary>
		/// 功能：
		///		读取所有采集点信息，并返回
		///		返回的结果，将被用于更新sourceAPList目录中的相应单元的值
		/// 说明：
		///		如果是“主动控制器”，就可能是不是直接读取设备数据，而是从缓存中获取最新值
		/// </summary>
		/// <param name="sourceAPList">软单元目录</param>
		/// <param name="timeout">最大超时值</param>
		/// <returns>供外部调用者能够查看到读取到的原始数据。大多数用于调试</returns>
		List<AcquireRawValue> ReadAllPoints (List<AcquirePoint> sourceAPList, TimeSpan timeout);

		/// <summary>
		/// 功能：
		///		直接从设备读取所有采集点信息，并返回
		/// 说明：
		///		1、总是从外部设备读取数据
		///		2、一般情况下，仅有“慢速设备”才通过这个函数获取数据
		///		3、如果是“主动控制器”，其控制器内部一般有独立专有的读取线程用于读取数据，并存入内部Cache
		/// </summary>
		/// <param name="sourceAPList">软单元目录</param>
		/// <param name="timeout">最大超时值</param>
		List<AcquireRawValue> ReadAllPointsDirect (List<AcquirePoint> sourceAPList, TimeSpan timeout);

		/// <summary>
		/// 将给定节点的“输出值”写入外部设备或PLC
		/// </summary>
		/// <param name="outputList">待输出的节点及其输出值列表</param>
		/// <param name="timeout">最大超时值</param>
		/// <returns>返回成功写的点数</returns>
		int WritePoints (List<AcquirePoint> outputList, TimeSpan timeout);

	}
}
