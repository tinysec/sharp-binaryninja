using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public sealed class Transform : AbstractSafeHandle<Transform>
	{
		internal Transform(IntPtr handle)
			:base(handle , false)
		{
			
		}
		
		internal static Transform MustFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}
			
			return new Transform(handle);
		}
		
		internal static Transform? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}
			
			return new Transform(handle);
		}

		public static Transform? FromName(string name)
		{
			return Transform.FromHandle(
				NativeMethods.BNGetTransformByName(name)
			);
		}
		
		public static Transform[] GetTransformTypes()
		{
			IntPtr arrayPointer = NativeMethods.BNGetTransformTypeList(
				out ulong arrayLength
			);

			return UnsafeUtils.TakeHandleArray(
				arrayPointer ,
				arrayLength ,
				Transform.MustFromHandle ,
				NativeMethods.BNFreeTransformTypeList
			);
		}
		
		public static Transform? RegisterTransformType(
			TransformType type,
			string name,
			string longname,
			string group,
			CustomTransform xform
		)
		{
			return Transform.FromHandle(

				NativeMethods.BNRegisterTransformType(
					type,
					name,
					longname,
					group,
					xform.ToNative()
				)
			);
		}
		
		public static Transform? RegisterTransformType(
			TransformType type,
			uint capabilities,
			string name,
			string longname,
			string group,
			CustomTransform xform
		)
		{
			return Transform.FromHandle(

				NativeMethods.BNRegisterTransformTypeWithCapabilities(
					type,
					capabilities,
					name,
					longname,
					group,
					xform.ToNative()
				)
			);
		}

		public TransformType Type
		{
			get
			{
				return NativeMethods.BNGetTransformType(this.handle);
			}
		}
		
		public uint Capabilities
		{
			get
			{
				return NativeMethods.BNGetTransformCapabilities(this.handle);
			}
		}
		
		public bool SupportsDetection
		{
			get
			{
				return NativeMethods.BNTransformSupportsDetection(this.handle);
			}
		}
		
		public bool SupportsContext
		{
			get
			{
				return NativeMethods.BNTransformSupportsContext(this.handle);
			}
		}
		
		public string Name
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
						NativeMethods.BNGetTransformName(this.handle)
				);
			}
		}
		
		public string LongName
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetTransformLongName(this.handle)
				);
			}
		}
		
		public string Group
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetTransformGroup(this.handle)
				);
			}
		}

		public bool CanDecode(BinaryView input)
		{
			return NativeMethods.BNCanDecode(this.handle, input.DangerousGetHandle());
		}

		public TransformParameterInfo[] Parameters
		{
			get
			{
				IntPtr arrayPointer = NativeMethods.BNGetTransformParameterList(
					this.handle ,
					out ulong arrayLength
				);
				
				return UnsafeUtils.TakeStructArrayEx<BNTransformParameterInfo,TransformParameterInfo> (
					arrayPointer ,
					arrayLength ,
					TransformParameterInfo.FromNative ,
					NativeMethods.BNFreeTransformParameterList
				);
			}
		}

		public byte[] Decode(byte[] input , TransformParameter[] parameters)
		{
			DataBuffer output = new DataBuffer();

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNDecode(
					this.handle ,
					DataBuffer.FromBytes(input).DangerousGetHandle() ,
					output.DangerousGetHandle() ,
					allocator.ConvertToNativeArrayEx<BNTransformParameter, TransformParameter>(
						parameters
					) ,
					(ulong)parameters.Length
				);
			}

			return output.Contents;
		}
		
		public byte[] Encode(byte[] input , TransformParameter[] parameters)
		{
			DataBuffer output = new DataBuffer();

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				bool ok = NativeMethods.BNEncode(
					this.handle ,
					DataBuffer.FromBytes(input).DangerousGetHandle() ,
					output.DangerousGetHandle() ,
					allocator.ConvertToNativeArrayEx<BNTransformParameter, TransformParameter>(
						parameters
					) ,
					(ulong)parameters.Length
				);
			}

			return output.Contents;
		}
		
		public bool DecodeWithContext(TransformContext context , TransformParameter[] parameters)
		{
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				return NativeMethods.BNDecodeWithContext(
					this.handle ,
					context.DangerousGetHandle() ,
					allocator.ConvertToNativeArrayEx<BNTransformParameter, TransformParameter>(
						parameters
					) ,
					(ulong)parameters.Length
				);
			}
		}
	}
	
	
	
}