using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RSG.DMF.Fields
{
	public class RegistryUtils
	{
		internal const string UserOptionsPath = "Software\\MacroView\\DMF\\UserOptions";

		internal const string UserServersPath = "Software\\MacroView\\DMF\\Servers";

		internal const string UserOptionsPolicyPath = "Software\\Policies\\MacroView\\DMF\\UserOptions";

		internal const string UserServersPolicyPath = "Software\\Policies\\MacroView\\DMF\\Servers";

		internal const string UserPreferencesPath = "Software\\MacroView\\DMF\\Preferences";

		internal const short CSIDL_APPDATA = 26;

		public RegistryUtils()
		{
		}

		public static object GetOPGSetting(string name)
		{
			object policySetting = null;
			RegistryKey registryKey = null;
			try
			{
				try
				{
					policySetting = RegistryUtils.GetPolicySetting(name);
					if (policySetting == null)
					{
						registryKey = Registry.LocalMachine.OpenSubKey("Software\\MacroView\\WISDOM", false);
						if (registryKey != null)
						{
							policySetting = registryKey.GetValue(name);
						}
					}
				}
				catch (Exception exception)
				{
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return policySetting;
		}

		public static object GetPolicySetting(string name)
		{
			object value = null;
			RegistryKey registryKey = null;
			try
			{
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey("Software\\Policies\\MacroView\\DMF\\UserOptions", false);
					if (registryKey != null)
					{
						value = registryKey.GetValue(name);
					}
				}
				catch (Exception exception)
				{
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return value;
		}

		public static object GetSetting(string name, out bool PolicySet)
		{
			object policySetting = null;
			PolicySet = false;
			RegistryKey registryKey = null;
			try
			{
				try
				{
					policySetting = RegistryUtils.GetPolicySetting(name);
					if (policySetting == null)
					{
						PolicySet = false;
						registryKey = Registry.CurrentUser.OpenSubKey("Software\\MacroView\\DMF\\UserOptions", false);
						if (registryKey != null)
						{
							policySetting = registryKey.GetValue(name);
						}
					}
					else
					{
						PolicySet = true;
					}
				}
				catch (Exception exception)
				{
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return policySetting;
		}

		public static object GetSetting(string name)
		{
			object policySetting = null;
			RegistryKey registryKey = null;
			try
			{
				try
				{
					policySetting = RegistryUtils.GetPolicySetting(name);
					if (policySetting == null)
					{
						registryKey = Registry.CurrentUser.OpenSubKey("Software\\MacroView\\DMF\\UserOptions", false);
						if (registryKey != null)
						{
							policySetting = registryKey.GetValue(name);
						}
					}
				}
				catch (Exception exception)
				{
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return policySetting;
		}

		public static string GetStoreFolder()
		{
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder(255);
			num = Convert.ToInt32(RegistryUtils.SHGetFolderPath(IntPtr.Zero, 26, IntPtr.Zero, 0, stringBuilder));
			return Path.Combine(stringBuilder.ToString(), "MacroView DMF\\Store");
		}

		public static bool HasUserOptions()
		{
			bool flag;
			try
			{
				flag = (Registry.CurrentUser.OpenSubKey("Software\\MacroView\\DMF\\UserOptions", false) == null ? false : true);
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		public static void SetSetting(string name, object value)
		{
			RegistryKey registryKey = null;
			try
			{
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey("Software\\MacroView\\DMF\\UserOptions", true);
					if (registryKey == null)
					{
						registryKey = Registry.CurrentUser.CreateSubKey("Software\\MacroView\\DMF\\UserOptions");
					}
					if ((registryKey == null ? false : value != null))
					{
						registryKey.SetValue(name, value);
					}
				}
				catch (Exception exception)
				{
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
		}

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwFlags, [Out] StringBuilder pszPath);
	}
}