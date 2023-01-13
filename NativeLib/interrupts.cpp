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
intr0(),
intr1(), intr2(), intr3(), intr4(), intr5(), intr6(), intr7(), intr8(), intr9(), intr10(),
intr11(), intr12(), intr13(), intr14(), intr15(), intr16(), intr17(), intr18(), intr19(), intr20(),
intr21(), intr22(), intr23(), intr24(), intr25(), intr26(), intr27(), intr28(), intr29(), intr30(),
intr31(), intr32(), intr33(), intr34(), intr35(), intr36(), intr37(), intr38(), intr39(), intr40(),
intr41(), intr42(), intr43(), intr44(), intr45(), intr46(), intr47(), intr48(), intr49(), intr50(),
intr51(), intr52(), intr53(), intr54(), intr55(), intr56(), intr57(), intr58(), intr59(), intr60(),
intr61(), intr62(), intr63(), intr64(), intr65(), intr66(), intr67(), intr68(), intr69(), intr70(),
intr71(), intr72(), intr73(), intr74(), intr75(), intr76(), intr77(), intr78(), intr79(), intr80(),
intr81(), intr82(), intr83(), intr84(), intr85(), intr86(), intr87(), intr88(), intr89(), intr90(),
intr91(), intr92(), intr93(), intr94(), intr95(), intr96(), intr97(), intr98(), intr99(), intr100(),
intr101(), intr102(), intr103(), intr104(), intr105(), intr106(), intr107(), intr108(), intr109(), intr110(),
intr111(), intr112(), intr113(), intr114(), intr115(), intr116(), intr117(), intr118(), intr119(), intr120(),
intr121(), intr122(), intr123(), intr124(), intr125(), intr126(), intr127(), intr128(), intr129(), intr130(),
intr131(), intr132(), intr133(), intr134(), intr135(), intr136(), intr137(), intr138(), intr139(), intr140(),
intr141(), intr142(), intr143(), intr144(), intr145(), intr146(), intr147(), intr148(), intr149(), intr150(),
intr151(), intr152(), intr153(), intr154(), intr155(), intr156(), intr157(), intr158(), intr159(), intr160(),
intr161(), intr162(), intr163(), intr164(), intr165(), intr166(), intr167(), intr168(), intr169(), intr170(),
intr171(), intr172(), intr173(), intr174(), intr175(), intr176(), intr177(), intr178(), intr179(), intr180(),
intr181(), intr182(), intr183(), intr184(), intr185(), intr186(), intr187(), intr188(), intr189(), intr190(),
intr191(), intr192(), intr193(), intr194(), intr195(), intr196(), intr197(), intr198(), intr199(), intr200(),
intr201(), intr202(), intr203(), intr204(), intr205(), intr206(), intr207(), intr208(), intr209(), intr210(),
intr211(), intr212(), intr213(), intr214(), intr215(), intr216(), intr217(), intr218(), intr219(), intr220(),
intr221(), intr222(), intr223(), intr224(), intr225(), intr226(), intr227(), intr228(), intr229(), intr230(),
intr231(), intr232(), intr233(), intr234(), intr235(), intr236(), intr237(), intr238(), intr239(), intr240(),
intr241(), intr242(), intr243(), intr244(), intr245(), intr246(), intr247(), intr248(), intr249(), intr250(),
intr251(), intr252(), intr253(), intr254(), intr255();


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
	idt_set(idt, 0, (uintptr_t)intr0, 0x08, 0x8E);
	idt_set(idt, 1, (uintptr_t)intr1, 0x08, 0x8E);
	idt_set(idt, 2, (uintptr_t)intr2, 0x08, 0x8E);
	idt_set(idt, 3, (uintptr_t)intr3, 0x08, 0x8E);
	idt_set(idt, 4, (uintptr_t)intr4, 0x08, 0x8E);
	idt_set(idt, 5, (uintptr_t)intr5, 0x08, 0x8E);
	idt_set(idt, 6, (uintptr_t)intr6, 0x08, 0x8E);
	idt_set(idt, 7, (uintptr_t)intr7, 0x08, 0x8E);
	idt_set(idt, 8, (uintptr_t)intr8, 0x08, 0x8E);
	idt_set(idt, 9, (uintptr_t)intr9, 0x08, 0x8E);
	idt_set(idt, 10, (uintptr_t)intr10, 0x08, 0x8E);
	idt_set(idt, 11, (uintptr_t)intr11, 0x08, 0x8E);
	idt_set(idt, 12, (uintptr_t)intr12, 0x08, 0x8E);
	idt_set(idt, 13, (uintptr_t)intr13, 0x08, 0x8E);
	idt_set(idt, 14, (uintptr_t)intr14, 0x08, 0x8E);
	idt_set(idt, 15, (uintptr_t)intr15, 0x08, 0x8E);
	idt_set(idt, 16, (uintptr_t)intr16, 0x08, 0x8E);
	idt_set(idt, 17, (uintptr_t)intr17, 0x08, 0x8E);
	idt_set(idt, 18, (uintptr_t)intr18, 0x08, 0x8E);
	idt_set(idt, 19, (uintptr_t)intr19, 0x08, 0x8E);
	idt_set(idt, 20, (uintptr_t)intr20, 0x08, 0x8E);
	idt_set(idt, 21, (uintptr_t)intr21, 0x08, 0x8E);
	idt_set(idt, 22, (uintptr_t)intr22, 0x08, 0x8E);
	idt_set(idt, 23, (uintptr_t)intr23, 0x08, 0x8E);
	idt_set(idt, 24, (uintptr_t)intr24, 0x08, 0x8E);
	idt_set(idt, 25, (uintptr_t)intr25, 0x08, 0x8E);
	idt_set(idt, 26, (uintptr_t)intr26, 0x08, 0x8E);
	idt_set(idt, 27, (uintptr_t)intr27, 0x08, 0x8E);
	idt_set(idt, 28, (uintptr_t)intr28, 0x08, 0x8E);
	idt_set(idt, 29, (uintptr_t)intr29, 0x08, 0x8E);
	idt_set(idt, 30, (uintptr_t)intr30, 0x08, 0x8E);
	idt_set(idt, 31, (uintptr_t)intr31, 0x08, 0x8E);
	idt_set(idt, 32, (uintptr_t)intr32, 0x08, 0x8E);
	idt_set(idt, 33, (uintptr_t)intr33, 0x08, 0x8E);
	idt_set(idt, 34, (uintptr_t)intr34, 0x08, 0x8E);
	idt_set(idt, 35, (uintptr_t)intr35, 0x08, 0x8E);
	idt_set(idt, 36, (uintptr_t)intr36, 0x08, 0x8E);
	idt_set(idt, 37, (uintptr_t)intr37, 0x08, 0x8E);
	idt_set(idt, 38, (uintptr_t)intr38, 0x08, 0x8E);
	idt_set(idt, 39, (uintptr_t)intr39, 0x08, 0x8E);
	idt_set(idt, 40, (uintptr_t)intr40, 0x08, 0x8E);
	idt_set(idt, 41, (uintptr_t)intr41, 0x08, 0x8E);
	idt_set(idt, 42, (uintptr_t)intr42, 0x08, 0x8E);
	idt_set(idt, 43, (uintptr_t)intr43, 0x08, 0x8E);
	idt_set(idt, 44, (uintptr_t)intr44, 0x08, 0x8E);
	idt_set(idt, 45, (uintptr_t)intr45, 0x08, 0x8E);
	idt_set(idt, 46, (uintptr_t)intr46, 0x08, 0x8E);
	idt_set(idt, 47, (uintptr_t)intr47, 0x08, 0x8E);
	idt_set(idt, 48, (uintptr_t)intr48, 0x08, 0x8E);
	idt_set(idt, 49, (uintptr_t)intr49, 0x08, 0x8E);
	idt_set(idt, 50, (uintptr_t)intr50, 0x08, 0x8E);
	idt_set(idt, 51, (uintptr_t)intr51, 0x08, 0x8E);
	idt_set(idt, 52, (uintptr_t)intr52, 0x08, 0x8E);
	idt_set(idt, 53, (uintptr_t)intr53, 0x08, 0x8E);
	idt_set(idt, 54, (uintptr_t)intr54, 0x08, 0x8E);
	idt_set(idt, 55, (uintptr_t)intr55, 0x08, 0x8E);
	idt_set(idt, 56, (uintptr_t)intr56, 0x08, 0x8E);
	idt_set(idt, 57, (uintptr_t)intr57, 0x08, 0x8E);
	idt_set(idt, 58, (uintptr_t)intr58, 0x08, 0x8E);
	idt_set(idt, 59, (uintptr_t)intr59, 0x08, 0x8E);
	idt_set(idt, 60, (uintptr_t)intr60, 0x08, 0x8E);
	idt_set(idt, 61, (uintptr_t)intr61, 0x08, 0x8E);
	idt_set(idt, 62, (uintptr_t)intr62, 0x08, 0x8E);
	idt_set(idt, 63, (uintptr_t)intr63, 0x08, 0x8E);
	idt_set(idt, 64, (uintptr_t)intr64, 0x08, 0x8E);
	idt_set(idt, 65, (uintptr_t)intr65, 0x08, 0x8E);
	idt_set(idt, 66, (uintptr_t)intr66, 0x08, 0x8E);
	idt_set(idt, 67, (uintptr_t)intr67, 0x08, 0x8E);
	idt_set(idt, 68, (uintptr_t)intr68, 0x08, 0x8E);
	idt_set(idt, 69, (uintptr_t)intr69, 0x08, 0x8E);
	idt_set(idt, 70, (uintptr_t)intr70, 0x08, 0x8E);
	idt_set(idt, 71, (uintptr_t)intr71, 0x08, 0x8E);
	idt_set(idt, 72, (uintptr_t)intr72, 0x08, 0x8E);
	idt_set(idt, 73, (uintptr_t)intr73, 0x08, 0x8E);
	idt_set(idt, 74, (uintptr_t)intr74, 0x08, 0x8E);
	idt_set(idt, 75, (uintptr_t)intr75, 0x08, 0x8E);
	idt_set(idt, 76, (uintptr_t)intr76, 0x08, 0x8E);
	idt_set(idt, 77, (uintptr_t)intr77, 0x08, 0x8E);
	idt_set(idt, 78, (uintptr_t)intr78, 0x08, 0x8E);
	idt_set(idt, 79, (uintptr_t)intr79, 0x08, 0x8E);
	idt_set(idt, 80, (uintptr_t)intr80, 0x08, 0x8E);
	idt_set(idt, 81, (uintptr_t)intr81, 0x08, 0x8E);
	idt_set(idt, 82, (uintptr_t)intr82, 0x08, 0x8E);
	idt_set(idt, 83, (uintptr_t)intr83, 0x08, 0x8E);
	idt_set(idt, 84, (uintptr_t)intr84, 0x08, 0x8E);
	idt_set(idt, 85, (uintptr_t)intr85, 0x08, 0x8E);
	idt_set(idt, 86, (uintptr_t)intr86, 0x08, 0x8E);
	idt_set(idt, 87, (uintptr_t)intr87, 0x08, 0x8E);
	idt_set(idt, 88, (uintptr_t)intr88, 0x08, 0x8E);
	idt_set(idt, 89, (uintptr_t)intr89, 0x08, 0x8E);
	idt_set(idt, 90, (uintptr_t)intr90, 0x08, 0x8E);
	idt_set(idt, 91, (uintptr_t)intr91, 0x08, 0x8E);
	idt_set(idt, 92, (uintptr_t)intr92, 0x08, 0x8E);
	idt_set(idt, 93, (uintptr_t)intr93, 0x08, 0x8E);
	idt_set(idt, 94, (uintptr_t)intr94, 0x08, 0x8E);
	idt_set(idt, 95, (uintptr_t)intr95, 0x08, 0x8E);
	idt_set(idt, 96, (uintptr_t)intr96, 0x08, 0x8E);
	idt_set(idt, 97, (uintptr_t)intr97, 0x08, 0x8E);
	idt_set(idt, 98, (uintptr_t)intr98, 0x08, 0x8E);
	idt_set(idt, 99, (uintptr_t)intr99, 0x08, 0x8E);
	idt_set(idt, 100, (uintptr_t)intr100, 0x08, 0x8E);
	idt_set(idt, 101, (uintptr_t)intr101, 0x08, 0x8E);
	idt_set(idt, 102, (uintptr_t)intr102, 0x08, 0x8E);
	idt_set(idt, 103, (uintptr_t)intr103, 0x08, 0x8E);
	idt_set(idt, 104, (uintptr_t)intr104, 0x08, 0x8E);
	idt_set(idt, 105, (uintptr_t)intr105, 0x08, 0x8E);
	idt_set(idt, 106, (uintptr_t)intr106, 0x08, 0x8E);
	idt_set(idt, 107, (uintptr_t)intr107, 0x08, 0x8E);
	idt_set(idt, 108, (uintptr_t)intr108, 0x08, 0x8E);
	idt_set(idt, 109, (uintptr_t)intr109, 0x08, 0x8E);
	idt_set(idt, 110, (uintptr_t)intr110, 0x08, 0x8E);
	idt_set(idt, 111, (uintptr_t)intr111, 0x08, 0x8E);
	idt_set(idt, 112, (uintptr_t)intr112, 0x08, 0x8E);
	idt_set(idt, 113, (uintptr_t)intr113, 0x08, 0x8E);
	idt_set(idt, 114, (uintptr_t)intr114, 0x08, 0x8E);
	idt_set(idt, 115, (uintptr_t)intr115, 0x08, 0x8E);
	idt_set(idt, 116, (uintptr_t)intr116, 0x08, 0x8E);
	idt_set(idt, 117, (uintptr_t)intr117, 0x08, 0x8E);
	idt_set(idt, 118, (uintptr_t)intr118, 0x08, 0x8E);
	idt_set(idt, 119, (uintptr_t)intr119, 0x08, 0x8E);
	idt_set(idt, 120, (uintptr_t)intr120, 0x08, 0x8E);
	idt_set(idt, 121, (uintptr_t)intr121, 0x08, 0x8E);
	idt_set(idt, 122, (uintptr_t)intr122, 0x08, 0x8E);
	idt_set(idt, 123, (uintptr_t)intr123, 0x08, 0x8E);
	idt_set(idt, 124, (uintptr_t)intr124, 0x08, 0x8E);
	idt_set(idt, 125, (uintptr_t)intr125, 0x08, 0x8E);
	idt_set(idt, 126, (uintptr_t)intr126, 0x08, 0x8E);
	idt_set(idt, 127, (uintptr_t)intr127, 0x08, 0x8E);
	idt_set(idt, 128, (uintptr_t)intr128, 0x08, 0x8E);
	idt_set(idt, 129, (uintptr_t)intr129, 0x08, 0x8E);
	idt_set(idt, 130, (uintptr_t)intr130, 0x08, 0x8E);
	idt_set(idt, 131, (uintptr_t)intr131, 0x08, 0x8E);
	idt_set(idt, 132, (uintptr_t)intr132, 0x08, 0x8E);
	idt_set(idt, 133, (uintptr_t)intr133, 0x08, 0x8E);
	idt_set(idt, 134, (uintptr_t)intr134, 0x08, 0x8E);
	idt_set(idt, 135, (uintptr_t)intr135, 0x08, 0x8E);
	idt_set(idt, 136, (uintptr_t)intr136, 0x08, 0x8E);
	idt_set(idt, 137, (uintptr_t)intr137, 0x08, 0x8E);
	idt_set(idt, 138, (uintptr_t)intr138, 0x08, 0x8E);
	idt_set(idt, 139, (uintptr_t)intr139, 0x08, 0x8E);
	idt_set(idt, 140, (uintptr_t)intr140, 0x08, 0x8E);
	idt_set(idt, 141, (uintptr_t)intr141, 0x08, 0x8E);
	idt_set(idt, 142, (uintptr_t)intr142, 0x08, 0x8E);
	idt_set(idt, 143, (uintptr_t)intr143, 0x08, 0x8E);
	idt_set(idt, 144, (uintptr_t)intr144, 0x08, 0x8E);
	idt_set(idt, 145, (uintptr_t)intr145, 0x08, 0x8E);
	idt_set(idt, 146, (uintptr_t)intr146, 0x08, 0x8E);
	idt_set(idt, 147, (uintptr_t)intr147, 0x08, 0x8E);
	idt_set(idt, 148, (uintptr_t)intr148, 0x08, 0x8E);
	idt_set(idt, 149, (uintptr_t)intr149, 0x08, 0x8E);
	idt_set(idt, 150, (uintptr_t)intr150, 0x08, 0x8E);
	idt_set(idt, 151, (uintptr_t)intr151, 0x08, 0x8E);
	idt_set(idt, 152, (uintptr_t)intr152, 0x08, 0x8E);
	idt_set(idt, 153, (uintptr_t)intr153, 0x08, 0x8E);
	idt_set(idt, 154, (uintptr_t)intr154, 0x08, 0x8E);
	idt_set(idt, 155, (uintptr_t)intr155, 0x08, 0x8E);
	idt_set(idt, 156, (uintptr_t)intr156, 0x08, 0x8E);
	idt_set(idt, 157, (uintptr_t)intr157, 0x08, 0x8E);
	idt_set(idt, 158, (uintptr_t)intr158, 0x08, 0x8E);
	idt_set(idt, 159, (uintptr_t)intr159, 0x08, 0x8E);
	idt_set(idt, 160, (uintptr_t)intr160, 0x08, 0x8E);
	idt_set(idt, 161, (uintptr_t)intr161, 0x08, 0x8E);
	idt_set(idt, 162, (uintptr_t)intr162, 0x08, 0x8E);
	idt_set(idt, 163, (uintptr_t)intr163, 0x08, 0x8E);
	idt_set(idt, 164, (uintptr_t)intr164, 0x08, 0x8E);
	idt_set(idt, 165, (uintptr_t)intr165, 0x08, 0x8E);
	idt_set(idt, 166, (uintptr_t)intr166, 0x08, 0x8E);
	idt_set(idt, 167, (uintptr_t)intr167, 0x08, 0x8E);
	idt_set(idt, 168, (uintptr_t)intr168, 0x08, 0x8E);
	idt_set(idt, 169, (uintptr_t)intr169, 0x08, 0x8E);
	idt_set(idt, 170, (uintptr_t)intr170, 0x08, 0x8E);
	idt_set(idt, 171, (uintptr_t)intr171, 0x08, 0x8E);
	idt_set(idt, 172, (uintptr_t)intr172, 0x08, 0x8E);
	idt_set(idt, 173, (uintptr_t)intr173, 0x08, 0x8E);
	idt_set(idt, 174, (uintptr_t)intr174, 0x08, 0x8E);
	idt_set(idt, 175, (uintptr_t)intr175, 0x08, 0x8E);
	idt_set(idt, 176, (uintptr_t)intr176, 0x08, 0x8E);
	idt_set(idt, 177, (uintptr_t)intr177, 0x08, 0x8E);
	idt_set(idt, 178, (uintptr_t)intr178, 0x08, 0x8E);
	idt_set(idt, 179, (uintptr_t)intr179, 0x08, 0x8E);
	idt_set(idt, 180, (uintptr_t)intr180, 0x08, 0x8E);
	idt_set(idt, 181, (uintptr_t)intr181, 0x08, 0x8E);
	idt_set(idt, 182, (uintptr_t)intr182, 0x08, 0x8E);
	idt_set(idt, 183, (uintptr_t)intr183, 0x08, 0x8E);
	idt_set(idt, 184, (uintptr_t)intr184, 0x08, 0x8E);
	idt_set(idt, 185, (uintptr_t)intr185, 0x08, 0x8E);
	idt_set(idt, 186, (uintptr_t)intr186, 0x08, 0x8E);
	idt_set(idt, 187, (uintptr_t)intr187, 0x08, 0x8E);
	idt_set(idt, 188, (uintptr_t)intr188, 0x08, 0x8E);
	idt_set(idt, 189, (uintptr_t)intr189, 0x08, 0x8E);
	idt_set(idt, 190, (uintptr_t)intr190, 0x08, 0x8E);
	idt_set(idt, 191, (uintptr_t)intr191, 0x08, 0x8E);
	idt_set(idt, 192, (uintptr_t)intr192, 0x08, 0x8E);
	idt_set(idt, 193, (uintptr_t)intr193, 0x08, 0x8E);
	idt_set(idt, 194, (uintptr_t)intr194, 0x08, 0x8E);
	idt_set(idt, 195, (uintptr_t)intr195, 0x08, 0x8E);
	idt_set(idt, 196, (uintptr_t)intr196, 0x08, 0x8E);
	idt_set(idt, 197, (uintptr_t)intr197, 0x08, 0x8E);
	idt_set(idt, 198, (uintptr_t)intr198, 0x08, 0x8E);
	idt_set(idt, 199, (uintptr_t)intr199, 0x08, 0x8E);
	idt_set(idt, 200, (uintptr_t)intr200, 0x08, 0x8E);
	idt_set(idt, 201, (uintptr_t)intr201, 0x08, 0x8E);
	idt_set(idt, 202, (uintptr_t)intr202, 0x08, 0x8E);
	idt_set(idt, 203, (uintptr_t)intr203, 0x08, 0x8E);
	idt_set(idt, 204, (uintptr_t)intr204, 0x08, 0x8E);
	idt_set(idt, 205, (uintptr_t)intr205, 0x08, 0x8E);
	idt_set(idt, 206, (uintptr_t)intr206, 0x08, 0x8E);
	idt_set(idt, 207, (uintptr_t)intr207, 0x08, 0x8E);
	idt_set(idt, 208, (uintptr_t)intr208, 0x08, 0x8E);
	idt_set(idt, 209, (uintptr_t)intr209, 0x08, 0x8E);
	idt_set(idt, 210, (uintptr_t)intr210, 0x08, 0x8E);
	idt_set(idt, 211, (uintptr_t)intr211, 0x08, 0x8E);
	idt_set(idt, 212, (uintptr_t)intr212, 0x08, 0x8E);
	idt_set(idt, 213, (uintptr_t)intr213, 0x08, 0x8E);
	idt_set(idt, 214, (uintptr_t)intr214, 0x08, 0x8E);
	idt_set(idt, 215, (uintptr_t)intr215, 0x08, 0x8E);
	idt_set(idt, 216, (uintptr_t)intr216, 0x08, 0x8E);
	idt_set(idt, 217, (uintptr_t)intr217, 0x08, 0x8E);
	idt_set(idt, 218, (uintptr_t)intr218, 0x08, 0x8E);
	idt_set(idt, 219, (uintptr_t)intr219, 0x08, 0x8E);
	idt_set(idt, 220, (uintptr_t)intr220, 0x08, 0x8E);
	idt_set(idt, 221, (uintptr_t)intr221, 0x08, 0x8E);
	idt_set(idt, 222, (uintptr_t)intr222, 0x08, 0x8E);
	idt_set(idt, 223, (uintptr_t)intr223, 0x08, 0x8E);
	idt_set(idt, 224, (uintptr_t)intr224, 0x08, 0x8E);
	idt_set(idt, 225, (uintptr_t)intr225, 0x08, 0x8E);
	idt_set(idt, 226, (uintptr_t)intr226, 0x08, 0x8E);
	idt_set(idt, 227, (uintptr_t)intr227, 0x08, 0x8E);
	idt_set(idt, 228, (uintptr_t)intr228, 0x08, 0x8E);
	idt_set(idt, 229, (uintptr_t)intr229, 0x08, 0x8E);
	idt_set(idt, 230, (uintptr_t)intr230, 0x08, 0x8E);
	idt_set(idt, 231, (uintptr_t)intr231, 0x08, 0x8E);
	idt_set(idt, 232, (uintptr_t)intr232, 0x08, 0x8E);
	idt_set(idt, 233, (uintptr_t)intr233, 0x08, 0x8E);
	idt_set(idt, 234, (uintptr_t)intr234, 0x08, 0x8E);
	idt_set(idt, 235, (uintptr_t)intr235, 0x08, 0x8E);
	idt_set(idt, 236, (uintptr_t)intr236, 0x08, 0x8E);
	idt_set(idt, 237, (uintptr_t)intr237, 0x08, 0x8E);
	idt_set(idt, 238, (uintptr_t)intr238, 0x08, 0x8E);
	idt_set(idt, 239, (uintptr_t)intr239, 0x08, 0x8E);
	idt_set(idt, 240, (uintptr_t)intr240, 0x08, 0x8E);
	idt_set(idt, 241, (uintptr_t)intr241, 0x08, 0x8E);
	idt_set(idt, 242, (uintptr_t)intr242, 0x08, 0x8E);
	idt_set(idt, 243, (uintptr_t)intr243, 0x08, 0x8E);
	idt_set(idt, 244, (uintptr_t)intr244, 0x08, 0x8E);
	idt_set(idt, 245, (uintptr_t)intr245, 0x08, 0x8E);
	idt_set(idt, 246, (uintptr_t)intr246, 0x08, 0x8E);
	idt_set(idt, 247, (uintptr_t)intr247, 0x08, 0x8E);
	idt_set(idt, 248, (uintptr_t)intr248, 0x08, 0x8E);
	idt_set(idt, 249, (uintptr_t)intr249, 0x08, 0x8E);
	idt_set(idt, 250, (uintptr_t)intr250, 0x08, 0x8E);
	idt_set(idt, 251, (uintptr_t)intr251, 0x08, 0x8E);
	idt_set(idt, 252, (uintptr_t)intr252, 0x08, 0x8E);
	idt_set(idt, 253, (uintptr_t)intr253, 0x08, 0x8E);
	idt_set(idt, 254, (uintptr_t)intr254, 0x08, 0x8E);
	idt_set(idt, 255, (uintptr_t)intr255, 0x08, 0x8E);
}