using Common.Models;
using Common.Utilities;

namespace TrickingLibrary.api.Data.store;

public class TricksStore
{
    private readonly List<Trick> _tricks = [];

    public List<Trick> All => _tricks;

    public Trick? GetById(string id) => _tricks.FirstOrDefault(t => t.Id.Equals(id));

    public Trick AddTrick(Trick trick)
    {
        trick.Id = Formatter.FormatePascalToKebab(trick.Name);
        _tricks.Add(trick);
        return trick;
    }
}