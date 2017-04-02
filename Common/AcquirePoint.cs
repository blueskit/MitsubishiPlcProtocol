using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 信息采集点定义
	/// </summary>
	public sealed class AcquirePoint
	{
		/// <summary>
		/// <软单元/采集点>全局唯一ID.
		/// </summary>
		public int Id { get; private set; }										// <软单元/采集点>全局唯一ID.

		/// <summary>
		/// 单元的唯一性名称（内部按名称查询时用），可以空白
		/// 一般针对PLC的 X/Y/M/T/C 等的摘要式名称
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 单元的解释性名称
		/// </summary>
		public string Title { get; private set; }

		public IControllerBase ControllerObject { get; set; }					// 通过哪个控制器通讯？这里是控制器对象的引用
		public short ChannelNo { get; set; }									// 如果控制器为多通道设备，则ChannelNo需要用到
		public string ChannelNoAlias { get; set; }								// 如果控制器是PLC,则这里是通道地址，例如“X1”“Y20”“C201”之类

		public AcquirePointUnitTypeConst UnitType { get; private set; }			// 单元类型：DI/DO/AI/AO/...
		public AcquirePointDataTypeConst DataType { get; private set; }			// 数据类型：Integer/Float/DateTime/...

		public AcquireValue AV;													// 值信息
		public AcquirePointConversion APConversion { get { return ControllerObject.APConversion; } }		// 采集值转换模式

		public AcquirePoint(int id)
			: this(id, null, string.Empty, null, 1, AcquirePointUnitTypeConst.aputNothing)
		{
		}

		public AcquirePoint(int id, string name)
			: this(id, name, string.Empty, null, 1, AcquirePointUnitTypeConst.aputNothing)
		{
		}

		public AcquirePoint(int id, string name, string title, IControllerBase controllerBase)
			: this(id, name, title, controllerBase, 1, AcquirePointUnitTypeConst.aputNothing)
		{
		}

		public AcquirePoint(int id, string name, string title, IControllerBase controllerBase, short channelNo, AcquirePointUnitTypeConst unitType)
			: this(id, name, title, controllerBase, channelNo, string.Empty, unitType, AcquirePointDataTypeConst.apdtInvalid)
		{
		}

		public AcquirePoint(int id, string name, string title, IControllerBase controllerBase, short channelNo, string channelNoAlias, AcquirePointUnitTypeConst unitType, AcquirePointDataTypeConst dataType)
		{
			Id = id;
			Name = name;
			Title = title;
			ControllerObject = controllerBase;
			ChannelNo = channelNo;
			ChannelNoAlias = channelNoAlias;
			UnitType = unitType;
			DataType = dataType;

			AV = new AcquireValue(id);
		}

		public override string ToString()
		{
			return string.Format("Id={0},<{1}>,{2} Channel={3}{4},AV::{5}", Id, Name, Title, ChannelNo, ChannelNoAlias, AV.ToString());
		}

	}
}
