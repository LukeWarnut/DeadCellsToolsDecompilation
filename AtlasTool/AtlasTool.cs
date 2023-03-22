using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using ModTools;
using Packer;

namespace AtlasTool
{
	internal class AtlasTool
	{
		public void Expand(string _atlasPath, string _outDirPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_outDirPath);

			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(true);
			}

			directoryInfo.Create();
			FileInfo fileInfo = new FileInfo(_atlasPath);
			fileInfo.Name.Substring(0, fileInfo.Name.Length - 6);
			Stream stream = File.OpenRead(_atlasPath);
			BinaryReader binaryReader = new BinaryReader(stream);
			new string(binaryReader.ReadChars(4));

			while (binaryReader.BaseStream.Position + 18L < stream.Length)
			{
				List<Tile> list = new List<Tile>();
				string text = this.ReadString(binaryReader);

				if (text == "")
				{
					break;
				}

				while (binaryReader.BaseStream.Position + 18L < stream.Length)
				{
					Tile tile = new Tile();
					tile.name = this.ReadString(binaryReader);

					if (tile.name == "")
					{
						break;
					}

					tile.index = (int)binaryReader.ReadUInt16();
					tile.x = (int)binaryReader.ReadUInt16();
					tile.y = (int)binaryReader.ReadUInt16();
					tile.width = (int)binaryReader.ReadUInt16();
					tile.height = (int)binaryReader.ReadUInt16();
					tile.offsetX = (int)binaryReader.ReadUInt16();
					tile.offsetY = (int)binaryReader.ReadUInt16();
					tile.originalWidth = (int)binaryReader.ReadUInt16();
					tile.originalHeight = (int)binaryReader.ReadUInt16();
					list.Add(tile);
				}

				this.CreateBitmapTree(list, directoryInfo, fileInfo, text, "");

				try
				{
					this.CreateBitmapTree(list, directoryInfo, fileInfo, text.Substring(0, text.Length - 4) + "_n.png", "_n");
				}
				catch (Exception)
				{
				}
			}

			binaryReader.Close();
		}

		public void CreateBitmapTree(List<Tile> _tiles, DirectoryInfo _outDir, FileInfo _atlasInfo, string _atlasName, string _suffix = "")
		{
			Bitmap bitmap = (Bitmap)Image.FromFile(Path.Combine(_atlasInfo.DirectoryName, _atlasName));
			Directory.CreateDirectory(_outDir.FullName);

			foreach (Tile tile in _tiles)
			{
				this.CopyBitmapFromAtlas(tile, _outDir.FullName, bitmap, _suffix);
			}

			bitmap.Dispose();
		}

		public void Collapse(string _inputDirPath, string _atlasPath, bool _exportBinaryAtlases)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_inputDirPath);
			new DirectoryInfo(new FileInfo(_atlasPath).DirectoryName).Create();

			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Directory not found : " + _inputDirPath);
			}

			List<Tile> list = new List<Tile>();
			new HashSet<Bitmap>();

			foreach (FileInfo fileInfo in from file in directoryInfo.GetFiles("*.png", SearchOption.AllDirectories)
				where !file.Name.EndsWith("_n.png")
				select file)
			{
				try
				{
					Bitmap bitmap = (Bitmap)Image.FromFile(fileInfo.FullName);
					Tile tile = new Tile();
					tile.width = (tile.originalWidth = bitmap.Width);
					tile.height = (tile.originalHeight = bitmap.Height);
					tile.bitmap = bitmap;
					tile.hasNormal = File.Exists(Path.Combine(fileInfo.DirectoryName, fileInfo.Name.Substring(0, fileInfo.Name.Length - 4) + "_n.png"));
					tile.name = fileInfo.FullName.Replace(directoryInfo.FullName, "").Substring(1);
					tile.name = tile.name.Substring(0, tile.name.Length - 4).Replace("\\", "/");
					int num = tile.name.IndexOf("-=-");

					if (num != -1)
					{
						int num2 = tile.name.IndexOf("-=-", num + 3);
						if (num2 != -1)
						{
							string text = tile.name.Substring(num + 3);
							text = text.Substring(0, text.IndexOf("-=-"));
							int.TryParse(text, out tile.index);
							tile.name = tile.name.Substring(0, num) + tile.name.Substring(num2 + 3);
						}
					}

					tile.originalFilePath = fileInfo.FullName;
					this.TrimTile(ref tile);
					this.ExtrudeTile(ref tile, false);

					if (tile.height != 0)
					{
						list.Add(tile);
						if (tile.width == 0)
						{
							throw new Exception("?? width should not be at 0, when height != 0");
						}
					}
				}

				catch (Exception ex)
				{
					Log.Error("Error collapsing " + fileInfo.Name, "");
					throw ex;
				}
			}

			list.Sort(delegate(Tile a, Tile b)
			{
				if (a.width > b.width)
				{
					return -1;
				}

				if (a.width != b.width)
				{
					return 1;
				}

				if (a.height > b.height)
				{
					return -1;
				}

				if (a.height == b.height)
				{
					return 0;
				}

				return 1;
			});

			Bin2DPacker bin2DPacker = new Bin2DPacker(new Size(32, 32), new Size(4096, 4096), Bin2DPacker.Algorithm.MaxRects);
			bin2DPacker.margin = new Size(1, 1);
			bin2DPacker.marginType = MarginType.All;

			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].bitmap != null)
				{
					bool flag;
					bin2DPacker.InsertElement((uint)i, new Size(list[i].width, list[i].height), out flag);
				}
			}

			int count = bin2DPacker.bins.Count;
			BinaryWriter binaryWriter = null;
			StreamWriter streamWriter = null;

			if (_exportBinaryAtlases)
			{
				binaryWriter = new BinaryWriter(File.OpenWrite(_atlasPath.Substring(0, _atlasPath.Length - 4) + ".atlas"));
			}

			else
			{
				streamWriter = new StreamWriter(_atlasPath.Substring(0, _atlasPath.Length - 4) + ".atlas");
			}

			for (int j = 0; j < count; j++)
			{
				Bin2D bin2D = bin2DPacker.bins[j];
				Bitmap bitmap2 = new Bitmap(bin2D.size.Width, bin2D.size.Height);
				Bitmap bitmap3 = new Bitmap(bin2D.size.Width, bin2D.size.Height);
				bool flag2 = false;

				foreach (KeyValuePair<uint, Rectangle> keyValuePair in bin2D.elements)
				{
					Tile tile2 = list[(int)keyValuePair.Key];
					tile2.x = keyValuePair.Value.X;
					tile2.y = keyValuePair.Value.Y;
					tile2.atlasIndex = j;
					this.CopyBitmapToAtlas(tile2, bitmap2);

					if (tile2.hasNormal)
					{
						tile2.bitmap = (Bitmap)Image.FromFile(tile2.originalFilePath.Substring(0, tile2.originalFilePath.Length - 4) + "_n.png");
						this.ExtrudeTile(ref tile2, true);
						this.CopyBitmapToAtlas(tile2, bitmap3);
						flag2 = true;
					}
				}

				string text2 = new FileInfo(_atlasPath).FullName;

				if (count > 1)
				{
					text2 = text2.Substring(0, text2.Length - 4) + j.ToString() + ".png";
				}

				bitmap2.Save(text2);

				if (flag2)
				{
					bitmap3.Save(text2.Substring(0, text2.Length - 4) + "_n.png");
					bitmap3.Dispose();
					bitmap3 = null;
				}

				if (_exportBinaryAtlases)
				{
					string text3 = "BATL";
					binaryWriter.Write(text3.ToCharArray());
					this.WriteString(binaryWriter, text2.Substring(text2.LastIndexOf('\\') + 1));

					foreach (Tile tile3 in list)
					{
						if (tile3.duplicateOf != null)
						{
							tile3.x = tile3.duplicateOf.x;
							tile3.y = tile3.duplicateOf.y;
						}
						this.WriteString(binaryWriter, tile3.name);
						binaryWriter.Write((ushort)tile3.index);
						binaryWriter.Write((int)((ushort)tile3.x + 1));
						binaryWriter.Write((int)((ushort)tile3.y + 1));
						binaryWriter.Write((ushort)tile3.width);
						binaryWriter.Write((ushort)tile3.height);
						binaryWriter.Write((ushort)tile3.offsetX);
						binaryWriter.Write((ushort)tile3.offsetY);
						binaryWriter.Write((ushort)tile3.originalWidth);
						binaryWriter.Write((ushort)tile3.originalHeight);
					}

					binaryWriter.Write(0);
				}

				else
				{
					streamWriter.WriteLine("");
					streamWriter.WriteLine(text2.Substring(text2.LastIndexOf('\\') + 1));
					streamWriter.WriteLine("size: {0},{1}", bitmap2.Width, bitmap2.Height);
					streamWriter.WriteLine("format: RGBA8888");
					streamWriter.WriteLine("filter: Linear,Linear");
					streamWriter.WriteLine("repeat: none");

					foreach (Tile tile4 in list)
					{
						if (tile4.duplicateOf != null)
						{
							tile4.x = tile4.duplicateOf.x;
							tile4.y = tile4.duplicateOf.y;
							tile4.atlasIndex = tile4.duplicateOf.atlasIndex;
						}

						if (tile4.atlasIndex == j)
						{
							streamWriter.WriteLine(tile4.name);
							streamWriter.WriteLine("  rotate: false");
							streamWriter.WriteLine("  xy: {0}, {1}", tile4.x + 1, tile4.y + 1);
							streamWriter.WriteLine("  size: {0}, {1}", tile4.width - 2, tile4.height - 2);
							streamWriter.WriteLine("  orig: {0}, {1}", tile4.originalWidth - 2, tile4.originalHeight - 2);
							streamWriter.WriteLine("  offset: {0}, {1}", tile4.offsetX, tile4.originalHeight - 2 - (tile4.height - 2 + tile4.offsetY));
							streamWriter.WriteLine("  index: {0}", tile4.index);
						}
					}
				}

				bitmap2.Dispose();
				bitmap2 = null;
			}

			if (_exportBinaryAtlases)
			{
				binaryWriter.Write(0);
				binaryWriter.Close();
				return;
			}

			streamWriter.Close();
		}

		private void CopyBitmapFromAtlas(Tile _tile, string _path, Bitmap _atlas, string _suffix)
		{
			string[] array = _tile.name.Split(new char[] { '/' });
			string text = _path;

			for (int i = 0; i < array.Length - 1; i++)
			{
				text = Directory.CreateDirectory(Path.Combine(text, array[i])).FullName;
			}

			string text2 = array[array.Length - 1];

			if (_tile.index != -1)
			{
				text2 = text2 + "-=-" + _tile.index.ToString() + "-=-";
			}

			BitmapData bitmapData = _atlas.LockBits(new Rectangle(_tile.x, _tile.y, _tile.width, _tile.height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			Bitmap bitmap = new Bitmap(_tile.originalWidth, _tile.originalHeight);
			BitmapData bitmapData2 = bitmap.LockBits(new Rectangle(_tile.offsetX, _tile.offsetY, _tile.width, _tile.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			
			for (int j = 0; j < _tile.height; j++)
			{
				Core.CopyMemory(bitmapData2.Scan0 + j * bitmapData2.Stride, bitmapData.Scan0 + j * bitmapData.Stride, (uint)(_tile.width * 4));
			}

			bitmap.UnlockBits(bitmapData2);
			_atlas.UnlockBits(bitmapData);
			string text3 = Path.Combine(text, text2 + _suffix + ".png");
			bitmap.Save(text3);
			bitmap.Dispose();
		}

		private void TrimTile(ref Tile _tile)
		{
			BitmapData bitmapData = _tile.bitmap.LockBits(new Rectangle(0, 0, _tile.width, _tile.height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			Tile tile;
			byte[] array = AtlasTool.buffer;
			string text;

			lock (array)
			{
				Marshal.Copy(bitmapData.Scan0, AtlasTool.buffer, 0, _tile.originalWidth * _tile.originalHeight * 4);
				text = Convert.ToBase64String(SHA256.Create().ComputeHash(AtlasTool.buffer, 0, _tile.originalWidth * _tile.originalHeight * 4));
			}

			if (!this.hashes.TryGetValue(text, out tile))
			{
				this.hashes.Add(text, _tile);
				bool flag2 = false;
				int num = 0;

				while (num < _tile.originalHeight && !flag2 && _tile.height > 1)
				{
					int num2 = 0;

					while (num2 < _tile.originalWidth && !flag2 && _tile.height > 1)
					{
						flag2 = ((long)Marshal.ReadInt32(bitmapData.Scan0 + (num * _tile.originalWidth + num2) * 4) & 0xFF000000u) != 0L;
						num2++;
					}

					if (!flag2)
					{
						_tile.offsetY++;
						_tile.height--;
					}

					num++;
				}

				flag2 = false;
				int num3 = 0;

				while (num3 < _tile.originalWidth && !flag2 && _tile.width > 1)
				{
					int num4 = _tile.offsetY;

					while (num4 < _tile.originalHeight && !flag2 && _tile.width > 1)
					{
						flag2 = ((long)Marshal.ReadInt32(bitmapData.Scan0 + (num4 * _tile.originalWidth + num3) * 4) & 0xFF000000u) != 0L;
						num4++;
					}

					if (!flag2)
					{
						_tile.offsetX++;
						_tile.width--;
					}

					num3++;
				}

				flag2 = false;
				int num5 = _tile.originalHeight - 1;

				while (num5 >= _tile.offsetY && !flag2 && _tile.height > 1)
				{
					int num6 = _tile.offsetX;

					while (num6 < _tile.originalWidth && !flag2 && _tile.height > 1)
					{
						flag2 = ((long)Marshal.ReadInt32(bitmapData.Scan0 + (num5 * _tile.originalWidth + num6) * 4) & 0xFF000000u) != 0L;
						num6++;
					}

					if (!flag2)
					{
						_tile.height--;
					}

					num5--;
				}

				flag2 = false;
				int num7 = _tile.originalWidth - 1;

				while (num7 >= _tile.offsetX && !flag2 && _tile.width > 1)
				{
					int num8 = _tile.offsetY;

					while (num8 < _tile.originalHeight && !flag2 && _tile.width > 1)
					{
						flag2 = ((long)Marshal.ReadInt32(bitmapData.Scan0 + (num8 * _tile.originalWidth + num7) * 4) & 0xFF000000u) != 0L;
						num8++;
					}

					if (!flag2)
					{
						_tile.width--;
					}

					num7--;
				}

				_tile.bitmap.UnlockBits(bitmapData);
				return;
			}

			_tile.duplicateOf = tile;
			_tile.x = tile.x;
			_tile.y = tile.y;
			_tile.offsetX = tile.offsetX;
			_tile.offsetY = tile.offsetY;
			_tile.originalWidth = tile.originalWidth;
			_tile.originalHeight = tile.originalHeight;
			_tile.width = tile.width;
			_tile.height = tile.height;
			_tile.bitmap = null;
		}

		private void ExtrudeTile(ref Tile _tile, bool _bForceBitmapResizeAndPreventUpdateTileInfo = false)
		{
			if (_tile.bitmap == null)
			{
				return;
			}

			string text = "none";

			try
			{
				if (_bForceBitmapResizeAndPreventUpdateTileInfo || _tile.offsetX == 0 || _tile.offsetY == 0 || _tile.originalWidth - _tile.width < _tile.offsetX + 2 || _tile.originalHeight - _tile.height < _tile.offsetY + 2)
				{
					int width = _tile.bitmap.Width;
					int height = _tile.bitmap.Height;
					Bitmap bitmap = new Bitmap(width + 2, height + 2);
					text = "reading old";
					BitmapData bitmapData = _tile.bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					text = "reading new";
					BitmapData bitmapData2 = bitmap.LockBits(new Rectangle(1, 1, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					
					for (int i = 0; i < height; i++)
					{
						Core.CopyMemory(bitmapData2.Scan0 + i * bitmapData2.Stride, bitmapData.Scan0 + i * bitmapData.Stride, (uint)(width * 4));
					}

					_tile.bitmap.UnlockBits(bitmapData);
					bitmap.UnlockBits(bitmapData2);
					_tile.bitmap.Dispose();
					_tile.bitmap = bitmap;

					if (!_bForceBitmapResizeAndPreventUpdateTileInfo)
					{
						_tile.offsetX++;
						_tile.offsetY++;
						_tile.originalHeight += 2;
						_tile.originalWidth += 2;
					}
				}

				if (!_bForceBitmapResizeAndPreventUpdateTileInfo)
				{
					_tile.offsetX--;
					_tile.offsetY--;
					_tile.width += 2;
					_tile.height += 2;
				}

				text = "writing";
				string.Format("_tile.offsetX = {0}, _tile.offsetY = {1}, _tile.width = {2}, _tile.height = {3}", new object[] { _tile.offsetX, _tile.offsetY, _tile.width, _tile.height });
				BitmapData bitmapData3 = _tile.bitmap.LockBits(new Rectangle(_tile.offsetX, _tile.offsetY, _tile.width, _tile.height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				
				for (int j = 0; j < _tile.height; j++)
				{
					Marshal.WriteInt32(bitmapData3.Scan0 + j * bitmapData3.Stride, Marshal.ReadInt32(bitmapData3.Scan0 + j * bitmapData3.Stride + 4));
					Marshal.WriteInt32(bitmapData3.Scan0 + j * bitmapData3.Stride + (_tile.width - 1) * 4, Marshal.ReadInt32(bitmapData3.Scan0 + j * bitmapData3.Stride + (_tile.width - 2) * 4));
				}
				
				int num = (_tile.height - 1) * bitmapData3.Stride;
				
				for (int k = 0; k < _tile.width; k++)
				{
					Marshal.WriteInt32(bitmapData3.Scan0 + k * 4, Marshal.ReadInt32(bitmapData3.Scan0 + bitmapData3.Stride + k * 4));
					int num2 = Marshal.ReadInt32(bitmapData3.Scan0 + num - bitmapData3.Stride + k * 4);
					Marshal.WriteInt32(bitmapData3.Scan0 + num + k * 4, num2);
				}
				
				_tile.bitmap.UnlockBits(bitmapData3);
			}
			
			catch (Exception ex)
			{
				Log.Error(string.Concat(new string[] { "Problem ", text, " ", _tile.name, " when extruding" }), "");
				throw ex;
			}
		}
		
		private void CopyBitmapToAtlas(Tile _tile, Bitmap _atlas)
		{
			if (_tile.bitmap == null)
			{
				return;
			}
			
			BitmapData bitmapData = _atlas.LockBits(new Rectangle(_tile.x, _tile.y, _tile.width, _tile.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			BitmapData bitmapData2 = _tile.bitmap.LockBits(new Rectangle(_tile.offsetX, _tile.offsetY, _tile.width, _tile.height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			
			for (int i = 0; i < _tile.height; i++)
			{
				Core.CopyMemory(bitmapData.Scan0 + i * bitmapData.Stride, bitmapData2.Scan0 + i * bitmapData2.Stride, (uint)(_tile.width * 4));
			
			}
			_atlas.UnlockBits(bitmapData);
			_tile.bitmap.UnlockBits(bitmapData2);
		}
		
		private string ReadString(BinaryReader _reader)
		{
			int num = (int)_reader.ReadByte();
			if (num == 255)
			{
				num = (int)_reader.ReadUInt16();
			}
			if (num != 0)
			{
				return new string(_reader.ReadChars(num));
			}
			return "";
		}
		
		private void WriteString(BinaryWriter _writer, string _stringToWrite)
		{
			if (_stringToWrite.Length >= 255)
			{
				_writer.Write((ushort)_stringToWrite.Length);
			}
			else
			{
				_writer.Write((byte)_stringToWrite.Length);
			}
			_writer.Write(_stringToWrite.ToCharArray());
		}
		
		private static byte[] buffer = new byte[67108864];
		private Dictionary<string, Tile> hashes = new Dictionary<string, Tile>();
	}
}
