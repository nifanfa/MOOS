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
	irq0(),irq1(), irq2(), irq3(), irq4(), irq5(), irq6(), irq7(), irq8(), irq9(), irq10(),
	irq11(), irq12(), irq13(), irq14(), irq15(), irq16(), irq17(), irq18(), irq19(), irq20(),
	irq21(), irq22(), irq23(), irq24(), irq25(), irq26(), irq27(), irq28(), irq29(), irq30(),
	irq31(), irq32(), irq33(), irq34(), irq35(), irq36(), irq37(), irq38(), irq39(), irq40(),
	irq41(), irq42(), irq43(), irq44(), irq45(), irq46(), irq47(), irq48(), irq49(), irq50(),
	irq51(), irq52(), irq53(), irq54(), irq55(), irq56(), irq57(), irq58(), irq59(), irq60(),
	irq61(), irq62(), irq63(), irq64(), irq65(), irq66(), irq67(), irq68(), irq69(), irq70(),
	irq71(), irq72(), irq73(), irq74(), irq75(), irq76(), irq77(), irq78(), irq79(), irq80(),
	irq81(), irq82(), irq83(), irq84(), irq85(), irq86(), irq87(), irq88(), irq89(), irq90(),
	irq91(), irq92(), irq93(), irq94(), irq95(), irq96(), irq97(), irq98(), irq99(), irq100(),
	irq101(), irq102(), irq103(), irq104(), irq105(), irq106(), irq107(), irq108(), irq109(), irq110(),
	irq111(), irq112(), irq113(), irq114(), irq115(), irq116(), irq117(), irq118(), irq119(), irq120(),
	irq121(), irq122(), irq123(), irq124(), irq125(), irq126(), irq127(), irq128(), irq129(), irq130(),
	irq131(), irq132(), irq133(), irq134(), irq135(), irq136(), irq137(), irq138(), irq139(), irq140(),
	irq141(), irq142(), irq143(), irq144(), irq145(), irq146(), irq147(), irq148(), irq149(), irq150(),
	irq151(), irq152(), irq153(), irq154(), irq155(), irq156(), irq157(), irq158(), irq159(), irq160(),
	irq161(), irq162(), irq163(), irq164(), irq165(), irq166(), irq167(), irq168(), irq169(), irq170(),
	irq171(), irq172(), irq173(), irq174(), irq175(), irq176(), irq177(), irq178(), irq179(), irq180(),
	irq181(), irq182(), irq183(), irq184(), irq185(), irq186(), irq187(), irq188(), irq189(), irq190(),
	irq191(), irq192(), irq193(), irq194(), irq195(), irq196(), irq197(), irq198(), irq199(), irq200(),
	irq201(), irq202(), irq203(), irq204(), irq205(), irq206(), irq207(), irq208(), irq209(), irq210(),
	irq211(), irq212(), irq213(), irq214(), irq215(), irq216(), irq217(), irq218(), irq219(), irq220(),
	irq221(), irq222(), irq223();


void Load_GDT(void* gdtr) {
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
	idt_set(idt, 48, (uintptr_t)irq16, 0x08, 0x8E);
	idt_set(idt, 49, (uintptr_t)irq17, 0x08, 0x8E);
	idt_set(idt, 50, (uintptr_t)irq18, 0x08, 0x8E);
	idt_set(idt, 51, (uintptr_t)irq19, 0x08, 0x8E);
	idt_set(idt, 52, (uintptr_t)irq20, 0x08, 0x8E);
	idt_set(idt, 53, (uintptr_t)irq21, 0x08, 0x8E);
	idt_set(idt, 54, (uintptr_t)irq22, 0x08, 0x8E);
	idt_set(idt, 55, (uintptr_t)irq23, 0x08, 0x8E);
	idt_set(idt, 56, (uintptr_t)irq24, 0x08, 0x8E);
	idt_set(idt, 57, (uintptr_t)irq25, 0x08, 0x8E);
	idt_set(idt, 58, (uintptr_t)irq26, 0x08, 0x8E);
	idt_set(idt, 59, (uintptr_t)irq27, 0x08, 0x8E);
	idt_set(idt, 60, (uintptr_t)irq28, 0x08, 0x8E);
	idt_set(idt, 61, (uintptr_t)irq29, 0x08, 0x8E);
	idt_set(idt, 62, (uintptr_t)irq30, 0x08, 0x8E);
	idt_set(idt, 63, (uintptr_t)irq31, 0x08, 0x8E);
	idt_set(idt, 64, (uintptr_t)irq32, 0x08, 0x8E);
	idt_set(idt, 65, (uintptr_t)irq33, 0x08, 0x8E);
	idt_set(idt, 66, (uintptr_t)irq34, 0x08, 0x8E);
	idt_set(idt, 67, (uintptr_t)irq35, 0x08, 0x8E);
	idt_set(idt, 68, (uintptr_t)irq36, 0x08, 0x8E);
	idt_set(idt, 69, (uintptr_t)irq37, 0x08, 0x8E);
	idt_set(idt, 70, (uintptr_t)irq38, 0x08, 0x8E);
	idt_set(idt, 71, (uintptr_t)irq39, 0x08, 0x8E);
	idt_set(idt, 72, (uintptr_t)irq40, 0x08, 0x8E);
	idt_set(idt, 73, (uintptr_t)irq41, 0x08, 0x8E);
	idt_set(idt, 74, (uintptr_t)irq42, 0x08, 0x8E);
	idt_set(idt, 75, (uintptr_t)irq43, 0x08, 0x8E);
	idt_set(idt, 76, (uintptr_t)irq44, 0x08, 0x8E);
	idt_set(idt, 77, (uintptr_t)irq45, 0x08, 0x8E);
	idt_set(idt, 78, (uintptr_t)irq46, 0x08, 0x8E);
	idt_set(idt, 79, (uintptr_t)irq47, 0x08, 0x8E);
	idt_set(idt, 80, (uintptr_t)irq48, 0x08, 0x8E);
	idt_set(idt, 81, (uintptr_t)irq49, 0x08, 0x8E);
	idt_set(idt, 82, (uintptr_t)irq50, 0x08, 0x8E);
	idt_set(idt, 83, (uintptr_t)irq51, 0x08, 0x8E);
	idt_set(idt, 84, (uintptr_t)irq52, 0x08, 0x8E);
	idt_set(idt, 85, (uintptr_t)irq53, 0x08, 0x8E);
	idt_set(idt, 86, (uintptr_t)irq54, 0x08, 0x8E);
	idt_set(idt, 87, (uintptr_t)irq55, 0x08, 0x8E);
	idt_set(idt, 88, (uintptr_t)irq56, 0x08, 0x8E);
	idt_set(idt, 89, (uintptr_t)irq57, 0x08, 0x8E);
	idt_set(idt, 90, (uintptr_t)irq58, 0x08, 0x8E);
	idt_set(idt, 91, (uintptr_t)irq59, 0x08, 0x8E);
	idt_set(idt, 92, (uintptr_t)irq60, 0x08, 0x8E);
	idt_set(idt, 93, (uintptr_t)irq61, 0x08, 0x8E);
	idt_set(idt, 94, (uintptr_t)irq62, 0x08, 0x8E);
	idt_set(idt, 95, (uintptr_t)irq63, 0x08, 0x8E);
	idt_set(idt, 96, (uintptr_t)irq64, 0x08, 0x8E);
	idt_set(idt, 97, (uintptr_t)irq65, 0x08, 0x8E);
	idt_set(idt, 98, (uintptr_t)irq66, 0x08, 0x8E);
	idt_set(idt, 99, (uintptr_t)irq67, 0x08, 0x8E);
	idt_set(idt, 100, (uintptr_t)irq68, 0x08, 0x8E);
	idt_set(idt, 101, (uintptr_t)irq69, 0x08, 0x8E);
	idt_set(idt, 102, (uintptr_t)irq70, 0x08, 0x8E);
	idt_set(idt, 103, (uintptr_t)irq71, 0x08, 0x8E);
	idt_set(idt, 104, (uintptr_t)irq72, 0x08, 0x8E);
	idt_set(idt, 105, (uintptr_t)irq73, 0x08, 0x8E);
	idt_set(idt, 106, (uintptr_t)irq74, 0x08, 0x8E);
	idt_set(idt, 107, (uintptr_t)irq75, 0x08, 0x8E);
	idt_set(idt, 108, (uintptr_t)irq76, 0x08, 0x8E);
	idt_set(idt, 109, (uintptr_t)irq77, 0x08, 0x8E);
	idt_set(idt, 110, (uintptr_t)irq78, 0x08, 0x8E);
	idt_set(idt, 111, (uintptr_t)irq79, 0x08, 0x8E);
	idt_set(idt, 112, (uintptr_t)irq80, 0x08, 0x8E);
	idt_set(idt, 113, (uintptr_t)irq81, 0x08, 0x8E);
	idt_set(idt, 114, (uintptr_t)irq82, 0x08, 0x8E);
	idt_set(idt, 115, (uintptr_t)irq83, 0x08, 0x8E);
	idt_set(idt, 116, (uintptr_t)irq84, 0x08, 0x8E);
	idt_set(idt, 117, (uintptr_t)irq85, 0x08, 0x8E);
	idt_set(idt, 118, (uintptr_t)irq86, 0x08, 0x8E);
	idt_set(idt, 119, (uintptr_t)irq87, 0x08, 0x8E);
	idt_set(idt, 120, (uintptr_t)irq88, 0x08, 0x8E);
	idt_set(idt, 121, (uintptr_t)irq89, 0x08, 0x8E);
	idt_set(idt, 122, (uintptr_t)irq90, 0x08, 0x8E);
	idt_set(idt, 123, (uintptr_t)irq91, 0x08, 0x8E);
	idt_set(idt, 124, (uintptr_t)irq92, 0x08, 0x8E);
	idt_set(idt, 125, (uintptr_t)irq93, 0x08, 0x8E);
	idt_set(idt, 126, (uintptr_t)irq94, 0x08, 0x8E);
	idt_set(idt, 127, (uintptr_t)irq95, 0x08, 0x8E);
	idt_set(idt, 128, (uintptr_t)irq96, 0x08, 0x8E);
	idt_set(idt, 129, (uintptr_t)irq97, 0x08, 0x8E);
	idt_set(idt, 130, (uintptr_t)irq98, 0x08, 0x8E);
	idt_set(idt, 131, (uintptr_t)irq99, 0x08, 0x8E);
	idt_set(idt, 132, (uintptr_t)irq100, 0x08, 0x8E);
	idt_set(idt, 133, (uintptr_t)irq101, 0x08, 0x8E);
	idt_set(idt, 134, (uintptr_t)irq102, 0x08, 0x8E);
	idt_set(idt, 135, (uintptr_t)irq103, 0x08, 0x8E);
	idt_set(idt, 136, (uintptr_t)irq104, 0x08, 0x8E);
	idt_set(idt, 137, (uintptr_t)irq105, 0x08, 0x8E);
	idt_set(idt, 138, (uintptr_t)irq106, 0x08, 0x8E);
	idt_set(idt, 139, (uintptr_t)irq107, 0x08, 0x8E);
	idt_set(idt, 140, (uintptr_t)irq108, 0x08, 0x8E);
	idt_set(idt, 141, (uintptr_t)irq109, 0x08, 0x8E);
	idt_set(idt, 142, (uintptr_t)irq110, 0x08, 0x8E);
	idt_set(idt, 143, (uintptr_t)irq111, 0x08, 0x8E);
	idt_set(idt, 144, (uintptr_t)irq112, 0x08, 0x8E);
	idt_set(idt, 145, (uintptr_t)irq113, 0x08, 0x8E);
	idt_set(idt, 146, (uintptr_t)irq114, 0x08, 0x8E);
	idt_set(idt, 147, (uintptr_t)irq115, 0x08, 0x8E);
	idt_set(idt, 148, (uintptr_t)irq116, 0x08, 0x8E);
	idt_set(idt, 149, (uintptr_t)irq117, 0x08, 0x8E);
	idt_set(idt, 150, (uintptr_t)irq118, 0x08, 0x8E);
	idt_set(idt, 151, (uintptr_t)irq119, 0x08, 0x8E);
	idt_set(idt, 152, (uintptr_t)irq120, 0x08, 0x8E);
	idt_set(idt, 153, (uintptr_t)irq121, 0x08, 0x8E);
	idt_set(idt, 154, (uintptr_t)irq122, 0x08, 0x8E);
	idt_set(idt, 155, (uintptr_t)irq123, 0x08, 0x8E);
	idt_set(idt, 156, (uintptr_t)irq124, 0x08, 0x8E);
	idt_set(idt, 157, (uintptr_t)irq125, 0x08, 0x8E);
	idt_set(idt, 158, (uintptr_t)irq126, 0x08, 0x8E);
	idt_set(idt, 159, (uintptr_t)irq127, 0x08, 0x8E);
	idt_set(idt, 160, (uintptr_t)irq128, 0x08, 0x8E);
	idt_set(idt, 161, (uintptr_t)irq129, 0x08, 0x8E);
	idt_set(idt, 162, (uintptr_t)irq130, 0x08, 0x8E);
	idt_set(idt, 163, (uintptr_t)irq131, 0x08, 0x8E);
	idt_set(idt, 164, (uintptr_t)irq132, 0x08, 0x8E);
	idt_set(idt, 165, (uintptr_t)irq133, 0x08, 0x8E);
	idt_set(idt, 166, (uintptr_t)irq134, 0x08, 0x8E);
	idt_set(idt, 167, (uintptr_t)irq135, 0x08, 0x8E);
	idt_set(idt, 168, (uintptr_t)irq136, 0x08, 0x8E);
	idt_set(idt, 169, (uintptr_t)irq137, 0x08, 0x8E);
	idt_set(idt, 170, (uintptr_t)irq138, 0x08, 0x8E);
	idt_set(idt, 171, (uintptr_t)irq139, 0x08, 0x8E);
	idt_set(idt, 172, (uintptr_t)irq140, 0x08, 0x8E);
	idt_set(idt, 173, (uintptr_t)irq141, 0x08, 0x8E);
	idt_set(idt, 174, (uintptr_t)irq142, 0x08, 0x8E);
	idt_set(idt, 175, (uintptr_t)irq143, 0x08, 0x8E);
	idt_set(idt, 176, (uintptr_t)irq144, 0x08, 0x8E);
	idt_set(idt, 177, (uintptr_t)irq145, 0x08, 0x8E);
	idt_set(idt, 178, (uintptr_t)irq146, 0x08, 0x8E);
	idt_set(idt, 179, (uintptr_t)irq147, 0x08, 0x8E);
	idt_set(idt, 180, (uintptr_t)irq148, 0x08, 0x8E);
	idt_set(idt, 181, (uintptr_t)irq149, 0x08, 0x8E);
	idt_set(idt, 182, (uintptr_t)irq150, 0x08, 0x8E);
	idt_set(idt, 183, (uintptr_t)irq151, 0x08, 0x8E);
	idt_set(idt, 184, (uintptr_t)irq152, 0x08, 0x8E);
	idt_set(idt, 185, (uintptr_t)irq153, 0x08, 0x8E);
	idt_set(idt, 186, (uintptr_t)irq154, 0x08, 0x8E);
	idt_set(idt, 187, (uintptr_t)irq155, 0x08, 0x8E);
	idt_set(idt, 188, (uintptr_t)irq156, 0x08, 0x8E);
	idt_set(idt, 189, (uintptr_t)irq157, 0x08, 0x8E);
	idt_set(idt, 190, (uintptr_t)irq158, 0x08, 0x8E);
	idt_set(idt, 191, (uintptr_t)irq159, 0x08, 0x8E);
	idt_set(idt, 192, (uintptr_t)irq160, 0x08, 0x8E);
	idt_set(idt, 193, (uintptr_t)irq161, 0x08, 0x8E);
	idt_set(idt, 194, (uintptr_t)irq162, 0x08, 0x8E);
	idt_set(idt, 195, (uintptr_t)irq163, 0x08, 0x8E);
	idt_set(idt, 196, (uintptr_t)irq164, 0x08, 0x8E);
	idt_set(idt, 197, (uintptr_t)irq165, 0x08, 0x8E);
	idt_set(idt, 198, (uintptr_t)irq166, 0x08, 0x8E);
	idt_set(idt, 199, (uintptr_t)irq167, 0x08, 0x8E);
	idt_set(idt, 200, (uintptr_t)irq168, 0x08, 0x8E);
	idt_set(idt, 201, (uintptr_t)irq169, 0x08, 0x8E);
	idt_set(idt, 202, (uintptr_t)irq170, 0x08, 0x8E);
	idt_set(idt, 203, (uintptr_t)irq171, 0x08, 0x8E);
	idt_set(idt, 204, (uintptr_t)irq172, 0x08, 0x8E);
	idt_set(idt, 205, (uintptr_t)irq173, 0x08, 0x8E);
	idt_set(idt, 206, (uintptr_t)irq174, 0x08, 0x8E);
	idt_set(idt, 207, (uintptr_t)irq175, 0x08, 0x8E);
	idt_set(idt, 208, (uintptr_t)irq176, 0x08, 0x8E);
	idt_set(idt, 209, (uintptr_t)irq177, 0x08, 0x8E);
	idt_set(idt, 210, (uintptr_t)irq178, 0x08, 0x8E);
	idt_set(idt, 211, (uintptr_t)irq179, 0x08, 0x8E);
	idt_set(idt, 212, (uintptr_t)irq180, 0x08, 0x8E);
	idt_set(idt, 213, (uintptr_t)irq181, 0x08, 0x8E);
	idt_set(idt, 214, (uintptr_t)irq182, 0x08, 0x8E);
	idt_set(idt, 215, (uintptr_t)irq183, 0x08, 0x8E);
	idt_set(idt, 216, (uintptr_t)irq184, 0x08, 0x8E);
	idt_set(idt, 217, (uintptr_t)irq185, 0x08, 0x8E);
	idt_set(idt, 218, (uintptr_t)irq186, 0x08, 0x8E);
	idt_set(idt, 219, (uintptr_t)irq187, 0x08, 0x8E);
	idt_set(idt, 220, (uintptr_t)irq188, 0x08, 0x8E);
	idt_set(idt, 221, (uintptr_t)irq189, 0x08, 0x8E);
	idt_set(idt, 222, (uintptr_t)irq190, 0x08, 0x8E);
	idt_set(idt, 223, (uintptr_t)irq191, 0x08, 0x8E);
	idt_set(idt, 224, (uintptr_t)irq192, 0x08, 0x8E);
	idt_set(idt, 225, (uintptr_t)irq193, 0x08, 0x8E);
	idt_set(idt, 226, (uintptr_t)irq194, 0x08, 0x8E);
	idt_set(idt, 227, (uintptr_t)irq195, 0x08, 0x8E);
	idt_set(idt, 228, (uintptr_t)irq196, 0x08, 0x8E);
	idt_set(idt, 229, (uintptr_t)irq197, 0x08, 0x8E);
	idt_set(idt, 230, (uintptr_t)irq198, 0x08, 0x8E);
	idt_set(idt, 231, (uintptr_t)irq199, 0x08, 0x8E);
	idt_set(idt, 232, (uintptr_t)irq200, 0x08, 0x8E);
	idt_set(idt, 233, (uintptr_t)irq201, 0x08, 0x8E);
	idt_set(idt, 234, (uintptr_t)irq202, 0x08, 0x8E);
	idt_set(idt, 235, (uintptr_t)irq203, 0x08, 0x8E);
	idt_set(idt, 236, (uintptr_t)irq204, 0x08, 0x8E);
	idt_set(idt, 237, (uintptr_t)irq205, 0x08, 0x8E);
	idt_set(idt, 238, (uintptr_t)irq206, 0x08, 0x8E);
	idt_set(idt, 239, (uintptr_t)irq207, 0x08, 0x8E);
	idt_set(idt, 240, (uintptr_t)irq208, 0x08, 0x8E);
	idt_set(idt, 241, (uintptr_t)irq209, 0x08, 0x8E);
	idt_set(idt, 242, (uintptr_t)irq210, 0x08, 0x8E);
	idt_set(idt, 243, (uintptr_t)irq211, 0x08, 0x8E);
	idt_set(idt, 244, (uintptr_t)irq212, 0x08, 0x8E);
	idt_set(idt, 245, (uintptr_t)irq213, 0x08, 0x8E);
	idt_set(idt, 246, (uintptr_t)irq214, 0x08, 0x8E);
	idt_set(idt, 247, (uintptr_t)irq215, 0x08, 0x8E);
	idt_set(idt, 248, (uintptr_t)irq216, 0x08, 0x8E);
	idt_set(idt, 249, (uintptr_t)irq217, 0x08, 0x8E);
	idt_set(idt, 250, (uintptr_t)irq218, 0x08, 0x8E);
	idt_set(idt, 251, (uintptr_t)irq219, 0x08, 0x8E);
	idt_set(idt, 252, (uintptr_t)irq220, 0x08, 0x8E);
	idt_set(idt, 253, (uintptr_t)irq221, 0x08, 0x8E);
	idt_set(idt, 254, (uintptr_t)irq222, 0x08, 0x8E);
	idt_set(idt, 255, (uintptr_t)irq223, 0x08, 0x8E);
}