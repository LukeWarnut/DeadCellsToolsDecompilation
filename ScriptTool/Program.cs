using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ModTools;

namespace ScriptTool
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			bool flag = false;
			bool flag2 = true;
			string text = "";
			string text2 = "";

			for (int i = 0; i < args.Length; i++)
			{
				string text3 = args[i];
				if (text3[0] == '-' || text3[0] == '/')
				{
					text3 = text3.Substring(1).ToUpper();

					if (text3 == "COMMANDLINE")
					{
						flag = true;
					}

					else if (text3 == "OUTSCRIPT")
					{
						text3 = args[++i];
						text2 = text3;
					}

					else if (text3 == "SILENT" || text3 == "S")
					{
						flag2 = false;
					}

					else
					{
						if (text3 == "?")
						{
							Console.WriteLine("-? : Display this help");
							Console.WriteLine("-CommandLine -NewFile -OutScript <output file name> [-s] : create a script file with default function and basic doc inside.");
							Console.WriteLine("arguments :");
							Console.WriteLine("-s/-silent : Do not display message error (deactivated by default)");
							return;
						}
						text = text3;
					}
				}
			}

			if (args.Length != 0 && !flag)
			{
				MessageBox.Show("The application has been launched with arguments, if you want to use this exe as command line, add \"-CommandLine\" in the arguments.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}

			if (!flag)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Main());
				return;
			}

			try
			{
				Console.WriteLine("Launching ScriptTool v" + Versionning.currentVersion + " action: " + text);

				if (!(text == "NEWFILE"))
				{
					throw new ArgumentException(string.Format("The action for \"{0}\" argument is not found, please refer to the doc", text), "strAction");
				}

				FileInfo fileInfo = new FileInfo(text2);
				fileInfo.Directory.Create();
				string text4 = fileInfo.FullName;

				if (fileInfo.Extension != ".hx")
				{
					text4 += ".hx";
				}

				File.WriteAllText(text4, ScriptWriter.instance.WriteWholeScript());
			}
			
			catch (Exception ex)
			{
				Error.Show(ex, flag2);
			}
		}
	}
}
