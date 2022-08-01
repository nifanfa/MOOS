_TEXT SEGMENT

pushaq MACRO
	push r15
	push r14
	push r13
	push r12
	push r11
	push r10
	push r9
	push r8
	push rdi
	push rsi
	push rbx
	push rdx
	push rcx
	push rax
ENDM

popaq MACRO
	pop rax
	pop rcx
	pop rdx
	pop rbx
	pop rsi
	pop rdi
	pop r8
	pop r9
	pop r10
	pop r11
	pop r12
	pop r13
	pop r14
	pop r15
ENDM

EXTERN exception_handler: PROC

isr MACRO num
	PUBLIC isr&num

	isr&num PROC
		cli	;prevent ThreadPool from switching thread
		push num ;push errorCode
		pushaq
		mov rcx, num
		mov rdx, rsp
		call exception_handler
		popaq
		add rsp,8 ;clean errorcode
		cli
		hlt
		jmp $
	isr&num ENDP
ENDM

EXTERN irq_handler: PROC

irq MACRO num
	PUBLIC irq&num

	irq&num PROC
		push rbp
		mov rbp, rsp
		pushaq
		mov rcx,20h
		add rcx, num
		mov rdx, rsp
		call irq_handler
		popaq
		pop rbp
		iretq
	irq&num ENDP
ENDM

isr 0
isr 1
isr 2
isr 3
isr 4
isr 5
isr 6
isr 7
isr 8
isr 9
isr 10
isr 11
isr 12
isr 13
isr 14
isr 15
isr 16
isr 17
isr 18
isr 19
isr 20
isr 21
isr 22
isr 23
isr 24
isr 25
isr 26
isr 27
isr 28
isr 29
isr 30
isr 31
isr 128

irq 0
irq 1
irq 2
irq 3
irq 4
irq 5
irq 6
irq 7
irq 8
irq 9
irq 10
irq 11
irq 12
irq 13
irq 14
irq 15
irq 16
irq 17
irq 18
irq 19
irq 20
irq 21
irq 22
irq 23
irq 24
irq 25
irq 26
irq 27
irq 28
irq 29
irq 30
irq 31
irq 32
irq 33
irq 34
irq 35
irq 36
irq 37
irq 38
irq 39
irq 40
irq 41
irq 42
irq 43
irq 44
irq 45
irq 46
irq 47
irq 48
irq 49
irq 50
irq 51
irq 52
irq 53
irq 54
irq 55
irq 56
irq 57
irq 58
irq 59
irq 60
irq 61
irq 62
irq 63
irq 64
irq 65
irq 66
irq 67
irq 68
irq 69
irq 70
irq 71
irq 72
irq 73
irq 74
irq 75
irq 76
irq 77
irq 78
irq 79
irq 80
irq 81
irq 82
irq 83
irq 84
irq 85
irq 86
irq 87
irq 88
irq 89
irq 90
irq 91
irq 92
irq 93
irq 94
irq 95
irq 96
irq 97
irq 98
irq 99
irq 100
irq 101
irq 102
irq 103
irq 104
irq 105
irq 106
irq 107
irq 108
irq 109
irq 110
irq 111
irq 112
irq 113
irq 114
irq 115
irq 116
irq 117
irq 118
irq 119
irq 120
irq 121
irq 122
irq 123
irq 124
irq 125
irq 126
irq 127
irq 128
irq 129
irq 130
irq 131
irq 132
irq 133
irq 134
irq 135
irq 136
irq 137
irq 138
irq 139
irq 140
irq 141
irq 142
irq 143
irq 144
irq 145
irq 146
irq 147
irq 148
irq 149
irq 150
irq 151
irq 152
irq 153
irq 154
irq 155
irq 156
irq 157
irq 158
irq 159
irq 160
irq 161
irq 162
irq 163
irq 164
irq 165
irq 166
irq 167
irq 168
irq 169
irq 170
irq 171
irq 172
irq 173
irq 174
irq 175
irq 176
irq 177
irq 178
irq 179
irq 180
irq 181
irq 182
irq 183
irq 184
irq 185
irq 186
irq 187
irq 188
irq 189
irq 190
irq 191
irq 192
irq 193
irq 194
irq 195
irq 196
irq 197
irq 198
irq 199
irq 200
irq 201
irq 202
irq 203
irq 204
irq 205
irq 206
irq 207
irq 208
irq 209
irq 210
irq 211
irq 212
irq 213
irq 214
irq 215
irq 216
irq 217
irq 218
irq 219
irq 220
irq 221
irq 222
irq 223

PUBLIC _reload_segreg

_reload_segreg PROC
	mov cx, 010h
	mov ds, cx
	mov es, cx
	mov fs, cx
	mov gs, cx
	mov ss, cx
	
	mov dx, 08h
	movzx rdx, dx
	lea rcx, exit
	push rdx
	push rcx
	retfq

exit:
	ret
_reload_segreg ENDP


PUBLIC Load_IDT
 
Load_IDT PROC
	lidt FWORD PTR[rcx]
	ret
Load_IDT ENDP


PUBLIC _cli

_cli PROC
	cli
	ret
_cli ENDP


PUBLIC _sti

_sti PROC
	sti
	ret
_sti ENDP


PUBLIC _hlt

_hlt PROC
	hlt
	ret
_hlt ENDP


PUBLIC _inttest

_inttest PROC
    mov rax,12345678h
	int 80h
	ret
_inttest ENDP


PUBLIC _iretq

_iretq PROC
    mov rsp, rcx
	popaq
	iretq
_iretq ENDP


PUBLIC _int20h

_int20h PROC
    int 20h
	ret
_int20h ENDP


_TEXT ENDS

END