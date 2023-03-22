using System;
using System.Collections.Generic;
using System.IO;
using CDBTool;
using ModTools;

namespace PAKTool
{
	internal class PAKTool
	{
		public byte version { get; private set; }
		public DirectoryData root { get; private set; }

		public void ExpandPAK(string _pakPath, string _destination)
		{
			if (!File.Exists(_pakPath))
			{
				throw new FileNotFoundException("File not found: " + _pakPath, _pakPath);
			}

			DirectoryInfo directoryInfo = Directory.CreateDirectory(_destination);
			BinaryReader binaryReader = this.ReadPAKHeader(_pakPath);
			this.CreateTree(binaryReader, directoryInfo, this.root);
			binaryReader.Close();
		}

		public void BuildPAK(string _expandedPakPath, string _destination)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_expandedPakPath);
			
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Directory " + _expandedPakPath + " not found");
			}

			FileInfo fileInfo = new FileInfo(_destination);
			fileInfo.Directory.Create();
			this.root = new DirectoryData(null, "");
			this.dataSize = 0;
			this.headerSize = 0;
			this.version = 0;
			this.CreatePAKDirectory(this.root, directoryInfo);
			this.headerSize += 16;
			BinaryWriter binaryWriter = new BinaryWriter(fileInfo.Create());
			binaryWriter.Write(new char[] { 'P', 'A', 'K' });
			binaryWriter.Write(this.version);
			binaryWriter.Write(this.headerSize);
			binaryWriter.Write(this.dataSize);
			this.WritePAKEntry(binaryWriter, this.root);
			binaryWriter.Write(new char[] { 'D', 'A', 'T', 'A' });
			this.WritePAKContent(binaryWriter, directoryInfo);
			binaryWriter.Close();
		}

		public void BuildDiffPAK(string _referencePAK, string _inputDir, string _diffPAKPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_inputDir);

			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Directory " + _inputDir + " not found.");
			}

			BinaryReader binaryReader = this.ReadPAKHeader(_referencePAK);
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			List<DirectoryData> list = new List<DirectoryData>();
			list.Add(this.root);

			for (int i = 0; i < list.Count; i++)
			{
				foreach (FileData fileData in list[i].files)
				{
					dictionary.Add(fileData.fullName, fileData.checksum);
				}

				foreach (DirectoryData directoryData in list[i].directories)
				{
					list.Add(directoryData);
				}
			}

			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			Adler32 adler = new Adler32();

			if (_inputDir.Length > 0 && _inputDir[_inputDir.Length - 1] != '\\')
			{
				_inputDir += "\\";
			}

			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
			{
				dictionary2.Add(fileInfo.FullName.Replace(_inputDir, ""), adler.Make(File.OpenRead(fileInfo.FullName)));
			}

			List<string> list2 = new List<string>();
			DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
			directoryInfo2.Create();
			bool flag = false;

			foreach (KeyValuePair<string, int> keyValuePair in dictionary2)
			{
				int num;
				if (dictionary.TryGetValue(keyValuePair.Key, out num))
				{
					if (num != keyValuePair.Value)
					{
						if (keyValuePair.Key.Substring(keyValuePair.Key.Length - 4, 4).ToUpper() == ".CDB")
						{
							CDBTool.CDBTool cdbtool = new CDBTool.CDBTool();
							DirectoryInfo directoryInfo3 = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
							directoryInfo3.Create();
							string text = Path.Combine(directoryInfo3.FullName, keyValuePair.Key);
							EntryData entryData;

							if (!this.headerData.TryGetValue(keyValuePair.Key, out entryData))
							{
								throw new Exception("original CDB called " + keyValuePair.Key + " not found.");
							}

							FileData fileData2 = (FileData)entryData;
							FileStream fileStream = new FileStream(text, FileMode.OpenOrCreate);
							binaryReader.BaseStream.Seek((long)fileData2.position, SeekOrigin.Begin);
							fileStream.Write(binaryReader.ReadBytes(fileData2.size), 0, fileData2.size);
							fileStream.Close();
							DirectoryInfo directoryInfo4 = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
							cdbtool.Expand(Path.Combine(_inputDir, keyValuePair.Key), directoryInfo4.FullName);
							DirectoryInfo directoryInfo5 = new DirectoryInfo(Path.Combine(directoryInfo2.FullName, keyValuePair.Key + "_"));
							directoryInfo5.Create();
							cdbtool.BuildDiffCDB(text, directoryInfo4.FullName, directoryInfo5.FullName);
							flag = true;
						}

						else
						{
							list2.Add(Path.Combine(directoryInfo2.FullName, keyValuePair.Key));
						}
					}
				}

				else
				{
					list2.Add(Path.Combine(directoryInfo2.FullName, keyValuePair.Key));
				}
			}

			binaryReader.Close();

			if (list2.Count == 0 && !flag)
			{
				throw new Exception("No diff pak created, no changed or added files found.");
			}

			foreach (string text2 in list2)
			{
				FileInfo fileInfo2 = new FileInfo(text2);
				fileInfo2.Directory.Create();
				File.Copy(Path.Combine(_inputDir, text2.Replace(directoryInfo2.FullName, "").Substring(1)), fileInfo2.FullName);
			}

			this.BuildPAK(directoryInfo2.FullName, _diffPAKPath);
			directoryInfo2.Delete(true);
		}

		private BinaryReader ReadPAKHeader(string _pakPath)
		{
			this.headerData.Clear();
			BinaryReader binaryReader = new BinaryReader(File.OpenRead(_pakPath));
			new string(binaryReader.ReadChars(3));
			this.version = binaryReader.ReadByte();
			this.headerSize = binaryReader.ReadInt32();
			this.dataSize = binaryReader.ReadInt32();
			this.root = (DirectoryData)this.ReadPAKEntry(binaryReader, null);
			return binaryReader;
		}

		private void CreatePAKDirectory(DirectoryData _pakDirectory, DirectoryInfo _physicalDirectory)
		{
			this.headerSize++;
			this.headerSize += ((_pakDirectory.parent == null) ? 0 : _physicalDirectory.Name.Length);
			this.headerSize++;
			this.headerSize += 4;
			Adler32 adler = new Adler32();

			foreach (DirectoryInfo directoryInfo in _physicalDirectory.GetDirectories())
			{
				DirectoryData directoryData = new DirectoryData(_pakDirectory, directoryInfo.Name);
				_pakDirectory.AddEntry(directoryData);
				this.CreatePAKDirectory(directoryData, directoryInfo);
			}

			foreach (FileInfo fileInfo in _physicalDirectory.GetFiles())
			{
				this.headerSize++;
				this.headerSize += fileInfo.Name.Length;
				this.headerSize++;
				this.headerSize += 12;
				Stream stream = fileInfo.OpenRead();
				FileData fileData = new FileData(_pakDirectory, fileInfo.Name, this.dataSize, (int)fileInfo.Length, adler.Make(stream));
				_pakDirectory.AddEntry(fileData);
				this.dataSize += (int)fileInfo.Length;
				stream.Close();
			}
		}

		private void CreateTree(BinaryReader _reader, DirectoryInfo _rootDir, DirectoryData _currentDir)
		{
			if (_currentDir.name != "")
			{
				_rootDir.CreateSubdirectory(_currentDir.fullName);
			}

			foreach (DirectoryData directoryData in _currentDir.directories)
			{
				this.CreateTree(_reader, _rootDir, directoryData);
			}

			foreach (FileData fileData in _currentDir.files)
			{
				FileStream fileStream = new FileStream(Path.Combine(_rootDir.FullName, fileData.fullName), FileMode.Create);
				_reader.BaseStream.Seek((long)fileData.position, SeekOrigin.Begin);
				fileStream.Write(_reader.ReadBytes(fileData.size), 0, fileData.size);
				fileStream.Close();
			}
		}

		private EntryData ReadPAKEntry(BinaryReader _reader, DirectoryData _parent)
		{
			string text = new string(_reader.ReadChars((int)_reader.ReadByte()));
			EntryData entryData2;

			if ((_reader.ReadByte() & 1) != 0)
			{
				DirectoryData directoryData = new DirectoryData(_parent, text);
				int num = _reader.ReadInt32();

				for (int i = 0; i < num; i++)
				{
					EntryData entryData = this.ReadPAKEntry(_reader, directoryData);
					directoryData.AddEntry(entryData);
					this.headerData.Add(entryData.fullName, entryData);
				}

				entryData2 = directoryData;
			}

			else
			{
				entryData2 = new FileData(_parent, text, this.headerSize + _reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32());
			}

			return entryData2;
		}

		private void WritePAKEntry(BinaryWriter _writer, EntryData _entry)
		{
			byte b = (byte)_entry.name.Length;
			_writer.Write(b);

			if (b > 0)
			{
				_writer.Write(_entry.name.ToCharArray());
			}

			if (_entry.isDirectory)
			{
				DirectoryData directoryData = (DirectoryData)_entry;
				_writer.Write(1);
				_writer.Write(directoryData.directories.Count + directoryData.files.Count);

				foreach (DirectoryData directoryData2 in directoryData.directories)
				{
					this.WritePAKEntry(_writer, directoryData2);
				}

				using (List<FileData>.Enumerator enumerator2 = directoryData.files.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						FileData fileData = enumerator2.Current;
						this.WritePAKEntry(_writer, fileData);
					}
					return;
				}
			}

			FileData fileData2 = (FileData)_entry;
			_writer.Write(0);
			_writer.Write(fileData2.position);
			_writer.Write(fileData2.size);
			_writer.Write(fileData2.checksum);
		}

		private void WritePAKContent(BinaryWriter _writer, DirectoryInfo _dir)
		{
			foreach (DirectoryInfo directoryInfo in _dir.GetDirectories())
			{
				this.WritePAKContent(_writer, directoryInfo);
			}

			foreach (FileInfo fileInfo in _dir.GetFiles())
			{
				BinaryReader binaryReader = new BinaryReader(fileInfo.OpenRead());
				_writer.Write(binaryReader.ReadBytes((int)fileInfo.Length));
				binaryReader.Close();
			}
		}

		private int headerSize { get; set; }
		private int dataSize { get; set; }

		private Dictionary<string, EntryData> headerData = new Dictionary<string, EntryData>();
	}
}
