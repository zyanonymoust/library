using library.Models;
using library.Repositories;

namespace library.Services;

public class MagazineService
{
    private readonly IMagazineRepository _repository;

    public MagazineService(
        IMagazineRepository repository)
    {
        _repository = repository;
    }

    public List<Magazine> GetAllMagazines()
    {
        return _repository.GetAll();
    }

    public Magazine? GetMagazineById(int id)
    {
        return _repository.GetById(id);
    }

    public List<Magazine> SearchMagazines(
        string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<Magazine>();
        }

        return _repository.Search(
            keyword.Trim()
        );
    }

    public void AddMagazine(
        Magazine magazine)
    {
        ValidateMagazine(magazine);

        _repository.Add(magazine);
    }

    public bool UpdateMagazine(
        Magazine magazine)
    {
        Magazine? existingMagazine =
            _repository.GetById(
                magazine.Id
            );

        if (existingMagazine == null)
        {
            return false;
        }

        ValidateMagazine(magazine);

        _repository.Update(magazine);

        return true;
    }

    public bool DeleteMagazine(int id)
    {
        Magazine? magazine =
            _repository.GetById(id);

        if (magazine == null)
        {
            return false;
        }

        _repository.Delete(id);

        return true;
    }

    private static void ValidateMagazine(
        Magazine magazine)
    {
        if (string.IsNullOrWhiteSpace(
            magazine.Title))
        {
            throw new ArgumentException(
                "Title cannot be empty."
            );
        }

        if (string.IsNullOrWhiteSpace(
            magazine.Publisher))
        {
            throw new ArgumentException(
                "Publisher cannot be empty."
            );
        }

        if (magazine.IssueNumber <= 0)
        {
            throw new ArgumentException(
                "Issue number must be greater than 0."
            );
        }

        if (magazine.PublishedDate ==
            default)
        {
            throw new ArgumentException(
                "Published date is required."
            );
        }
    }
}