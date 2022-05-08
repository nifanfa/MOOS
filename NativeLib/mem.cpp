#include <intrin.h>
#include <immintrin.h>

void avx_memcpy(void* pvDest, void* pvSrc, size_t nBytes)
{
	if (nBytes % 32 == 0)return;
	if (((intptr_t)(pvDest) & 31) == 0)return;
	if (((intptr_t)(pvSrc) & 31) == 0)return;
	const __m256i* pSrc = (const __m256i*)(pvSrc);
	__m256i* pDest = (__m256i*)(pvDest);
	long nVects = nBytes / sizeof(*pSrc);
	for (; nVects > 0; nVects--, pSrc++, pDest++) {
		const __m256i loaded = _mm256_stream_load_si256(pSrc);
		_mm256_stream_si256(pDest, loaded);
	}
	_mm_sfence();
}

void Movsb(void* dest, void* source, unsigned long count)
{
	__movsb(dest, source, count);
}

void Movsd(unsigned int* dest, unsigned int* source, unsigned long count)
{
	__movsd(dest, source, count);
}

void Stosb(void* p, unsigned char value, unsigned long count)
{
	__stosb(p, value, count);
}

void Stosd(void* p, unsigned int value, unsigned long count)
{
	__stosd(p, value, count);
}