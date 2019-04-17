using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGLTF
{
	public static class ImageHelper
	{
		/// <summary>
		/// Start of the PNG binary data
		/// </summary>
		private static readonly byte[] PngHeaderStart = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

		/// <summary>
		/// Try to calculate the width and height of an image.
		/// Support PNG only
		/// Return 0 / 0 if unable to calculate image size
		/// </summary>
		public static void CalculateImageWidthHeight(byte[] buffer, out int w, out int h)
		{
			if (buffer.Length > PngHeaderStart.Length + 16 && StartWith(buffer, PngHeaderStart))
			{
				// Decode PNG
				w = ReadBigEndianInt32(buffer, PngHeaderStart.Length + 8);
				h = ReadBigEndianInt32(buffer, PngHeaderStart.Length + 8 + 4);
			}
			else
			{
				w = 0;
				h = 0;
			}
		}

		private static bool StartWith(byte[] source, byte[] start)
		{
			if (source.Length < start.Length)
			{
				return false;
			}

			for (int i = 0; i < start.Length; i += 1)
			{
				if (start[i] != source[i])
				{
					return false;
				}
			}
			return true;
		}

		private static int ReadBigEndianInt32(byte[] buffer, int start)
		{
			byte[] bytes = new byte[sizeof(int)];
			Array.Copy(buffer, start, bytes, 0, sizeof(int));

			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}

			return BitConverter.ToInt32(bytes, 0);
		}
	}
}
