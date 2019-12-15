using System.Collections.Generic;
using Lab1.Repository;

namespace Lab1
{
    public class Service<ID, E> where E : Entity<ID>
    {
        protected FileRepository<ID, E> _repository;

        public Service(FileRepository<ID, E> repository)
        {
            _repository = repository;
        }

        public E FindOne(ID id)
        {
            return _repository.FindOne(id);
        }

        public IEnumerable<E> FindAll()
        {
            return _repository.FindAll();
        }

        public E Save(E entity)
        {
            return _repository.Save(entity);
        }

        public E Delete(ID id)
        {
            return _repository.Delete(id);
        }

        public E Update(E entity)
        {
            return _repository.Update(entity);
        }

        public void WriteAllToFile()
        {
            _repository.WriteAllToFile();
        }
    }
}