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

MULTIBOOT_HEADER_FLAGS   equ  MULTIBOOT_AOUT_KLUDGE|MULTIBOOT_ALIGN|MULTIBOOT_MEMINFO;|MULTIBOOT_VBE_MODE
CHECKSUM                 equ  -(MULTIBOOT_HEADER_MAGIC + MULTIBOOT_HEADER_FLAGS)

KERNEL_STACK             equ  0x00200000  
;Stack starts at the 2mb address & grows down

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
        ;dd 00
        ;dd 1024
        ;dd 768
        ;dd 32

multiboot_entry:
        mov    esp, KERNEL_STACK       ;Setup the stack
        push   0                       ;Reset EFLAGS
        popf

        push   eax                     ;2nd argument is magic number
        push   ebx                     ;1st argument multiboot info pointer
        jmp   Enter_Long_Mode          

die:
        cli
endloop:
        hlt
        jmp   endloop

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
    mov edi, p4_table
    ; Zero out the 16KiB buffer.
    ; Since we are doing a rep stosd, count should be bytes/4.   
    push di                           ; REP STOSD alters DI.
    
    ; map first P4 entry to P3 table
    mov eax, p3_table
    or eax, 0b11 ; present + writable
    mov [p4_table], eax

    ; map first P3 entry to P2 table
    mov eax, p2_table
    or eax, 0b11 ; present + writable
    mov [p3_table], eax

    ; map each P2 entry to a huge 2MiB page
    mov ecx, 0         ; counter variable

.Map_P2_Table:
    ; map ecx-th P2 entry to a huge page that starts at address 2MiB*ecx
    mov eax, 0x200000  ; 2MiB
    mul ecx            ; start address of ecx-th page
    or eax, 0b10000011 ; present + writable + huge
    mov [p2_table + ecx * 8], eax ; map ecx-th entry

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
 
    jmp 0x0008:_Main             ; Load CS with 64 bit segment and flush the instruction cache
 
 
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

ALIGN 4096
p4_table:
resb 4096
p3_table:
resb 4096
p2_table:
resb 4096

[BITS 64]      
_Main:
    mov ax, 0x0010
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov ss, ax
    
    jmp 0x201000
    times 0x100000-($-$$) db 0
