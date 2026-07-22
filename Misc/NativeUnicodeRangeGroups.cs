using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Owns the temporary native arrays used by the core Unicode range APIs.
	/// </summary>
	internal sealed class NativeUnicodeRangeGroups : IDisposable
	{
		private readonly ScopedAllocator allocator;

		internal NativeUnicodeRangeGroups(UnicodeRange[][] groups)
		{
			if (null == groups)
			{
				throw new ArgumentNullException(nameof(groups));
			}

			for (int i = 0; i < groups.Length; i++)
			{
				if (null == groups[i])
				{
					throw new ArgumentException("Unicode range groups cannot contain null arrays.", nameof(groups));
				}
			}

			this.allocator = new ScopedAllocator();
			this.Count = (ulong)groups.Length;
			if (0 == groups.Length)
			{
				return;
			}

			try
			{
				this.Starts = this.allocator.AllocHGlobal(checked(IntPtr.Size * groups.Length));
				this.Ends = this.allocator.AllocHGlobal(checked(IntPtr.Size * groups.Length));
				ulong[] counts = new ulong[groups.Length];

				for (int i = 0; i < groups.Length; i++)
				{
					UnicodeRange[] group = groups[i];
					uint[] starts = new uint[group.Length];
					uint[] ends = new uint[group.Length];
					for (int j = 0; j < group.Length; j++)
					{
						starts[j] = group[j].Start;
						ends[j] = group[j].End;
					}

					counts[i] = (ulong)group.Length;
					Marshal.WriteIntPtr(
						this.Starts,
						i * IntPtr.Size,
						this.allocator.AllocIntegerArray(starts));
					Marshal.WriteIntPtr(
						this.Ends,
						i * IntPtr.Size,
						this.allocator.AllocIntegerArray(ends));
				}

				this.Counts = this.allocator.AllocIntegerArray(counts);
			}
			catch
			{
				this.allocator.Dispose();
				throw;
			}
		}

		internal IntPtr Starts { get; private set; }

		internal IntPtr Ends { get; private set; }

		internal IntPtr Counts { get; private set; }

		internal ulong Count { get; }

		public void Dispose()
		{
			this.allocator.Dispose();
		}
	}
}
