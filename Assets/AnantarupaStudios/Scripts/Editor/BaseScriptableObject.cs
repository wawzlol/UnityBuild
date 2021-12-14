using SimpleJSON;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnantarupStudios.Editor
{
	public abstract class BaseScriptableObject : ScriptableObject, IScriptableObject
	{
		protected abstract string assetBundlePath { get; }
		protected bool IsValueChanged = false;

		public virtual void ValueChanged()
		{
			IsValueChanged = true;
		}

		public virtual void ValueSet()
		{
			IsValueChanged = false;
		}

		[HorizontalGroup("General/Split", 0.3f, MaxWidth = 180)]
		[HorizontalGroup("General/Split/Right")]
		[Button(ButtonSizes.Large), GUIColor(1, 0.2f, 1)]
		[LabelText("Revert")]
		[EnableIf("IsValueChanged", true)] // ShowIf will reset focus
		public abstract void Load();

		[HorizontalGroup("General/Split/Right")]
		[Button(ButtonSizes.Large), GUIColor(0, 1f, 0)]
		[EnableIf("IsValueChanged", true)]
		public abstract void Save();

		public void SaveFile(string path, string filename, string json)
		{
			string completePath = Path.Combine(path, filename + ".json");
			
			if (File.Exists(completePath))
			{
				File.WriteAllText(completePath, json.ToString());
			}
			else
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
					AssetDatabase.Refresh();
				}

				path = EditorUtility.SaveFilePanel("Save object as", path, filename, "json");

				if (!string.IsNullOrEmpty(path) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), path, out path))
				{
					using (StreamWriter outfile = new StreamWriter(path))
					{
						outfile.Write(json);
					}
				}
			}
		}
		
		protected void ChangeListCount<T>(int Count, ref List<T> paramList)
		{
			int temp = Count - paramList.Count;
			if (temp != 0)
			{
				if (temp < 0)
				{
					paramList.RemoveRange(Count, Mathf.Abs(temp));
				}
				else
				{
					while (paramList.Count < Count)
					{
						paramList.Add(default);
					}
				}
			}
		}

		#region Load Utility
		protected void GetString(JSONNode data, string status, out string result)
		{
			result = data[status].Value;
		}

		protected void GetInt(JSONNode data, string status, out int result)
		{
			result = data[status].AsInt;
		}

		protected void GetDouble(JSONNode data, string status, out double result)
		{
			result = data[status].AsDouble;
		}

		protected void GetBool(JSONNode data, string status, out bool result)
		{
			result = data[status].AsBool;
		}

		protected void GetVector2(JSONNode data, string status, out Vector2Double result)
		{
			result = new Vector2Double(data[status]);
		}

		protected void GetEnum<T>(JSONNode data, string status, out T result)
		{
			result = (T)Enum.ToObject(typeof(T), data[status].AsInt);
		}

		protected void GetStringEnum<T>(JSONNode data, string status, out T result) where T : struct
		{
			if (data.HasKey(status))
			{
				if (!Enum.TryParse(data[status].Value, out result))
				{
					Debug.LogError($"\"{data[status].Value}\" is not a part of \"{status}\" Enum");
				}
			}
			else
			{
				Debug.LogError($"Key \"{status}\" for Enum not found");
				result = default;
			}
		}

		#region Array
		protected void GetIntArray(JSONNode data, string status, out List<int> result)
		{
			if (data[status].IsArray)
			{
				JSONArray arr = data[status].AsArray;
				result = new List<int>();
				for (int i = 0; i < arr.Count; i++)
				{
					result.Add(arr[i].AsInt);
				}
			}
			else
			{
				result = new List<int>();
				result.Add(data[status].AsInt);
			}
		}

		protected void GetStringArray(JSONNode data, string status, out List<string> result)
		{
			if (data[status].IsArray)
			{
				JSONArray arr = data[status].AsArray;
				result = new List<string>();
				for (int i = 0; i < arr.Count; i++)
				{
					result.Add(arr[i].Value);
				}
			}
			else
			{
				result = new List<string>();
				result.Add(data[status].Value);
			}
		}

		protected void GetEnumArray<T>(JSONNode data, string status, out List<T> result)
		{
			List<int> temp;
			GetIntArray(data, status, out temp);
			result = temp.Cast<T>().ToList();
		}

		protected void GetEnumStringArray<T>(JSONNode data, string status, out List<T> result) where T : struct
		{
			if (data[status].IsArray)
			{
				JSONArray arr = data[status].AsArray;
				result = new List<T>();
				for (int i = 0; i < arr.Count; i++)
				{
					T temp;
					if (!Enum.TryParse(data[status][i].Value, out temp))
					{
						Debug.LogError($"\"{data[status][i].Value}\" is not a part of \"{status}\" Enum");
					}
					else
					{
						result.Add(temp);
					}
				}
			}
			else
			{
				result = new List<T>();
				T temp;
				if (!Enum.TryParse(data[status].Value, out temp))
				{
					Debug.LogError($"\"{data[status].Value}\" is not a part of \"{status}\" Enum");
				}
				else
				{
					result.Add(temp);
				}
			}
		}

		protected void GetDoubleArray(JSONNode data, string status, out List<double> result)
		{
			if (data[status].IsArray)
			{
				JSONArray arr = data[status].AsArray;
				result = new List<double>();
				for (int i = 0; i < arr.Count; i++)
				{
					result.Add(arr[i].AsDouble);
				}
			}
			else
			{
				result = new List<double>();
				result.Add(data[status].AsDouble);
			}
		}

		protected void GetVector2Array(JSONNode data, string status, out List<Vector2Double> result)
		{
			if (data[status].IsArray)
			{
				JSONArray arr = data[status].AsArray;
				result = new List<Vector2Double>();
				for (int i = 0; i < arr.Count; i++)
				{
					result.Add(new Vector2Double(arr[i]));
				}
			}
			else
			{
				result = new List<Vector2Double>();
				result.Add(new Vector2Double(data[status]));
			}
		}
		#endregion
		#endregion

		#region Save Utility
		protected void SetString(string data, string status, ref JSONObject result)
		{
			result[status] = data; // result.Add(status, data);
		}

		protected void SetInt(int data, string status, ref JSONObject result)
		{
			result[status] = data; // result.Add(status, data);
		}

		protected void SetDouble(double data, string status, ref JSONObject result)
		{
			result[status] = data;
		}

		protected void SetBool(bool data, string status, ref JSONObject result)
		{
			result[status] = data; // result.Add(status, data);
		}

		protected void SetVector2(Vector2Double data, string status, ref JSONObject result)
		{
			JSONObject temp = new JSONObject();
			temp["x"] = data.x;
			temp["y"] = data.y;
			result.Add(status, temp);
		}

		protected void SetIntArray(List<int> array, string status, ref JSONObject result, bool smartConvert = true)
		{
			if (array.Count > 1)
			{
				JSONArray ints = new JSONArray();
				for (int i = 0; i < array.Count; i++)
				{
					ints.Add(array[i]);
				}
				result.Add(status, ints);
			}
			else if (smartConvert)
			{
				if (array.Count == 1)
				{
					result.Add(status, array[0]);
				}
				else
				{
					JSONArray ints = new JSONArray();
					result.Add(status, null);
				}
			}
			else
			{
				JSONArray ints = new JSONArray();
				if (array.Count == 1)
				{
					ints.Add(array[0]);
				}

				result.Add(status, ints);
			}
		}

		protected void SetStringArray(List<string> array, string status, ref JSONObject result, bool smartConvert = true)
		{
			if (array.Count > 1)
			{
				JSONArray strings = new JSONArray();
				for (int i = 0; i < array.Count; i++)
				{
					strings.Add(array[i]);
				}
				result.Add(status, strings);
			}
			else if (smartConvert)
			{
				if (array.Count == 1)
				{
					result.Add(status, array[0]);
				}
				else
				{
					JSONArray ints = new JSONArray();
					result.Add(status, null);
				}
			}
			else
			{
				JSONArray ints = new JSONArray();
				if (array.Count == 1)
				{
					ints.Add(array[0]);
				}

				result.Add(status, ints);
			}
		}

		protected void SetEnumArray<T>(List<T> array, string status, ref JSONObject result, bool smartConvert = true)
		{
			if (array.Count > 1)
			{
				JSONArray ints = new JSONArray();
				int[] converted = array.Cast<int>().ToArray();
				for (int i = 0; i < converted.Length; i++)
				{
					ints.Add(converted[i]);
				}
				result.Add(status, ints);
			}
			else if (smartConvert)
			{
				if (array.Count == 1)
				{
					int[] converted = array.Cast<int>().ToArray();
					result.Add(status, converted[0]);
				}
				else
				{
					JSONArray ints = new JSONArray();
					result.Add(status, ints);
				}
			}
			else
			{
				JSONArray ints = new JSONArray();
				int[] converted = array.Cast<int>().ToArray();
				if (array.Count == 1)
				{
					ints.Add(converted[0]);
				}

				result.Add(status, ints);
			}
		}

		protected void SetDoubleArray(List<double> array, string status, ref JSONObject result, bool smartConvert = true)
		{
			if (array.Count > 1)
			{
				JSONArray doubles = new JSONArray();
				for (int i = 0; i < array.Count; i++)
				{
					doubles.Add(array[i]);
				}
				result[status] = doubles; // result.Add(status, doubles);
			}
			else if (smartConvert)
			{
				if (array.Count == 1)
				{
					result.Add(status, array[0]);
				}
				else
				{
					JSONArray doubles = new JSONArray();
					result[status] = doubles; // result.Add(status, doubles);
				}
			}
			else
			{
				JSONArray doubles = new JSONArray();
				if (array.Count == 1)
				{
					doubles.Add(array[0]);
				}
				
				result[status] = doubles; // result.Add(status, doubles);
			}
		}

		protected void SetVector2Array(List<Vector2Double> array, string status, ref JSONObject result)
		{
			JSONArray vector2s = new JSONArray();
			for (int i = 0; i < array.Count; i++)
			{
				JSONObject temp = new JSONObject();
				temp["x"] = array[i].x;
				temp["y"] = array[i].y;
				vector2s.Add(temp);
			}
			result[status] = vector2s; // result.Add(status, doubles);
		}
		#endregion

		#region Vector2
		[Serializable]
		public class Vector2Double
		{
			[HorizontalGroup("General", 0.3f, LabelWidth = 30, Width = 50)] public double x;
			[HorizontalGroup("General", 0.3f, LabelWidth = 30, Width = 50)] public double y;

			public Vector2Double(JSONNode json)
			{
				x = json["x"].AsDouble;
				y = json["y"].AsDouble;
			}
		}
		#endregion
	}
}