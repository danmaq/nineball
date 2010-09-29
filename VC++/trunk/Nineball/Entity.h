#pragma once

class CState;

#include "State.h"

class CEntity
{
private:
	int m_counter;
	CState *m_previousState;
	CState *m_currentState;
public:
	CState *nextState;

	CEntity(CState *firstState = NULL, void *privateMembers = NULL);
	virtual ~CEntity(void);

	inline CState *currentState() const;
	inline CState *previousState() const;

	virtual void update();
	virtual void draw();
};
