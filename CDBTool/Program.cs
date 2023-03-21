using System;
using System.Globalization;
using System.Threading;
using ModTools;

namespace CDBTool
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
						text3 = text6;
					}

					else if (text6 == "INDIR")
					{
						text6 = args[++i];
						text4 = text6;
					}

					else if (text6 == "REFCDB")
					{
						text6 = args[++i];
						text = text6;
					}

					else if (text6 == "OUTCDB")
					{
						text6 = args[++i];
						text2 = text6;
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
							Console.WriteLine("-Expand -outDir <output directory> -refCDB <input cdb path> [-s]: Expands a given CDB to a file tree");
							Console.WriteLine("-Collapse -inDir <input directory> -outCDB <output cdb path> [-s]: Collapse a given file tree to a cdb");
							Console.WriteLine("-CreateDiffCDB -inDir <input directory> -outDir <output cdb path> -refCDB <reference cdb path> [-s]: Copy only changed or added expanded CDB files from a directory path to an outputpath(typically for mods)");
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
				CDBTool cdbtool = new CDBTool();
				Console.WriteLine("Launching CDBTool v" + Versionning.currentVersion + ", action: " + text5);

				if (text5 == "EXPAND")
				{
					cdbtool.Expand(text, text3);
				}

				else if (text5 == "COLLAPSE")
				{
					cdbtool.Collapse(text4, text2);
				}

				else
				{
					if (!(text5 == "CREATEDIFFCDB"))
					{
						throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", text5), "strAction");
					}
					cdbtool.BuildDiffCDB(text, text4, text3);
				}
			}

			catch (Exception ex)
			{
				Error.Show(ex, flag);
			}
		}
	}
}
