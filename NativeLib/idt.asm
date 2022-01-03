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
		mov rcx, num
		call exception_handler
		hlt
	isr&num ENDP
ENDM

irq MACRO num
	EXTERN irq&num&_handler: PROC
	PUBLIC irq&num

	irq&num PROC
		push rbp
		mov rbp, rsp
		pushaq
		call irq&num&_handler
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


PUBLIC load_idt
 
load_idt PROC
	lidt FWORD PTR[rcx]
	ret
load_idt ENDP


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


_TEXT ENDS

END