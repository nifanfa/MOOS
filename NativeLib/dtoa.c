#include "..\LibC\printf.h"

dtoa_(double value, char* buffer)
{
	snprintf(buffer, 22, "%lf", value);
}