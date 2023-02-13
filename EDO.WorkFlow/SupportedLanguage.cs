using System.Collections.Generic;

namespace EDO.WorkFlow;

public class SupportedLanguage
{
    public readonly string Caption;

    public readonly string LangCode;

    public SupportedLanguage(string caption, string langCode)
    {
        Caption = caption;
        LangCode = langCode;
    }

    public static readonly IReadOnlyList<SupportedLanguage> List = new[] {
        new SupportedLanguage("Русский", "ru"),
        new SupportedLanguage("English", "en"),
        new SupportedLanguage("O'zbekcha", "uz"),
    };
}
