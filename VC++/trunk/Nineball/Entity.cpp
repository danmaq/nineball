#include "stdafx.h"
#include "Entity.h"

CEntity::CEntity(CState *firstState, void *privateMembers)
{
	m_previousState = const_cast<CState *>(&CState::empty);
	m_currentState = const_cast<CState *>(&CState::empty);
	nextState = firstState;
	m_counter = 0;
}

CEntity::~CEntity(void)
{
}

void CEntity::update()
{
	currentState()->update(this, NULL);
}

void CEntity::draw()
{
	currentState()->draw(this, NULL);
}

CState *CEntity::currentState() const
{
	return m_currentState;
}

CState *CEntity::previousState() const
{
	return m_previousState;
}
