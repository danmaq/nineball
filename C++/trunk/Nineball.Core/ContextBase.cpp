#include "StdAfx.h"
#include "ContextBase.h"

namespace NineballCore
{
	ContextBase::ContextBase(void)
	{
	}

	ContextBase::~ContextBase(void)
	{
		if(encapsulation != NULL)
		{
			SAFE_DELETE(this->encapsulation);
		}
	}

	ContextEncapsulationBase *ContextBase::getEncapsulation(void)
	{
		return this->encapsulation;
	}

	ContextEncapsulationBase *ContextBase::createEncapsulation(void)
	{
		return new ContextEncapsulationBase(this);
	}
}
