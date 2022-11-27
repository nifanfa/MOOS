@echo off
if "%1" equ "" (
	goto error
)
if not exist "bin\debug\win-x64\native\%1.mue" (
	echo "Please select the project that you want to Run on Visual Studio Solution Explorer and then press Ctrl+B to build the project first!"
	echo "Make sure the project that you build is what you selected on Visual Studio Dropdown Menu!"
	pause
	exit
)

cd Tools
del /q grub2\boot\ramdisk.tar
7-Zip\7z.exe a grub2\boot\ramdisk.tar ..\Ramdisk\*
nasm.exe -fbin Trampoline.asm -o trampoline.o
nasm.exe -fbin EntryPoint.asm -o loader.o
copy /b loader.o+..\bin\debug\win-x64\native\%1.mue grub2\boot\kernel.bin
del /q trampoline.o
del /q loader.o
mkisofs.exe -relaxed-filenames -J -R -o ..\bin\debug\win-x64\native\%1.iso -b boot/grub/i386-pc/eltorito.img -no-emul-boot -boot-load-size 4 -boot-info-table grub2
cd..

if "%2" equ "qemu" (
	if not exist "C:\Program Files\Intel\HAXM\IntelHaxm.sys" (
		echo "Please install Intel Hardware Accelerated Execution Manager (HAXM) in order to speed up QEMU https://github.com/intel/haxm/releases"
		pause
		exit
	)
	if not exist "C:\Program Files\qemu\qemu-system-x86_64.exe" (
		echo "Please install QEMU in order to debug MOOS!(do not modify the path) https://www.qemu.org/download/#windows"
		pause
		exit
	)
	"C:\Program Files\qemu\qemu-system-x86_64.exe" -accel hax -m 1024 -smp 2 -k en-gb -boot d -cdrom "bin\debug\win-x64\native\%1.iso" -d guest_errors -serial stdio -device AC97 -rtc base=localtime
) else if "%2" equ "vmware" (
	if exist "C:\Program Files (x86)\VMware\VMware Workstation\vmplayer.exe" (
		"C:\Program Files (x86)\VMware\VMware Workstation\vmplayer.exe" "Tools\VMWare\MOOS\MOOS.vmx"
	) else if exist "C:\Program Files (x86)\VMware\VMware Player\vmplayer.exe" (
		"C:\Program Files (x86)\VMware\VMware Player\vmplayer.exe" "Tools\VMWare\MOOS\MOOS.vmx"
	) else (
		echo "Please install VMWare Player in order to Run MOOS!"
		pause
		exit
	)
) else (
	:error
	echo "Invalid parameters, do not run this batch file manually!" 
	pause
	exit
)
