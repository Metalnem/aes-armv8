using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Intrinsics
{
	[InProcess]
	public class AesBenchmark
	{
		private static readonly byte[] input16B = new byte[16];
		private static readonly byte[] output16B = new byte[input16B.Length];

		private static readonly byte[] input1K = new byte[1024];
		private static readonly byte[] output1K = new byte[input1K.Length];

		private static readonly byte[] input1M = new byte[1024 * 1024];
		private static readonly byte[] output1M = new byte[input1M.Length];

		private readonly ICryptoTransform openSsl = Aes.Create().CreateEncryptor();
		private readonly AesArm64 intrinsics = new AesArm64();
		private readonly IBufferedCipher bouncySlow;
		private readonly IBufferedCipher bouncyFast;

		public AesBenchmark()
		{
#pragma warning disable 0618
			var engine = new AesFastEngine();
#pragma warning restore 0618

			bouncySlow = CipherUtilities.GetCipher("AES/ECB/NoPadding");
			bouncySlow.Init(true, new KeyParameter(new byte[16]));

			bouncyFast = new BufferedBlockCipher(engine);
			bouncyFast.Init(true, new KeyParameter(new byte[16]));
		}

		[Benchmark] public void OpenSsl16B() => openSsl.TransformBlock(input16B, 0, input16B.Length, output16B, 0);
		[Benchmark] public void Intrinsics16B() => intrinsics.Encrypt(input16B, output16B);
		[Benchmark] public void BouncyCastleSlow16B() => bouncySlow.ProcessBytes(input16B, 0, input16B.Length, output16B, 0);
		[Benchmark] public void BouncyCastleFast16B() => bouncyFast.ProcessBytes(input16B, 0, input16B.Length, output16B, 0);

		[Benchmark] public void OpenSsl1K() => openSsl.TransformBlock(input1K, 0, input1K.Length, output1K, 0);
		[Benchmark] public void Intrinsics1K() => intrinsics.Encrypt(input1K, output1K);
		[Benchmark] public void IntrinsicsPipelined1K() => intrinsics.EncryptPipelined(input1K, output1K);
		[Benchmark] public void BouncyCastleSlow1K() => bouncySlow.ProcessBytes(input1K, 0, input1K.Length, output1K, 0);
		[Benchmark] public void BouncyCastleFast1K() => bouncyFast.ProcessBytes(input1K, 0, input1K.Length, output1K, 0);

		[Benchmark] public void OpenSsl1M() => openSsl.TransformBlock(input1M, 0, input1M.Length, output1M, 0);
		[Benchmark] public void Intrinsics1M() => intrinsics.Encrypt(input1M, output1M);
		[Benchmark] public void IntrinsicsPipelined1M() => intrinsics.EncryptPipelined(input1M, output1M);
		[Benchmark] public void BouncyCastleSlow1M() => bouncySlow.ProcessBytes(input1M, 0, input1M.Length, output1M, 0);
		[Benchmark] public void BouncyCastleFast1M() => bouncyFast.ProcessBytes(input1M, 0, input1M.Length, output1M, 0);
	}
}
