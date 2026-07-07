using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNCustomTransform 
	{
		// BNTransformParameterInfo* (*getParameters)(void* ctxt, size_t* count);
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate IntPtr GetParametersDelegate(
			IntPtr ctxt,
			IntPtr count
		);
		
		// void (*freeParameters)(BNTransformParameterInfo* params, size_t count);
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate void FreeParametersDelegate(
			IntPtr parameters,
			ulong count
		);
		
		// bool (*decode)(void* ctxt, BNDataBuffer* input, BNDataBuffer* output, BNTransformParameter* params, size_t paramCount);
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate bool DecodeDelegate(
			IntPtr ctxt,
			IntPtr input,
			IntPtr output,
			IntPtr parameters,
			ulong  paramCount
		);
		
		// bool (*encode)(void* ctxt, BNDataBuffer* input, BNDataBuffer* output, BNTransformParameter* params, size_t paramCount);
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate bool EncodeDelegate(
			IntPtr ctxt,
			IntPtr input,
			IntPtr output,
			IntPtr parameters,
			ulong  paramCount
		);
		
		// bool (*decodeWithContext)(void* ctxt, BNTransformContext* context, BNTransformParameter* params, size_t paramCount);
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate bool DecodeWithContextDelegate(
			IntPtr ctxt,
			IntPtr context,
			IntPtr parameters,
			ulong  paramCount
		);
		
		// bool (*canDecode)(void* ctxt, BNBinaryView* input);
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal unsafe delegate bool CanDecodeDelegate(
			IntPtr ctxt,
			IntPtr input
		);
		
		/// <summary>
		/// void* context
		/// </summary>
		internal IntPtr context;
		
		/// <summary>
		/// void** getParameters
		/// </summary>
		internal IntPtr getParameters;
		
		/// <summary>
		/// void** freeParameters
		/// </summary>
		internal IntPtr freeParameters;
		
		/// <summary>
		/// void** decode
		/// </summary>
		internal IntPtr decode;
		
		/// <summary>
		/// void** encode
		/// </summary>
		internal IntPtr encode;
		
		/// <summary>
		/// void** decodeWithContext
		/// </summary>
		internal IntPtr decodeWithContext;
		
		/// <summary>
		/// void** canDecode
		/// </summary>
		internal IntPtr canDecode;
	}

	public class CustomTransform : INativeWrapper<BNCustomTransform>
	{
		// Cached thunk delegates for the ToNative() direction. A function pointer returned by
		// GetFunctionPointerForDelegate stays valid only while its source delegate is alive; the
		// inline method-group delegates (this.DecodeThunk) would otherwise be collectible the moment
		// ToNative() returns, and the next native callback into this registered transform would
		// dereference freed memory (AccessViolation).
		private BNCustomTransform.GetParametersDelegate? m_getParametersThunk = null;

		private BNCustomTransform.FreeParametersDelegate? m_freeParametersThunk = null;

		private BNCustomTransform.DecodeDelegate? m_decodeThunk = null;

		private BNCustomTransform.EncodeDelegate? m_encodeThunk = null;

		private BNCustomTransform.DecodeWithContextDelegate? m_decodeWithContextThunk = null;

		private BNCustomTransform.CanDecodeDelegate? m_canDecodeThunk = null;

		public CustomTransform()
		{

		}

		public Transform? RegisterTransformType(
			TransformType type,
			string name,
			string longname,
			string group
		)
		{
			return Transform.FromHandle(

				NativeMethods.BNRegisterTransformType(
					type,
					name,
					longname,
					group,
					this.ToNative()
				)
			);
		}
		
		public Transform? RegisterTransformTypeWithCapabilities(
			TransformType type,
			uint capabilities,
			string name,
			string longname,
			string group
		)
		{
			return Transform.FromHandle(

				NativeMethods.BNRegisterTransformTypeWithCapabilities(
					type,
					capabilities,
					name,
					longname,
					group,
					this.ToNative()
				)
			);
		}

		public BNCustomTransform ToNative()
		{
			// Build the thunk delegates once and store them in fields so they stay rooted for the
			// lifetime of this transform. The core keeps the function pointers after Register(), so
			// the delegate objects must outlive every native callback.
			BNCustomTransform.GetParametersDelegate getParametersThunk =
				new BNCustomTransform.GetParametersDelegate(this.GetParametersThunk);

			BNCustomTransform.FreeParametersDelegate freeParametersThunk =
				new BNCustomTransform.FreeParametersDelegate(this.FreeParametersThunk);

			BNCustomTransform.DecodeDelegate decodeThunk =
				new BNCustomTransform.DecodeDelegate(this.DecodeThunk);

			BNCustomTransform.EncodeDelegate encodeThunk =
				new BNCustomTransform.EncodeDelegate(this.EncodeThunk);

			BNCustomTransform.DecodeWithContextDelegate decodeWithContextThunk =
				new BNCustomTransform.DecodeWithContextDelegate(this.DecodeWithContextThunk);

			BNCustomTransform.CanDecodeDelegate canDecodeThunk =
				new BNCustomTransform.CanDecodeDelegate(this.CanDecodeThunk);

			this.m_getParametersThunk = getParametersThunk;
			this.m_freeParametersThunk = freeParametersThunk;
			this.m_decodeThunk = decodeThunk;
			this.m_encodeThunk = encodeThunk;
			this.m_decodeWithContextThunk = decodeWithContextThunk;
			this.m_canDecodeThunk = canDecodeThunk;

			return new BNCustomTransform()
			{
				context = IntPtr.Zero,
				getParameters = Marshal.GetFunctionPointerForDelegate(getParametersThunk),
				freeParameters = Marshal.GetFunctionPointerForDelegate(freeParametersThunk),
				decode = Marshal.GetFunctionPointerForDelegate(decodeThunk),
				encode = Marshal.GetFunctionPointerForDelegate(encodeThunk),
				decodeWithContext = Marshal.GetFunctionPointerForDelegate(decodeWithContextThunk),
				canDecode = Marshal.GetFunctionPointerForDelegate(canDecodeThunk),
			};
		}
		
		
		// BNTransformParameterInfo* (*getParameters)(void* ctxt, size_t* count);
		private IntPtr GetParametersThunk(
			IntPtr ctxt ,
			IntPtr count
		)
		{
			TransformParameterInfo[] parameters = this.GetParameters();

			if (parameters.Length == 0)
			{
				return IntPtr.Zero;
			}

			BNTransformParameterInfo[] natives = TransformParameterInfo.UnsafeToNativeArray(
				parameters
			);
			
			return UnsafeUtils.AllocStructArray<BNTransformParameterInfo>(natives);
		}

		// void (*freeParameters)(BNTransformParameterInfo* params, size_t count);
		private void FreeParametersThunk(
			IntPtr parameters ,
			ulong parametersCount
		)
		{
			if (0 == parameters)
			{
				return;
			}
			
			if (0 == parametersCount)
			{
				return;
			}

			for (ulong i = 0; i < parametersCount; i++)
			{
				int offset = checked((int)(i * (ulong)Marshal.SizeOf<BNTransformParameterInfo>()));
					
				IntPtr addressOfElement = IntPtr.Add(parameters, offset);
					
				BNTransformParameterInfo? parameter = Marshal.PtrToStructure<BNTransformParameterInfo>(
					addressOfElement
				);

				if (null != parameter)
				{
					BNTransformParameterInfo info = parameter.Value;
					
					if (IntPtr.Zero != info.name)
					{
						Marshal.FreeHGlobal(info.name);
					}
					
					if (IntPtr.Zero != info.longName)
					{
						Marshal.FreeHGlobal(info.longName);
					}
				}
			}
			
			Marshal.FreeHGlobal(parameters);
		}

		// bool (*decode)(void* ctxt, BNDataBuffer* input, BNDataBuffer* output, BNTransformParameter* params, size_t paramCount);
		private bool DecodeThunk(
			IntPtr ctxt ,
			IntPtr input ,
			IntPtr output ,
			IntPtr rawParameters ,
			ulong paramCount
		)
		{
			TransformParameter[] parameters = UnsafeUtils.ReadStructArray<BNTransformParameter,TransformParameter>(
				rawParameters ,
				paramCount,
				TransformParameter.FromNative
			);
			
			return this.Decode(
				DataBuffer.MustBorrowHandle(input) ,
				DataBuffer.MustBorrowHandle(output) ,
				parameters
			);
		}

		// bool (*encode)(void* ctxt, BNDataBuffer* input, BNDataBuffer* output, BNTransformParameter* params, size_t paramCount);
		private bool EncodeThunk(
			IntPtr ctxt ,
			IntPtr input ,
			IntPtr output ,
			IntPtr rawParameters ,
			ulong paramCount
		)
		{
			TransformParameter[] parameters = UnsafeUtils.ReadStructArray<BNTransformParameter,TransformParameter>(
				rawParameters ,
				paramCount,
				TransformParameter.FromNative
			);
			
			return this.Encode(
				DataBuffer.MustBorrowHandle(input) ,
				DataBuffer.MustBorrowHandle(output) ,
				parameters
			);
		}

		// bool (*decodeWithContext)(void* ctxt, BNTransformContext* context, BNTransformParameter* params, size_t paramCount);
		private bool DecodeWithContextThunk(
			IntPtr ctxt ,
			IntPtr context ,
			IntPtr rawParameters ,
			ulong paramCount
		)
		{
			TransformParameter[] parameters = UnsafeUtils.ReadStructArray<BNTransformParameter,TransformParameter>(
				rawParameters ,
				paramCount,
				TransformParameter.FromNative
			);
			
			return this.DecodeWithContext(
				TransformContext.MustBorrowHandle(context),
				parameters
			);
		}

		// bool (*canDecode)(void* ctxt, BNBinaryView* input);
		private bool CanDecodeThunk(
			IntPtr ctxt ,
			IntPtr input
		)
		{
			return this.CanDecode(
				BinaryView.MustBorrowHandle(input)
			);
		}

		#region methods

		public virtual TransformParameterInfo[] GetParameters()
		{
			return Array.Empty<TransformParameterInfo>();
		}
		
		public virtual bool Decode(
			DataBuffer input ,
			DataBuffer output ,
			TransformParameter[] parameters
		)
		{
			return false;
		}

		public virtual bool Encode(
			DataBuffer input ,
			DataBuffer output ,
			TransformParameter[] parameters
		)
		{
			return false;
		}
		
		public virtual bool DecodeWithContext(TransformContext context , TransformParameter[] parameters)
		{
			return false;
		}
		
		public virtual bool CanDecode(BinaryView input)
		{
			return false;
		}

		#endregion methods
    }
}