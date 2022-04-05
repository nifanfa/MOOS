_TEXT SEGMENT

PUBLIC _int80h

_int80h PROC
    int 80h
	ret
_int80h ENDP


_TEXT ENDS

END