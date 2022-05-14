NUM_ACTIVED_PROCESSORS     EQU 0x80000
AP_MAIN     EQU 0x80008
STACKS     EQU 0x800016
SHARED_PAGE_TABLE     EQU 0x81000

[BITS 16]
[ORG 0x90000]
Startup:
    cli
    cld
    xor ax,ax
    mov ds, ax
    mov es, ax
    mov ss, ax
    mov eax, cr4
    or eax, 0x20
    mov cr4, eax

    mov ecx, SHARED_PAGE_TABLE
    mov cr3, ecx

    mov ecx, 0xC0000080
    rdmsr
    or eax, 1 << 8
    or eax, 1 << 11
    wrmsr

    mov eax, cr0
    or eax, 0x80000001
    mov cr0, eax

    lgdt [cs:GDT.Pointer]
    jmp dword 0x08:APMain

    cli
    hlt

[BITS 64]
APMain:
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov ss, ax

    mov rbx,STACKS
    add qword [rbx],32768
    mov rsp,[rbx]

    mov rbx,NUM_ACTIVED_PROCESSORS
    inc word [rbx]

    mov rbx,AP_MAIN
    mov rbx,[rbx]
    call rbx

    cli
    hlt
    jmp $

; Global Descriptor Table
GDT:
.Null:
    dq 0x0000000000000000             ; Null Descriptor - should be present.
 
.Code:
    dq 0x00209A0000000000             ; 64-bit code descriptor (exec/read).
    dq 0x0000920000000000             ; 64-bit data descriptor (read/write).
 
ALIGN 4
    dw 0                              ; Padding to make the "address of the GDT" field aligned on a 4-byte boundary
 
.Pointer:
    dw $ - GDT - 1                    ; 16-bit Size (Limit) of GDT.
    dd GDT                            ; 32-bit Base Address of GDT. (CPU will zero extend to 64-bit)
