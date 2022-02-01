grub-mkimage.exe -o grub.img --format=i386-pc-pxe --prefix="(tftp)/grub" pxe tftp http
grub-mkimage.exe -o grubx64.efi --format=x86_64-efi --prefix="(tftp)/grub" efinet tftp http
grub-mkimage.exe -o grubi386.efi --format=i386-efi --prefix="(tftp)/grub" efinet tftp http
rem ./grub-mkimage -d ./grub-core -o bootx64.efi -O x86_64-efi -p "" fat iso9660 part_gpt part_msdos normal boot linux configfile loopback chain efifwsetup efi_gop efi_uga ls search search_label search_fs_uuid search_fs_file gfxterm gfxterm_background gfxterm_menu test all_video loadenv exfat ext2 ntfs btrfs hfsplus udf tftp efinet
pause

