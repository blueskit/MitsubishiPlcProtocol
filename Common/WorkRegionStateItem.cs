using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControls.Common
{
    public class WorkRegionStateItem
    {
        /// <summary>
        /// 工作区域号
        /// </summary>
        public WorkRegionEnum RegionNo { get; set; }
        /// <summary>
        /// 工作设备
        /// </summary>
        public int DeviceNo { get; set; }
        /// <summary>
        /// 工作阶段
        /// </summary>
        public int StepNo { get; set; }
        /// <summary>
        /// 使用工艺
        /// </summary>
        public int CraftworkId { get; set; }
        /// <summary>
        /// 生产总量
        /// </summary>
        public double QtyTotal { get; set; }
        /// <summary>
        /// 单次搅拌重量
        /// </summary>
        public double QtyOnce { get; set; }
        /// <summary>
        /// 已完成搅拌次数
        /// </summary>
        public int MixedCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        public WorkRegionStateItem()
        {

        }
    }
}
