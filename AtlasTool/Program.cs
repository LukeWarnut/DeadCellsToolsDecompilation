using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ModTools;

namespace AtlasTool
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			string text = "";
			string inDir = "";
			string outDir = "";
			string text2 = "";
			bool flag = true;
			bool bBinary = true;

			for (int i = 0; i < args.Length; i++)
			{
				string text3 = args[i];

				if (text3[0] == '-' || text3[0] == '/')
				{
					text3 = text3.Substring(1).ToUpper();

					if (text3 == "INDIR")
					{
						text3 = args[++i];
						inDir = text3;
					}
					else if (text3 == "OUTDIR")
					{
						text3 = args[++i];
						outDir = text3;
					}
					else if (text3 == "ATLAS" || text3 == "A")
					{
						text3 = args[++i];
						text = text3;
					}
					else if (text3 == "SILENT" || text3 == "S")
					{
						flag = false;
					}
					else if (text3 == "ASCII")
					{
						bBinary = false;
					}

					else
					{
						if (text3 == "?")
						{
							Console.WriteLine("-? : Display this help");
							Console.WriteLine("-Expand -outdir <output directory> -Atlas <input atlas path> [-s]: Expands a given Atlas to a file tree");
							Console.WriteLine("-ExpandAll -indir <input atlases directory> -outdir <output directory> [-s]: Expands every atlas found in indir into outdir");
							Console.WriteLine("-Collapse -indir <input directory> -Atlas <output atlas path> [-s][-ascii]: Collapse a given file tree to an atlas");
							Console.WriteLine("-CollapseAll -indir <input directories> -outdir <output atlases path> [-s][-ascii]: Collapse every directory in the input directory into atlases");
							Console.WriteLine("arguments :");
							Console.WriteLine("-s/-silent : Do not display message error (deactivated by default)");
							Console.WriteLine("-ascii : Export atlases as ascii (binary by default)");
							return;
						}

						text2 = text3.ToUpper();
					}
				}
			}

			try
			{
				AtlasTool atlasTool = new AtlasTool();
				Console.WriteLine("Launching AtlasTool v" + Versionning.currentVersion + ", action: " + text2);

				if (text2 == "EXPAND")
				{
					atlasTool.Expand(text, outDir);
				}

				else if (text2 == "COLLAPSE")
				{
					atlasTool.Collapse(inDir, text, bBinary);
				}

				else if (text2 == "EXPANDALL")
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(inDir);
					List<Task> list = new List<Task>();
					FileInfo[] files = directoryInfo.GetFiles("*.atlas");

					for (int j = 0; j < files.Length; j++)
					{
						FileInfo atlas = files[j];

						Task task = Task.Factory.StartNew(delegate
						{
							AtlasTool atlasTool2 = new AtlasTool();
							string text4 = atlas.Name.Substring(0, atlas.Name.Length - 6);
							atlasTool2.Expand(atlas.FullName, Path.Combine(outDir, text4));
						});

						list.Add(task);
					}
					Task.WaitAll(list.ToArray());
				}

				else
				{
					if (!(text2 == "COLLAPSEALL"))
					{
						throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", text2), "strAction");
					}

					DirectoryInfo directoryInfo2 = new DirectoryInfo(inDir);
					List<Task> list2 = new List<Task>();
					DirectoryInfo[] directories = directoryInfo2.GetDirectories();

					for (int j = 0; j < directories.Length; j++)
					{
						DirectoryInfo dir = directories[j];

						Task task2 = Task.Factory.StartNew(delegate
						{
							AtlasTool atlasTool3 = new AtlasTool();
							string name = dir.Name;
							atlasTool3.Collapse(Path.Combine(inDir, name), Path.Combine(outDir, name) + ".png", bBinary);
						});

						list2.Add(task2);
					}
					Task.WaitAll(list2.ToArray());
				}
			}

			catch (Exception ex)
			{
				Error.Show(ex, flag);
			}
		}
	}
}
