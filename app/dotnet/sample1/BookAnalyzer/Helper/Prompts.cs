namespace BookAnalyzer.Helper;
public static class Prompts
{
    public  const string KeyQuotes = """
    Analyze the following book text.

    Select up to 3 quotes that are genuinely insightful, thought-provoking,
    or emotionally meaningful.

    Prefer quotes that:
    - express a deep insight about life, people, society, relationships,
      human nature, or the author's experience
    - contain an idea that makes the reader stop and think
    - reveal something distinctive about the author's perspective
    - remain meaningful when read on their own

    Avoid quotes that are:
    - merely introductory or transitional
    - factual or descriptive without deeper meaning
    - important only because they move the story forward
    - generic, clichéd, motivational, or inspirational
    - interesting but not thought-provoking
    - Selecting quotes that are too long or too short. Aim for quotes that are concise yet impactful.

    The goal is not to select the most important sentences in the text.
    The goal is to select the sentences that are most worth remembering
    or reflecting upon.

    Do not paraphrase or invent quotes.
    The quotes must be copied exactly from the ORIGINAL TEXT.

    Populate only "KeyQuotes".

    Document text:
    {0}
    """;
}