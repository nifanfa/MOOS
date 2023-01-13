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


EXTERN intr_handler: PROC

intr MACRO num
	PUBLIC intr&num

	intr&num PROC
		push rbp
		mov rbp, rsp
		pushaq
		mov rcx, num
		mov rdx, rsp
		call intr_handler
		popaq
		pop rbp
		iretq
	intr&num ENDP
ENDM

intr 0
intr 1
intr 2
intr 3
intr 4
intr 5
intr 6
intr 7
intr 8
intr 9
intr 10
intr 11
intr 12
intr 13
intr 14
intr 15
intr 16
intr 17
intr 18
intr 19
intr 20
intr 21
intr 22
intr 23
intr 24
intr 25
intr 26
intr 27
intr 28
intr 29
intr 30
intr 31
intr 32
intr 33
intr 34
intr 35
intr 36
intr 37
intr 38
intr 39
intr 40
intr 41
intr 42
intr 43
intr 44
intr 45
intr 46
intr 47
intr 48
intr 49
intr 50
intr 51
intr 52
intr 53
intr 54
intr 55
intr 56
intr 57
intr 58
intr 59
intr 60
intr 61
intr 62
intr 63
intr 64
intr 65
intr 66
intr 67
intr 68
intr 69
intr 70
intr 71
intr 72
intr 73
intr 74
intr 75
intr 76
intr 77
intr 78
intr 79
intr 80
intr 81
intr 82
intr 83
intr 84
intr 85
intr 86
intr 87
intr 88
intr 89
intr 90
intr 91
intr 92
intr 93
intr 94
intr 95
intr 96
intr 97
intr 98
intr 99
intr 100
intr 101
intr 102
intr 103
intr 104
intr 105
intr 106
intr 107
intr 108
intr 109
intr 110
intr 111
intr 112
intr 113
intr 114
intr 115
intr 116
intr 117
intr 118
intr 119
intr 120
intr 121
intr 122
intr 123
intr 124
intr 125
intr 126
intr 127
intr 128
intr 129
intr 130
intr 131
intr 132
intr 133
intr 134
intr 135
intr 136
intr 137
intr 138
intr 139
intr 140
intr 141
intr 142
intr 143
intr 144
intr 145
intr 146
intr 147
intr 148
intr 149
intr 150
intr 151
intr 152
intr 153
intr 154
intr 155
intr 156
intr 157
intr 158
intr 159
intr 160
intr 161
intr 162
intr 163
intr 164
intr 165
intr 166
intr 167
intr 168
intr 169
intr 170
intr 171
intr 172
intr 173
intr 174
intr 175
intr 176
intr 177
intr 178
intr 179
intr 180
intr 181
intr 182
intr 183
intr 184
intr 185
intr 186
intr 187
intr 188
intr 189
intr 190
intr 191
intr 192
intr 193
intr 194
intr 195
intr 196
intr 197
intr 198
intr 199
intr 200
intr 201
intr 202
intr 203
intr 204
intr 205
intr 206
intr 207
intr 208
intr 209
intr 210
intr 211
intr 212
intr 213
intr 214
intr 215
intr 216
intr 217
intr 218
intr 219
intr 220
intr 221
intr 222
intr 223
intr 224
intr 225
intr 226
intr 227
intr 228
intr 229
intr 230
intr 231
intr 232
intr 233
intr 234
intr 235
intr 236
intr 237
intr 238
intr 239
intr 240
intr 241
intr 242
intr 243
intr 244
intr 245
intr 246
intr 247
intr 248
intr 249
intr 250
intr 251
intr 252
intr 253
intr 254
intr 255

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