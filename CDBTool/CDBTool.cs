using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using ModTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CDBTool
{
	public class CDBTool
	{
		public void Expand(string _sourceCDBPath, string _output)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_output);
			string[] array = new string[] { "id", "item", "name", "room", "animId" };
			JObject jobject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(_sourceCDBPath));
			int num = 0;

			if (!File.Exists(_sourceCDBPath))
			{
				throw new FileNotFoundException("CDB file not found : " + _sourceCDBPath, _sourceCDBPath);
			}

			directoryInfo.Create();
			
			foreach (JToken jtoken in ((IEnumerable<JToken>)jobject["sheets"]))
			{
				JObject jobject2 = (JObject)jtoken;
				string text = jobject2["name"].ToString();
				DirectoryInfo directoryInfo2 = directoryInfo.CreateSubdirectory(text);
				JArray jarray = jobject2.Value<JArray>("columns");
				JObject jobject3 = new JObject();
				jobject3.Add("__columns", jarray);
				jobject3.Add("__table_index", num);

				if (this.bMultiThread)
				{
					this.WriteContentAsync(Path.Combine(directoryInfo2.FullName, "__STRUCTURE__.json"), jobject3.ToString()).ContinueWith(delegate(Task t)
					{
						Error.Show(t.Exception, false);
					}, TaskContinuationOptions.OnlyOnFaulted);
				}
				
				else
				{
					CDBTool.WriteContentSync(Path.Combine(directoryInfo2.FullName, "__STRUCTURE__.json"), jobject3.ToString());
				}
				
				List<Separator> list = new List<Separator>();
				JArray jarray2 = jobject2.Value<JArray>("separators");
				JArray jarray3 = null;
				
				if (jarray2 != null)
				{
					JObject jobject4 = jobject2.Value<JObject>("props");

					if (jobject4 != null)
					{
						jarray3 = jobject4.Value<JArray>("separatorTitles");
					}
				}
				
				if (jarray3 != null)
				{
					List<int> list2 = new List<int>();
					foreach (JToken jtoken2 in jarray2)
					{
						int num2 = (int)jtoken2;
						list2.Add(num2);
					}

					List<string> list3 = new List<string>();
					foreach (JToken jtoken3 in jarray3)
					{
						string text2 = (string)jtoken3;
						list3.Add(text2);
					}

					for (int i = 0; i < list2.Count; i++)
					{
						list.Add(new Separator(i, list3[i], list2[i]));
					}
				}

				int count = list.Count;
				if (count > 0)
				{
					list.Sort((Separator a, Separator b) => a.lineIndex.CompareTo(b.lineIndex));
				}

				JObject jobject5 = (JObject)jobject2["props"];
				jobject5.Remove("separatorTitles");

				if (this.bMultiThread)
				{
					this.WriteContentAsync(Path.Combine(directoryInfo2.FullName, "__PROPS__.json"), jobject5.ToString()).ContinueWith(delegate(Task t)
					{
						Error.Show(t.Exception, false);
					}, TaskContinuationOptions.OnlyOnFaulted);
				}

				else
				{
					CDBTool.WriteContentSync(Path.Combine(directoryInfo2.FullName, "__PROPS__.json"), jobject5.ToString());
				}

				int num3 = 0;
				string text3 = "";
				JArray jarray4 = jobject2.Value<JArray>("lines");
				int num4 = (int)Math.Floor(Math.Log10((double)jarray4.Count)) + 1;
				string text4 = "D" + num4;
				int num5 = -1;

				foreach (JToken jtoken4 in jarray4)
				{
					JObject jobject6 = (JObject)jtoken4;

					while (count > 0 && num5 + 1 < count && num3 >= list[num5 + 1].lineIndex)
					{
						num5++;
					}

					jobject6.Add("__separator_group_ID", num5);
					string text5 = "";

					if (num5 >= 0 && num5 < count)
					{
						text5 = list[num5].name;
					}

					jobject6.Add("__separator_group_Name", text5);
					jobject6.Add("__original_Index", num3);
					string text6 = null;

					if (text3 == "")
					{
						for (int j = 0; j < array.Length; j++)
						{
							if (text6 != null)
							{
								break;
							}
							try
							{
								text6 = jobject6[array[j]].ToString();
								text3 = array[j];
							}
							catch (Exception)
							{
							}
						}
					}

					else
					{
						try
						{
							text6 = jobject6[text3].ToString();
						}
						catch (Exception)
						{
						}
					}

					string text7 = num3.ToString(text4);
					if (string.IsNullOrEmpty(text6))
					{
						text6 = text7 + "-UnnamedLine";
					}
					
					string text8 = jobject6.ToString();
					string text9 = Path.Combine(directoryInfo2.FullName, text5);
					Directory.CreateDirectory(text9);
					
					if (this.bMultiThread)
					{
						this.WriteContentAsync(Path.Combine(text9, text7 + "---" + text6 + ".json"), text8).ContinueWith(delegate(Task t)
						{
							Error.Show(t.Exception, false);
						}, TaskContinuationOptions.OnlyOnFaulted);
					}
					else
					{
						CDBTool.WriteContentSync(Path.Combine(directoryInfo2.FullName, text5, text7 + "---" + text6 + ".json"), text8);
					}
					
					num3++;
				}
				num++;
			}
		}

		public void Collapse(string _rootInput, string _destCDBPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_rootInput);

			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Input directory not found : " + _rootInput);
			}

			JObject jobject = new JObject();
			JArray jarray = new JArray();
			jobject.Add("sheets", jarray);
			List<KeyValuePair<int, JObject>> list = new List<KeyValuePair<int, JObject>>();

			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				JObject jobject2 = new JObject();
				JProperty jpropName = new JProperty("name", directoryInfo2.Name);
				JArray jarray2 = new JArray();
				JObject jobject3 = null;
				JArray jarray3 = new JArray();
				JProperty jpropLines = new JProperty("lines", jarray2);
				JProperty jpropSeparators = new JProperty("separators", jarray3);
				jobject2.Add(jpropName);
				List<Separator> list2 = new List<Separator>();

				foreach (FileInfo fileInfo in directoryInfo2.GetFiles("*.json", SearchOption.AllDirectories))
				{
					if (fileInfo.Name != "__STRUCTURE__.json" && fileInfo.Name != "__PROPS__.json")
					{
						object obj = JsonConvert.DeserializeObject(File.ReadAllText(fileInfo.FullName));

						if (CSBCallSite == null)
						{
							CSBCallSite = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(JObject), typeof(CDBTool)));
						}

						Func<CallSite, object, JObject> target = CSBCallSite.Target;
						CallSite cs = CSBCallSite;

						if (CSBCallSite2 == null)
						{
							CSBCallSite2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", null, typeof(CDBTool), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
						}

						JObject jobject4 = target(cs, CSBCallSite2.Target(CSBCallSite2, obj));
						int num = jobject4.Value<int>("__separator_group_ID");
						string name = jobject4.Value<string>("__separator_group_Name");
						Separator separator = new Separator(num, name, 0);

						if (num != -1 && list2.Find((Separator s) => s.name == name) == null)
						{
							bool flag = false;
							do
							{
								if (list2.Count != 0)
								{
									int num2 = 0;
									while (num2 < list2.Count && list2[num2].id != separator.id)
									{
										num2++;
									}

									flag = num2 < list2.Count;

									if (flag)
									{
										foreach (Separator separator2 in list2)
										{
											if (separator2.id > separator.id)
											{
												separator2.id++;
											}
										}
										separator.id++;
									}
								}
							}
							while (flag);
							list2.Add(separator);
						}
					}
				}

				List<KeyValuePair<long, int>> list3 = new List<KeyValuePair<long, int>>();
				List<JObject> list4 = new List<JObject>();

				foreach (FileInfo fileInfo2 in directoryInfo2.GetFiles("*.json", SearchOption.AllDirectories))
				{
					object obj2 = JsonConvert.DeserializeObject(File.ReadAllText(fileInfo2.FullName));
					if (fileInfo2.Name == "__STRUCTURE__.json")
					{
						if (CSBCallSite3 == null)
						{
							CSBCallSite3 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(JObject), typeof(CDBTool)));
						}

						JObject jobject5 = CSBCallSite3.Target(CSBCallSite3, obj2);
						jobject2.Add(new JProperty("columns", jobject5.Value<JArray>("__columns").DeepClone()));
						list.Add(new KeyValuePair<int, JObject>(jobject5.Value<int>("__table_index"), jobject2));
					}
					else if (fileInfo2.Name == "__PROPS__.json")
					{
						if (CSBCallSite4 == null)
						{
							CSBCallSite4 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(JObject), typeof(CDBTool)));
						}

						Func<CallSite, object, JObject> target2 = CSBCallSite4.Target;
						CallSite cs2 = CSBCallSite4;

						if (CSBCallSite5 == null)
						{
							CSBCallSite5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", null, typeof(CDBTool), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
						}

						jobject3 = target2(cs2, CSBCallSite5.Target(CSBCallSite5, obj2));
					}
					else
					{
						if (CSBCallSite6 == null)
						{
							CSBCallSite6 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(JObject), typeof(CDBTool)));
						}

						Func<CallSite, object, JObject> target3 = CSBCallSite6.Target;
						CallSite cs3 = CSBCallSite6;

						if (CSBCallSite7 == null)
						{
							CSBCallSite7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeepClone", null, typeof(CDBTool), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
						}

						JObject jobject6 = target3(cs3, CSBCallSite7.Target(CSBCallSite7, obj2));
						int num3 = -1;
						string text = jobject6.Value<string>("__separator_group_Name");

						if (list2.Count > 0 && !string.IsNullOrEmpty(text))
						{
							if (list2.Count == 0 || text == "")
							{
							}

							int num4 = 0;
							while (num4 < list2.Count && list2[num4].name != text)
							{
								num4++;
							}

							num3 = list2[num4].id;
						}

						for (int k = 0; k < list2.Count; k++)
						{
							if (list2[k].id > num3)
							{
								list2[k].pushLine();
							}

							else if (list2[k].id == num3)
							{
								string text2 = jobject6.Value<string>("__separator_group_Name");
								if (list2[k].name != text2)
								{
									list2[k].pushLine();
								}
							}
						}

						int num5 = jobject6.Value<int>("__original_Index");
						list3.Add(new KeyValuePair<long, int>(((long)num5 << 32) | (long)(num3 + 1), list4.Count));
						list4.Add(jobject6);
						jobject6.Remove("__separator_group_ID");
						jobject6.Remove("__separator_group_Name");
						jobject6.Remove("__original_Index");
					}
				}

				list3.Sort((KeyValuePair<long, int> a, KeyValuePair<long, int> b) => a.Key.CompareTo(b.Key));

				for (int l = 0; l < list3.Count; l++)
				{
					jarray2.Add(list4[list3[l].Value]);
				}

				list2.Sort((Separator a, Separator b) => a.lineIndex.CompareTo(b.lineIndex));
				JArray jarray4 = new JArray();
				JProperty jproperty4 = new JProperty("separatorTitles", jarray4);
				jobject3.AddFirst(jproperty4);

				foreach (Separator separator3 in list2)
				{
					if (separator3.lineIndex > -1)
					{
						jarray3.Add(separator3.lineIndex);
						jarray4.Add(separator3.name);
					}
				}

				jobject2.Add(jpropLines);
				jobject2.Add(jpropSeparators);
				jobject2.Add(new JProperty("props", jobject3));
			}
			list.Sort((KeyValuePair<int, JObject> a, KeyValuePair<int, JObject> b) => a.Key.CompareTo(b.Key));

			for (int m = 0; m < list.Count; m++)
			{
				jarray.Add(list[m].Value);
			}

			jobject.Add("compress", false);
			jobject.Add("customTypes", new JArray());
			Directory.CreateDirectory(new FileInfo(_destCDBPath).Directory.FullName);
			string cdbjobjectAsString = CDBTool.GetCDBJObjectAsString(jobject);
			this.WriteContentAsync(_destCDBPath, cdbjobjectAsString).Wait();
		}

		public static string GetCDBJObjectAsString(JObject _root)
		{
			StringWriter stringWriter = new StringWriter();
			CastleJsonTextWriter castleJsonTextWriter = new CastleJsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented,
				Indentation = 1,
				IndentChar = '\t'
			};
			new JsonSerializer().Serialize(castleJsonTextWriter, _root);
			string text = stringWriter.ToString().Replace("\r", "");
			castleJsonTextWriter.Close();
			stringWriter.Close();
			return text;
		}

		public void BuildDiffCDB(string _referenceCDBPath, string _inputDirPath, string _outputDirPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
			string fullName = directoryInfo.FullName;
			this.Expand(_referenceCDBPath, fullName);
			Dictionary<string, CDBTool.Test> dictionary = this.BuildCRCFileMap(directoryInfo);
			DirectoryInfo directoryInfo2 = new DirectoryInfo(_inputDirPath);
			Dictionary<string, CDBTool.Test> dictionary2 = this.BuildCRCFileMap(directoryInfo2);
			List<string> list = new List<string>();

			foreach (KeyValuePair<string, CDBTool.Test> keyValuePair in dictionary2)
			{
				CDBTool.Test test;
				if (dictionary.TryGetValue(keyValuePair.Key, out test))
				{
					if (test.i != keyValuePair.Value.i)
					{
						list.Add(keyValuePair.Value.originalFileName);
					}
				}

				else
				{
					list.Add(keyValuePair.Value.originalFileName);
				}
			}

			if (list.Count > 0)
			{
				new DirectoryInfo(_outputDirPath).Create();
				foreach (string text in list)
				{
					FileInfo fileInfo = new FileInfo(Path.Combine(_outputDirPath, this.GetOriginalName(text)));
					Directory.CreateDirectory(fileInfo.Directory.FullName);
					File.Copy(Path.Combine(_inputDirPath, text), fileInfo.FullName, true);
				}
			}
			directoryInfo.Delete(true);
		}

		private Dictionary<string, CDBTool.Test> BuildCRCFileMap(DirectoryInfo _rootDir)
		{
			int length = _rootDir.FullName.Length;
			Dictionary<string, CDBTool.Test> dictionary = new Dictionary<string, CDBTool.Test>();
			List<DirectoryInfo> list = new List<DirectoryInfo>();
			list.Add(_rootDir);
			Adler32 adler = new Adler32();

			for (int i = 0; i < list.Count; i++)
			{
				foreach (DirectoryInfo directoryInfo in list[i].GetDirectories())
				{
					list.Add(directoryInfo);
				}

				foreach (FileInfo fileInfo in list[i].GetFiles())
				{
					string originalName = this.GetOriginalName(fileInfo.FullName.Substring(length + 1));

					try
					{
						JObject jobject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(fileInfo.FullName));
						int num;

						if (jobject.Remove("__original_Index"))
						{
							num = adler.Make(new MemoryStream(Encoding.UTF8.GetBytes(jobject.ToString())));
						}

						else
						{
							num = adler.Make(File.OpenRead(fileInfo.FullName));
						}

						dictionary.Add(originalName, new CDBTool.Test(num, jobject, fileInfo.FullName.Substring(length + 1)));
					}
					catch (Exception ex)
					{
						Log.Error("Failing to build CRC for file : " + originalName + " from " + fileInfo.FullName, "");
						throw ex;
					}
				}
			}
			return dictionary;
		}

		private string GetOriginalName(string _indexedAndCategorizedName)
		{
			string[] array = _indexedAndCategorizedName.Split(new char[] { '\\' });
			if (array.Length == 0)
			{
				array = _indexedAndCategorizedName.Split(new char[] { '/' });
			}
			List<string> list = new List<string>(array);
			while (list.Count > 2)
			{
				list.RemoveAt(1);
			}
			string text = list[list.Count - 1];
			int num = text.IndexOf("---");
			if (num != -1)
			{
				list[list.Count - 1] = text.Substring(num + 3);
			}
			return Path.Combine(list.ToArray());
		}

		private static void WriteContentSync(string _fileName, string _content)
		{
			StreamWriter streamWriter = new FileInfo(_fileName).CreateText();
			streamWriter.Write(_content);
			streamWriter.Close();
		}

		private async Task WriteContentAsync(string _fileName, string _content)
		{
			FileInfo fileInfo = new FileInfo(_fileName);
			StreamWriter writer = fileInfo.CreateText();
			await writer.WriteAsync(_content);
			writer.Close();
		}

		public const string structureFileName = "__STRUCTURE__.json";
		public const string propertyFileName = "__PROPS__.json";
		public const string columnsNodeName = "__columns";
		public const string tableIndexName = "__table_index";
		public const string separatorGroupIDName = "__separator_group_ID";
		public const string separatorGroupNameName = "__separator_group_Name";
		public const string originalIndexName = "__original_Index";
		public bool bMultiThread = true;

		public CallSite<Func<CallSite, object, JObject>> CSBCallSite;
		public CallSite<Func<CallSite, object, object>>  CSBCallSite2;
		public CallSite<Func<CallSite, object, JObject>> CSBCallSite3, CSBCallSite4;
        public CallSite<Func<CallSite, object, object>>  CSBCallSite5;
        public CallSite<Func<CallSite, object, JObject>> CSBCallSite6;
        public CallSite<Func<CallSite, object, object>>  CSBCallSite7;
        
        private class Test
		{
			public Test(int _i, JObject _obj, string _originalFileName)
			{
				this.i = _i;
				this.obj = _obj;
				this.originalFileName = _originalFileName;
			}

			public int i;

			public JObject obj;

			public string originalFileName;
		}
	}
}
