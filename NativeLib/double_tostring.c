#include "..\LibC\printf.h"

void double_tostring(char* buffer, double value)
{
	snprintf(buffer, 22, "%lf", value);
}