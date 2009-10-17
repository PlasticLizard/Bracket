namespace Bracket.Events
{
    public interface IBracketEventLogger
    {
        void LogEvent(object sender, BracketEvent e);
    }
}