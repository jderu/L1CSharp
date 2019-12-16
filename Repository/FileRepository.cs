using System;
using System.Collections.Generic;

namespace Lab1.Repository
{
    public abstract class FileRepository<ID, E> : ICrudRepository<ID, E> where E : Entity<ID>
    {
        private Dictionary<ID, E> _data;
        private string _filePath;
        private IValidator<E> _validator;

        protected FileRepository(IValidator<E> validator, string filePath)
        {
            _validator = validator;
            _filePath = filePath;
            _data = new Dictionary<ID, E>();
        }

        protected abstract E ReadEntity(string line);
        protected abstract string WriteEntity(E entity);

        public void ReadAllFromFile()
        {
            if (System.IO.File.Exists(_filePath))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(_filePath);
                while (!file.EndOfStream)
                {
                    E entity = ReadEntity(file.ReadLine());
                    _data.Add(entity.Id, entity);
                }
                file.Close();
            }
        }

        public void WriteAllToFile()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(_filePath))
            {
                foreach (E entity in _data.Values)
                    file.WriteLine(WriteEntity(entity));
            }
        }

        public E FindOne(ID id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (_data.ContainsKey(id))
                return _data[id];
            return null;
        }

        public IEnumerable<E> FindAll()
        {
            return _data.Values;
        }

        public E Save(E entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _validator.Validate(entity);
            if (_data.ContainsKey(entity.Id))
                return entity;
            _data.Add(entity.Id, entity);
            return null;
        }

        public E Delete(ID id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (!_data.ContainsKey(id))
                return null;
            E entity = _data[id];
            _data.Remove(id);
            return entity;
        }

        public E Update(E entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _validator.Validate(entity);
            if (!_data.ContainsKey(entity.Id))
                return entity;
            _data[entity.Id] = entity;
            return null;
        }
    }
}