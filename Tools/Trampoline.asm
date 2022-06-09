NUM_ACTIVED_PROCESSORS     EQU 0x50000
AP_MAIN     EQU 0x50008
STACKS     EQU 0x50016
SHARED_GDT     EQU 0x50024
SHARED_IDT     EQU 0x50032
SHARED_PAGE_TABLE     EQU 0x51000

[BITS 16]
[ORG 0x60000]
Startup:
    cli

    lidt [IDT] 

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
    lock
    add qword [rbx],1048576
    mov rsp,[rbx]
    mov rbp,rsp

    mov rbx,NUM_ACTIVED_PROCESSORS
    mov rcx,[rbx]
    lock
    inc word [rbx]

    mov rax,SHARED_GDT
    mov rax,[rax]
    lgdt [rax]

    mov rax,SHARED_IDT
    mov rax,[rax]
    lidt [rax]

    sti

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

ALIGN 4
IDT:
    .Length       dw 0
    .Base         dd 0

times 512 - ($-$$) db 0