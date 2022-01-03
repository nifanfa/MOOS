#include <stdint.h>
#include <intrin.h>


typedef struct {
	uint16_t base_low;
	uint16_t selector;
	uint8_t reserved0;
	uint8_t type_attr;
	uint16_t base_mid;
	uint32_t base_high;
	uint32_t reserved1;
} IDTEntry;


int
	isr0(), isr1(), isr2(), isr3(), isr4(), isr5(), isr6(), isr7(),
	isr8(), isr9(), isr10(), isr11(), isr12(), isr13(), isr14(), isr15(),
	isr16(), isr17(), isr18(), isr19(), isr20(), isr21(), isr22(), isr23(),
	isr24(), isr25(), isr26(), isr27(), isr28(), isr29(), isr30(), isr31(),
	isr128();

int
	irq0(), irq1(), irq2(), irq3(), irq4(), irq5(), irq6(), irq7(),
	irq8(), irq9(), irq10(), irq11(), irq12(), irq13(), irq14(), irq15();


void load_gdt(void* gdtr) {
	_lgdt(gdtr);
	_reload_segreg();
}

void idt_set(IDTEntry* idt, int idx, uintptr_t base, uint16_t selector, uint8_t type_attr) {
	idt[idx].base_low = base & 0xFFFF;
	idt[idx].base_mid = (base >> 16) & 0xFFFF;
	idt[idx].base_high = (base >> 32) & 0xFFFFFFFF;
	idt[idx].selector = selector;
	idt[idx].type_attr = type_attr;
	idt[idx].reserved0 = 0;
	idt[idx].reserved1 = 0;
}

void set_idt_entries(IDTEntry* idt) {
	/* set ISR entries */
	idt_set(idt, 0, (uintptr_t)isr0, 0x08, 0x8E);
	idt_set(idt, 1, (uintptr_t)isr1, 0x08, 0x8E);
	idt_set(idt, 2, (uintptr_t)isr2, 0x08, 0x8E);
	idt_set(idt, 3, (uintptr_t)isr3, 0x08, 0x8E);
	idt_set(idt, 4, (uintptr_t)isr4, 0x08, 0x8E);
	idt_set(idt, 5, (uintptr_t)isr5, 0x08, 0x8E);
	idt_set(idt, 6, (uintptr_t)isr6, 0x08, 0x8E);
	idt_set(idt, 7, (uintptr_t)isr7, 0x08, 0x8E);
	idt_set(idt, 8, (uintptr_t)isr8, 0x08, 0x8E);
	idt_set(idt, 9, (uintptr_t)isr9, 0x08, 0x8E);
	idt_set(idt, 10, (uintptr_t)isr10, 0x08, 0x8E);
	idt_set(idt, 11, (uintptr_t)isr11, 0x08, 0x8E);
	idt_set(idt, 12, (uintptr_t)isr12, 0x08, 0x8E);
	idt_set(idt, 13, (uintptr_t)isr13, 0x08, 0x8E);
	idt_set(idt, 14, (uintptr_t)isr14, 0x08, 0x8E);
	idt_set(idt, 15, (uintptr_t)isr15, 0x08, 0x8E);
	idt_set(idt, 16, (uintptr_t)isr16, 0x08, 0x8E);
	idt_set(idt, 17, (uintptr_t)isr17, 0x08, 0x8E);
	idt_set(idt, 18, (uintptr_t)isr18, 0x08, 0x8E);
	idt_set(idt, 19, (uintptr_t)isr19, 0x08, 0x8E);
	idt_set(idt, 20, (uintptr_t)isr20, 0x08, 0x8E);
	idt_set(idt, 21, (uintptr_t)isr21, 0x08, 0x8E);
	idt_set(idt, 22, (uintptr_t)isr22, 0x08, 0x8E);
	idt_set(idt, 23, (uintptr_t)isr23, 0x08, 0x8E);
	idt_set(idt, 24, (uintptr_t)isr24, 0x08, 0x8E);
	idt_set(idt, 25, (uintptr_t)isr25, 0x08, 0x8E);
	idt_set(idt, 26, (uintptr_t)isr26, 0x08, 0x8E);
	idt_set(idt, 27, (uintptr_t)isr27, 0x08, 0x8E);
	idt_set(idt, 28, (uintptr_t)isr28, 0x08, 0x8E);
	idt_set(idt, 29, (uintptr_t)isr29, 0x08, 0x8E);
	idt_set(idt, 30, (uintptr_t)isr30, 0x08, 0x8E);
	idt_set(idt, 31, (uintptr_t)isr31, 0x08, 0x8E);
	idt_set(idt, 128, (uintptr_t)isr128, 0x08, 0x8E);

	/* set IDT entries */
	idt_set(idt, 32, (uintptr_t)irq0, 0x08, 0x8E);
	idt_set(idt, 33, (uintptr_t)irq1, 0x08, 0x8E);
	idt_set(idt, 34, (uintptr_t)irq2, 0x08, 0x8E);
	idt_set(idt, 35, (uintptr_t)irq3, 0x08, 0x8E);
	idt_set(idt, 36, (uintptr_t)irq4, 0x08, 0x8E);
	idt_set(idt, 37, (uintptr_t)irq5, 0x08, 0x8E);
	idt_set(idt, 38, (uintptr_t)irq6, 0x08, 0x8E);
	idt_set(idt, 39, (uintptr_t)irq7, 0x08, 0x8E);
	idt_set(idt, 40, (uintptr_t)irq8, 0x08, 0x8E);
	idt_set(idt, 41, (uintptr_t)irq9, 0x08, 0x8E);
	idt_set(idt, 42, (uintptr_t)irq10, 0x08, 0x8E);
	idt_set(idt, 43, (uintptr_t)irq11, 0x08, 0x8E);
	idt_set(idt, 44, (uintptr_t)irq12, 0x08, 0x8E);
	idt_set(idt, 45, (uintptr_t)irq13, 0x08, 0x8E);
	idt_set(idt, 46, (uintptr_t)irq14, 0x08, 0x8E);
	idt_set(idt, 47, (uintptr_t)irq15, 0x08, 0x8E);
}