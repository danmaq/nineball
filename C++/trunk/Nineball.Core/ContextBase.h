#pragma once
#include "StateBase.h"
#include "ContextEncapsulationBase.h"
namespace NineballCore
{
	class ContextEncapsulationBase;
	class StateBase;
	class ContextBase
	{
	public:
		ContextBase(void);
		virtual ~ContextBase(void);
	protected:
		ContextEncapsulationBase *getEncapsulation(void);
		virtual ContextEncapsulationBase *createEncapsulation(void);
	private:
		ContextEncapsulationBase *encapsulation;
		StateBase *previousState;
		StateBase *currentState;
		StateBase *nextState;
	};
}
