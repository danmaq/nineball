#pragma once

namespace nineball
{
	namespace data
	{
		class SPhase
		{
		public:
			SPhase(void);
			virtual ~SPhase(void){}
		private:
			int m_nPhase;
			int m_nNextPhase;
			int m_nPrevPhase;
			int m_nCount;
			int m_nPhaseStartCount;
		};
	};
};
