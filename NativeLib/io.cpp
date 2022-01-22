#include<intrin.h>

void Out8(unsigned short port, unsigned char value) {
	__outbyte(port, value);
}

void Out16(unsigned short port, unsigned short value) {
	__outword(port, value);
}

void Out32(unsigned short port, unsigned int value) {
	__outdword(port, value);
}

unsigned char In8(unsigned short port) {
	return __inbyte(port);
}

unsigned short In16(unsigned short port) {
	return __inword(port);
}

unsigned int In32(unsigned short port) {
	return __indword(port);
}