using System.Reflection;

namespace GroshieHub.API;

public static class AssemblyReference
{
	public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}