// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:Using directives should be placed correctly",
        Justification = "Placing usings within namespaces is not default in VS or in generated code.")]
[assembly:
    SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this",
        Justification = "Visual Studio by default recommends removing prefixed 'this'.")]
[assembly:
    SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:Field names should not begin with underscore",
        Justification = "MS style guide uses _ for private members.")]