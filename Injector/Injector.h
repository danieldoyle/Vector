
#pragma once

__declspec(dllexport)
LRESULT __stdcall MessageHookProc(int nCode, WPARAM wparam, LPARAM lparam);

using namespace System;

namespace ProcessInjector
{
	public ref class Injector : System::Object
	{
		public:

		static void Inject(System::IntPtr windowHandle, System::String^ assemblyName, System::String^ className, System::String^ methodName);

		static void Log(System::String^ message, bool append);
	};
}