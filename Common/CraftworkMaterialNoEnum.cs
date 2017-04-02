using System;
using System.Collections.Generic;
using System.Text;

namespace InControls.Common
{
	/// <summary>
	/// 配方材料编号
	/// 冷料/热料均独立从1开始编号，最大各支持到10
	/// </summary>
	public enum CraftworkMaterialNoEnum : int
	{
		HM01 = 1,					// 热料
		HM02 = 2,
		HM03 = 3,
		HM04 = 4,
		HM05 = 5,
		HM06 = 6,
		HM07 = 7,
		HM08 = 8,
		HM09 = 9,
		HM10 = 10,

		CM01 = 1,					// 冷料
		CM02 = 2,
		CM03 = 3,
		CM04 = 4,
		CM05 = 5,
		CM06 = 6,
		CM07 = 7,
		CM08 = 8,
		CM09 = 9,
		CM10 = 10,
	}
}
