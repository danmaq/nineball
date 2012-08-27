#pragma once
#include "ContextEncapsulationBase.h"

namespace NineballCore
{
	class ContextEncapsulationBase;
	class StateBase
	{
	public:
		StateBase(void);
		virtual ~StateBase(void);
		virtual void Setup(ContextEncapsulationBase *contextEncapsulation) = 0;
		virtual void Execute(ContextEncapsulationBase *contextEncapsulation) = 0;
		virtual void Teardown(ContextEncapsulationBase *contextEncapsulation) = 0;
	};
}
