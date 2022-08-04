[BITS 32]
[global _start]
[ORG 0x100000]
;If using '-f bin' we need to specify the
;origin point for our code with ORG directive
;multiboot loaders load us at physical 
;address 0x100000

MULTIBOOT_AOUT_KLUDGE    equ  1 << 16
;FLAGS[16] indicates to GRUB we are not
;an ELF executable and the fields
;header address, load address, load end address;
;bss end address and entry address will be available
;in Multiboot header
MULTIBOOT_ALIGN          equ  1<<0   
; align loaded modules on page boundaries
MULTIBOOT_MEMINFO        equ  1<<1   
; provide memory map

MULTIBOOT_VBE_MODE equ 1<<2

MULTIBOOT_HEADER_MAGIC   equ  0x1BADB002
;magic number GRUB searches for in the first 8k
;of the kernel file GRUB is told to load

MULTIBOOT_HEADER_FLAGS   equ  MULTIBOOT_AOUT_KLUDGE|MULTIBOOT_ALIGN|MULTIBOOT_MEMINFO|MULTIBOOT_VBE_MODE
CHECKSUM                 equ  -(MULTIBOOT_HEADER_MAGIC + MULTIBOOT_HEADER_FLAGS)

_start:
        xor    eax, eax                ;Clear eax and ebx in the event
        xor    ebx, ebx                ;we are not loaded by GRUB.
        jmp    multiboot_entry         ;Jump over the multiboot header
        align  4                       ;Multiboot header must be 32
                                       ;bits aligned to avoid error 13
multiboot_header:
        dd   MULTIBOOT_HEADER_MAGIC    ;magic number
        dd   MULTIBOOT_HEADER_FLAGS    ;flags
        dd   CHECKSUM                  ;checksum
        dd   multiboot_header          ;header address
        dd   _start                    ;load address of code entry point
                                       ;in our case _start
        dd   00                        ;load end address : not necessary
        dd   00                        ;bss end address : not necessary
        dd   multiboot_entry           ;entry address GRUB will start at

        ; Uncomment this and "|MULTIBOOT_VBE_MODE" in MULTIBOOT_HEADER_FLAGS to enable VBE
        dd 00
        ; Safe resolution
        dd 1024
        dd 768
        dd 32

multiboot_entry:
        mov    esp, STACKTOP       ;Setup the stack
        push   0                       ;Reset EFLAGS
        popf

        push   eax                     ;2nd argument is magic number
        push   ebx                     ;1st argument multiboot info pointer
        mov   [multiboot_pointer],ebx
        jmp   Enter_Long_Mode          
        cli
        hlt
        jmp $

multiboot_pointer:
        dq 0

ALIGN 4
IDT:
    .Length       dw 0
    .Base         dd 0
 
; Function to switch directly to long mode from real mode.
; Identity maps the first 1GiB.
; Uses Intel syntax.
 
; es:edi    Should point to a valid page-aligned 16KiB buffer, for the PML4, PDPT, PD and a PT.
; ss:esp    Should point to memory that can be used as a small (1 uint32_t) stack

Enter_Long_Mode:
    mov edi, P4_TABLE
    ; Zero out the 16KiB buffer.
    ; Since we are doing a rep stosd, count should be bytes/4.   
    push di                           ; REP STOSD alters DI.
    
    ; map first P4 entry to P3 table
    mov eax, P3_TABLE
    or eax, 0b11 ; present + writable
    mov [P4_TABLE], eax

    ; map first P3 entry to P2 table
    mov eax, P2_TABLE
    or eax, 0b11 ; present + writable
    mov [P3_TABLE], eax

    ; map each P2 entry to a huge 2MiB page
    mov ecx, 0         ; counter variable

.Map_P2_Table:
    ; map ecx-th P2 entry to a huge page that starts at address 2MiB*ecx
    mov eax, 0x200000  ; 2MiB
    mul ecx            ; start address of ecx-th page
    or eax, 0b10000011 ; present + writable + huge
    mov [P2_TABLE + ecx * 8], eax ; map ecx-th entry

    inc ecx            ; increase counter
    cmp ecx, 512       ; if counter == 512, the whole P2 table is mapped
    jne .Map_P2_Table  ; else map the next entry
    ;1024MB of memory should be mapped now
 
    pop di                            ; Restore DI.
 
    ; Disable IRQs
    mov al, 0xFF                      ; Out 0xFF to 0xA1 and 0x21 to disable all IRQs.
    out 0xA1, al
    out 0x21, al
    cli
 
    nop
    nop
 
    lidt [IDT]                        ; Load a zero length IDT so that any NMI causes a triple fault.
 
    ; Enter long mode.
    mov eax, 10100000b                ; Set the PAE and PGE bit.
    mov cr4, eax
 
    mov edx, edi                      ; Point CR3 at the PML4.
    mov cr3, edx
 
    mov ecx, 0xC0000080               ; Read from the EFER MSR. 
    rdmsr    
 
    or eax, 0x00000100                ; Set the LME bit.
    wrmsr
 
    mov ebx, cr0                      ; Activate long mode -
    or ebx,0x80000001                 ; - by enabling paging and protection simultaneously.
    mov cr0, ebx                    
 
    lgdt [GDT.Pointer]                ; Load GDT.Pointer defined below.

    sti
 
    jmp 0x0008:Main             ; Load CS with 64 bit segment and flush the instruction cache
 
 
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

struc DOSHeader
        .e_magic: resb 2
        .e_cblp: resb 2
        .e_cp: resb 2
        .e_crlc: resb 2
        .e_cparhdr: resb 2
        .e_minalloc: resb 2
        .e_maxalloc: resb 2
        .e_ss: resb 2
        .e_sp: resb 2
        .e_csum: resb 2
        .e_ip: resb 2
        .e_cs: resb 2
        .e_lfarlc: resb 2
        .e_ovno: resb 2
        .e_res1: resb 8
        .e_oemid: resb 2
        .e_oeminfo: resb 2
        .e_res2: resb 20
        .e_lfanew: resb 4
endstruc

struc NTHeader
        .Signature: resb 4
        .Machine: resb 2
        .NumberOfSections: resb 2
        .TimeDateStamp: resb 4
        .PointerToSymbolTable: resb 4
        .NumberOfSymbols: resb 4
        .SizeOfOptionalHeader: resb 2
        .Characteristics: resb 2
        .Magic: resb 2
        .MajorLinkerVersion: resb 1
        .MinorLinkerVersion: resb 1
        .SizeOfCode: resb 4
        .SizeOfInitializedData: resb 4
        .SizeOfUninitializedData: resb 4
        .AddressOfEntryPoint: resb 4
        .BaseOfCode: resb 4
        .ImageBase: resb 8
        .SectionAlignment: resb 4
        .FileAlignment: resb 4
        .MajorOperatingSystemVersion: resb 2
        .MinorOperatingSystemVersion: resb 2
        .MajorImageVersion: resb 2
        .MinorImageVersion: resb 2
        .MajorSubsystemVersion: resb 2
        .MinorSubsystemVersion: resb 2
        .Win32VersionValue: resb 4
        .SizeOfImage: resb 4
        .SizeOfHeaders: resb 4
        .CheckSum: resb 4
        .Subsystem: resb 2
        .DllCharacteristics: resb 2
        .SizeOfStackReserve: resb 8
        .SizeOfStackCommit: resb 8
        .SizeOfHeapReserve: resb 8
        .SizeOfHeapCommit: resb 8
        .LoaderFlags: resb 4
        .NumberOfRvaAndSizes: resb 4
        .Tables: resb 128
endstruc

struc SectionHeader
        .Name: resb 8
        .PhysicalAddress_VirtualSize: resb 4
        .VirtualAddress: resb 4
        .SizeOfRawData: resb 4
        .PointerToRawData: resb 4
        .PointerToRelocations: resb 4
        .PointerToLineNumbers: resb 4
        .NumberOfRelocations: resb 2
        .NumberOfLineNumbers: resb 2
        .Characteristics: resb 4
endstruc

LOAD_IMAGE_PARAMS_STACK_SIZE   equ  64

[BITS 64]      
Main:
    mov ax, 0x0010
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov ss, ax

    mov rsp,STACKTOP
    mov rbp,rsp

    sub rsp,LOAD_IMAGE_PARAMS_STACK_SIZE
    
    xor rbx,rbx
    mov ebx,[EXE+DOSHeader.e_lfanew]
    lea ebx,[ebx+EXE+NTHeader.NumberOfSections]
    xor rcx,rcx
    mov cx,[rbx]
    mov [rsp+16],rcx
    
    xor rbx,rbx
    mov ebx,[EXE+DOSHeader.e_lfanew]
    lea ebx,[ebx+EXE+NTHeader.ImageBase]
    mov rbx,[rbx]
    mov [rsp+24],rbx

    xor rbx,rbx
    mov ebx,[EXE+DOSHeader.e_lfanew]
    lea ebx,[ebx+EXE+NTHeader.AddressOfEntryPoint]
    mov ebx,[ebx]
    mov rax,[rsp+24]
    add rbx,rax
    mov [rsp+8],rbx

    xor rbx,rbx
    mov ebx,[EXE+DOSHeader.e_lfanew]
    lea ebx,[ebx+EXE+NTHeader.SizeOfImage]
    mov ebx,[ebx]
    mov [rsp+48],rbx

    mov rdi,[rsp+24]
    mov rcx,[rsp+48]
    mov rax,0
    rep stosb

    xor rbx,rbx
    mov ebx,[EXE+DOSHeader.e_lfanew]
    lea ebx,[ebx+EXE+NTHeader_size]
    mov r15,0
LoadSection:
    xor rsi,rsi
    xor rdi,rdi
    xor rcx,rcx
    xor r13,r13
    lea esi,[ebx+SectionHeader.PointerToRawData]
    mov esi,[esi]
    add rsi,EXE
    lea ecx,[ebx+SectionHeader.SizeOfRawData]
    mov ecx,[ecx]
    lea edi,[ebx+SectionHeader.VirtualAddress]
    mov edi,[edi]
    mov rax,[rsp+24]
    add rdi,rax
    lea r13d,[ebx+SectionHeader.Name]
    mov r13,[r13]
    mov qword r14,0x73656C75646F6D2E
    cmp qword r13,r14
    jne Skip
    mov qword [rsp+32],rdi
Skip:
    rep movsb
    inc r15
    add ebx,SectionHeader_size
    mov r14,[rsp+16]
    cmp r15,r14
    jne LoadSection

    mov rcx,[multiboot_pointer]
    mov rdx,[rsp+32]
    mov rax,[rsp+8]
    mov r8,Trampoline
    add rsp,LOAD_IMAGE_PARAMS_STACK_SIZE
    call rax
    cli
    hlt
    jmp $

align 4096
Trampoline:
incbin 'trampoline.o'

align 4096

STACKBOTTOM:
resb 32768 
STACKTOP:

P4_TABLE:
resb 4096
P3_TABLE:
resb 4096
P2_TABLE:
resb 4096

align 4096
EXE:
    