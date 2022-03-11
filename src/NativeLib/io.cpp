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

void Insd(unsigned short port,unsigned int* data,unsigned long count)
{
	__indwordstring(port, data, count);
}

void Insw(unsigned short port, unsigned short* data, unsigned long count)
{
	__inwordstring(port, data, count);
}

void Insb(unsigned short port, unsigned char* data, unsigned long count)
{
	__inbytestring(port, data, count);
}

void Outsd(unsigned short port, unsigned int* data, unsigned long count)
{
	__outdwordstring(port, data, count);
}

void Outsw(unsigned short port, unsigned short* data, unsigned long count)
{
	__outwordstring(port, data, count);
}

void Outsb(unsigned short port, unsigned char* data, unsigned long count)
{
	__outbytestring(port, data, count);
}