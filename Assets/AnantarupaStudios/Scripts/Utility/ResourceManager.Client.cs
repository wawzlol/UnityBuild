using UnityEngine;

namespace AnantarupaStudios.Utility
{
	public static partial class ResourceManager
	{
		public static Sprite LoadSprite(string path, string name) => Load<Sprite>(AssetSource.AssetBundle, path, name);
		public static Material LoadMaterial(AssetSource source, string path, string name) => Load<Material>(source, path, name);
	}
}