using System;
using System.Globalization;
using System.Threading;
using ModTools;

namespace PAKTool
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			bool flag = true;
			for (int i = 0; i < args.Length; i++)
			{
				string text6 = args[i];
				if (text6[0] == '-' || text6[0] == '/')
				{
					text6 = text6.Substring(1).ToUpper();
					if (text6 == "OUTDIR")
					{
						text6 = args[++i];
						text2 = text6;
					}
					else if (text6 == "INDIR")
					{
						text6 = args[++i];
						text = text6;
					}
					else if (text6 == "REFPAK")
					{
						text6 = args[++i];
						text3 = text6;
					}
					else if (text6 == "OUTPAK")
					{
						text6 = args[++i];
						text4 = text6;
					}
					else if (text6 == "SILENT" || text6 == "S")
					{
						flag = false;
					}
					else
					{
						if (text6 == "?")
						{
							Console.WriteLine("-? : Display this help");
							Console.WriteLine("-Expand -outdir <output directory> -refpak <input pak path> [-s]: Expands a given PAK to a file tree");
							Console.WriteLine("-Collapse -indir <input directory> -outpak <output pak path> [-s]: Collapse a given file tree to a pak");
							Console.WriteLine("-CreateDiffPak -refpak <input pak path> -indir <input directory> -outPak <output pak path> [-s]: Create a pak from a directory with only what has changed or been added from the ref pak (typically for mods)");
							Console.WriteLine("arguments :");
							Console.WriteLine("-s/-silent : Do not display message error (deactivated by default)");
							return;
						}
						text5 = text6.ToUpper();
					}
				}
			}
			try
			{
				PAKTool paktool = new PAKTool();
				Console.WriteLine("Launching PAKTool v" + Versionning.currentVersion + " action: " + text5);
				if (text5 == "EXPAND")
				{
					paktool.ExpandPAK(text3, text2);
				}
				else if (text5 == "COLLAPSE")
				{
					paktool.BuildPAK(text, text4);
				}
				else
				{
					if (!(text5 == "CREATEDIFFPAK"))
					{
						throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", text5), "strAction");
					}
					paktool.BuildDiffPAK(text3, text, text4);
				}
			}
			catch (Exception ex)
			{
				Error.Show(ex, flag);
			}
		}
	}
}
