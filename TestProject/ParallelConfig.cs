


#if PARALLEL_FEATURES
using NUnit.Framework;
[assembly: Parallelizable(ParallelScope.Fixtures)]
#endif
#if PARALLEL_SCENARIOS
using NUnit.Framework;
[assembly: Parallelizable(ParallelScope.All)]
#endif

#if COUNT_2
[assembly: LevelOfParallelism(2)]
#endif
#if COUNT_3
[assembly: LevelOfParallelism(3)]
#endif
#if COUNT_4
[assembly: LevelOfParallelism(4)]
#endif
namespace TestProject;
internal class ParallelConfig {
}
