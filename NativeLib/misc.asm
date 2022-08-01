_TEXT SEGMENT

enable_avx PROC
    push rax
    push rcx
    push rdx
 
    xor rcx, rcx
    xgetbv ;Load XCR0 register
    or eax, 7 ;Set AVX, SSE, X87 bits
    xsetbv ;Save back to XCR0
 
    pop rdx
    pop rcx
    pop rax
    ret
enable_avx ENDP

enable_sse PROC
    mov rcx,200h
    mov rbx,cr4
    or rbx,rcx
    mov cr4,rbx
    fninit
    ret
enable_sse ENDP

Schedule_Next PROC
    mov rdx,61666E6166696Eh
    int 20h
    ret
Schedule_Next ENDP

_TEXT ENDS

END