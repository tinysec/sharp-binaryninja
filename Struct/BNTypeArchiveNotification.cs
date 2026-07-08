using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNTypeArchiveNotification 
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;
		
		/// <summary>
		/// void** typeAdded
		/// </summary>
		public IntPtr typeAdded;
		
		/// <summary>
		/// void** typeUpdated
		/// </summary>
		public IntPtr typeUpdated;
		
		/// <summary>
		/// void** typeRenamed
		/// </summary>
		public IntPtr typeRenamed;
		
		/// <summary>
		/// void** typeDeleted
		/// </summary>
		public IntPtr typeDeleted;
	}

    /// <summary>
    /// Base class for receiving event notifications from a <see cref="TypeArchive"/>, mirroring
    /// Python <c>TypeArchiveNotification</c> (typearchive.py:732). Subclass and override the
    /// virtual methods for the events of interest, then register an instance via
    /// <see cref="TypeArchive.RegisterNotification"/>.
    /// </summary>
    public abstract class TypeArchiveNotification
    {
        /// <summary>
        /// Called when a type is added to the archive, mirroring Python <c>type_added</c>
        /// (typearchive.py:741).
        /// </summary>
        /// <param name="archive">The archive the type was added to.</param>
        /// <param name="id">The id of the added type.</param>
        /// <param name="definition">The definition of the added type.</param>
        public virtual void OnTypeAdded(TypeArchive archive, string id, Type? definition)
        {
        }

        /// <summary>
        /// Called when a type in the archive is updated to a new definition, mirroring Python
        /// <c>type_updated</c> (typearchive.py:748).
        /// </summary>
        /// <param name="archive">The archive the type belongs to.</param>
        /// <param name="id">The id of the updated type.</param>
        /// <param name="oldDefinition">The previous definition.</param>
        /// <param name="newDefinition">The current definition.</param>
        public virtual void OnTypeUpdated(
            TypeArchive archive, string id, Type? oldDefinition, Type? newDefinition)
        {
        }

        /// <summary>
        /// Called when a type in the archive is renamed, mirroring Python <c>type_renamed</c>
        /// (typearchive.py:755).
        /// </summary>
        /// <param name="archive">The archive the type belongs to.</param>
        /// <param name="id">The id of the renamed type.</param>
        /// <param name="oldName">The previous name.</param>
        /// <param name="newName">The current name.</param>
        public virtual void OnTypeRenamed(
            TypeArchive archive, string id, QualifiedName oldName, QualifiedName newName)
        {
        }

        /// <summary>
        /// Called when a type is deleted from the archive, mirroring Python <c>type_deleted</c>
        /// (typearchive.py:762).
        /// </summary>
        /// <param name="archive">The archive the type was deleted from.</param>
        /// <param name="id">The id of the deleted type.</param>
        /// <param name="definition">The definition of the deleted type.</param>
        public virtual void OnTypeDeleted(TypeArchive archive, string id, Type? definition)
        {
        }
    }
}