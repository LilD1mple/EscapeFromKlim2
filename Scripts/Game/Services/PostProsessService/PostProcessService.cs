using EFK2.Game.PostProcess.Interfaces;
using EFK2.Game.Save;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace EFK2.Game.PostProcess
{
	public sealed class PostProcessService : IPostProcessService
	{
		private readonly bool _defaultEnable = true;

		private readonly VolumeProfile _volumeProfile;

		private const string _postProcessConst = "Post-Process";

		public PostProcessService(VolumeProfile volumeProfile)
		{
			_volumeProfile = volumeProfile;
		}

		bool IPostProcessService.IsEnable => SaveUtility.LoadData(_postProcessConst, _defaultEnable);

		void IPostProcessService.Reset()
		{
			SaveUtility.DeleteKey(_postProcessConst);

			SetProfileEnable(_defaultEnable);
		}

		void IPostProcessService.SetPostProcessEnable(bool enable)
		{
			SetProfileEnable(enable);

			SaveUtility.SaveData(_postProcessConst, enable);
		}

		private void SetProfileEnable(bool enable)
		{
			List<VolumeComponent> volumeComponents = _volumeProfile.components;

			for (int i = 0; i < volumeComponents.Count; i++)
			{
				volumeComponents[i].active = enable;
			}

			SaveUtility.SaveData(_postProcessConst, enable);
		}
	}
}
