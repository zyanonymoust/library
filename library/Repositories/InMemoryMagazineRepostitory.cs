using library.Models;

namespace library.Repositories;

public class InMemoryMagazineRepository
    : IMagazineRepository
{
    private readonly List<Magazine> _magazines = new();

    private int _nextId = 1;

    public List<Magazine> GetAll()
    {
        return _magazines.ToList();
    }

    public Magazine? GetById(int id)
    {
        return _magazines.FirstOrDefault(
            magazine => magazine.Id == id
        );
    }

    public List<Magazine> Search(string keyword)
    {
        return _magazines
            .Where(magazine =>
                magazine.Title.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase
                )
                ||
                magazine.Publisher.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .ToList();
    }

    public void Add(Magazine magazine)
    {
        magazine.Id = _nextId;

        _nextId++;

        _magazines.Add(magazine);
    }

    public void Update(Magazine magazine)
    {
        int index = _magazines.FindIndex(
            existingMagazine =>
                existingMagazine.Id == magazine.Id
        );

        if (index == -1)
        {
            return;
        }

        _magazines[index] = magazine;
    }

    public void Delete(int id)
    {
        Magazine? magazine =
            GetById(id);

        if (magazine == null)
        {
            return;
        }

        _magazines.Remove(magazine);
    }
}