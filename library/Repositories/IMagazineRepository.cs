using library.Models;

namespace library.Repositories;

public interface IMagazineRepository
{
    List<Magazine> GetAll();

    Magazine? GetById(int id);

    List<Magazine> Search(string keyword);

    void Add(Magazine magazine);

    void Update(Magazine magazine);

    void Delete(int id);
}