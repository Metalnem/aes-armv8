using System;
using System.Runtime.Intrinsics.Arm.Arm64;

namespace Intrinsics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine(Aes.IsSupported);
		}
	}
}
