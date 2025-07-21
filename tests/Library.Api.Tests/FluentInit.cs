using FluentAssertions;
using FluentAssertions.Extensibility;

[assembly: AssertionEngineInitializer(typeof(FluentInit), nameof(FluentInit.Init))]

public static class FluentInit
{
    public static void Init() => License.Accepted = true;
}
