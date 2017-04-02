using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Reflection;

using Vila.Extensions;

namespace InControls.Common
{

	public class ConfigItemCatalogo
	{
		private static ConfigItemCatalogo _Instance = new ConfigItemCatalogo();

		public static ConfigItemCatalogo Instance
		{
			get { return ConfigItemCatalogo._Instance; }
		}

		private Dictionary<ConfigItemEnum, ConfigItem> _list;


		public ConfigItem this[ConfigItemEnum key]
		{
			get
			{
				if (_list.ContainsKey(key))
					return _list[key];
				else
					return _list[ConfigItemEnum.UnDefined];

			}
		}

		private ConfigItemCatalogo()
		{
			_list = new Dictionary<ConfigItemEnum, ConfigItem>(251);
		}

		/// <summary>
		/// 从指定的DataTable中读取数据并构建内部表
		/// tblConfig
		/// </summary>
		/// <param name="dtSource">tblConfig</param>
		public void LoadFrom(DataTable dtSource)
		{
			_list.Clear();

			// 总是有一个 “未定义”项
			ConfigItem item = new ConfigItem(ConfigItemEnum.UnDefined, "ConfigItemNoDefined", 0, "未定义项");
			_list.Add(ConfigItemEnum.UnDefined, item);

			// 主界面刷新周期间隔
			item = new ConfigItem(ConfigItemEnum.UI_ScanInterval, "UI_ScanInterval", 100, "UI_ScanInterval");
			_list.Add(item.Key, item);


			// 从数据库中取值
			for (int i = 0; i < dtSource.Rows.Count; i++) {
				DataRow r = dtSource.Rows[i];

				string keyName = r["Key"].ToDbString().Replace('.', '_');

				ConfigItemEnum key = ParseFrom(keyName);
				if (key != ConfigItemEnum.UnDefined) {
					item = new ConfigItem(key, keyName);

					item.KeyName = keyName;
					item.Comment = r["Comment"].ToDbString();
					item.Value = r["Value"].ToDbDouble();

					_list.Add(key, item);
				} else {
					System.Diagnostics.Debug.Print("配置表中的项目 {0} 没有对应的枚举值存在！请检查。", keyName);
				}
			}
		}


		private static ConfigItemEnum ParseFrom(string enumName)
		{
			ConfigItemEnum eResult;

			if (Enum.TryParse<ConfigItemEnum>(enumName, true, out eResult)) {
				return eResult;
			} else {
				return ConfigItemEnum.UnDefined;
			}
		}

	}
}
