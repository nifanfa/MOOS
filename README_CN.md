# MOOS  
欢迎来到 *MOOS(to make **M**y **O**wn **O**perating **S**ystem)-项目*
> MOOS 是一个使用微软Native AOT技术编译的C# 64位操作系统.
  
作者QQ: 3244735564  

*请看下面的截图*

## 构建
获取如何构建MOOS的信息, 请阅读 [build wiki page](https://github.com/nifanfa/MOOS/wiki/How-do-you-build-or-compile-MOOS%3F).

### 调试要求
- *VMware Workstation Player - https://www.vmware.com/products/workstation-player.html*
- *Intel Hardware Accelerated Execution Manager (HAXM) - https://github.com/intel/haxm/releases*  
- *Visual studio 2022 - https://visualstudio.microsoft.com/*  
- *QEMU - https://www.qemu.org/download/ - 当然你也可以用 VMware*

注意: 你至少需要8G运存和64位的Windows10或以上才能调试MOOS

**玩得开心!**

## 展示
| 截图(MOOS支持英语和中文，默认是英语) |
| ------ |
| ![image](Screenshot1.png) |
C# 操作系统演示，使用Native AOT(Core RT)编译并使用multiboot引导  

## 项目进度

| 名称 | 实现与否 | 是否在实机上能用 (在 Supermicro X9DRI-LN4F+ 和 Dell Optiplex 390 上测试) | 注意 |
| ----- | ----------- | ----------------------------------------------------------- | ----- |
| 应用程序(.exe) | ✅ | ✅ |
| 错误处理 | ❌ | ❌ | 
| 垃圾回收 | ⚠️ | ❓ | 不安全 |
| 多处理器 | ✅ | ✅ |
| 多线程 | ✅ | ✅ |
| EHCI(USB2.0) | ✅ | ✅ |
| USB 键盘 | ⚠️ | ❓ | 还没做完 |
| USB 鼠标 | ✅ | ❓ |
| USB HUB | ❌ | ❌ |
| PS2 鼠标/键盘(USB 兼容) | ✅ | ✅ |
| 红白机模拟器 | ✅ | ✅ |
| 毁灭战士(doomgeneric) | ✅ | ✅ |
| Intel千兆网卡 | ✅ | ✅ |
| 瑞昱 RTL8139 | ✅ | ❓ |
| ExFAT | ✅ | ✅ | 不支持FAT32/16/12! |
| I/O APIC | ✅ | ✅ |
| Local APIC | ✅ | ✅ |
| SATA | ✅ | ❓ |
| IDE | ✅ | ✅ |
| SMBIOS | ✅ | ✅ |
| ACPI | ✅ | ✅ |
| IPv4 | ✅ | ✅ |
| IPv6 | ❌ | ❌ |
| TCP(WIP) | ⚠️ | ❌ | 不能接受大包 |
| UDP | ✅ | ❓ |
| 局域网 | ✅ | ✅ |
| 广域网 | ✅ | ✅ |
