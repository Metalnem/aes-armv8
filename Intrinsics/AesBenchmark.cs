using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;

namespace Intrinsics
{
	[InProcess]
	public class AesBenchmark
	{
		private static readonly byte[] input1K = new byte[1024];
		private static readonly byte[] output1K = new byte[input1K.Length];

		private static readonly byte[] input1M = new byte[1024 * 1024];
		private static readonly byte[] output1M = new byte[input1M.Length];

		private readonly ICryptoTransform openSsl = Aes.Create().CreateEncryptor();
		private readonly AesArm64 intrinsics = new AesArm64();

		[Benchmark] public void OpenSsl1K() => openSsl.TransformBlock(input1K, 0, input1K.Length, output1K, 0);
		[Benchmark] public void Intrinsics1K() => intrinsics.Encrypt(input1K, output1K);

		[Benchmark] public void OpenSsl1M() => openSsl.TransformBlock(input1M, 0, input1M.Length, output1M, 0);
		[Benchmark] public void Intrinsics1M() => intrinsics.Encrypt(input1M, output1M);
	}
}
