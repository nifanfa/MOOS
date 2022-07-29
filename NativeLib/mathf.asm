_TEXT SEGMENT

;https://en.m.wikipedia.org/wiki/X86_instruction_listings

_fsin PROC
    fld DWORD PTR [rcx]
    fsin
    fst DWORD PTR [rcx]
    ret
_fsin ENDP

_fcos PROC
    fld DWORD PTR [rcx]
    fcos
    fst DWORD PTR [rcx]
    ret
_fcos ENDP

_fsqrt PROC
    fld DWORD PTR [rcx]
    fsqrt
    fst DWORD PTR [rcx]
    ret
_fsqrt ENDP

_frndint PROC
    fld DWORD PTR [rcx]
    frndint
    fst DWORD PTR [rcx]
    ret
_frndint ENDP

_TEXT ENDS

END