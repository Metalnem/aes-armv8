using System;
using BenchmarkDotNet.Running;

namespace Intrinsics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<AesBenchmark>();
		}

		public static void Test()
		{
			var input = new byte[] { 0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34 };
			var output = new byte[input.Length];

			var aes = new AesArm64();
			aes.Encrypt(input, output);

			// 3925841d02dc09fbdc118597196a0b32
			Console.WriteLine(BitConverter.ToString(output).Replace("-", String.Empty).ToLower());
		}
	}
}
