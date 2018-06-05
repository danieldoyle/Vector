using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Vector
{	
	public static class ProcessExtensions
	{
		[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

		public static bool Is64BitProcess(this Process process)
		{
			if (!Environment.Is64BitOperatingSystem)
				return false;

			bool isWow64Process;
			if (!IsWow64Process(process.Handle, out isWow64Process))
				throw new Win32Exception(Marshal.GetLastWin32Error());

			return !isWow64Process;
		}
	}

	public static class InjectionSearch
	{
		public static bool CheckInjected(Process process, InjectionPayload payload)
		{
			Scan(process, out List<VMChunkInfo> chunkInfos, out List<VMRegionInfo> mappingInfos, out UIntPtr addressLimit);
			if (chunkInfos != null)
			{
				foreach (var chunk in chunkInfos)
				{
					if (chunk.regionName != null && chunk.regionName.Contains(payload.Assembly.ManifestModule.Name))
						return true;
				}
			}
			return false;
		}

		#region Helper structures
		private class VMRegionInfo
		{
			public UIntPtr regionStartAddress;
			public UInt64 regionSize;
			public string regionName;

			public bool ContainsAddress(UIntPtr address)
			{
				return
					((UInt64)regionStartAddress <= (UInt64)address) &&
					((UInt64)regionEndAddress >= (UInt64)address);
			}

			public UIntPtr regionEndAddress
			{
				get { return UIntPtr.Add(regionStartAddress, (int)(regionSize - 1)); }
			}
		}

		private class VMChunkInfo : VMRegionInfo
		{
			public PageState state;
			public PageType type;
		}

		private enum PageState
		{
			Committed = 0x1000,
			Free = 0x10000,
			Reserved = 0x2000
		}

		private enum PageType
		{
			Image = 0x01000000,     // Mapped into the view of an image section
			Mapped = 0x00040000,    // Mapped into the view of a section
			Private = 0x00020000    // Private (not shared with other processes)
		}
		#endregion

		#region P/Invoke
		[StructLayout(LayoutKind.Sequential)]
		private struct MEMORY_BASIC_INFORMATION
		{
			public UIntPtr BaseAddress;
			public UIntPtr AllocationBase;
			public uint AllocationProtect;
			public UIntPtr RegionSize;
			public uint State;
			public uint Protect;
			public uint Type;
		}

		[DllImport("kernel32.dll")]
		private static extern int VirtualQueryEx(IntPtr hProcess, UIntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

		
		[DllImport("psapi.dll", SetLastError = true)]
		private static extern uint GetMappedFileName(IntPtr m_hProcess, UIntPtr lpv, StringBuilder
				lpFilename, uint nSize);

		private static string GetMappedFileName(IntPtr process, UIntPtr address)
		{
			StringBuilder fn = new StringBuilder(250);
			if (GetMappedFileName(process, address, fn, 250) > 0)
				return fn.ToString();
			else
				return string.Empty;
		}

		

		#endregion

		#region Helper methods
		private static void Scan(Process process, out List<VMChunkInfo> chunkInfos, out List<VMRegionInfo> mappingInfos, out UIntPtr addressLimit)
		{
			IntPtr processHandle = process.Handle;
			MEMORY_BASIC_INFORMATION m = new MEMORY_BASIC_INFORMATION();

			chunkInfos = new List<VMChunkInfo>();

			const UInt64 GB = 1024 * 1024 * 1024;
			UInt64 maxRegionSize = (UInt64)2 * GB;

			UIntPtr memoryLimit;
			if (process.Is64BitProcess())
				memoryLimit = (UIntPtr)((UInt64)6 * GB);
			else
				memoryLimit = UIntPtr.Subtract(UIntPtr.Zero, 1);

			addressLimit = memoryLimit;

			// Use UIntPtr so that we can cope with addresses above 2GB in a /3GB or "4GT" environment, or 64-bit Windows
			UIntPtr address = (UIntPtr)0;
			while ((UInt64)address < (UInt64)memoryLimit)
			{
				int result = VirtualQueryEx(processHandle, address, out m, (uint)Marshal.SizeOf(m));
				if (0 == result || (UInt64)m.RegionSize > maxRegionSize)
				{
					// Record the 'end' of the address scale
					// (Expect 2GB in the case of a Win32 process running under 32-bit Windows, but may be
					// extended to up to 3GB if the OS is configured for "4 GT tuning" with the /3GB switch
					// Expect 4GB in the case of a Win32 process running under 64-bit Windows)
					addressLimit = address;
					break;
				}

				VMChunkInfo chunk = new VMChunkInfo();
				chunk.regionStartAddress = (UIntPtr)(UInt64)m.BaseAddress;
				chunk.regionSize = (UInt64)m.RegionSize;
				chunk.type = (PageType)m.Type;
				chunk.state = (PageState)m.State;

				if ((chunk.type == PageType.Image) || (chunk.type == PageType.Mapped))
				{
					// .Net 4 maps assemblies into memory using the memory-mapped file mechanism;
					// they don't show up in Process.Modules list
					string fileName = GetMappedFileName(processHandle, chunk.regionStartAddress);
					if (fileName.Length > 0)
					{
						fileName = Path.GetFileName(fileName);
						chunk.regionName = fileName;
					}
				}

				chunkInfos.Add(chunk);

				UIntPtr oldAddress = address;
				// It's maddening, but UIntPtr.Add can't cope with 64-bit offsets under Win64!
				address = UIntPtr.Add(address, (int)m.RegionSize);
				if ((UInt64)address <= (UInt64)oldAddress)
				{
					addressLimit = oldAddress;
					break;
				}
			};

			mappingInfos = new List<VMRegionInfo>();
			try
			{
				foreach (ProcessModule module in process.Modules)
				{
					VMRegionInfo mappingInfo = new VMRegionInfo();

					mappingInfo.regionStartAddress = (UIntPtr)(UInt64)module.BaseAddress;
					mappingInfo.regionSize = (UInt64)module.ModuleMemorySize;
					mappingInfo.regionName = Path.GetFileName(module.FileName);

					mappingInfos.Add(mappingInfo);
				}
			}
			catch { }

			// Sort by address
			mappingInfos.Sort(delegate (VMRegionInfo map1, VMRegionInfo map2)
			{
				return Comparer<UInt64>.Default.Compare((UInt64)map1.regionStartAddress, (UInt64)map2.regionStartAddress);
			});
		}
		#endregion
	}
}
