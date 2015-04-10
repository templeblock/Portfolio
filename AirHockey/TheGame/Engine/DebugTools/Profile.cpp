#if PROFILING_ON
#include <DebugTools\Profile.h>
#include <DebugTools\Profiler.h>

namespace Profiling
{
	Profile::Profile(const char* category) : category(category)
	{
		profileTimer.startTimer();
	}
	Profile::~Profile()
	{
		profiler.addEntry(category, profileTimer.getLastTimeInterval()*1000.0f);
	}
}

#endif // PROFILING_ON
