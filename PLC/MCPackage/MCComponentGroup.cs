using System.Collections.Generic;
using System.Linq;
using System.Text;
using InControls.PLC.FX;
using InControls.Common;

namespace InControls.PLC.MCPackage
{
	/// <summary>
	/// 元件地址及其值的描述
	/// </summary>
	public class MCComponent
	{
		public FxAddress Address { get; set; }
		public object Value { get; set; }

		/// <summary>
		/// 初始化一个地址
		/// </summary>
		public MCComponent(string addressName, object value)
		{
			this.Address = new FxAddress(addressName, ControllerTypeConst.ctPLC_QnMC);
			this.Value = value;
		}

		/// <summary>
		/// 初始化一个地址
		/// </summary>
		public MCComponent(string addressName)
			: this(addressName, (short)0)
		{
		}

		/// <summary>
		/// 初始化来自某个PLC控制器的IO点
		/// </summary>
		public MCComponent(IControllerBase controller, string tagName)
			: this(controller.ControllerType, tagName)
		{
		}

		/// <summary>
		/// 初始化来自某个PLC控制器类型的IO点（一个点）
		/// </summary>
		public MCComponent(ControllerTypeConst controllerType, string tagName)
		{
			this.Address = new FxAddress(tagName, controllerType);
			this.Value = (short)0;
		}

		public override string ToString()
		{
			return string.Format("Value={0},Value={1}", Address, Value);
		}
	}


	/// <summary>
	/// 元件地址、元件地址范围的描述
	/// </summary>
	public class MCComponentGroup
	{
		public List<MCComponent> Components { get; set; }

		public MCComponentGroup()
		{
			this.Components = new List<MCComponent>();
		}

		/// <summary>
		/// 初始化一个地址
		/// </summary>
		public MCComponentGroup(string addressName, object value)
		{
			Components = new List<MCComponent>() {
				new MCComponent (addressName,value )
			};
		}

		/// <summary>
		/// 初始化来自某个PLC控制器的IO点
		/// </summary>
		public MCComponentGroup(IControllerBase controller, string tagName)
			: this(controller.ControllerType, tagName)
		{
		}

		/// <summary>
		/// 初始化来自某个PLC控制器类型的IO点（一个点）
		/// </summary>
		public MCComponentGroup(ControllerTypeConst controllerType, string tagName)
		{
			Components = new List<MCComponent>() {
				new MCComponent (tagName,controllerType )
			};
		}

		public override string ToString()
		{
			return string.Format("Addr={0},Count={1}", Components, Components.Count);
		}
	}



	public static class MCComponentGroupExtensions
	{
		/// <summary>
		/// 判断并返回给定元件是否“位元件”、或“点元件”
		/// </summary>
		public static bool IsBitComponent(this MCComponentGroup c)
		{
			if (c.Components.Count == 0)
				return false;
			else {
				var t = c.Components.First().Address.AddressType;
				return t == FxAddressType.X || t == FxAddressType.Y || t == FxAddressType.M;
			}
		}

		/// <summary>
		/// 判断并返回给定元件是否“字元件”
		/// </summary>
		public static bool IsWordComponent(this MCComponentGroup c)
		{
			if (c.Components.Count == 0)
				return false;
			else {
				var t = c.Components.First().Address.AddressType;
				return t == FxAddressType.D || t == FxAddressType.C;
			}
		}

		public static int GetWordComponentCount(this List<MCComponentGroup> list)
		{
			int ct = 0;
			list.ForEach(c => {
				if (c.IsWordComponent()) {
					ct++;
				}
			});

			return ct;
		}

		public static int GetBitComponentCount(this List<MCComponentGroup> list)
		{
			int ct = 0;
			list.ForEach(c => {
				if (c.IsBitComponent()) {
					ct++;
				}
			});

			return ct;
		}

		public static string ToString(this List<MCComponentGroup> list)
		{
			StringBuilder sb = new StringBuilder(list.Count * 32);
			list.ForEach(c => {
				if (sb.Length > 0)
					sb.Append(",");
				sb.AppendFormat(" [{0}]", c.ToString());
			});

			return sb.ToString();
		}
	}

}
