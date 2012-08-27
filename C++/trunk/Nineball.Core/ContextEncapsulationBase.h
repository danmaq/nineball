#pragma once
#include "ContextBase.h"

namespace NineballCore
{
	class ContextBase;
	class ContextEncapsulationBase
	{
	public:
		ContextEncapsulationBase(ContextBase *context);
		virtual ~ContextEncapsulationBase();
		ContextBase *getContext(void) const;
	private:
		ContextBase *context;
	};
}
