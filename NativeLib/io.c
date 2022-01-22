
void Out8(unsigned short port, unsigned char value) {
	__outbyte(port, value);
}

void Out16(unsigned short port, unsigned short value) {
	__outword(port, value);
}

unsigned char In8(unsigned short port) {
	return __inbyte(port);
}

unsigned short In16(unsigned short port) {
	return __inword(port);
}