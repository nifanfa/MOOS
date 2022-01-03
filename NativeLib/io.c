
void outb(unsigned short port, unsigned char value) {
	__outbyte(port, value);
}

void outw(unsigned short port, unsigned short value) {
	__outword(port, value);
}

unsigned char inb(unsigned short port) {
	return __inbyte(port);
}

unsigned short inw(unsigned short port) {
	return __inword(port);
}