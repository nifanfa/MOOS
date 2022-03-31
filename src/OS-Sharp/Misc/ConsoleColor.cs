/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/
using OS_Sharp;
using System;

public static unsafe class ConsoleColor
{
    public static uint Black { get { return 0; } }
    public static uint Red { get{ return Framebuffer.VideoMemory != null ? 0xFF0000U : 0x04; } }
    public static uint Green { get { return Framebuffer.VideoMemory != null ? 0x00FF00U : 0x02; } }
    public static uint Blue { get { return Framebuffer.VideoMemory != null ? 0x0000FFU : 0x01; } }
    public static uint White { get { return Framebuffer.VideoMemory != null ? 0xFFFFFFU : 0x0F; } }
}