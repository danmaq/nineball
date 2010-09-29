#pragma once

class CEntity;

#include "Entity.h"

class CState
{
protected:
	CState(void);

public:
	static const CState empty;
	virtual ~CState(void);
	virtual void setup(CEntity *entity, void *privateMembers) = 0;
	virtual void update(CEntity *entity, void *privateMembers) = 0;
	virtual void draw(CEntity *entity, void *privateMembers) = 0;
	virtual void teardown(CEntity *entity, void *privateMembers, CState *next) = 0;
};
