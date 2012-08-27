#pragma once
#pragma warning(disable:4514)
#include <stdlib.h>

#define SAFE_DELETE(p) delete (p);(p)=NULL;
