using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RSG.DMF.Fields.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance;

		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("http://mvdev10/_layouts/MacroViewDMFServiceMOSS.asmx")]
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		public string RSG_DMF_Fields_DMF_MOSS_MessageServiceMOSS
		{
			get
			{
				return (string)this["RSG_DMF_Fields_DMF_MOSS_MessageServiceMOSS"];
			}
		}

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("http://mvdev10/_vti_bin/webs.asmx")]
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		public string RSG_DMF_Fields_MOSS_Webs_Webs
		{
			get
			{
				return (string)this["RSG_DMF_Fields_MOSS_Webs_Webs"];
			}
		}

		static Settings()
		{
			Settings.defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		}

		public Settings()
		{
		}
	}
}