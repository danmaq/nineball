#include "StdAfx.h"
#include "ContextEncapsulationBase.h"

namespace NineballCore
{

	ContextEncapsulationBase::ContextEncapsulationBase(ContextBase *context)
	{
		this->context = context;
	}

	ContextEncapsulationBase::~ContextEncapsulationBase()
	{
	}

	ContextBase *ContextEncapsulationBase::getContext(void) const
	{
		return this->context;
	}
}
