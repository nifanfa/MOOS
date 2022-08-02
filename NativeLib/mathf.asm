_TEXT SEGMENT

_fabs PROC
	fld DWORD PTR [rcx] ; ST(0) ( a )
	fabs                ; Computes absolute value of a number, removing the sign
	fst DWORD PTR [rcx] ; ST(0) ( a )
	ret                 ; return
_fabs ENDP

_fyl2x PROC
	fld DWORD PTR [rcx] ; ST(0) ( x )
	fld DWORD PTR [rdx] ; ST(1) ( y )
	fyl2x               ; Replace ST(1) with 'ST(1) * log2 ST(0)' and pops the register stack.
	fst DWORD PTR [rdx] ; ST(1) ( y )
	ret                 ; return
_fyl2x ENDP

_fsqrt PROC
	fld DWORD PTR [rcx] ; ST(0) ( x )
	fsqrt               ; Computes square root of ST(0) and stores the result in ST(0).
	fst DWORD PTR [rcx] ; ST(0) ( x )
	ret                 ; return
_fsqrt ENDP

_fcos PROC
	fld DWORD PTR [rcx] ; ST(0) ( x )
	fcos                ; Replace ST(0) with its approximate cosine.
	fst DWORD PTR [rcx] ; ST(0) ( x )
	ret                 ; return
_fcos ENDP

_fsin PROC
	fld DWORD PTR [rcx] ; ST(0) ( x )
	fsin                ; Replace ST(0) with the approximate of its sine.
	fst DWORD PTR [rcx] ; ST(0) ( x )
	ret                 ; return
_fsin ENDP

_frndint PROC
	fld DWORD PTR [rcx] ; ST(0) ( x )
	frndint             ; Round ST(0) to an integer.
	fst DWORD PTR [rcx] ; ST(0) ( x )
	ret                 ; return
_frndint ENDP

_TEXT ENDS

END