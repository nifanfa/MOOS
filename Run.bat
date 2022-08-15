@echo off
if "%2" equ "qemu" (
	if not exist "C:\Program Files\Intel\HAXM\IntelHaxm.sys" (
		echo "Please install Intel Hardware Accelerated Execution Manager (HAXM) in order to speed up QEMU https://github.com/intel/haxm/releases"
	)
	if not exist "C:\Program Files\qemu\qemu-system-x86_64.exe" (
		echo "Please install QEMU in order to debug MOOS!(do not modify the path) https://www.qemu.org/download/#windows"
	)
	"C:\Program Files\qemu\qemu-system-x86_64.exe" -accel hax -m 1024 -smp 2 -k en-gb -boot d -cdrom "bin\debug\net6.0\win-x64\native\%1.iso" -d guest_errors -serial stdio -device AC97 -rtc base=localtime
) else if "%2" equ "vmware" (
	if exist "C:\Program Files (x86)\VMware\VMware Workstation\vmplayer.exe" (
		"C:\Program Files (x86)\VMware\VMware Workstation\vmplayer.exe" "Tools\VMWare\MOOS\MOOS.vmx"
	) else if exist "C:\Program Files (x86)\VMware\VMware Player\vmplayer.exe" (
		"C:\Program Files (x86)\VMware\VMware Player\vmplayer.exe" "Tools\VMWare\MOOS\MOOS.vmx"
	) else (
		echo "Please install VMWare Player in order to Run MOOS!"
	)
) else (
	echo Invalid parameters, do not running this batch file manually! 
)
